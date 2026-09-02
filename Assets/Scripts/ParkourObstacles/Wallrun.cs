using System.Collections;
using UnityEngine;

public class Wallrun : ParkourObstacles
{
    [SerializeField] float wallRunDuration = 0.5f;
    [SerializeField] float wallRunSpeed = 10f;
    float wallLength;
    bool isWallrunning;

    void Awake()
    {
        wallLength = gameObject.transform.localScale.z;
    }
    protected override void Parkour()
    {
        if (isWallrunning) return;

        Vector3 playerForward = playerInField.transform.forward;
        playerForward.y = 0f;
        playerForward.Normalize();

        float dot = Vector3.Dot(playerForward, transform.forward);

        if (Mathf.Abs(dot) < 0.7f)
            return;

        Vector3 wallDirection = transform.forward;

        if (dot < 0f)
            wallDirection = -transform.forward;

        StartCoroutine(WallRun(wallDirection));
    }

    IEnumerator WallRun(Vector3 wallDirection)
    {
        isWallrunning = true;
        playerMovement.enabled = false;
        Rigidbody rb = playerInField.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        float time = 0;

        while (time < wallRunDuration)
        {
            time += Time.fixedDeltaTime;

            Vector3 position = rb.position;
            position += wallDirection * wallRunSpeed * Time.fixedDeltaTime;

            rb.MovePosition(position);

            yield return new WaitForFixedUpdate();
        }
        rb.useGravity = true;
        rb.isKinematic = false;
        playerMovement.enabled = true;
        isWallrunning = false;

    }
}
