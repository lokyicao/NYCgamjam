using UnityEngine;

public class ShowTextOnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject textObject;

    private void Start()
    {
        // Hide the text when the game starts
        if (textObject != null)
            textObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            textObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            textObject.SetActive(false);
    }

}
