using FairyGUI;
using FairyGUI.Utils;

namespace ET
{public class UIPlayerDataUnitComponentData : FairyGUI.GComponent
{
	#region 自动生成可替换代码

	#region FairyGUI控件

	public GLoader Loader_PlayerIcon;
	public GTextField Text_PlayerName;
	public GTextField Text_Profession;
	public GTextField Text_Hp;
	public GTextField Text_Energy;
	public GTextField Text_Attack;
	public GTextField Text_Defense;
	public GTextField Text_Speed;
	public GProgressBar Pro_HP;
	public GProgressBar Pro_Energy;
	public GProgressBar Pro_AttackValueBar;
	public GProgressBar Pro_DefenseValueBar;
	public GProgressBar Pro_SpeedValueBar;

	#endregion

	public override void ConstructFromXML(XML xml)
	{
		base.ConstructFromXML(xml);

		GetAllComponent();
	}

	public void GetAllComponent()
	{
		#region 自动获取FairyGUI控件

		Pro_HP = this.GetChild("Pro_HP") as GProgressBar;
		Pro_Energy = this.GetChild("Pro_Energy") as GProgressBar;
		Pro_AttackValueBar = this.GetChild("Pro_AttackValueBar") as GProgressBar;
		Pro_DefenseValueBar = this.GetChild("Pro_DefenseValueBar") as GProgressBar;
		Pro_SpeedValueBar = this.GetChild("Pro_SpeedValueBar") as GProgressBar;
		Loader_PlayerIcon = this.GetChild("Loader_PlayerIcon") as GLoader;
		Text_PlayerName = this.GetChild("Text_PlayerName") as GTextField;
		Text_Profession = this.GetChild("Text_Profession") as GTextField;
		Text_Hp = this.GetChild("Text_Hp") as GTextField;
		Text_Energy = this.GetChild("Text_Energy") as GTextField;
		Text_Attack = this.GetChild("Text_Attack") as GTextField;
		Text_Defense = this.GetChild("Text_Defense") as GTextField;
		Text_Speed = this.GetChild("Text_Speed") as GTextField;

		#endregion
	}


	#endregion 自动生成可替换代码结束

}

}
