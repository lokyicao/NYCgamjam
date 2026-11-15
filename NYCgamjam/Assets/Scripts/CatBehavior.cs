using UnityEngine;
using UnityEngine.TestTools;

public class CatBehavior : MonoBehaviour
{
    Vector3 startPos, prevPos, targetPos;
    public float pursuitRange = 8f;
    float aggroRange = 4f;
    bool isChasing = false;
    const float jumpTime = 1f;
    GameObject player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        targetPos = startPos;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 playerPos = player.transform.position;
        if (isChasing)
        {
            if (Vector2.Distance(transform.position, playerPos) > pursuitRange)
            {
                isChasing = false;
            } else
            {
                if (transform.position == targetPos)
                {
                    targetPos = playerPos;
                    prevPos = transform.position;
                    Debug.Log("startJump");
                } else
                {
                    Vector3 moveAmount = (targetPos - prevPos) * Time.deltaTime / jumpTime;
                    transform.position += moveAmount;
                    if (Vector2.Distance(transform.position, targetPos) < moveAmount.magnitude)
                    {
                        transform.position = targetPos;
                        Debug.Log("landJump " + transform.position);
                    }
                }
            }
        } else
        {
            if (Vector2.Distance(transform.position, playerPos) < aggroRange)
            {
                isChasing = true;
            }
        }
            
    }

    Vector3 GetRandomPos()
    {
        float x = Random.Range(startPos.x - pursuitRange, startPos.x + pursuitRange);
        float y = Random.Range(startPos.y - pursuitRange, startPos.y + pursuitRange);
        return new Vector3(x, y, 0);
    }
}
