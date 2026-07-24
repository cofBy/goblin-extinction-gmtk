using UnityEngine;

public class planetGravity : MonoBehaviour
{
    [Header("getting object")]
    Rigidbody[] rbs;

    [Header("gravity Settings")]
    public float gravityStrength;

    private void Awake()
    {
        rbs = FindObjectsByType<Rigidbody>();
    }
    private void Update()
    {
        foreach (Rigidbody rb in rbs)
        {
            Vector3 dir = transform.position - rb.transform.position;
            float f = gravityStrength * rb.mass / dir.magnitude;

            rb.AddForce(dir.normalized * f);
        }
    }
}
