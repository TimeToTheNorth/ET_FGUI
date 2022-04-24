using FairyGUI;
using BaseClass;

public static class UIHelper
{
	public static void RegisterCompoment()
	{
	}

	public static void SetIcon(this GLoader loader,UIPackageEnum packageEnum, string resName)
	{
		loader.icon = GetUrl(packageEnum,resName);
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
	Main,
	Public,
}

public enum UIEnum
{
	Panel_Main,
}

#endregion

