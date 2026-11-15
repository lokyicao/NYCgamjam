//using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesToSpawn;
    public GameObject enemyToSpawn;
    public Vector3 enemyMoveDirection;
    Vector3 minPos;
    Vector3 maxPos;
    public float spawnCooldown = 2f;
    public float timeDelay = 0;
    float timeSinceSpawn;
    bool isActive = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        minPos = collider.bounds.min;
        maxPos = collider.bounds.max;
        timeSinceSpawn = -timeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        if (isActive && timeSinceSpawn > spawnCooldown)
        {
            
            timeSinceSpawn = 0;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector3 pos = GetPosInRange();
        
        
        if (enemiesToSpawn != null && enemiesToSpawn.Length > 0)
        {
            int num = Random.Range(0, enemiesToSpawn.Length); 
            Debug.Log("enemiesToSpawn.Length: " + enemiesToSpawn.Length);  
            Debug.Log("num: " + num);
            enemyToSpawn = enemiesToSpawn[num]; 
        }

        
        GameObject newEnemy = Instantiate(enemyToSpawn, pos, enemyToSpawn.transform.rotation);
        newEnemy.GetComponent<EnemyMovement>().SetDirection(enemyMoveDirection);
    }

    Vector3 GetPosInRange()
    {
        float x = Mathf.Floor((int)Random.Range(minPos.x, maxPos.x)) - 0.5f;
        float y = Mathf.Floor((int)Random.Range(minPos.y, maxPos.y)) + 0.5f;
        return new Vector3(x, y, 0);
    }
}
