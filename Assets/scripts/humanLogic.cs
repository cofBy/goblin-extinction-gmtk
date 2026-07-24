using UnityEngine;

public class humanLogic : MonoBehaviour
{
    public Rigidbody rb;

    [Header("balance")]
    public Transform planet;
    public float alignSpeed;

    [Header("movement")]
    public float movementSpeed;

    [Header("checking grounded")]
    public float groundRayLength;
    public LayerMask groundMask;
    bool grounded;

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, -transform.up, groundRayLength, groundMask);
    }

    private void FixedUpdate()
    {
        alignToPlanet();

        if (grounded == false) return;
        Vector3 up = transform.up;
        Vector3 verticalVel = Vector3.Project(rb.linearVelocity, up);
        rb.linearVelocity = transform.forward * movementSpeed + verticalVel;
    }

    void alignToPlanet()
    {
        Vector3 gravityUp = (transform.position - planet.position).normalized;
        Quaternion targetRot = Quaternion.FromToRotation(transform.up, gravityUp) * rb.rotation;

        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, alignSpeed * Time.fixedDeltaTime));
    }
}
