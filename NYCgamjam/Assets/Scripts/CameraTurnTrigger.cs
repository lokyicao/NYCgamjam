using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraTurnTrigger : MonoBehaviour
{
    [SerializeField] private PlayerCamera playerCamera;

    [Tooltip("Absolute camera Z rotation: 0, 90, 180, or 270.")]
    [SerializeField] private float streetAngle;

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        PlayerBehavior player = other.GetComponentInParent<PlayerBehavior>();

        if (player == null || playerCamera == null) return;

        playerCamera.SetStreetDirection(streetAngle);
    }
}