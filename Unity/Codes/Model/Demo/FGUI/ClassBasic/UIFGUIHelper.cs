using FairyGUI;

namespace ET
{
    public static class UIFGUIHelper
    {
        public static void RegisterCompoment()
        {
            UIObjectFactory.SetPackageItemExtension("ui://MainPackage/Unit_card", typeof (UIcardUnitComponentData));
            UIObjectFactory.SetPackageItemExtension("ui://MainPackage/Unit_PlayerData", typeof (UIPlayerDataUnitComponentData));
        }

        public static void SetIcon(this GLoader loader, UIPackageEnum packageEnum, string resName)
        {
            loader.icon = GetUrl(packageEnum, resName);
        }

        public static string GetUrl(UIPackageEnum packageEnum, string resName)
        {
            return $"ui://{packageEnum.ToString()}/{resName}";
        }
    }

    #region 自动生成FGUI枚举代码

    public enum UIPackageEnum
    {
        Null,
        MainPackage,
    }

    public enum UIEnum
    {
        Panel_Main,
    }

    #endregion
}