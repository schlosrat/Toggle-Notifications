﻿using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public class TabsUI
    {
        public List<IPageContent> Pages = new List<IPageContent>();
        private List<IPageContent> _filteredPages = new List<IPageContent>();
        private IPageContent CurrentPage;
        private List<float> TabsWidth = new List<float>();

        private bool _tabButton(bool isCurrent, bool isActive, string txt, GUIContent icon)
        {
            GUIStyle style = isActive ? TNBaseStyle.TabActive : TNBaseStyle.TabNormal;
            return icon == null ? GUILayout.Toggle((isCurrent ? 1 : 0) != 0, txt, style, GUILayout.ExpandWidth(true)) : GUILayout.Toggle((isCurrent ? 1 : 0) != 0, icon, style, GUILayout.ExpandWidth(true));
        }
        public int DrawTabs(int current, float maxWidth = 300f)
        {
            current = GeneralTools.ClampInt(current, 0, _filteredPages.Count - 1);
            GUILayout.BeginHorizontal();
            int num1 = current;

            if (TabsWidth.Count != _filteredPages.Count)
            {
                TabsWidth.Clear();
                for (int index = 0; index < _filteredPages.Count; ++index)
                {
                    IPageContent filteredPage = _filteredPages[index];
                    float minWidth;
                    TNBaseStyle.TabNormal.CalcMinMaxWidth(new GUIContent(filteredPage.Name, ""), out minWidth, out float _);
                    TabsWidth.Add(minWidth);
                }
            }

            float num2 = 0.0f;

            for (int index = 0; index < _filteredPages.Count; ++index)
            {
                IPageContent filteredPage = _filteredPages[index];
                float num3 = TabsWidth[index];

                if (num2 > maxWidth)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    num2 = 0.0f;
                }

                num2 += num3;
                bool isCurrent = current == index;

                // Create the tab label with page number
                GUIContent tabLabel = new GUIContent($"{filteredPage.Name} ({index + 1}/{_filteredPages.Count})");

                if (_tabButton(isCurrent, filteredPage.IsRunning, tabLabel.text, filteredPage.Icon) && !isCurrent)
                    num1 = index;
            }

            GUILayout.EndHorizontal();
            UITools.Separator();
            return num1;
        }


        public void Init()
        {
            int mainTabIndex = TNBaseSettings.MainTabIndex;
            int validTabIndex = Mathf.Clamp(mainTabIndex, 0, Pages.Count - 1);
            TNBaseSettings.MainTabIndex = validTabIndex;
            CurrentPage = Pages[validTabIndex];
            CurrentPage.UIVisible = true;
        }

        public void Update()
        {
            this._filteredPages = new List<IPageContent>();
            for (int index = 0; index < this.Pages.Count; ++index)
            {
                if (this.Pages[index].IsActive)
                    this._filteredPages.Add(this.Pages[index]);
            }
        }

        public void OnGUI()
        {
            int mainTabIndex = TNBaseSettings.MainTabIndex;
            if (this._filteredPages.Count == 0)
            {
                UITools.Error("NO active Tab tage !!!");
            }
            else
            {
                int index = GeneralTools.ClampInt(this._filteredPages.Count != 1 ? this.DrawTabs(mainTabIndex) : 0, 0, this._filteredPages.Count - 1);
                IPageContent filteredPage = this._filteredPages[index];
                if (filteredPage != this.CurrentPage)
                {
                    this.CurrentPage.UIVisible = false;
                    this.CurrentPage = filteredPage;
                    this.CurrentPage.UIVisible = true;
                }
                TNBaseSettings.MainTabIndex = index;
                this.CurrentPage.OnGUI();
            }
        }
    }
}
