using UnityEngine;
using UnityEngine.TestTools;

public class CatBehavior : MonoBehaviour
{
    Vector3 startPos, prevPos, targetPos;
    public float pursuitRange = 8f;
    float aggroRange = 4f;
    bool isChasing = false;
    bool isWaiting = false;

    // animation bools
    bool isStartingJump = false;
    bool isMidair = false;
    bool isLanding = false;


    float timeBeforeNextAction;
    const float jumpTime = 0.5f;
    const float waitTime = 1f;
    const float preJumpWaitTime = 0.2f;
    GameObject player;
    SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        targetPos = startPos;
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(startPos);
        sr = GetComponent<SpriteRenderer>();
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
                if (isWaiting)
                {
                    timeBeforeNextAction -= Time.deltaTime;
                    if (timeBeforeNextAction <= 0)
                    {
                        isWaiting = false;
                    }
                } else if (isStartingJump)
                {

                } else
                {
                    if (Vector2.Distance(transform.position, targetPos) == 0)
                    {
                        targetPos = playerPos;
                        prevPos = transform.position;
                        Debug.Log("startJump");
                    }
                    else
                    {
                        Vector2 moveAmount = (targetPos - prevPos) * Time.deltaTime / jumpTime;
                        transform.position += new Vector3(moveAmount.x, moveAmount.y, 0);
                        Debug.Log(transform.position);
                        if (Vector2.Distance(transform.position, targetPos) < moveAmount.magnitude)
                        {
                            transform.position = new Vector3(targetPos.x, targetPos.y, 0);
                            isWaiting = true;
                            timeBeforeNextAction = waitTime;
                            Debug.Log("landJump " + transform.position);
                        }
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
