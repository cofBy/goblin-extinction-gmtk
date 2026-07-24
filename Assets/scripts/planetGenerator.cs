using System.Collections.Generic;
using UnityEngine;

public class planetGenerator : MonoBehaviour
{
    [Header("making cube")]
    public float size;
    public int subDivisions;

    [Header("random generation")]
    public float noiseScale;
    public float noiseStrength;
    [Range(0, 1)] public float groundPercentage;
    int seed;

    [Header("collider")]
    public MeshCollider col;

    private void Start()
    {
        seed = Random.Range(0, 99);
        makeCube();
    }
    void makeCube()
    {
        MeshFilter filter = GetComponent<MeshFilter>();

        Mesh mesh = new Mesh();
        mesh.name = "planet";

        int res = subDivisions + 2;
        int surfaceVertices = (int)Mathf.Pow(res, 3) - (int)Mathf.Pow(subDivisions, 3);

        Vector3[] vertices = new Vector3[surfaceVertices];
        Vector2[] uv = new Vector2[surfaceVertices];
        Vector2[] noiseUV = new Vector2[surfaceVertices];

        var indexLookup = new Dictionary<int, int>();

        float half = ((float)subDivisions + 1) * 0.5f;
        int index = 0;
        for (int x = 0; x < res; x++)
        {
            for (int y = 0; y < res; y++)
            {
                for (int z = 0; z < res; z++)
                {
                    bool isSurface = (x == 0 || x == subDivisions + 1 ||
                                      y == 0 || y == subDivisions + 1 ||
                                      z == 0 || z == subDivisions + 1);

                    if (!isSurface) continue;

                    float u = (float)x / (subDivisions + 1);
                    float v = (float)y / (subDivisions + 1);
                    float w = (float)z / (subDivisions + 1);
                    if (x == 0 || x == subDivisions + 1)
                    {
                        uv[index] = new Vector2(w, v);
                    }
                    else if (y == 0 || y == subDivisions + 1)
                    {
                        uv[index] = new Vector2(w, u);
                    }
                    else if (z == 0 || z == subDivisions + 1)
                    {
                        uv[index] = new Vector2(u, v);
                    }

                    indexLookup[x + y * res + z * res * res] = index;

                    Vector3 pos = new Vector3((x - half) * size / (subDivisions + 1), (y - half) * size / (subDivisions + 1), (z - half) * size / (subDivisions + 1));

                    float noiseValue = Mathf.Clamp01(Mathf.PerlinNoise(uv[index].x * noiseScale + seed, uv[index].y * noiseScale + seed) + groundPercentage - 0.5f);
                    vertices[index] = pos.normalized * (size + (noiseValue - 0.5f) * noiseStrength);
                    noiseUV[index] = new Vector2(noiseValue, 0);

                    index++;
                }
            }
        }

        int GetIndex(int gx, int gy, int gz) => indexLookup[gx + gy * res + gz * res * res];

        var triList = new List<int>();

        void AddQuad(int a, int b, int c, int d)
        {
            triList.Add(a);
            triList.Add(b);
            triList.Add(c);
            triList.Add(a);
            triList.Add(c);
            triList.Add(d);
        }

        int last = subDivisions + 1;

        for (int y = 0; y < last; y++)
        {
            for (int z = 0; z < last; z++)
            {
                AddQuad(GetIndex(0, y, z), GetIndex(0, y, z + 1), GetIndex(0, y + 1, z + 1), GetIndex(0, y + 1, z));
                AddQuad(GetIndex(last, y, z), GetIndex(last, y + 1, z), GetIndex(last, y + 1, z + 1), GetIndex(last, y, z + 1));
            }
        }
        for (int x = 0; x < last; x++)
        {
            for (int z = 0; z < last; z++)
            {
                AddQuad(GetIndex(x, 0, z), GetIndex(x + 1, 0, z), GetIndex(x + 1, 0, z + 1), GetIndex(x, 0, z + 1));
                AddQuad(GetIndex(x, last, z), GetIndex(x, last, z + 1), GetIndex(x + 1, last, z + 1), GetIndex(x + 1, last, z));
            }
        }
        for (int x = 0; x < last; x++)
        {
            for (int y = 0; y < last; y++)
            {
                AddQuad(GetIndex(x, y, 0), GetIndex(x, y + 1, 0), GetIndex(x + 1, y + 1, 0), GetIndex(x + 1, y, 0));
                AddQuad(GetIndex(x, y, last), GetIndex(x + 1, y, last), GetIndex(x + 1, y + 1, last), GetIndex(x, y + 1, last));
            }
        }

        int[] triangles = triList.ToArray();

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.uv2 = noiseUV;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        filter.mesh = mesh;
        if (col != null)
        {
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }
    }

}
