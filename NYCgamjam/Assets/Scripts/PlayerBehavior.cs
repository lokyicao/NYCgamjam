using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator _animator;

    // audio stuff
    public AudioClip flapClip;
    public AudioClip bounceClip;
    private AudioSource audioSource;

    
    private SpriteRenderer _spriteRenderer;
    float accel = 1.5f;
    float stunControl = 0.5f;
    float rotationSpeed = 120f;
    
    Vector3 velocity = Vector3.zero;
    float friction = 15f;
    Vector3 desiredDirection = Vector3.zero;
    float terminalVelocity = 10f;
    bool isInvincible = false;
    bool isStunned = false;
    bool isJumping = false;
    float currentInvincibleTime = 0f;
    float currentStunnedTime = 0f;
    float invincibleCoolDownTime;
    const float maxInvincibleTime = 2f;
    const float totalStunTime = 0.5f;

    // determines if movement is rotational
    const bool sideKeysTurn = false;

    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (GameTimer.Instance == null)
        {
            new GameObject("GameTimer").AddComponent<GameTimer>();
        }
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        desiredDirection.Normalize();
        if (!isInvincible)
        {
            _animator.SetBool("isJumping", false);
        }
        if (isInvincible)
        {
            //PlaySound(flapClip);
            _animator.SetBool("isJumping", true); 
        }
        else if (desiredDirection != Vector3.zero)
        {
            
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }


    }

    //private void OnTriggerEnter2d(Collider2D c)
    //{
    //    Debug.Log("Got Hittt");
    //}

    void FixedUpdate()
    {
        if (isStunned)
        {
            PlaySound(bounceClip);
            velocity += desiredDirection * accel * stunControl;
            currentStunnedTime += Time.deltaTime;
            if (currentStunnedTime > totalStunTime)
            {
                isStunned = false;
            }
            //transform.position += Vector3.ClampMagnitude(velocity, terminalVelocity * stunControl) * Time.deltaTime;
        } else
        {
            velocity += desiredDirection * accel;
            //transform.position += Vector3.ClampMagnitude(velocity, terminalVelocity) * Time.deltaTime;
        }

        //transform.position += velocity * Time.deltaTime;

        // Frame-rate independent exponential decay
        velocity *= Mathf.Exp(-friction * Time.deltaTime);

        // Optional: Snap to zero when nearly stopped to prevent endless micro-sliding
        if (velocity.sqrMagnitude < 0.001f)
        {
            velocity = Vector3.zero;
        }
        rb.MovePosition(transform.position + velocity * Time.deltaTime);
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isInvincible && other.CompareTag("Enemy"))
        {
            Debug.Log("ouch!!");
            Vector3 difference = transform.position - other.gameObject.transform.position;
            difference.Normalize();
            velocity = difference * 40;
            isStunned = true;
            currentStunnedTime = 0;
        }
    }

    //void OnColliderEnter2D(Collider2D other)
    //{
    //    Debug.Log("Ouch bro");
    //}

    void OnCollisionEnter2D(Collision2D other)
    {
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

            desiredDirection = Vector3.zero;
            Vector3 up = cameraTransform != null
            ? cameraTransform.up
            : Vector3.up;
                
            Vector3 right = cameraTransform != null
                ? cameraTransform.right
                : Vector3.right;

            if (Keyboard.current.wKey.isPressed)
                desiredDirection += up;

            if (Keyboard.current.sKey.isPressed)
                desiredDirection -= up;

            if (Keyboard.current.aKey.isPressed){
                desiredDirection -= right;
                _spriteRenderer.flipX = true;
            }


            if (Keyboard.current.dKey.isPressed){
                desiredDirection += right;
                _spriteRenderer.flipX = false;
            }
        }
    }
}
