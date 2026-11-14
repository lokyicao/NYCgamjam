using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Following")]
    [SerializeField, Min(0f)] private float followSpeed = 8f;
    [SerializeField] private float cameraZ = -10f;

    [Header("Turning")]
    [SerializeField, Min(0f)] private float rotationSpeed = 160f;

    private float targetAngle;
    private float startingAngle;

    private void Start()
    {
        startingAngle = transform.eulerAngles.z;
        targetAngle = startingAngle;

        if (player == null)
        {
            Debug.LogWarning("PlayerCamera: assign the player Transform in the Inspector.", this);
            return;
        }

        ResetCamera();
    }

    private void LateUpdate()
    {
        if (player == null) return;

        FollowPlayer();
        RotateCamera();
    }

    private void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(
            player.position.x, player.position.y, cameraZ);

    
        float blend = 1f - Mathf.Exp(-followSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPosition, blend);
    }

    private void RotateCamera()
    {
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void SetStreetDirection(float angle)
    {
        targetAngle = Mathf.Repeat(Mathf.Round(angle / 90f) * 90f, 360f);
    }

    public void TurnLeft()
    {
        SetStreetDirection(targetAngle + 90f);
    }

    public void TurnRight()
    {
        SetStreetDirection(targetAngle - 90f);
    }

    public void ResetCamera()
    {
        targetAngle = startingAngle;

        if (player == null) return;

        transform.position = new Vector3(
            player.position.x, player.position.y, cameraZ);
        transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
    }
}
