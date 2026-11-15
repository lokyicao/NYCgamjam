using UnityEngine;
using UnityEngine.TestTools;

public class CatBehavior : MonoBehaviour
{
    Vector3 startPos, prevPos, targetPos;
    float pursuitRange = 8f;
    GameObject player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position == targetPos)
        {
            targetPos = player.transform.position;
        }
        Debug.Log(targetPos);
    }

    Vector3 GetRandomPos()
    {
        float x = Random.Range(startPos.x - pursuitRange, startPos.x + pursuitRange);
        float y = Random.Range(startPos.y - pursuitRange, startPos.y + pursuitRange);
        return new Vector3(x, y, 0);
    }
}
