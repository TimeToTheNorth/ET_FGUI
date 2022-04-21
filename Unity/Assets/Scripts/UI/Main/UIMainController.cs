using FairyGUI;
using BaseClass;
using FairyGUI.Utils;
using UnityEngine;

public class UIMainController : BaseUI
{
	#region 自动生成可替换代码

	#region FairyGUI控件

	public GGraph Graph_Ground;
	public GTextField Text_Hello;

	#endregion

	public static UIMainController GetInstance()
	{
		return UIManager.Instance.GetWindow<UIMainController>(UIEnum.Panel_Main);
	}

	public override void GetAllComponent()
	{
		base.GetAllComponent();

		#region 自动获取FairyGUI控件

		Graph_Ground = contentPane.GetChild("Graph_Ground") as GGraph;
		Text_Hello = contentPane.GetChild("Text_Hello") as GTextField;

		#endregion
	}


	#endregion 自动生成可替换代码结束

	public override void AddEvent()
	{
		Graph_Ground.onClick.Add(() =>
		{
			Debug.Log("111111");
		});
		
	}
}

