using System;
using UnityEngine;

namespace ET
{
    public class UIMainPanelComponentAwakeSystem: AwakeSystem<UIMainPanelComponent>
    {
        public override void Awake(UIMainPanelComponent self)
        {
         
            self.Awake();
        }
    }

    public static class UIMainPanelComponentSystem
    {
        public static void Awake(this UIMainPanelComponent self)
        {
            

        }

        public static void OnShow(this UIMainPanelComponent self)
        {
            self.UIMainController.Graph_Ground.onClick.Add(() =>
            {
                Debug.Log("点击+++++++++");
            });
        }
    }
}