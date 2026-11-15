using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class comicmovement : MonoBehaviour
{
    public Camera targetCamera;
    public float targetOrthographicSize = 10f;
    public float zoomduration =1.0f;    
    private bool hasZoomed = false;
    int moveSpeed = 8;
    int frame = 0;
    Vector3 position1, position2, position3, position4, position5;

    Vector3 positionGoal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        position1 = new Vector3(8.69f, -4.84f, 0f);
        position2 = new Vector3(-8.71f, -4.84f, 0f);
        position3 = new Vector3(8.69f, 4.86f, 0f);
        position4 = new Vector3(-8.71f, 4.86f, 0f);
        position5 = new Vector3(0f, 0f, 0f);
        positionGoal = position1;
        transform.position = positionGoal;


    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        transform.position = Vector3.MoveTowards(transform.position, positionGoal, moveSpeed * Time.deltaTime);
        
    }
    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }
    public void TriggerZoomOut()
    {
        if (!hasZoomed)
        {
            hasZoomed = true;
            StartCoroutine(ZoomOutRoutine());
        }
    }
    private IEnumerator ZoomOutRoutine()
    {
        float startSize = targetCamera.orthographicSize;
        float elapsed = 0f;

        while (elapsed < zoomduration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / zoomduration);
            
            // SmoothStep creates a nice ease-in and ease-out curve
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            targetCamera.orthographicSize = Mathf.Lerp(startSize, targetOrthographicSize, smoothT);

            yield return null;
        }

        // Ensure it lands on the exact target size at the end
        targetCamera.orthographicSize = targetOrthographicSize;
    }
    void HandleInput()
    {
        if(Keyboard.current != null){
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (frame == 0)
                {
                    positionGoal = position2;
                    frame = 1;
                }
                else if (frame == 1)
                {
                    positionGoal = position3;
                    frame = 2;
                }
                else if (frame == 2)
                {
                    positionGoal = position4;
                    frame = 3;
                }
                else if (frame == 3)
                {
                    TriggerZoomOut();
                    positionGoal = position5;
                    frame = 4;
                    
                }
            }
        }
    }
}
