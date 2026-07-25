using Unity.Cinemachine;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class mechLogic : MonoBehaviour
{
    [Header("movement")]
    public float movementSpeed;

    public float turnSpeed;

    [Header("setting on ground")]
    public float distanceFromGround;
    float groundDistance;

    public Transform planet;
    public LayerMask groundMask;
    public Vector3 size;

    [Header("attack animations")]
    public float timeToAttack;
    public float timeToRecover;
    float timer;
    bool inAction;

    public AnimationCurve animation;

    [Header("attack logic")]
    public float attackRadius;
    public LayerMask humanMask;
    public DecalProjector humanDecal;

    private void Start()
    {
        groundDistance = distanceFromGround;
    }
    private void Update()
    {
        Vector3 dir = (transform.position - planet.position).normalized;

        Vector3 origin = transform.position + transform.up * 30;
        if (Physics.BoxCast(origin, size, -transform.up, out RaycastHit hit, transform.rotation, 999, groundMask))
        {
            float currentDistance = hit.distance - 30;
            float correction = currentDistance - groundDistance;
            transform.position += -transform.up * correction;
        }

        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        transform.rotation = Quaternion.FromToRotation(transform.up, dir) * transform.rotation;
        transform.rotation = Quaternion.AngleAxis(inputDir.x * turnSpeed * Time.deltaTime, dir) * transform.rotation;

        transform.position += inputDir.y * transform.forward * movementSpeed * Time.deltaTime;

        if (Input.GetButton("Fire"))
        {
            inAction = true;
        }

        if (inAction == true)
        {
            timer += Time.deltaTime;
            if (timer < timeToAttack)
            {
                StartCoroutine(FEEL.CameraShake(1, timeToAttack, Vector3.one));
                Collider[] humansAttacked = Physics.OverlapSphere(transform.position, attackRadius, humanMask);
                foreach (Collider human in humansAttacked)
                {
                    Quaternion decalRot = Quaternion.LookRotation((planet.position - human.transform.position).normalized, human.transform.forward);

                    PoolManager.SpawnObject(humanDecal, human.transform.position, decalRot);
                    PoolManager.ReturnToPool(human.gameObject);
                }
            }
            groundDistance = Mathf.LerpUnclamped(0, distanceFromGround, animation.Evaluate(timer / (timeToAttack + timeToRecover)));

            if (timer > timeToAttack + timeToRecover)
            {
                groundDistance = distanceFromGround;
                timer = 0;
                inAction = false;
            }
        }
    }
}
