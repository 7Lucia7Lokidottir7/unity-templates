using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PG.MenuManagement
{
    public class TabsController : MonoBehaviour
    {
        [SerializeField] private Tab[] _tabs;
        public Dictionary<string, Tab> tabs = new Dictionary<string, Tab>();

        [System.Serializable]
        public class Tab
        {
            public string name;
            public GameObject gameObject;
            public Selectable selectableObject;
        }
        void Start()
        {
        
        }
        public void SelectTab(int index)
        {
            for (int i = 0; i < _tabs.Length; i++)
            {
                if (i == index)
                {
                    if (_tabs[i].gameObject.TryGetComponent(out UIShowHide targetUiShowHide))
                    {
                        targetUiShowHide.Show();
                    }
                    else
                    {
                        _tabs[i].gameObject.SetActive(true);
                    }
                    _tabs[i].selectableObject.Select();
                    continue;
                }
                if (_tabs[i].gameObject.TryGetComponent(out UIShowHide uiShowHide))
                {
                    uiShowHide.Hide();
                }
                else
                {
                    _tabs[i].gameObject.SetActive(false);
                }
            }
        }
        public void SelectTab(string value)
        {
            tabs.TryGetValue(value, out var tab);
            if (tab.gameObject.TryGetComponent(out UIShowHide targetUiShowHide))
            {
                targetUiShowHide.Show();
            }
            else
            {
                tab.gameObject.SetActive(true);
            }
            tab.selectableObject.Select();

            for (int i = 0; i < _tabs.Length; i++)
            {
                if (_tabs[i].name == value)
                {
                    continue;
                }
                if (_tabs[i].gameObject.TryGetComponent(out UIShowHide uiShowHide))
                {
                    uiShowHide.Hide();
                }
                else
                {
                    _tabs[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
