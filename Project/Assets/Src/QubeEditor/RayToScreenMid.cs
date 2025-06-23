
using UnityEngine;
namespace QubeWorld
{
    public class RayToScreenMid : MonoBehaviour
    {
        public GameObject HitOBJ;
        // 从视角摄像机向屏幕中间发射射线
        public Camera MianCamera;                // 这是一个摄像机对象
        public LayerMask LayerMask;            // 这是一个层遮罩，用于指定射线检测的层

        Vector3 ScreenMidPos;             // 这是屏幕中央的点的坐标

        Material material;          // 用于设置射线碰撞到的物体的材质

        private void Start()
        {
            Application.targetFrameRate = 60; // 设置目标帧率为30帧每秒
            ScreenMidPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);  // 初始化屏幕中央的点的坐标
            material = HitOBJ.GetComponent<MeshRenderer>().material; // 获取HitOBJ的材质
        }
        void Update()
        {
            HitOBJ.SetActive(false);
            if (Input.GetKey(KeyCode.E))
            {
                HitOBJ.SetActive(true);
                RayShotE();              // 如果鼠标左键按下，则发射一条射线
            }

            if (Input.GetKey(KeyCode.R))
            {
                HitOBJ.SetActive(true);
                RayShotR();              // 如果鼠标左键按下，则发射一条射线
            }
            //FPController.Instance.enabled= !HitOBJ.activeSelf;

        }

        private void RayShotE()
        {
            //从摄像机出发向屏幕中间发射射线！    
            ScreenMidPos = Input.mousePosition;
            Ray OneShotRay = MianCamera.ScreenPointToRay(ScreenMidPos);          // 以屏幕中央点为原点，发射射线
            if (Physics.Raycast(OneShotRay, out RaycastHit hit, 20, LayerMask))                          // 如果射线碰到了物体
            {
                if (hit.collider != null)
                {
                    material.SetColor("_BaseColor", UIPanelManager.Instance.GetColor(UIPanelManager.Instance.ColorIndex)); // 设置材质颜色
                    var pos = hit.point - hit.normal * 0.5f;
                    HitOBJ.transform.position = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
                    HitOBJ.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    //Debug.Log($"射线发射成功 {hit.collider.name}  {hit.point}  {hit.normal}");
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
            //从摄像机出发向屏幕中间发射射线！    
            ScreenMidPos = Input.mousePosition;
            Ray OneShotRay = MianCamera.ScreenPointToRay(ScreenMidPos);          // 以屏幕中央点为原点，发射射线
            if (Physics.Raycast(OneShotRay, out RaycastHit hit, 20))                          // 如果射线碰到了物体
            {
                if (hit.collider != null)
                {
                    material.SetColor("_BaseColor", UIPanelManager.Instance.GetColor(UIPanelManager.Instance.ColorIndex)); // 设置材质颜色
                    var pos = hit.point + hit.normal * 0.5f;
                    HitOBJ.transform.position = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
                    HitOBJ.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    //Debug.Log($"射线发射成功 {hit.collider.name}  {hit.point}  {hit.normal}");
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