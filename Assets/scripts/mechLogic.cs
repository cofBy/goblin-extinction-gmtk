using UnityEngine;

public class mechLogic : MonoBehaviour
{
    [Header("movement")]
    public float movementSpeed;

    public float turnSpeed;

    [Header("setting on ground")]
    public float distanceFromGround;
    public Transform planet;
    public LayerMask groundMask;
    public Vector3 size;

    private void Update()
    {
        Vector3 dir = (transform.position - planet.position).normalized;

        Vector3 origin = transform.position + transform.up * 30;
        if (Physics.BoxCast(origin, size, -transform.up, out RaycastHit hit, transform.rotation, 999, groundMask))
        {
            float currentDistance = hit.distance - 30;
            float correction = currentDistance - distanceFromGround;
            transform.position += -transform.up * correction;
        }

        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        transform.rotation = Quaternion.FromToRotation(transform.up, dir) * transform.rotation;
        transform.rotation = Quaternion.AngleAxis(inputDir.x * turnSpeed * Time.deltaTime, dir) * transform.rotation;

        transform.position += inputDir.y * transform.forward * movementSpeed * Time.deltaTime;
    }
}
