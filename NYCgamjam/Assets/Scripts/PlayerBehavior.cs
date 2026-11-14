using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    float accel = 1f;
    float rotationSpeed = 120f;
    Vector3 velocity = Vector3.zero;
    float friction = 0.95f;
    Vector3 desiredDirection = Vector3.zero;
    float terminalVelocity = 10f;
    bool isInvincible = false;
    float currentInvincibleTime = 0f;
    float invincibleCoolDownTime;
    const float maxInvincibleTime = 2f;

    // determines if movement is rotational
    const bool sideKeysTurn = false;

    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Hellooo");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        rb.simulated = !isInvincible;
        desiredDirection.Normalize();
        velocity += desiredDirection * accel;
        transform.position += Vector3.ClampMagnitude(velocity, terminalVelocity) * Time.deltaTime;
        velocity *= friction;

    }

    //private void OnTriggerEnter2d(Collider2D c)
    //{
    //    Debug.Log("Got Hittt");
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with something");
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("I Got Hit by an Enemyyy");
        }
    }

    //void OnColliderEnter2D(Collider2D other)
    //{
    //    Debug.Log("Ouch bro");
    //}

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("ouch!!");
        Vector3 difference = transform.position - other.gameObject.transform.position;
        difference.Normalize();
        velocity = difference * 50;

    }

    void HandleInput()
    {
        if (Keyboard.current != null)
        {
            isInvincible = false;
            if (invincibleCoolDownTime <= 0 && Keyboard.current.spaceKey.isPressed)
            {
                isInvincible = true;
                currentInvincibleTime += Time.deltaTime;
                if (currentInvincibleTime > maxInvincibleTime)
                {
                    invincibleCoolDownTime = currentInvincibleTime * 2;
                    currentInvincibleTime = 0;
                }
            } else
            {
                if (currentInvincibleTime > 0)
                {
                    invincibleCoolDownTime = currentInvincibleTime * 2;
                    currentInvincibleTime = 0;
                }
                invincibleCoolDownTime -= Time.deltaTime;
            }
            Debug.Log(currentInvincibleTime + " " + invincibleCoolDownTime);

            desiredDirection = Vector3.zero;
            if (sideKeysTurn)
            {
                if (Keyboard.current.wKey.isPressed)
                {
                    desiredDirection += transform.up;
                }
                if (Keyboard.current.sKey.isPressed)
                {
                    desiredDirection -= transform.up;
                }
                if (Keyboard.current.aKey.isPressed)
                {
                    transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                }
                if (Keyboard.current.dKey.isPressed)
                {
                    transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
                }
            } else
            {
                if (Keyboard.current.wKey.isPressed)
                {
                    desiredDirection += Vector3.up;
                }
                if (Keyboard.current.sKey.isPressed)
                {
                    desiredDirection += Vector3.down;
                }
                if (Keyboard.current.aKey.isPressed)
                {
                    desiredDirection += Vector3.left;
                }
                if (Keyboard.current.dKey.isPressed)
                {
                    desiredDirection += Vector3.right;
                }
            }
        }
    }
}
