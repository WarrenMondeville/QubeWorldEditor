using System.Collections.Generic;
using UnityEngine;
namespace QubeWorld
{
    public class WorldManager : MonoBehaviour
    {

        public Transform player;
        public Vector3 playerpos;

        public int X = int.MaxValue, Z = int.MaxValue;

        private Dictionary<Vector3Int, Chunk> showTile = new();

        private List<Chunk> tileList = new List<Chunk>();

        void Update()
        {
            TerrianGenerator();
        }


        private void TerrianGenerator()
        {
            if (!CheckPos(player.position)) return;

            int x = Mathf.RoundToInt(player.position.x / ChunkHelper.ChunkStep);
            int z = Mathf.RoundToInt(player.position.z / ChunkHelper.ChunkStep);
            if (x == X && z == Z) return;
            X = x;
            Z = z;

            playerpos = player.position;

            ChunkHelper.RefreshChunk(GetShowList(x, 0, z));
        }

        bool CheckPos(Vector3 pos)
        {
            var off = playerpos - pos;
            if (Mathf.Abs(off.x) > 5f) return true;
            if (Mathf.Abs(off.y) > 5f) return true;
            if (Mathf.Abs(off.z) > 5f) return true;
            return false;
        }
        List<Vector3Int> GetShowList(int x, int y, int z)
        {
            List<Vector3Int> showlist = new List<Vector3Int>(9);
            // 添加周围九个格子
            showlist.Add(new Vector3Int(x, 0, z));


            showlist.Add(new Vector3Int(x, 0, z + 1));
            showlist.Add(new Vector3Int(x, 0, z - 1));
            showlist.Add(new Vector3Int(x + 1, 0, z));
            showlist.Add(new Vector3Int(x - 1, 0, z));
            showlist.Add(new Vector3Int(x + 1, 0, z + 1));
            showlist.Add(new Vector3Int(x - 1, 0, z + 1));
            showlist.Add(new Vector3Int(x + 1, 0, z - 1));
            showlist.Add(new Vector3Int(x - 1, 0, z - 1));

            showlist.Add(new Vector3Int(x - 1, 0, z + 2));
            showlist.Add(new Vector3Int(x - 1, 0, z + -2));
            showlist.Add(new Vector3Int(x, 0, z + 2));
            showlist.Add(new Vector3Int(x, 0, z + -2));
            showlist.Add(new Vector3Int(x + 1, 0, z + 2));
            showlist.Add(new Vector3Int(x + 1, 0, z + -2));

            showlist.Add(new Vector3Int(x - 2, 0, z + 1));
            showlist.Add(new Vector3Int(x + 2, 0, z + 1));
            showlist.Add(new Vector3Int(x - 2, 0, z));
            showlist.Add(new Vector3Int(x + 2, 0, z));
            showlist.Add(new Vector3Int(x - 2, 0, z - 1));
            showlist.Add(new Vector3Int(x + 2, 0, z - 1));
            return showlist;
        }

    }

}