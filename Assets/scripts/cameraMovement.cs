using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    [Header("parenting system")]
    public Transform target;
    Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }
    private void Update()
    {

        transform.position = target.position + offset;
    }
}
