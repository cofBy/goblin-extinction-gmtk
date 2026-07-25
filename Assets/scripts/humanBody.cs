using UnityEngine;
using UnityEngine.Rendering.Universal;

public class humanBody : MonoBehaviour
{
    [Header("fading")]
    public DecalProjector decal;
    public float timeBeforeFade;
    public float fadingSpeed;
    float timer;

    private void OnEnable()
    {
        decal.fadeFactor = 1;
        timer = 0;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBeforeFade)
        {
            if (decal.fadeFactor > 0)
            {
                decal.fadeFactor -= fadingSpeed * Time.deltaTime;
            }
            else
            {
                PoolManager.ReturnToPool(gameObject);
            }
        }
    }
}
