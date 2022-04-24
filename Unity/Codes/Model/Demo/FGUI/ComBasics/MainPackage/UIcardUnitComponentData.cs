using FairyGUI;
using FairyGUI.Utils;

namespace ET
{public class UIcardUnitComponentData : FairyGUI.GButton
{
	#region 自动生成可替换代码

	#region FairyGUI控件

private Controller _SkillState;
	public GTextField Text_SkillName;
	public GTextField Text_Expend;
	public GTextField Text_Power;
	public GTextField Text_Phase_SkillPhaseCom;
	public GLoader Loader_Icon;
	public GProgressBar Pro_SkillCDProgressBar;
	public GProgressBar Pro_SkillStateProgressBar;
	public GList List_Affixes;
	public GComponent Com_SkillPhaseCom;

	#endregion

	public override void ConstructFromXML(XML xml)
	{
		base.ConstructFromXML(xml);

		GetAllComponent();
	}

	public void GetAllComponent()
	{
		#region 自动获取FairyGUI控件

		Com_SkillPhaseCom = this.GetChild("Com_SkillPhaseCom") as GComponent;
		Pro_SkillCDProgressBar = this.GetChild("Pro_SkillCDProgressBar") as GProgressBar;
		Pro_SkillStateProgressBar = this.GetChild("Pro_SkillStateProgressBar") as GProgressBar;
		_SkillState = GetController("SkillState");
		Text_SkillName = this.GetChild("Text_SkillName") as GTextField;
		Text_Expend = this.GetChild("Text_Expend") as GTextField;
		Text_Power = this.GetChild("Text_Power") as GTextField;
		Text_Phase_SkillPhaseCom = Com_SkillPhaseCom.GetChild("Text_Phase") as GTextField;
		Loader_Icon = this.GetChild("Loader_Icon") as GLoader;
		List_Affixes = this.GetChild("List_Affixes") as GList;

		#endregion
	}


	#endregion 自动生成可替换代码结束

}

}
