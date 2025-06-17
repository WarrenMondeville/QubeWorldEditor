using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QubeWorld
{
    public class UIPanelManager : MonoBehaviour
    {

        public static UIPanelManager Instance;
        public List<GameObject> panels = new List<GameObject>();
        public List<KeyCode> keys = new List<KeyCode>();
        public OurColors ourColor;
        public bool Interactive = false;
        public byte ColorIndex = 0;


        public void Awake()
        {
            Instance = this;
            ColorIndex = (byte)PlayerPrefs.GetInt("ColorIndex");
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                foreach (var key in keys)
                {
                    if (Input.GetKeyDown(key))
                    {
                        var index = keys.FindIndex(k => k == key);
                        var panel = panels[index];
                        panel.SetActive(!panel.activeSelf);
                        Interactive = panel.activeSelf;
                        FPController.Instance.enabled = !panel.activeSelf;
                    }
                }
            }
        }

        public void ShowPanel(GameObject panel)
        {
            foreach (var p in panels)
            {
                p.SetActive(false);
            }
            panel.SetActive(true);
        }
        public void HideAllPanels()
        {
            foreach (var p in panels)
            {
                p.SetActive(false);
            }
        }

        internal Color GetColor(byte colorIndex)
        {
            PlayerPrefs.SetInt("ColorIndex", colorIndex);
            return ourColor.colors[colorIndex];
        }
    }

}