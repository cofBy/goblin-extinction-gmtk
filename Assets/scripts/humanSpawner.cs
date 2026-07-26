using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class humanSpawner : MonoBehaviour
{
    [Header("points on a sphere")]
    public float radius;

    [Header("spawning")]
    public humanLogic humanPrefab;
    public Transform planet;
    public Transform parent;
    public Transform mech;

    [Header("counting humans")]
    public TextMeshProUGUI humansCount;
    List<humanLogic> humanInstances = new List<humanLogic>();
    int count;

    [Header("winning")]
    public GameObject winningScreen;
    public cursesSystem curses;

    private void Start()
    {
        spawn(50);
    }
    public void spawn(int amount)
    {
        if (winningScreen != null) winningScreen.SetActive(false);

        humanInstances.Clear();

        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = polarCoords(Random.Range(-Mathf.PI, Mathf.PI), Random.Range(-Mathf.PI, Mathf.PI), radius);
            humanLogic human = PoolManager.SpawnObject(humanPrefab, pos, Quaternion.identity);
            humanInstances.Add(human);

            human.planet = planet;
            if (mech != null) human.mech = mech;
            if (curses != null) human.curses = curses;
        }
    }
    private void Update()
    {
        if (humansCount == null) return;
        count = 0;
        for (int i = 0; i < humanInstances.Count; i++)
        {
            if (humanInstances[i].gameObject.activeSelf)
            {
                count += 1;
            }
        }
        humansCount.text = count.ToString();
        count = Mathf.Max(count, 0);

        if (count <= 0)
        {
            if (winningScreen.activeSelf == false)
            {
                winningScreen.SetActive(true);
                curses.win();
            }
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
