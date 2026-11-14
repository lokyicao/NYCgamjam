using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject objectToSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (objectToSpawn != null & Keyboard.current.fKey.wasPressedThisFrame)
        {
            Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        } 
    }
}
