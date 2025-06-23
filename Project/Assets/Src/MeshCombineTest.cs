
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class MeshCombineTest : MonoBehaviour
{
    public Mesh[] meshs;
    public Material material;
    void Start()
    {
        foreach (var item in meshs)
        {
            LogMeshData(item);
        }
       

        //CreateMesh();
        //CreatSikaMesh();
    }

    private void LogMeshData(Mesh mesh)
    {
        var str = "vertices ";
        foreach (var item in mesh.vertices)
        {
            str += $"new Vector3({item.x + 0.5f}f,{item.y + 0.5f}f,{item.z + 0.5f}f),";
        }
        Debug.Log(str);
        str = "uv";

        foreach (var item in mesh.uv)
        {
            str += $"new Vector2({item.x}f,{item.y}f),";
        }
        Debug.Log(str);
        str = "triangles";
        foreach (var item in mesh.triangles)
        {
            str += $"{item},";
        }
        Debug.Log(str);
        str = "normals";
        foreach (var item in mesh.normals)
        {
            str += $"new Vector3({item.x}f,{item.y}f,{item.z}f),";
        }
        Debug.Log(str);
    }

    void CreatSikaMesh()
    {
        var name = "Sika";
        var go = new GameObject(name);
        var mesh = go.AddComponent<MeshFilter>();
        var mat = go.AddComponent<MeshRenderer>();
        mat.material = material;
        mesh.mesh = new Mesh();
        List<Vector3> vertices = new();
        List<Vector2> uvs = new(4);
        List<Vector3> normals = new();
        

        vertices.Add(CubeData.vertices[12]);
        vertices.Add(CubeData.vertices[13]);
        vertices.Add(CubeData.vertices[14]);
        vertices.Add(CubeData.vertices[15]);
        vertices.Add(CubeData.vertices[30]);
        List<int> triangles = new() { 
        0,1,2,
        0,2,3,

        4,1,0,
        4,2,1,
        4,3,2,
        4,0,3

        };

        // 1. 定义顶点索引（假设顶点顺序：0-3为底面，4为顶点）
        // 底面：0(0,0,0), 1(1,0,0), 2(1,0,1), 3(0,0,1)
        // 顶点：4(0.5,1,0.5)

        // 2. 底面UV：平铺到纹理左下1/4区域
        uvs[0] = new Vector2(0.0f, 0.0f); // 底面左下
        uvs[1] = new Vector2(0.5f, 0.0f); // 底面右下
        uvs[2] = new Vector2(0.5f, 0.5f); // 底面右上
        uvs[3] = new Vector2(0.0f, 0.5f); // 底面左上

        // 3. 侧面UV：对称排布在右上1/2区域
        // 侧面0（顶点0,1,4）
        uvs[4] = new Vector2(0.6f, 1.0f);  // 顶点→纹理顶部中心
        uvs[0] = new Vector2(0.5f, 0.5f);  // 边01中点→右侧中部
        uvs[1] = new Vector2(0.7f, 0.5f);

        // 侧面1（顶点1,2,4）- 旋转对称
        uvs[4] = new Vector2(0.6f, 1.0f); // 同一顶点
        uvs[1] = new Vector2(0.7f, 0.5f);
        uvs[2] = new Vector2(0.7f, 0.5f);


        mesh.mesh.vertices = vertices.ToArray();
        mesh.mesh.uv = uvs.ToArray();
        mesh.mesh.triangles = triangles.ToArray();
        //mesh.mesh.normals = normals.ToArray();

        mesh.mesh.RecalculateNormals();
        mesh.mesh.bounds = new Bounds(Vector3.zero, new Vector3(1f, 1f, 1f));
        CubeData.SaveMesh(mesh.mesh, name, true, true);
    }

    void CreateMesh()
    {


        /*        {
                    var go = new GameObject("Cube" + index);
                    var mesh = go.AddComponent<MeshFilter>();
                    var mat = go.AddComponent<MeshRenderer>();
                    mat.material = material;
                    mesh.mesh = new Mesh();
                    mesh.mesh.vertices = CubeData.vertices;
                    mesh.mesh.uv = CubeData.uv;
                    mesh.mesh.triangles = CubeData.triangles;
                    mesh.mesh.normals = CubeData.normals;
                }*/
        var index = 0;
        for (int i = 0; i < 12; i++)
        {
            var name = "Cube" + index;
            var go = new GameObject(name);
            var mesh = go.AddComponent<MeshFilter>();
            var mat = go.AddComponent<MeshRenderer>();
            mat.material = material;
            mesh.mesh = new Mesh();
            List<Vector3> vertices = new();
            List<Vector2> uv = new();
            List<Vector3> normals = new();
            List<int> triangles = new();
            int[] trs = new int[3];
            for (int j = 0; j < 3; j++)
            {

                var tr = CubeData.triangles[index];
                trs[j] = j;
                var contain = triangles.Contains(tr);
                triangles.Add(tr);
                if (contain) continue;
                vertices.Add(CubeData.vertices[tr]);

                uv.Add(CubeData.uv[tr]);

                normals.Add(CubeData.normals[tr]);
                index++;
            }
            mesh.mesh.vertices = vertices.ToArray();
            mesh.mesh.uv = uv.ToArray();
            mesh.mesh.triangles = trs;
            mesh.mesh.normals = normals.ToArray();
            mesh.mesh.bounds = new Bounds(Vector3.zero, new Vector3(1f, 1f, 1f));
            //CubeData.SaveMesh(mesh.mesh, name, true, true);
        }
        index = 0;
        for (int i = 0; i < 12; i++)
        {
            var name = "Cuber" + index;
            var go = new GameObject(name);
            var mesh = go.AddComponent<MeshFilter>();
            var mat = go.AddComponent<MeshRenderer>();
            mat.material = material;
            mesh.mesh = new Mesh();
            List<Vector3> vertices = new();
            List<Vector2> uv = new();
            List<Vector3> normals = new();
            List<int> triangles = new();
            int[] trs = new int[3];
            for (int j = 0; j < 3; j++)
            {

                var tr = CubeData.trianglesr[index];
                trs[j] = j;
                var contain = triangles.Contains(tr);
                triangles.Add(tr);
                if (contain) continue;
                vertices.Add(CubeData.vertices[tr]);

                uv.Add(CubeData.uv[tr]);

                normals.Add(CubeData.normals[tr]);
                index++;
            }
            mesh.mesh.vertices = vertices.ToArray();
            mesh.mesh.uv = uv.ToArray();
            mesh.mesh.triangles = trs;
            mesh.mesh.normals = normals.ToArray();
            mesh.mesh.bounds = new Bounds(Vector3.zero, new Vector3(1f, 1f, 1f));
            //CubeData.SaveMesh(mesh.mesh,name, true, true);

        }
    }
}


public static class CubeData
{


    public static Vector3[] vertices = new Vector3[] {
        //new Vector3(0.5f,-0.5f,0.5f),new Vector3(-0.5f,-0.5f,0.5f),new Vector3(0.5f,0.5f,0.5f),       //0,1,2
        //new Vector3(-0.5f,0.5f,0.5f),new Vector3(0.5f,0.5f,-0.5f),new Vector3(-0.5f,0.5f,-0.5f),      //3,4,5
        //new Vector3(0.5f,-0.5f,-0.5f),new Vector3(-0.5f,-0.5f,-0.5f),new Vector3(0.5f,0.5f,0.5f),     //6,7,8
        //new Vector3(-0.5f,0.5f,0.5f),new Vector3(0.5f,0.5f,-0.5f),new Vector3(-0.5f,0.5f,-0.5f),      //9,10,11
        //new Vector3(0.5f,-0.5f,-0.5f),new Vector3(0.5f,-0.5f,0.5f),new Vector3(-0.5f,-0.5f,0.5f),     //12,13,14
        //new Vector3(-0.5f,-0.5f,-0.5f),new Vector3(-0.5f,-0.5f,0.5f),new Vector3(-0.5f,0.5f,0.5f),    //15,16,17
        //new Vector3(-0.5f,0.5f,-0.5f),new Vector3(-0.5f,-0.5f,-0.5f),new Vector3(0.5f,-0.5f,-0.5f),   //18,19,20
        //new Vector3(0.5f,0.5f,-0.5f),new Vector3(0.5f,0.5f,0.5f),new Vector3(0.5f,-0.5f,0.5f),        //21,22,23

        //new Vector3(0f,-1f,0f),new Vector3(-1f,-1f,0f),new Vector3(0f,0f,0f),
        //new Vector3(-1f,0f,0f),new Vector3(0f,0f,-1f),new Vector3(-1f,0f,-1f),
        //new Vector3(0f,-1f,-1f),new Vector3(-1f,-1f,-1f),new Vector3(0f,0f,0f),
        //new Vector3(-1f,0f,0f),new Vector3(0f,0f,-1f),new Vector3(-1f,0f,-1f),
        //new Vector3(0f,-1f,-1f),new Vector3(0f,-1f,0f),new Vector3(-1f,-1f,0f),
        //new Vector3(-1f,-1f,-1f),new Vector3(-1f,-1f,0f),new Vector3(-1f,0f,0f),
        //new Vector3(-1f,0f,-1f),new Vector3(-1f,-1f,-1f),new Vector3(0f,-1f,-1f),
        //new Vector3(0f,0f,-1f),new Vector3(0f,0f,0f),new Vector3(0f,-1f,0f),


        new Vector3(1f,0f,1f),new Vector3(0f,0f,1f),new Vector3(1f,1f,1f),  //0,1,2
        new Vector3(0f,1f,1f),new Vector3(1f,1f,0f),new Vector3(0f,1f,0f),  //3,4,5
        new Vector3(1f,0f,0f),new Vector3(0f,0f,0f),new Vector3(1f,1f,1f),  //6,7,8
        new Vector3(0f,1f,1f),new Vector3(1f,1f,0f),new Vector3(0f,1f,0f),  //9,10,11
        new Vector3(1f,0f,0f),new Vector3(1f,0f,1f),new Vector3(0f,0f,1f),  //12,13,14
        new Vector3(0f,0f,0f),new Vector3(0f,0f,1f),new Vector3(0f,1f,1f),  //15,16,17
        new Vector3(0f,1f,0f),new Vector3(0f,0f,0f),new Vector3(1f,0f,0f),  //18,19,20
        new Vector3(1f,1f,0f),new Vector3(1f,1f,1f),new Vector3(1f,0f,1f),  //21,22,23


        new Vector3(0.5f,0f,0.5f),      //24
        new Vector3(0.5f,1f,0.5f),      //25

        new Vector3(0f,0.5f,0.5f),      //26
        new Vector3(1f,0.5f,0.5f),      //27

        new Vector3(0.5f,0.5f,0f),      //28
        new Vector3(0.5f,0.5f,1f),      //29
        new Vector3(0.5f,0.5f,0.5f),      //30

    };

    public static Vector3[] centers = new Vector3[] {
        //new Vector3(0.5f,0.5f,0.5f),


    };
    public static Vector2[] uv = new Vector2[] {
        new Vector2(0f,0f),new Vector2(1f,0f),new Vector2(0f,1f),new Vector2(1f,1f),
        new Vector2(0f,1f),new Vector2(1f,1f),new Vector2(0f,1f),new Vector2(1f,1f),
        new Vector2(0f,0f),
        new Vector2(1f,0f),new Vector2(0f,0f),new Vector2(1f,0f),
        new Vector2(0f,0f),new Vector2(0f,1f),new Vector2(1f,1f),
        new Vector2(1f,0f),new Vector2(0f,0f),new Vector2(0f,1f),
        new Vector2(1f,1f),new Vector2(1f,0f),new Vector2(0f,0f),
        new Vector2(0f,1f),new Vector2(1f,1f),new Vector2(1f,0f),
    };

    public static int[] triangles = new int[] {
        //Front
        0,2,3,
        0,3,1,
        //Top
        8,4,5,
        8,5,9,
        //Back
        10,6,7,
        10,7,11,
        //Bottom
        12,13,14,
        12,14,15,
        //Left
        16,17,18,
        16,18,19,
        //Right
        20,21,22,
        20,22,23,
    };

    public static int[] trianglesr = new int[] {
        0,2,1,
        2,3,1,

        8,4,9,
        4,5,9,

        10,6,11,
        6,7,11,

        12,13,15,
        13,14,15,

        16,17,19,
        17,18,19,

        20,21,23,
        21,22,23,
    };

    public static Vector3[] normals = new Vector3[] {
        new Vector3(0f,0f,1f),new Vector3(0f,0f,1f),new Vector3(0f,0f,1f),
        new Vector3(0f,0f,1f),new Vector3(0f,1f,0f),new Vector3(0f,1f,0f),
        new Vector3(0f,0f,-1f),new Vector3(0f,0f,-1f),new Vector3(0f,1f,0f),
        new Vector3(0f,1f,0f),new Vector3(0f,0f,-1f),new Vector3(0f,0f,-1f),
        new Vector3(0f,-1f,0f),new Vector3(0f,-1f,0f),new Vector3(0f,-1f,0f),
        new Vector3(0f,-1f,0f),new Vector3(-1f,0f,0f),new Vector3(-1f,0f,0f),
        new Vector3(-1f,0f,0f),new Vector3(-1f,0f,0f),new Vector3(1f,0f,0f),
        new Vector3(1f,0f,0f),new Vector3(1f,0f,0f),new Vector3(1f,0f,0f),
        };


    public static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
    {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/StreamingAssets/GenMesh", name, "asset");
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = (makeNewInstance) ? UnityEngine.Object.Instantiate(mesh) as Mesh : mesh;

        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);

        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }


}
