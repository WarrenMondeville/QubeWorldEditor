using UnityEngine;
using System.Collections.Generic;

//[RequireComponent(typeof(MeshFilter)RequireComponent(typeof(MeshRenderer))]
public class DynamicCubeMesh : MonoBehaviour
{
    public bool[] corners = new bool[8]; // 8���ǵĴ���״̬��Ĭ��ȫfalse��
    public Vector3[] baseVertices;
    void Start()
    {

        // ������8���ǵ��������꣨����ڵ�ǰ�����λ�ã�
        baseVertices = new Vector3[8]
        {
            transform.TransformPoint(new Vector3(-0.5f, -0.5f, -0.5f)), // 0 ��ǰ��
            transform.TransformPoint(new Vector3( 0.5f, -0.5f, -0.5f)), // 1 ��ǰ��
            transform.TransformPoint(new Vector3( 0.5f,  0.5f, -0.5f)), // 2 ��ǰ��
            transform.TransformPoint(new Vector3(-0.5f,  0.5f, -0.5f)), // 3 ��ǰ��
            transform.TransformPoint(new Vector3(-0.5f, -0.5f,  0.5f)), // 4 �����
            transform.TransformPoint(new Vector3( 0.5f, -0.5f,  0.5f)), // 5 �Һ���
            transform.TransformPoint(new Vector3( 0.5f,  0.5f,  0.5f)), // 6 �Һ���
            transform.TransformPoint(new Vector3(-0.5f,  0.5f,  0.5f))  // 7 �����
        };

        GenerateMesh();
    }

    public void SetCorners(byte b)
    {
        for (int i = 0; i < 8; i++)
        {
            // ���� (7-i) λ��MSB ���ȣ�
            corners[i] = (b & (1 << (7 - i))) != 0;
        }
    }
    public void SetCorners(bool[] b)
    {
        corners = b;
    }

    // ����corners״̬��������
    [ContextMenu("Generate Cub")]
    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;



        // �ռ����ڵĶ���
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();

        // �������6���棨ÿ����4�����㣬��˳ʱ��˳��
        int[,] faces = new int[6, 4]
        {
            {0, 1, 2, 3}, // ǰ��
            {1, 5, 6, 2}, // ����
            {5, 4, 7, 6}, // ����
            {4, 0, 3, 7}, // ����
            {3, 2, 6, 7}, // ����
            {4, 5, 1, 0}  // ����
        };

        // ����ÿ����
        for (int face = 0; face < 6; face++)
        {
            List<int> faceVerts = new List<int>();

            // �ռ���ǰ����ڵĶ���
            for (int i = 0; i < 4; i++)
            {
                int cornerIndex = faces[face, i];
                if (corners[cornerIndex])
                {
                    faceVerts.Add(cornerIndex);
                }
            }

            // �Դ��ڵĶ�����������ʷ�
            if (faceVerts.Count >= 3)
            {
                // ʹ�����������ʷ�
                for (int i = 1; i < faceVerts.Count - 1; i++)
                {
                    vertices.Add(baseVertices[faceVerts[0]]);
                    vertices.Add(baseVertices[faceVerts[i]]);
                    vertices.Add(baseVertices[faceVerts[i + 1]]);

                    var o = baseVertices[faceVerts[0]];
                    var a = baseVertices[faceVerts[i]];
                    var b = baseVertices[faceVerts[i + 1]];

                    triangles.Add(vertices.IndexOf(o));

                    triangles.Add(vertices.IndexOf(b));
                    triangles.Add(vertices.IndexOf(a));

                    {
                        Vector3 normal = -Vector3.Cross(
                            a - o,
                            b - o
                        ).normalized;
                        normals.Add(normal);
                        normals.Add(normal);
                        normals.Add(normal);
                    }


                }
            }
        }

        // ������������
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        //mesh.RecalculateNormals();
        mesh.normals = normals.ToArray();
        mesh.RecalculateBounds();
    }

    // �ڱ༭���ж�corners������ӻ��༭

    private void OnDrawGizmos()
    {

        for (int i = 0; i < 8; i++)
        {
            if (corners[i])
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.blue;
            }
            Gizmos.DrawCube(baseVertices[i], Vector3.one * .1f);
        }

    }

}