using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform teleportLocation; // Assign this in the inspector for each door's specific teleport spot
    private Transform playerTransform;
    private bool playerInRange = false;
    public SpriteRenderer sprite, spriteE;

    void Start()
    {
        playerTransform = PlayerControl.Instance.transform;
        spriteE.enabled = false;
    }

    void Update()
    {
        playerInRange = Vector2.Distance(playerTransform.position, transform.position) < 1f;
        var distance123 = Vector2.Distance(playerTransform.position, transform.position);
        // Check if the player presses 'F' while in range of the door
        spriteE.enabled = playerInRange;
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (teleportLocation != null)
            {
                PlayerControl.Instance.transform.position = teleportLocation.position;
                
            }
            else
            {
                Debug.LogWarning($"Get CLoser to Door{distance123}");
                

            }

        } 

    }
}
