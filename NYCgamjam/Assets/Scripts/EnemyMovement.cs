using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    Vector3 direction = Vector3.down;
    Collider2D collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collider.CompareTag("Human") && other.CompareTag("Car"))
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 direction)
    {
        direction.Normalize();
        this.direction = direction;
    }
}
