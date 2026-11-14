using UnityEngine;

public class PersonBehavior : MonoBehaviour
{
    Vector3 startPos;
    Vector3 targetPos = Vector3.zero;
    float moveRange = 3f;
    float moveSpeed = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        targetPos = GetRandomPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, targetPos) < moveSpeed * Time.deltaTime)
        {
            targetPos = GetRandomPos();
        } else
        {
            Vector3 difference = targetPos - transform.position;
            difference.Normalize();
            transform.position += difference * moveSpeed * Time.deltaTime;
        }
    }

    Vector3 GetRandomPos()
    {
        float x = Random.Range(startPos.x - moveRange, startPos.x + moveRange);
        float y = Random.Range(startPos.y - moveRange, startPos.y + moveRange);
        return new Vector3(x, y, 0);
    }
}
