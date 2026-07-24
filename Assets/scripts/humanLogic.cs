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

    [Header("animations")]
    public float swayAmount;
    public float maxBounce;
    public float animationSpeed;
    float timer;

    public Transform model;

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, -transform.up, groundRayLength, groundMask);

        timer += Time.deltaTime * animationSpeed;
        model.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(timer * 2) * swayAmount);
        model.localPosition = Vector3.up * Mathf.Sin(timer) * maxBounce;
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
