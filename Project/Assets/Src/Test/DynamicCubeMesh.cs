using UnityEngine;
using System.Collections.Generic;

//[RequireComponent(typeof(MeshFilter)RequireComponent(typeof(MeshRenderer))]
public class DynamicCubeMesh : MonoBehaviour
{
    public bool[] corners = new bool[8]; // 8个角的存在状态（默认全false）
    public Vector3[] baseVertices;
    void Start()
    {

        // 立方体8个角的世界坐标（相对于当前物体的位置）
        baseVertices = new Vector3[8]
        {
            transform.TransformPoint(new Vector3(-0.5f, -0.5f, -0.5f)), // 0 左前下
            transform.TransformPoint(new Vector3( 0.5f, -0.5f, -0.5f)), // 1 右前下
            transform.TransformPoint(new Vector3( 0.5f,  0.5f, -0.5f)), // 2 右前上
            transform.TransformPoint(new Vector3(-0.5f,  0.5f, -0.5f)), // 3 左前上
            transform.TransformPoint(new Vector3(-0.5f, -0.5f,  0.5f)), // 4 左后下
            transform.TransformPoint(new Vector3( 0.5f, -0.5f,  0.5f)), // 5 右后下
            transform.TransformPoint(new Vector3( 0.5f,  0.5f,  0.5f)), // 6 右后上
            transform.TransformPoint(new Vector3(-0.5f,  0.5f,  0.5f))  // 7 左后上
        };

        GenerateMesh();
    }

    public void SetCorners(byte b)
    {
        for (int i = 0; i < 8; i++)
        {
            // 检查第 (7-i) 位（MSB 优先）
            corners[i] = (b & (1 << (7 - i))) != 0;
        }
    }
    public void SetCorners(bool[] b)
    {
        corners = b;
    }

    // 根据corners状态生成网格
    [ContextMenu("Generate Cub")]
    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;



        // 收集存在的顶点
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();

        // 立方体的6个面（每个面4个顶点，按顺时针顺序）
        int[,] faces = new int[6, 4]
        {
            {0, 1, 2, 3}, // 前面
            {1, 5, 6, 2}, // 右面
            {5, 4, 7, 6}, // 后面
            {4, 0, 3, 7}, // 左面
            {3, 2, 6, 7}, // 上面
            {4, 5, 1, 0}  // 下面
        };

        // 处理每个面
        for (int face = 0; face < 6; face++)
        {
            List<int> faceVerts = new List<int>();

            // 收集当前面存在的顶点
            for (int i = 0; i < 4; i++)
            {
                int cornerIndex = faces[face, i];
                if (corners[cornerIndex])
                {
                    faceVerts.Add(cornerIndex);
                }
            }

            // 对存在的顶点进行三角剖分
            if (faceVerts.Count >= 3)
            {
                // 使用三角形扇剖分
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

        // 设置网格数据
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        //mesh.RecalculateNormals();
        mesh.normals = normals.ToArray();
        mesh.RecalculateBounds();
    }

    // 在编辑器中对corners数组可视化编辑

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