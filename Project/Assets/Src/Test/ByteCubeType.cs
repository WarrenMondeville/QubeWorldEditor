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
            if (!isValid) continue; // �������С��3��������
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
        bool[] corners = new bool[8]; // 8���ǵĴ���״̬��Ĭ��ȫfalse��
        for (int i = 0; i < 8; i++)
        {
            // ���� (7-i) λ��MSB ���ȣ�
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
