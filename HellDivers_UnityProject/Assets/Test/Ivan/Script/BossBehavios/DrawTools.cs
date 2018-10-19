using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTools {


    public static GameObject GO;
    public static MeshFilter mf;
    public static MeshRenderer mr;
    public static Material material;
    public static Shader shader;

    public static GameObject CreateMesh(List<Vector3> vertices)
    {
        int[] triangles;
        Mesh mesh = new Mesh();
        
        int triangleAmount = (vertices.Count - 2);
        triangles = new int[3 * triangleAmount];
        for (int i = 0; i < triangleAmount; i++)
        {
            triangles[3 * i] = 0;
            triangles[3 * i + 1] = i + 1;
            triangles[3 * i + 2] = i + 2;
        }
        if (GO == null)
        {
            GO = new GameObject("mesh");
            mf = GO.AddComponent<MeshFilter>();
            mr = GO.AddComponent<MeshRenderer>();
            shader = Shader.Find("Unlit/Color");
        }
        else
        {
            mf = GO.GetComponent<MeshFilter>();
            mr = GO.GetComponent<MeshRenderer>();
            shader = mr.material.shader;
        }
        GO.transform.position = new Vector3(0, 0.1f, 0);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;

        mf.mesh = mesh;
        mr.material.shader = shader ;
        mr.material.color = Color.red;

        return GO;
    }

    public static void DrawCircleSolid(Transform t, Vector3 center, float radius)
    {
        int pointAmount = 100;
        float eachAngle = 360f / pointAmount;
        Vector3 vForward = t.forward;

        List<Vector3> vertices = new List<Vector3>();

        for (int i = 0; i < pointAmount; i++)
        {
            Vector3 pos = Quaternion.Euler(0, eachAngle * i, 0) * vForward * radius + center;
            vertices.Add(pos);
        }
        CreateMesh(vertices);
    }

    public static void DrawRectangleSolid(Transform t, Vector3 bottomMiddle, float length, float width)
    {
        List<Vector3> vertices = new List<Vector3>
        {
            bottomMiddle - t.right * (width * 0.5f),
            bottomMiddle - t.right * (width * 0.5f) + t.forward * length,
            bottomMiddle + t.right * (width * 0.5f) + t.forward * length,
            bottomMiddle + t.right * (width * 0.5f)
        };

        CreateMesh(vertices);
    }

}
