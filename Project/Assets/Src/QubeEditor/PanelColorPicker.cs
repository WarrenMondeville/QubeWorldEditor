using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace QubeWorld
{
    public class PanelColorPicker : MonoBehaviour
    {


        public Button colorButton;

        public List<Button> btns = new List<Button>();
        private void OnEnable()
        {
            if (btns.Count == 0)
            {
                var ourColor = UIPanelManager.Instance.ourColor;
                for (int i = 0; i < ourColor.colors.Length; i++)
                {
                    var btn = Instantiate(colorButton, transform);
                    btn.gameObject.SetActive(true);
                    btn.image.color = ourColor.colors[i];
                    byte b = (byte)i;
                    btn.onClick.AddListener(() => OnButtonClick(b));
                    btns.Add(btn);

                }
            }
        }

        public void OnButtonClick(byte b)
        {
            Debug.Log(b);
            UIPanelManager.Instance.ColorIndex = b;
        }
    }
}