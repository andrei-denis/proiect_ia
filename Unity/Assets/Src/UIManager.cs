// #define TRAINING

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIManager : MonoBehaviour
{  //960-600 ( 5:4)
    public enum Screens{
        MainMenu = 0,
        InGame = 1,
        GameOver = 2
    }

    private VisualElement[] screens;

    private Screens currentScreen;

    private void Awake()
    {
        #if TRAINING
            return;
        #endif
        VisualElement root = this.GetComponent<UIDocument>().rootVisualElement;

        this.screens = new VisualElement[Enum.GetNames(typeof(Screens)).Length];

        this.screens[(int)Screens.MainMenu] = root.Q<VisualElement>("MainMenu");
        this.screens[(int)Screens.InGame] = root.Q<VisualElement>("InGame");
        this.screens[(int)Screens.GameOver] = root.Q<VisualElement>("GameOver");
    }

    public void ChangeScreen(Screens newScreen)
    {
        #if TRAINING
            return;
        #endif
        this.screens[(int)this.currentScreen].style.display = DisplayStyle.None;
        this.currentScreen = newScreen;
        this.screens[(int)this.currentScreen].style.display = DisplayStyle.Flex;
    }

    public void UpdateScore(int score)
    {
        #if TRAINING
            return;
        #endif
        if(this.currentScreen != Screens.InGame){
            return;
        }

        this.screens[(int)Screens.InGame].Q<Label>("score").text = score.ToString();
    }

    public void UpdateFinalScore(int score)
    {
        #if TRAINING
            return;
        #endif
        if(this.currentScreen != Screens.GameOver){
            return;
        }

        this.screens[(int)Screens.GameOver].Q<Label>("finalScore").text = score.ToString();
    }

    public void UpdateHearts(int hearts)
    {
        #if TRAINING
            return;
        #endif
        if(this.currentScreen != Screens.InGame){
            return;
        }

        VisualElement root = this.screens[(int)Screens.InGame];

        if(hearts > 3){
            root.Q<VisualElement>("heart1").style.display = DisplayStyle.Flex;
            root.Q<VisualElement>("heart2").style.display = DisplayStyle.Flex;
            root.Q<VisualElement>("heart3").style.display = DisplayStyle.Flex;
            root.Q<Label>("hearts").text = "+" + hearts.ToString();
        }
        else if(hearts == 3){
            root.Q<VisualElement>("heart1").style.display = DisplayStyle.Flex;
            root.Q<VisualElement>("heart2").style.display = DisplayStyle.Flex;
            root.Q<VisualElement>("heart3").style.display = DisplayStyle.Flex;
            root.Q<Label>("hearts").text = "";
        }
        else if(hearts == 2){
            root.Q<VisualElement>("heart1").style.display = DisplayStyle.Flex;
            root.Q<VisualElement>("heart2").style.display = DisplayStyle.Flex;
            root.Q<VisualElement>("heart3").style.display = DisplayStyle.None;
            root.Q<Label>("hearts").text = "";
        }
        else if(hearts == 1){
            root.Q<VisualElement>("heart1").style.display = DisplayStyle.Flex;
            root.Q<VisualElement>("heart2").style.display = DisplayStyle.None;
            root.Q<VisualElement>("heart3").style.display = DisplayStyle.None;
            root.Q<Label>("hearts").text = "";
        }
        else{
            root.Q<VisualElement>("heart1").style.display = DisplayStyle.None;
            root.Q<VisualElement>("heart2").style.display = DisplayStyle.None;
            root.Q<VisualElement>("heart3").style.display = DisplayStyle.None;
            root.Q<Label>("hearts").text = "";
        }
    }

    public void SetButtonCallBack(Screens screen, string buttonName, Action callBack)
    {
        #if TRAINING
            return;
        #endif
        this.screens[((int)screen)].Q<Button>(buttonName).clicked += callBack;
    }
}
