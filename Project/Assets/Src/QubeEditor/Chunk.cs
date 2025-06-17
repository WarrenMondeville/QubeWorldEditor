using System.Collections.Generic;
using UnityEngine;
namespace QubeWorld
{

    public struct UVPoint
    {
        public Vector2 o, u, r, v;
    }

    public static class ChunkHelper
    {
        public static byte[] GroundBytes = new byte[] { 42, 43, 44, 45, 46, 47, 10, 9, 8, 7, 50 };
        public static int ChunkStep = 32;
        public static Vector3 Half = Vector3.one * 0.5f;
        private static Chunk _chunk;
        private static Chunk chunk
        {
            get
            {
                if (_chunk == null)
                {
                    _chunk = Resources.Load<Chunk>("Chunk");
                }
                return _chunk;
            }
        }

        private static Dictionary<Vector3Int, Chunk> chunkDic = new();
        private static Queue<Chunk> poolTile = new Queue<Chunk>();
        public static void RefreshChunk(List<Vector3Int> showlist)
        {
            foreach (var item in showlist)
            {
                if (chunkDic.TryGetValue(item, out Chunk chunk))
                {

                }
                else
                {
                    var terrain = DeQueue(item);
                    chunkDic[item] = terrain;
                }
            }

            //清理不在显示范围内的Chunk
            if (chunkDic.Count > 40)
            {
                List<Vector3Int> removeList = new List<Vector3Int>(chunkDic.Count - showlist.Count);
                foreach (var item in chunkDic)
                {
                    if (!showlist.Contains(item.Key))
                    {
                        removeList.Add(item.Key);
                    }
                }
                foreach (var item in removeList)
                {
                    Enqueue(chunkDic[item]);
                    chunkDic.Remove(item);
                }
            }

        }



        public static void Enqueue(Chunk chunk)
        {
            chunk.ReleaseChunk();
            poolTile.Enqueue(chunk);
        }

        public static Chunk DeQueue(Vector3Int position)
        {

            Chunk terrain;
            if (poolTile.Count > 0)
            {
                terrain = poolTile.Dequeue();
                //terrain.gameObject.SetActive(true);

            }
            else
            {
                terrain = GameObject.Instantiate<Chunk>(chunk);
            }
            terrain.InitChunk(position);
            return terrain;
        }


        public static Chunk GetNeighbourChunk(Vector3Int position)
        {
            if (chunkDic.TryGetValue(position, out Chunk terrain))
            {
                return terrain;
            }
            return null;
        }

        private static Dictionary<byte, UVPoint> uvCache = new Dictionary<byte, UVPoint>();
        public static UVPoint GetUVPoint(byte typeid)
        {
            if (!uvCache.TryGetValue(typeid, out UVPoint uv))
            {
                uv = new UVPoint();
                float x = (int)typeid / 16;
                float y = (int)typeid % 16;
                Vector2 one = new Vector2(0.0625f, 0.0625f);
                uv.o = new Vector2(x, y) * one;
                uv.u = uv.o + Vector2.up * one.y;
                uv.r = uv.o + Vector2.right * one.x;
                uv.v = uv.o + Vector2.one * one;
                uvCache[typeid] = uv;
            }
            return uv;
        }

        public static Vector3Int ToVector3Int(this Vector3 v)
        {
            return new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
        }

    }

    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(MeshFilter))]
    public class Chunk : MonoBehaviour
    {
        public MeshCollider meshCollider;
        public MeshFilter meshFilter;


        public Vector3Int IntPos;
        public Vector3Int RelPos;


        public string TileName = "";
        public bool Changed = false;
        //byte[,,] map;
        Terrain _terrain;

        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        private void OnDestroy()
        {
            ReleaseChunk();
        }

        public void InitChunk(Vector3Int it)
        {
            RelPos = it * ChunkHelper.ChunkStep;
            this.transform.position = RelPos - ChunkHelper.Half;
            this.transform.rotation = Quaternion.identity;
            this.gameObject.SetActive(true);
            Changed = false;
            IntPos = it;

            //初始化Map
            TileName = $"Terrian{it.x}_{it.y}_{it.z}.bytes";
            this.gameObject.name = TileName;
            //map = TerrainDataCtr.LoadDataMap(TileName);
            _terrain = TerrainDataCtr.LoadTerrain(TileName);
            //if (map == null) map = GenerateDefaultMap();
            if (_terrain == null) _terrain = GenerateDefaultMap();
            //根据生成的信息，Build出Chunk的网格
            BuildChunk();
        }

        public void ReleaseChunk()
        {
            if (Changed) TerrainDataCtr.SaveTerrain(_terrain, TileName);
            _terrain = null;
            Changed = false;
            this.gameObject.SetActive(false);
        }

        public void SetBuildChunk(Vector3 pos, byte b)
        {

            var p = (pos - RelPos).ToVector3Int();

            //当前位置是否在Chunk内
            if ((p.x < 0) || (p.z < 0) || (p.x >= ChunkHelper.ChunkStep) || (p.z >= ChunkHelper.ChunkStep))
            {
                var neighbour = GetNeighbour(p.x, p.y, p.z);
                neighbour?.SetBuildChunk(pos, b);
                return;
            }
            Changed = true;
            SetBlockType(p.x, p.y, p.z, b);
            BuildChunk();
        }

        Terrain GenerateDefaultMap()
        {

            var map = new byte[ChunkHelper.ChunkStep, ChunkHelper.ChunkStep, ChunkHelper.ChunkStep];
            var sty = new byte[ChunkHelper.ChunkStep, ChunkHelper.ChunkStep, ChunkHelper.ChunkStep];

            //遍历map，生成其中每个Block的信息
            for (int x = 0; x < ChunkHelper.ChunkStep; x++)
            {
                for (int y = 0; y < ChunkHelper.ChunkStep; y++)
                {
                    for (int z = 0; z < ChunkHelper.ChunkStep; z++)
                    {

                        map[x, y, z] = y <= 10 ? ChunkHelper.GroundBytes[y] : (byte)0;
                    }
                }
            }

            return new Terrain() { map = map, sty = sty };
        }

        public void BuildChunk()
        {
            var chunkMesh = meshFilter.mesh;
            chunkMesh.Clear();
            verts.Clear();
            uvs.Clear();
            tris.Clear();
            normals.Clear();
            //遍历chunk, 生成其中的每一个Block
            for (int x = 0; x < ChunkHelper.ChunkStep; x++)
            {
                for (int y = 0; y < ChunkHelper.ChunkStep; y++)
                {
                    for (int z = 0; z < ChunkHelper.ChunkStep; z++)
                    {
                        BuildBlock1(x, y, z, verts, uvs, tris);
                    }
                }
            }

            chunkMesh.vertices = verts.ToArray();
            chunkMesh.uv = uvs.ToArray();
            chunkMesh.triangles = tris.ToArray();
            chunkMesh.normals = normals.ToArray();
            //chunkMesh.RecalculateBounds();
            chunkMesh.RecalculateNormals();

            //meshFilter.mesh = chunkMesh;
            meshCollider.sharedMesh = chunkMesh;
        }

        void BuildBlock(int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
        {
            if (_terrain.map[x, y, z] == 0) return;

            byte typeid = _terrain.map[x, y, z];

            //Left
            if (CheckNeedBuildFace(x - 1, y, z))
                BuildFace(typeid, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
            //Right
            if (CheckNeedBuildFace(x + 1, y, z))
                BuildFace(typeid, new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, verts, uvs, tris);

            //Bottom
            if (CheckNeedBuildFace(x, y - 1, z))
                BuildFace(typeid, new Vector3(x, y, z), Vector3.forward, Vector3.right, false, verts, uvs, tris);
            //Top
            if (CheckNeedBuildFace(x, y + 1, z))
                BuildFace(typeid, new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true, verts, uvs, tris);

            //Back
            if (CheckNeedBuildFace(x, y, z - 1))
                BuildFace(typeid, new Vector3(x, y, z), Vector3.up, Vector3.right, true, verts, uvs, tris);
            //Front
            if (CheckNeedBuildFace(x, y, z + 1))
                BuildFace(typeid, new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, verts, uvs, tris);
        }

        bool CheckNeedBuildFace(int x, int y, int z)
        {
            if (y < 0) return false;
            var type = GetBlockType(x, y, z);
            switch (type)
            {
                case 0:
                    return true;
                default:
                    return false;
            }
        }

        public byte GetBlockType(int x, int y, int z)
        {
            if (y < 0 || y > ChunkHelper.ChunkStep - 1)
            {
                return 0;
            }

            //当前位置是否在Chunk内
            if ((x < 0) || (z < 0) || (x >= ChunkHelper.ChunkStep) || (z >= ChunkHelper.ChunkStep))
            {
                //隔壁chunk数据
                return 0;
                //return GetNeighbourBlockType(x, y, z);
            }
            return _terrain.map[x, y, z];
        }

        private byte GetNeighbourBlockType(int x, int y, int z)
        {
            Vector3Int pos = IntPos;
            if (x < 0)
            {
                pos.x -= 1;
                x = x + ChunkHelper.ChunkStep;
            }
            else if (x >= ChunkHelper.ChunkStep)
            {
                pos.x += 1;
                x = x - ChunkHelper.ChunkStep;
            }

            if (z < 0)
            {
                pos.z -= 1;
                z = z + ChunkHelper.ChunkStep;
            }
            else if (z >= ChunkHelper.ChunkStep)
            {
                pos.z += 1;
                z = z - ChunkHelper.ChunkStep;
            }

            var neighbour = ChunkHelper.GetNeighbourChunk(pos);
            if (neighbour == null) return 0;
            return neighbour.GetBlockType(x, y, z);
        }

        public void SetBlockType(int x, int y, int z, byte b)
        {
            if (y < 0 || y > ChunkHelper.ChunkStep - 1)
            {
                return;
            }


            _terrain.map[x, y, z] = b;
        }

        private Chunk GetNeighbour(int x, int y, int z)
        {
            Vector3Int pos = IntPos;
            if (x < 0)
            {
                pos.x -= 1;
            }
            else if (x >= ChunkHelper.ChunkStep)
            {
                pos.x += 1;
            }

            if (z < 0)
            {
                pos.z -= 1;
            }
            else if (z >= ChunkHelper.ChunkStep)
            {
                pos.z += 1;
            }

            return ChunkHelper.GetNeighbourChunk(pos);
        }

        void BuildFace(byte typeid, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
        {
            int index = verts.Count;

            verts.Add(corner);
            verts.Add(corner + up);
            verts.Add(corner + up + right);
            verts.Add(corner + right);

            var uvPoint = ChunkHelper.GetUVPoint(typeid);
            uvs.Add(uvPoint.o);
            uvs.Add(uvPoint.u);
            uvs.Add(uvPoint.r);
            uvs.Add(uvPoint.v);


            if (reversed)
            {
                tris.Add(index + 0);
                tris.Add(index + 1);
                tris.Add(index + 2);
                tris.Add(index + 2);
                tris.Add(index + 3);
                tris.Add(index + 0);
            }
            else
            {
                tris.Add(index + 1);
                tris.Add(index + 0);
                tris.Add(index + 2);
                tris.Add(index + 3);
                tris.Add(index + 2);
                tris.Add(index + 0);
            }
        }


        void BuildBlock1(int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
        {
            if (_terrain.map[x, y, z] == 0) return;

            byte typeid = _terrain.map[x, y, z];
            var corner = new Vector3(x, y, z);
            //Left
            if (CheckNeedBuildFace(x - 1, y, z))
            {
                //BuildFace(typeid, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
                int index = verts.Count;

                verts.Add(CubeData.vertices[16] + corner);
                verts.Add(CubeData.vertices[17] + corner);
                verts.Add(CubeData.vertices[18] + corner);
                verts.Add(CubeData.vertices[19] + corner);

                normals.Add(CubeData.normals[16]);
                normals.Add(CubeData.normals[17]);
                normals.Add(CubeData.normals[18]);
                normals.Add(CubeData.normals[19]);

                var uvPoint = ChunkHelper.GetUVPoint(typeid);
                uvs.Add(uvPoint.o);
                uvs.Add(uvPoint.u);
                uvs.Add(uvPoint.r);
                uvs.Add(uvPoint.v);

                tris.Add(index + 0);
                tris.Add(index + 1);
                tris.Add(index + 2);
                tris.Add(index + 0);
                tris.Add(index + 2);
                tris.Add(index + 3);

            }
            //Right
            if (CheckNeedBuildFace(x + 1, y, z))
            //BuildFace(typeid, new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, verts, uvs, tris);
            {
                //BuildFace(typeid, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
                int index = verts.Count;
                //var corner = new Vector3(x + 1, y, z);
                verts.Add(CubeData.vertices[20] + corner);
                verts.Add(CubeData.vertices[21] + corner);
                verts.Add(CubeData.vertices[22] + corner);
                verts.Add(CubeData.vertices[23] + corner);

                normals.Add(CubeData.normals[20]);
                normals.Add(CubeData.normals[21]);
                normals.Add(CubeData.normals[22]);
                normals.Add(CubeData.normals[23]);

                var uvPoint = ChunkHelper.GetUVPoint(typeid);
                uvs.Add(uvPoint.o);
                uvs.Add(uvPoint.u);
                uvs.Add(uvPoint.r);
                uvs.Add(uvPoint.v);

                tris.Add(index + 0);
                tris.Add(index + 1);
                tris.Add(index + 2);
                tris.Add(index + 0);
                tris.Add(index + 2);
                tris.Add(index + 3);

            }
            //Bottom
            if (CheckNeedBuildFace(x, y - 1, z))
            //BuildFace(typeid, new Vector3(x, y, z), Vector3.forward, Vector3.right, false, verts, uvs, tris);
            {
                //BuildFace(typeid, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
                int index = verts.Count;
                //var corner = new Vector3(x, y, z);
                verts.Add(CubeData.vertices[12] + corner);
                verts.Add(CubeData.vertices[13] + corner);
                verts.Add(CubeData.vertices[14] + corner);
                verts.Add(CubeData.vertices[15] + corner);

                normals.Add(CubeData.normals[12]);
                normals.Add(CubeData.normals[13]);
                normals.Add(CubeData.normals[14]);
                normals.Add(CubeData.normals[15]);

                var uvPoint = ChunkHelper.GetUVPoint(typeid);
                uvs.Add(uvPoint.o);
                uvs.Add(uvPoint.u);
                uvs.Add(uvPoint.r);
                uvs.Add(uvPoint.v);

                tris.Add(index + 0);
                tris.Add(index + 1);
                tris.Add(index + 2);
                tris.Add(index + 0);
                tris.Add(index + 2);
                tris.Add(index + 3);

            }
            //Top
            if (CheckNeedBuildFace(x, y + 1, z))
            {
                //BuildFace(typeid, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
                int index = verts.Count;
                //var corner = new Vector3(x, y + 1, z);
                verts.Add(CubeData.vertices[4] + corner);
                verts.Add(CubeData.vertices[5] + corner);
                verts.Add(CubeData.vertices[8] + corner);
                verts.Add(CubeData.vertices[9] + corner);

                normals.Add(CubeData.normals[4]);
                normals.Add(CubeData.normals[5]);
                normals.Add(CubeData.normals[8]);
                normals.Add(CubeData.normals[9]);

                var uvPoint = ChunkHelper.GetUVPoint(typeid);
                uvs.Add(uvPoint.o);
                uvs.Add(uvPoint.u);
                uvs.Add(uvPoint.r);
                uvs.Add(uvPoint.v);

                tris.Add(index + 2);
                tris.Add(index + 0);
                tris.Add(index + 1);
                tris.Add(index + 2);
                tris.Add(index + 1);
                tris.Add(index + 3);

            }

            //Back
            if (CheckNeedBuildFace(x, y, z - 1))
            //BuildFace(typeid, new Vector3(x, y, z), Vector3.up, Vector3.right, true, verts, uvs, tris);
            {
                //BuildFace(typeid, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
                int index = verts.Count;
                //var corner = new Vector3(x, y, z);
                verts.Add(CubeData.vertices[6] + corner);
                verts.Add(CubeData.vertices[7] + corner);
                verts.Add(CubeData.vertices[10] + corner);
                verts.Add(CubeData.vertices[11] + corner);

                normals.Add(CubeData.normals[6]);
                normals.Add(CubeData.normals[7]);
                normals.Add(CubeData.normals[10]);
                normals.Add(CubeData.normals[11]);

                var uvPoint = ChunkHelper.GetUVPoint(typeid);
                uvs.Add(uvPoint.o);
                uvs.Add(uvPoint.u);
                uvs.Add(uvPoint.r);
                uvs.Add(uvPoint.v);

                tris.Add(index + 2);
                tris.Add(index + 0);
                tris.Add(index + 1);
                tris.Add(index + 2);
                tris.Add(index + 1);
                tris.Add(index + 3);

            }//Front
            if (CheckNeedBuildFace(x, y, z + 1))
            //BuildFace(typeid, new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, verts, uvs, tris);
            {
                //BuildFace(typeid, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
                int index = verts.Count;
                //var corner = new Vector3(x, y, z+1);
                verts.Add(CubeData.vertices[0] + corner);
                verts.Add(CubeData.vertices[1] + corner);
                verts.Add(CubeData.vertices[2] + corner);
                verts.Add(CubeData.vertices[3] + corner);

                normals.Add(CubeData.normals[0]);
                normals.Add(CubeData.normals[1]);
                normals.Add(CubeData.normals[2]);
                normals.Add(CubeData.normals[3]);

                var uvPoint = ChunkHelper.GetUVPoint(typeid);
                uvs.Add(uvPoint.o);
                uvs.Add(uvPoint.u);
                uvs.Add(uvPoint.r);
                uvs.Add(uvPoint.v);

                tris.Add(index + 0);
                tris.Add(index + 2);
                tris.Add(index + 3);
                tris.Add(index + 0);
                tris.Add(index + 3);
                tris.Add(index + 1);
            }
        }
    }


}