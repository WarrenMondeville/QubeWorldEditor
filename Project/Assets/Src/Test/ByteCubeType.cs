using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByteCubeType : MonoBehaviour
{

    public Material material;
    void Start()
    {
        for (int i = 255; i >= 0; i--)
        {
            var (corners, isValid) = GetCorners((byte)i);
            if (!isValid) continue; // 如果角数小于3，则跳过
            var go = new GameObject(i.ToString());
            go.transform.position = new Vector3(i, 0, 0);
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>().material = material;
            go.AddComponent<DynamicCubeMesh>().SetCorners(corners);
        }
    }

    (bool[], bool) GetCorners(byte b)
    {
        int count = 0;
        bool[] corners = new bool[8]; // 8个角的存在状态（默认全false）
        for (int i = 0; i < 8; i++)
        {
            // 检查第 (7-i) 位（MSB 优先）
            var result = (b & (1 << (7 - i))) != 0;
            if (result)
            {
                count++;
            }
            corners[i] = result;
        }
        return (corners,count>=3);
    }
}
