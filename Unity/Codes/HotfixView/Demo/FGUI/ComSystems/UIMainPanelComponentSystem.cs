using FairyGUI;
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
            string name = UIHelperComponentSystem.GetClassName(UIEnum.Panel_Main.ToString());
            self.UIMainController = (UIMainController)UIHelperComponentSystem.LoadWindow(name);
            self.UIMainController.SelfUIPackageEnum = UIPackageEnum.MainPackage;
            self.UIMainController.UiEnum = UIEnum.Panel_Main;
        }

        public static void Show(this UIMainPanelComponent self)
        {
            Game.Scene.GetComponent<UIManagerComponent>().OpenWindow(self.UIMainController);
            self.AddChild<UIPlayerUnitComponent,GComponent>(self.UIMainController.Unit_PlayerData00);//todo 扩展组件awake的方法触发的方法 
            self.AddChild<UIPlayerUnitComponent,GComponent>(self.UIMainController.Unit_PlayerData01);//todo 扩展组件awake的方法触发的方法 
            
            self.OnShow();
        }

        public static void OnShow(this UIMainPanelComponent self)
        {
            self.AddEvent();
        }

        public static void AddEvent(this UIMainPanelComponent self)
        {
            self.UIMainController.Btn_Down.onClick.Add(() =>
            {
                Debug.Log("按下下方向键");
            });
        }

        public static void DelEvent(this UIMainPanelComponent self)
        {
        }

        public static void Hide(this UIMainPanelComponent self)
        {
            Game.Scene.GetComponent<UIManagerComponent>().CloseWindow(self.UIMainController);
            self.DelEvent();
        }

        public static void TurnToOpenPanel(this UIMainPanelComponent self, UIEnum className, UiLayer layer,
        UIPackageEnum packageName = UIPackageEnum.Null, bool isModal = false, params object[] values)
        {
            Game.Scene.GetComponent<UIManagerComponent>().TurnToOpenPanel(self.UIMainController, className, layer, packageName, isModal, values);
        }
    }
}