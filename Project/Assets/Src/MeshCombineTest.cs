using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class MeshCombineTest : MonoBehaviour
{
    public Mesh mesh;
    public Material material;
    void Start()
    {
        var str = "";
        foreach (var item in mesh.vertices)
        {
            str += $"new Vector3({item.x+0.5f}f,{item.y + 0.5f}f,{item.z + 0.5f}f),";
        }
        Debug.Log(str);
        str = "";

        foreach (var item in mesh.uv)
        {
            str += $"new Vector2({item.x}f,{item.y}f),";
        }
        Debug.Log(str);
        str = "";
        foreach (var item in mesh.triangles)
        {
            str += $"{item},";
        }
        Debug.Log(str);
        str = "";
        foreach (var item in mesh.normals)
        {
            str += $"new Vector3({item.x}f,{item.y}f,{item.z}f),";
        }
        Debug.Log(str);
        str = "";

        CreateMesh();
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
            List<Vector3> vertices = new ();
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
        //new Vector3(0.5f,-0.5f,0.5f),new Vector3(-0.5f,-0.5f,0.5f),new Vector3(0.5f,0.5f,0.5f),//0,1,2
        //new Vector3(-0.5f,0.5f,0.5f),new Vector3(0.5f,0.5f,-0.5f),new Vector3(-0.5f,0.5f,-0.5f),//3,4,5
        //new Vector3(0.5f,-0.5f,-0.5f),new Vector3(-0.5f,-0.5f,-0.5f),new Vector3(0.5f,0.5f,0.5f),//6,7,8
        //new Vector3(-0.5f,0.5f,0.5f),new Vector3(0.5f,0.5f,-0.5f),new Vector3(-0.5f,0.5f,-0.5f),//9,10,11
        //new Vector3(0.5f,-0.5f,-0.5f),new Vector3(0.5f,-0.5f,0.5f),new Vector3(-0.5f,-0.5f,0.5f),//12,13,14
        //new Vector3(-0.5f,-0.5f,-0.5f),new Vector3(-0.5f,-0.5f,0.5f),new Vector3(-0.5f,0.5f,0.5f),//15,16,17
        //new Vector3(-0.5f,0.5f,-0.5f),new Vector3(-0.5f,-0.5f,-0.5f),new Vector3(0.5f,-0.5f,-0.5f),//18,19,20
        //new Vector3(0.5f,0.5f,-0.5f),new Vector3(0.5f,0.5f,0.5f),new Vector3(0.5f,-0.5f,0.5f),//21,22,23

        //new Vector3(0f,-1f,0f),new Vector3(-1f,-1f,0f),new Vector3(0f,0f,0f),
        //new Vector3(-1f,0f,0f),new Vector3(0f,0f,-1f),new Vector3(-1f,0f,-1f),
        //new Vector3(0f,-1f,-1f),new Vector3(-1f,-1f,-1f),new Vector3(0f,0f,0f),
        //new Vector3(-1f,0f,0f),new Vector3(0f,0f,-1f),new Vector3(-1f,0f,-1f),
        //new Vector3(0f,-1f,-1f),new Vector3(0f,-1f,0f),new Vector3(-1f,-1f,0f),
        //new Vector3(-1f,-1f,-1f),new Vector3(-1f,-1f,0f),new Vector3(-1f,0f,0f),
        //new Vector3(-1f,0f,-1f),new Vector3(-1f,-1f,-1f),new Vector3(0f,-1f,-1f),
        //new Vector3(0f,0f,-1f),new Vector3(0f,0f,0f),new Vector3(0f,-1f,0f),


        new Vector3(1f,0f,1f),new Vector3(0f,0f,1f),new Vector3(1f,1f,1f),
        new Vector3(0f,1f,1f),new Vector3(1f,1f,0f),new Vector3(0f,1f,0f),
        new Vector3(1f,0f,0f),new Vector3(0f,0f,0f),new Vector3(1f,1f,1f),
        new Vector3(0f,1f,1f),new Vector3(1f,1f,0f),new Vector3(0f,1f,0f),
        new Vector3(1f,0f,0f),new Vector3(1f,0f,1f),new Vector3(0f,0f,1f),
        new Vector3(0f,0f,0f),new Vector3(0f,0f,1f),new Vector3(0f,1f,1f),
        new Vector3(0f,1f,0f),new Vector3(0f,0f,0f),new Vector3(1f,0f,0f),
        new Vector3(1f,1f,0f),new Vector3(1f,1f,1f),new Vector3(1f,0f,1f),

    };
    public static Vector2[] uv = new Vector2[] {
        new Vector2(0f,0f),new Vector2(1f,0f),new Vector2(0f,1f),
        new Vector2(1f,1f),new Vector2(0f,1f),new Vector2(1f,1f),
        new Vector2(0f,1f),new Vector2(1f,1f),new Vector2(0f,0f),
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
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/GameAssets/GenMesh", name, "asset");
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);

        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }


}
