using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    Vector3 minPos;
    Vector3 maxPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        minPos = collider.bounds.min;
        maxPos = collider.bounds.max;
        Debug.Log(minPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyToSpawn != null & Keyboard.current.fKey.wasPressedThisFrame)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector3 pos = GetPosInRange();
        Instantiate(enemyToSpawn, pos, Quaternion.identity);
    }

    Vector3 GetPosInRange()
    {
        float x = Mathf.Floor((int)Random.Range(minPos.x, maxPos.x)) - 0.5f;
        float y = Mathf.Floor((int)Random.Range(minPos.y, maxPos.y)) - 0.5f;
        return new Vector3(x, y, 0);
    }
}
