using UnityEngine;

public class GlobalKiller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rat") || other.CompareTag("Human") || other.CompareTag("Car"))
        {
            Destroy(other.gameObject);
        }
        Debug.Log("Blow up Object");
    }
}
