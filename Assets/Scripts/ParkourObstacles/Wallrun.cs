using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Wallrun : ParkourObstacles
{
    [SerializeField] BoxCollider[] triggers;
    [SerializeField] float wallRunSpeed = 10f;
    bool isWallrunning;
    int currentTrigger;

    void Update()
    {
        if (canParkour && playerInField != null)
        {
            for (int i = 0; i < triggers.Length; i++)
            {
                if (triggers[i].bounds.Contains(playerInField.transform.position))
                {
                    currentTrigger = i;
                    break;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Parkour();
            }
        }
    }

    protected override void Parkour()
    {
        if (isWallrunning) return;

        if (currentTrigger > triggers.Length) return;

        StartCoroutine(WallRun());
    }

    IEnumerator WallRun()
    {
        isWallrunning = true;
        playerMovement.enabled = false;
        Rigidbody rb = playerInField.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        Vector3 target;
        if (currentTrigger > 0)
        {
            target = triggers[0].bounds.center;
        }
        else
        {
            target = triggers[currentTrigger + 1].bounds.center;
        }
        target.y = rb.position.y;

        while (Vector3.Distance(rb.position, target) > 0.5f)
        {
            Vector3 direction = (target - rb.position).normalized;
            rb.MovePosition(rb.position + direction * wallRunSpeed * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }
        rb.useGravity = true;
        rb.isKinematic = false;
        playerMovement.enabled = true;
        isWallrunning = false;

    }
}
