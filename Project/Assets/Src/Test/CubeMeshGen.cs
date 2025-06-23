using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeMeshGen : MonoBehaviour
{
    public List<Transform> trans = new List<Transform>();
    public Vector3[] corners = new Vector3[8]; // 8���ǵ���������
    public bool[] cornerExists = new bool[8];   // �ǵĴ���״̬
    private MeshFilter meshFilter;
    private Mesh mesh;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        CalculateCorners(5f); // �ߴ�Ϊ1
    }

    [ContextMenu("Generate Cube Mesh")]
    void GenCube()
    {

        mesh = new Mesh();
        meshFilter.mesh = mesh;
        GenerateMesh();
    }

    private void OnDrawGizmos()
    {

        for (int i = 0; i < 8; i++)
        {
            if (cornerExists[i])
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.blue;
            }
            Gizmos.DrawSphere(corners[i], 1f);
        }

    }

    void CalculateCorners(float size)
    {
        float halfSize = size / 2f;
        corners = new Vector3[] {
        new Vector3(-halfSize, -halfSize, -halfSize), // 0: ��ǰ��
        new Vector3(halfSize, -halfSize, -halfSize),  // 1: ��ǰ��
        new Vector3(halfSize, -halfSize, halfSize),   // 2: �Һ���
        new Vector3(-halfSize, -halfSize, halfSize),  // 3: �����
        new Vector3(-halfSize, halfSize, -halfSize),  // 4: ��ǰ��
        new Vector3(halfSize, halfSize, -halfSize),   // 5: ��ǰ��
        new Vector3(halfSize, halfSize, halfSize),    // 6: �Һ���
        new Vector3(-halfSize, halfSize, halfSize)    // 7: �����
    };
    }

    List<int> GenerateTriangles()
    {
        List<int> triangles = new List<int>();
        int[][] faceQuads = new int[][] {
    new int[]{0,1,2,3}, // ���棨�跴ת���ߣ�
    new int[]{4,5,6,7}, // ����
    new int[]{0,4,5,1},  // ǰ��
    new int[]{1,5,6,2},  // ����
    new int[]{2,6,7,3},  // ����
    new int[]{3,7,4,0}   // ����
};

        foreach (var quad in faceQuads)
        {
            if (cornerExists[quad[0]] && cornerExists[quad[1]] &&
                cornerExists[quad[2]] && cornerExists[quad[3]])
            {
                // ������������Σ�˳ʱ����ƣ�
                triangles.Add(quad[0]); triangles.Add(quad[1]); triangles.Add(quad[2]);
                triangles.Add(quad[0]); triangles.Add(quad[2]); triangles.Add(quad[3]);
            }
        }
        return triangles;
    }

    void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = corners; // 24����
        List<int> triangles = GenerateTriangles(); // ��̬������

        // ���߼���
        Vector3[] normals = new Vector3[vertices.Length];
        for (int i = 0; i < triangles.Count; i += 3)
        {
            Vector3 normal = Vector3.Cross(
                vertices[triangles[i + 1]] - vertices[triangles[i]],
                vertices[triangles[i + 2]] - vertices[triangles[i]]
            ).normalized;
            normals[triangles[i]] = normals[triangles[i + 1]] = normals[triangles[i + 2]] = normal;
        }

        // Ӧ������
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals;
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;
    }

}
