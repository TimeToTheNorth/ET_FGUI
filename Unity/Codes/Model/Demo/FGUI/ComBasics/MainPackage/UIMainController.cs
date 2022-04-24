using FairyGUI;
using FairyGUI.Utils;
using UnityEngine;

namespace ET
{public class UIMainController : BaseUI
{
	#region 自动生成可替换代码

	#region FairyGUI控件

	public GButton Btn_Refresh;
	public GButton Btn_Up;
	public GButton Btn_Down;
	public GButton Btn_Left;
	public GButton Btn_Right;
	public GButton Btn_Seting;
	public GList List_l0;
	public GList List_l1;
	public GComponent Com_Map;
	public GGraph Graph_Myself_Map;
	public GGraph Graph_Enemy_Map;
	public GTextField Text_MoveTimes;
	public GTextField Text_ActionName;
	public FairyGUI.GComponent Unit_PlayerData00;
	public FairyGUI.GComponent Unit_PlayerData01;

	#endregion

	public override void GetAllComponent()
	{
		base.GetAllComponent();

		#region 自动获取FairyGUI控件

		Com_Map = contentPane.GetChild("Com_Map") as GComponent;
		Btn_Refresh = contentPane.GetChild("Btn_Refresh") as GButton;
		Btn_Up = contentPane.GetChild("Btn_Up") as GButton;
		Btn_Down = contentPane.GetChild("Btn_Down") as GButton;
		Btn_Left = contentPane.GetChild("Btn_Left") as GButton;
		Btn_Right = contentPane.GetChild("Btn_Right") as GButton;
		Btn_Seting = contentPane.GetChild("Btn_Seting") as GButton;
		List_l0 = contentPane.GetChild("List_l0") as GList;
		List_l1 = contentPane.GetChild("List_l1") as GList;
		Graph_Myself_Map = Com_Map.GetChild("Graph_Myself") as GGraph;
		Graph_Enemy_Map = Com_Map.GetChild("Graph_Enemy") as GGraph;
		Text_MoveTimes = contentPane.GetChild("Text_MoveTimes") as GTextField;
		Text_ActionName = contentPane.GetChild("Text_ActionName") as GTextField;
		Unit_PlayerData00 = contentPane.GetChild("Unit_PlayerData00") as FairyGUI.GComponent;
		Debug.Log(Unit_PlayerData00.name);
		Unit_PlayerData01 = contentPane.GetChild("Unit_PlayerData01") as FairyGUI.GComponent;
		
		#endregion
	}

	#region 自动注册\销毁按钮事件



	#endregion


	#endregion 自动生成可替换代码结束

	

}

}
