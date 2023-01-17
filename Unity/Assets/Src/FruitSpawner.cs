using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FruitSpawner : MonoBehaviour {
    [SerializeField]
    private GameObject[] fruitsPrefab;
    private BoxCollider2D boxCollider;

    public SpriteRenderer spriteRenderer;

    private bool left = false;

    private void Start()
    {
        int dir = Random.Range(0, 2);
        if(dir == 1){
            left = true;
        }

        this.boxCollider = this.GetComponent<BoxCollider2D>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public GameObject SpawnFruit()
    {
        int fruit_index = Random.Range(0, this.fruitsPrefab.Length);

        Vector3 fruit_pos = new Vector3(
            Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x),
            Random.Range(boxCollider.bounds.min.y, boxCollider.bounds.max.y),
            0
        );

        GameObject fruit = (GameObject)Instantiate(this.fruitsPrefab[fruit_index], fruit_pos, Quaternion.identity, boxCollider.gameObject.transform.parent);
        
        return fruit;
    }

    public Vector3 EdgeCaseSpawn()
    {
        if(left){
            left = false;
            return new Vector3(boxCollider.bounds.min.x, boxCollider.bounds.min.y, 0);
        }
        left = true;
        return new Vector3(boxCollider.bounds.max.x, boxCollider.bounds.min.y, 0);
    }

    public GameObject SpawnFruitAtPosition(Vector3 fruit_pos)
    {
        if(fruit_pos.x > boxCollider.bounds.max.x || fruit_pos.x < boxCollider.bounds.min.x ||
            fruit_pos.y > boxCollider.bounds.max.y || fruit_pos.y < boxCollider.bounds.min.y){
                return null;
        }

        int fruit_index = Random.Range(0, this.fruitsPrefab.Length);

        GameObject fruit = (GameObject)Instantiate(this.fruitsPrefab[fruit_index], fruit_pos, Quaternion.identity, boxCollider.gameObject.transform.parent);
        
        return fruit;
    }

}