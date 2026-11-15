using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;

public class CatBehavior : MonoBehaviour
{
    [SerializeField] public Animator _animator;
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
                isWaiting = true;
                isStartingJump = false;
                isMidair = false;
                isLanding = false;
            } else
            {
                if (isWaiting)
                {
                    timeBeforeNextAction -= Time.deltaTime;
                    if (timeBeforeNextAction <= 0)
                    {
                        isWaiting = false;
                        isStartingJump = true;
                        timeBeforeNextAction = preJumpWaitTime;
                        Debug.Log("startJump");
                    }
                } else if (isStartingJump)
                {
                    timeBeforeNextAction -= Time.deltaTime;
                    if (timeBeforeNextAction <= 0)
                    {
                        isMidair = true;
                        isStartingJump = false;
                        Debug.Log("startMidair");
                    }
                } else if (isLanding)
                {
                    timeBeforeNextAction -= Time.deltaTime;
                    if (timeBeforeNextAction <= 0)
                    {
                        isWaiting = true;
                        isLanding = false;
                        timeBeforeNextAction = waitTime;
                        Debug.Log("startLand");
                    }
                } else
                {
                    if (Vector2.Distance(transform.position, targetPos) == 0)
                    {
                        targetPos = playerPos;
                        prevPos = transform.position;
                    }
                    else
                    {
                        Vector2 moveAmount = (targetPos - prevPos) * Time.deltaTime / jumpTime;
                        transform.position += new Vector3(moveAmount.x, moveAmount.y, 0);
                        if (Vector2.Distance(transform.position, targetPos) < moveAmount.magnitude)
                        {
                            transform.position = new Vector3(targetPos.x, targetPos.y, 0);
                            isMidair = false;
                            isLanding = true;
                            timeBeforeNextAction = preJumpWaitTime;
                            Debug.Log("StartWait");
                        }
                    }
                }
                
            }
        } else
        {
            transform.position = startPos;
            if (Vector2.Distance(transform.position, playerPos) < aggroRange)
            {
                isChasing = true;
                isWaiting = true;
                isStartingJump = false;
                isMidair = false;
                isLanding = false;
                timeBeforeNextAction = 0;
            }
        }
            
    }

    void Update()
    {
        _animator.SetBool("isWaiting", isWaiting);
        _animator.SetBool("isStartingJump", isStartingJump);
        _animator.SetBool("isMidair", isMidair);
        _animator.SetBool("isLanding", isLanding);
    }
}
