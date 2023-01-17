using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private enum TrainingScenario
    {
        RandomSpawn,
        EdgeSpawn,
        Both
    }

    public bool TRAINING;
    [SerializeField]
    private TrainingScenario training_scenario;

    [SerializeField]
    private FruitSpawner fruitSpawner;
    [SerializeField]
    public Character character;
    public PopoAgent agent;
    [SerializeField]
    private UIManager uiManager;

    public List<GameObject> fruits = new List<GameObject>();
    private int score = 0;

    private bool gameStarted = false;

    private bool canSpawnFruit = true;

    #region timers
    private float spawnTime = 1.2f;
    private float currentSpawnTime = 0f;
    #endregion

    private void Awake() {
        this.character.OnFruitCathced += this.FruitCatched;
        this.character.OnDying += this.FinishGame;
        this.character.OnTeleport += this.PunishAgent;
    }

    private void Start() {  
        this.uiManager.SetButtonCallBack(UIManager.Screens.MainMenu, "StartButton", this.StartGame);
        this.uiManager.SetButtonCallBack(UIManager.Screens.GameOver, "RestartButton", this.RestartGame);
    }

    private void Update()
    {
        if(!gameStarted){
            return;
        }

        if(TRAINING){
            if(Utility.CountdownTimer(ref currentSpawnTime, spawnTime, Time.deltaTime)){
                SpawnFruit();
            }
        }
        else{
            // if(Utility.CountdownTimer(ref currentSpawnTime, spawnTime, Time.deltaTime)){
            //     SpawnFruit();
            // }
            GetPlayerInput();
            if(Utility.CountdownTimer(ref currentSpawnTime, spawnTime, Time.deltaTime)){
                canSpawnFruit = true;
            }
        }
        
    }

    private void PunishAgent()
    {
        if(TRAINING){
            agent.AddReward(0.4f);
        }
    }

    private void StartGame()
    {
        this.gameStarted = true;
        this.character.enabled = true;
        this.agent.enabled = true;
        this.character.hearts = 3;
        this.score = 0;
        this.uiManager.ChangeScreen(UIManager.Screens.InGame);
        this.uiManager.UpdateHearts(character.hearts);
        this.uiManager.UpdateScore(this.score);
    }

    public void RestartGame()
    {
        this.character.gameObject.transform.localPosition = new Vector3(0, -3, 0);
        StartGame();
    }

    private void FruitCatched(GameObject fruit)
    {
        if(TRAINING){
            agent.AddReward(1f);
            fruitSpawner.spriteRenderer.color = Color.green;
            if(score > 10){
                agent.EndEpisode();
            }
        }
        fruits.Remove(fruit);
        Destroy(fruit);
        score++;
        if(score > 160){
            spawnTime = 1.3f;
        }
        else if(score > 80){
            spawnTime = 1.65f;
        }
        uiManager.UpdateScore(score);
    }

    private void SpawnFruit()
    {
        switch (training_scenario)
        {
            case TrainingScenario.RandomSpawn:
                fruits.Add(fruitSpawner.SpawnFruit());
                break;
            case TrainingScenario.EdgeSpawn:
                fruits.Add(fruitSpawner.SpawnFruitAtPosition(fruitSpawner.EdgeCaseSpawn()));
                break;
            case TrainingScenario.Both:
                int which = UnityEngine.Random.Range(0, 6);
                if(which == 1){
                    fruits.Add(fruitSpawner.SpawnFruitAtPosition(fruitSpawner.EdgeCaseSpawn()));
                }
                else{
                    fruits.Add(fruitSpawner.SpawnFruit());
                }
                break;
            default:
                break;
        }
    }

    private void MissedFruit(GameObject fruit)
    {
        fruits.Remove(fruit);
        Destroy(fruit);
        if(TRAINING){
            agent.AddReward(-.9f);
            fruitSpawner.spriteRenderer.color = Color.red;
            agent.EndEpisode();
        }
        character.TakeDamage();
        uiManager.UpdateHearts(character.hearts);
    }

    private void ClearFruits()
    {
        while(this.fruits.Count > 0){
            Destroy(this.fruits[0]);
            this.fruits.RemoveAt(0);
        }
    }

    private void FinishGame()
    {
        ClearFruits();
        this.gameStarted = false;
        this.uiManager.ChangeScreen(UIManager.Screens.GameOver);
        this.uiManager.UpdateFinalScore(score);
        this.character.StopAnimation();
        this.character.enabled = false;
        if(TRAINING){
            agent.EndEpisode();
        }
    }

    private void GetPlayerInput()
    {
        if(canSpawnFruit && Input.GetMouseButtonDown(0)){
            Vector3 clickPos = this.character.cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

            GameObject fruit = this.fruitSpawner.SpawnFruitAtPosition(clickPos);

            if(fruit){
                fruits.Add(fruit);
                canSpawnFruit = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Fruit"){
            this.MissedFruit(other.gameObject);
        }
    }

}
