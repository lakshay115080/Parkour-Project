using UnityEngine;

public class ParkourObstacles : MonoBehaviour
{
    protected GameObject playerInField;
    protected bool canParkour;
    protected PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    void Update()
    {
        if (canParkour && playerInField != null && Input.GetKeyDown(KeyCode.Space))
        {
            Parkour();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canParkour = true;
            playerInField = other.gameObject;
            playerMovement.canJump = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canParkour = false;
            playerInField = null;
            playerMovement.canJump = true;
        }
    }

    protected virtual void Parkour() { }
}
