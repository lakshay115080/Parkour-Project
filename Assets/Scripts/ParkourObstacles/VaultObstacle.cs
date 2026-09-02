using System.Collections;
using UnityEngine;

public class VaultObstacle : ParkourObstacles
{
    [SerializeField] float vaultDistance = 2f;
    [SerializeField] float vaultDuration = 0.5f;
    Animator animator;
    float vaultHeight;
    bool isVaulting;

    void Awake()
    {
        vaultHeight = gameObject.transform.localScale.y + 0.5f;
    }
    protected override void Parkour()
    {
        if (isVaulting) return;

        StartCoroutine(Vault());
    }

    IEnumerator Vault()
    {
        isVaulting = true;
        playerMovement.enabled = false;
        Rigidbody rb = playerInField.GetComponent<Rigidbody>();
        animator = playerInField.GetComponentInChildren<Animator>();
        animator.SetTrigger("Vault");
        rb.isKinematic = true;
        Vector3 start = rb.position;
        Vector3 direction = (rb.position - transform.position).normalized;
        direction.y = 0f;
        Vector3 end = start - direction * vaultDistance;
        float time = 0;

        while (time < vaultDuration)
        {
            time += Time.fixedDeltaTime;
            float t = time / vaultDuration;

            t = Mathf.SmoothStep(0, 1, t);
            Vector3 position = Vector3.Lerp(start, end, t);
            position.y += Mathf.Sin(t * Mathf.PI) * vaultHeight;
            rb.MovePosition(position);
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(end);
        playerMovement.enabled = true;
        rb.isKinematic = false;
        isVaulting = false;
    }
}
