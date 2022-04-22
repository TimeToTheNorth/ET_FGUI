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
            string name= UIHelperComponentSystem.GetClassName(UIEnum.Panel_Main.ToString());
            self.UIMainController =(UIMainController) UIHelperComponentSystem.LoadWindow(name);
            self.UIMainController.SelfUIPackageEnum = UIPackageEnum.Main;
            self.UIMainController.UiEnum = UIEnum.Panel_Main;
        }

        public static void Show(this UIMainPanelComponent self)
        {
            Game.Scene.GetComponent<UIManagerComponent>().OpenWindow(self.UIMainController);
            self.OnShow();
        }

        private static void OnShow(this UIMainPanelComponent self)
        {
            self.AddEvent();
        }

        //添加事件监听
        public static void AddEvent(this UIMainPanelComponent self)
        {
            self.UIMainController.Graph_Ground.onClick.Add(() =>
            {
                Debug.Log("点击+++++++++");
                self.Hide();
            });
        }

        //移除事件监听
        public static void DelEvent(this UIMainPanelComponent self)
        {
            self.UIMainController.Graph_Ground.onClick.Clear();
        }

        public static void Hide(this UIMainPanelComponent self)
        {
            Game.Scene.GetComponent<UIManagerComponent>().CloseWindow(UIEnum.Panel_Main);
            self.DelEvent();
        }

        /// <summary>
        /// 跳转打开其他界面
        /// </summary>
        public static void TurnToOpenPanel(this UIMainPanelComponent self, UIEnum className, UiLayer layer,
        UIPackageEnum packageName = UIPackageEnum.Null, bool isModal = false,
        params object[] values)
        {
            self.UIMainController.children = Game.Scene.GetComponent<UIManagerComponent>().OpenWindow(className, packageName, layer, isModal, values);
            if (self.UIMainController.children != null)
            {
                self.UIMainController.children.Selfparent = self.UIMainController;
            }
        }
    }
}