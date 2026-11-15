using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator _animator;
    public Image staminabar;
    public Canvas canvas;
    float amount = 1f;

    // audio stuff
    public AudioClip walkClip;
    public AudioClip flapClip;
    public AudioClip bounceClip;
    private AudioSource audioSource;
    private AudioSource movementAudioSource;

    
    private SpriteRenderer _spriteRenderer;
    float accel = 1.25f;
    float stunControl = 0.5f;
    float flyControl = 1.75f;
    float rotationSpeed = 120f;
    
    Vector3 velocity = Vector3.zero;
    float friction = 15f;
    Vector3 desiredDirection = Vector3.zero;
    float terminalVelocity = 10f;
    bool isInvincible = false;
    bool isStunned = false;
    bool isJumping = false;
    float currentInvincibleTime = 0f;
    public float invincibleTimeMeter { get; private set; } = maxInvincibleTime;
    float currentStunnedTime = 0f;
    const float maxInvincibleTime = 2f;
    const float totalStunTime = 0.5f;

    // determines if movement is rotational

    Rigidbody2D rb;
    SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (GameTimer.Instance == null)
        {
            new GameObject("GameTimer").AddComponent<GameTimer>();
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        movementAudioSource = gameObject.AddComponent<AudioSource>();
        movementAudioSource.playOnAwake = false;
        movementAudioSource.loop = true;
        movementAudioSource.spatialBlend = 0f;
        movementAudioSource.volume = audioSource.volume;
        movementAudioSource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        desiredDirection.Normalize();
        UpdateMovementAudio();
        float alpha = 1f;
        if (!isInvincible)
        {
            _animator.SetBool("isJumping", false);
            alpha = 1f;
            transform.localScale = new Vector2(1f, 1f);
        }
        if (isInvincible)
        {
            // Flight audio is managed as a loop by UpdateMovementAudio.
            _animator.SetBool("isJumping", true);
            alpha = 0.9f;
            float scaleUp = 1f + Mathf.Clamp(currentInvincibleTime, 0f, 0.05f);
            transform.localScale = new Vector2(scaleUp, scaleUp);
        }
        else if (desiredDirection != Vector3.zero)
        {
            
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }
        Color color = sr.color;
        color.a = alpha;
        sr.color = color;

        amount = invincibleTimeMeter / maxInvincibleTime;

        staminabar.fillAmount = amount;
        if (canvas != null)
        {
            if (amount == 1f)
            {
                canvas.enabled = false;
            } else
            {
                canvas.enabled = true;
            
            }
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
            velocity += desiredDirection * accel * stunControl;
            currentStunnedTime += Time.deltaTime;
            if (currentStunnedTime > totalStunTime)
            {
                isStunned = false;
            }
        } else if (isInvincible)
        {
         
            velocity += desiredDirection * accel * flyControl;
        } else
        {
            velocity += desiredDirection * accel;
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
    private void UpdateMovementAudio()
    {
        if (movementAudioSource == null) return;

        AudioClip targetClip = null;
        if (Time.timeScale > 0f && Keyboard.current != null && Application.isFocused)
        {
            bool flying = isInvincible && Keyboard.current.spaceKey.isPressed;
            if (flying)
                targetClip = flapClip;
            else if (desiredDirection.sqrMagnitude > 0f && !isStunned)
                targetClip = walkClip;
        }

        if (targetClip == null)
        {
            movementAudioSource.Stop();
            movementAudioSource.clip = null;
            return;
        }

        if (movementAudioSource.clip != targetClip)
        {
            movementAudioSource.Stop();
            movementAudioSource.clip = targetClip;
        }
        if (!movementAudioSource.isPlaying)
            movementAudioSource.Play();
    }

    private void OnDisable()
    {
        if (movementAudioSource != null) movementAudioSource.Stop();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null) return;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }

        audioSource.PlayOneShot(clip);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isInvincible && (other.CompareTag("Rat")|| other.CompareTag("Human")|| other.CompareTag("Car")|| other.CompareTag("Cat")))
        {
            Debug.Log("ouch!!");
            Vector3 difference = transform.position - other.gameObject.transform.position;
            difference.Normalize();
            velocity = difference * 40;
            isStunned = true;
            currentStunnedTime = 0;
            PlaySound(bounceClip);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
    }

    void HandleInput()
    {
        if (Keyboard.current != null)
        {
            if (isInvincible)
            {
                if (invincibleTimeMeter <= 0 || !Keyboard.current.spaceKey.isPressed)
                {
                    isInvincible = false;
                } else
                {
                    invincibleTimeMeter -= Time.deltaTime;
                    currentInvincibleTime += Time.deltaTime;
                }
                

            } else
            {
                if (invincibleTimeMeter > 0 && Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    isInvincible = true;
                } else
                {
                    invincibleTimeMeter += Time.deltaTime / 2;
                    invincibleTimeMeter = Mathf.Clamp(invincibleTimeMeter, 0f, maxInvincibleTime);
                }
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
