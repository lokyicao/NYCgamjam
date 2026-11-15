using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class comicmovement : MonoBehaviour
{
    int moveSpeed = 5;
    int frame = 0;
    Vector3 position1, position2, position3, position4;

    Vector3 positionGoal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        position1 = new Vector3(8.69f, -4.84f, 0f);
        position2 = new Vector3(-8.71f, -4.84f, 0f);
        position3 = new Vector3(8.69f, 4.86f, 0f);
        position4 = new Vector3(-8.71f, 4.86f, 0f);
        positionGoal = position1;
        transform.position = positionGoal;


    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        transform.position = Vector3.MoveTowards(transform.position, positionGoal, moveSpeed * Time.deltaTime);
        
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

            }
        }
    }
}
