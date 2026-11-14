using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    float moveSpeed = 1f;
    Vector3 direction = Vector3.down;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void SetDirection(Vector3 direction)
    {
        direction.Normalize();
        this.direction = direction;
        Debug.Log(direction);
    }
}
