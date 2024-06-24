using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Creator : MonoBehaviour
{
    public int width, height;
    public int minRoomWdt, minRoomHgt;
    public int maxIterations;
    public int corridorWidth;

    public Material material;

    [Range(0.0f, 0.3f)]
    public float roomBottomCornerMod;
    [Range(0.7f, 1.0f)]
    public float roomTopCornerMod;
    [Range(0, 2)]
    public int roomOffset;

    void Start()
    {
        Create();
    }

    void Create()
    {
        ProcGenerator gen = new ProcGenerator(width, height);
        var listOfRooms = gen.Calculate(maxIterations, 
            minRoomWdt, 
            minRoomHgt, 
            roomBottomCornerMod, 
            roomTopCornerMod, 
            roomOffset);

        foreach (var item in listOfRooms)
        {
            CreateMesh(item.BottomLeftCorner, item.TopRightCorner);
        }

    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 botttomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            botttomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
            {
                0,1,2,2,1,3
            };

        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject floor = new GameObject("TEST_FLOOR: " + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        floor.transform.position = Vector3.zero;
        floor.transform.localScale = Vector3.one;
        floor.GetComponent<MeshFilter>().mesh = mesh;
        floor.GetComponent<MeshRenderer>().material = material;

    }
}
