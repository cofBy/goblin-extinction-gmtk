using UnityEngine;

public class humanSpawner : MonoBehaviour
{
    [Header("points on a sphere")]
    public float radius;
    public float amount;

    [Header("spawning")]
    public humanLogic humanPrefab;
    public Transform planet;

    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = polarCoords(Random.Range(-Mathf.PI, Mathf.PI), Random.Range(-Mathf.PI, Mathf.PI), radius);
            humanLogic human = Instantiate(humanPrefab, pos, Quaternion.identity);

            human.planet = planet;
        }
    }
    Vector3 polarCoords(float pitch, float yaw, float r)
    {
        float x = Mathf.Sin(pitch) * Mathf.Cos(yaw);
        float y = Mathf.Sin(pitch) * Mathf.Sin(yaw);
        float z = Mathf.Cos(pitch);

        return new Vector3(x, y, z) * r;
    }
}
