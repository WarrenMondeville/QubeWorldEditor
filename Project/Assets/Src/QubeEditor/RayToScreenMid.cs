
using UnityEngine;
namespace QubeWorld
{
    public class RayToScreenMid : MonoBehaviour
    {
        public GameObject HitOBJ;
        // ���ӽ����������Ļ�м䷢������
        public Camera MianCamera;                // ����һ�����������
        public LayerMask LayerMask;            // ����һ�������֣�����ָ�����߼��Ĳ�

        Vector3 ScreenMidPos;             // ������Ļ����ĵ������

        Material material;          // ��������������ײ��������Ĳ���

        private void Start()
        {
            Application.targetFrameRate = 60; // ����Ŀ��֡��Ϊ30֡ÿ��
            ScreenMidPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);  // ��ʼ����Ļ����ĵ������
            material = HitOBJ.GetComponent<MeshRenderer>().material; // ��ȡHitOBJ�Ĳ���
        }
        void Update()
        {
            HitOBJ.SetActive(false);
            if (Input.GetKey(KeyCode.E))
            {
                HitOBJ.SetActive(true);
                RayShotE();              // ������������£�����һ������
            }

            if (Input.GetKey(KeyCode.R))
            {
                HitOBJ.SetActive(true);
                RayShotR();              // ������������£�����һ������
            }
            //FPController.Instance.enabled= !HitOBJ.activeSelf;

        }

        private void RayShotE()
        {
            //���������������Ļ�м䷢�����ߣ�    
            ScreenMidPos = Input.mousePosition;
            Ray OneShotRay = MianCamera.ScreenPointToRay(ScreenMidPos);          // ����Ļ�����Ϊԭ�㣬��������
            if (Physics.Raycast(OneShotRay, out RaycastHit hit, 20, LayerMask))                          // �����������������
            {
                if (hit.collider != null)
                {
                    material.SetColor("_BaseColor", UIPanelManager.Instance.GetColor(UIPanelManager.Instance.ColorIndex)); // ���ò�����ɫ
                    var pos = hit.point - hit.normal * 0.5f;
                    HitOBJ.transform.position = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
                    HitOBJ.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    //Debug.Log($"���߷���ɹ� {hit.collider.name}  {hit.point}  {hit.normal}");
                    if (Input.GetMouseButtonDown(0))
                    {
                        var chunk = hit.collider.GetComponent<Chunk>();
                        chunk.SetBuildChunk(HitOBJ.transform.position, UIPanelManager.Instance.ColorIndex);
                    }

                }
            }

        }


        private void RayShotR()
        {
            //���������������Ļ�м䷢�����ߣ�    
            ScreenMidPos = Input.mousePosition;
            Ray OneShotRay = MianCamera.ScreenPointToRay(ScreenMidPos);          // ����Ļ�����Ϊԭ�㣬��������
            if (Physics.Raycast(OneShotRay, out RaycastHit hit, 20))                          // �����������������
            {
                if (hit.collider != null)
                {
                    material.SetColor("_BaseColor", UIPanelManager.Instance.GetColor(UIPanelManager.Instance.ColorIndex)); // ���ò�����ɫ
                    var pos = hit.point + hit.normal * 0.5f;
                    HitOBJ.transform.position = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
                    HitOBJ.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    //Debug.Log($"���߷���ɹ� {hit.collider.name}  {hit.point}  {hit.normal}");
                    if (Input.GetMouseButtonDown(0))
                    {
                        var chunk = hit.collider.GetComponent<Chunk>();
                        chunk.SetBuildChunk(HitOBJ.transform.position, UIPanelManager.Instance.ColorIndex);
                    }

                }
            }

        }

    }
}