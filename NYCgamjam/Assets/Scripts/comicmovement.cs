using UnityEngine;
using UnityEngine.InputSystem;

public class comicmovement : MonoBehaviour
{
    int moveSpeed = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;

    }
    void HandleInput()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {

        }
    }
}
