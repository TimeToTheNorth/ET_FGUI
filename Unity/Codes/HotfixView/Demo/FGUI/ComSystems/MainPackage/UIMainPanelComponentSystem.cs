using FairyGUI;

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
            string name = UIManagerComponentSystem.GetClassName(UIEnum.Panel_Main.ToString());
            self.UIMainController = (UIMainController)UIManagerComponentSystem.LoadWindow(name);
            self.UIMainController.SelfUIPackageEnum = UIPackageEnum.MainPackage;
            self.UIMainController.UiEnum = UIEnum.Panel_Main;
        }

        public static void Show(this UIMainPanelComponent self)
        {
            Game.Scene.GetComponent<UIManagerComponent>().OpenWindow(self.UIMainController);
            self.OnShow();
        }

        public static void OnShow(this UIMainPanelComponent self)
        {
            self.AddEvent();
        }

        public static void AddEvent(this UIMainPanelComponent self)
        {
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