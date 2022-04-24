using System.IO;
using System.Text;
using UnityEngine;

public static class GenerateETCode
{
    /// <summary>
    /// 生成ET 组件代码
    /// </summary>
    /// <param name="ClassName"></param>
    public static void GenerateETComponentCode(string ClassName,string extensionClass)
    {
        string path = Application.dataPath.Replace("Unity/Assets", "Unity/Codes/Model/Demo/FGUI/Coms/");

        StringBuilder code = new StringBuilder();

        string etClass = null;
        etClass = (string)ClassName.Clone();
        etClass = GetClassName(etClass);
        code.Append("namespace ET\n{");
     
        
        string InheritClassName = "Entity," + "IAwake";
        if (extensionClass!="BaseUI")
        {
            InheritClassName += $",IAwake<{extensionClass}>";
        }
        code.Append($"public class {etClass} :{InheritClassName}\n");
        code.Append("{\n");
        code.Append($"public {ClassName} {ClassName};\n");
        code.Append("}\n");
        code.Append("}\n");

        File.WriteAllText(path + $"{etClass}.cs", code.ToString());
    }

    public static string GetClassName(string comName)
    {
        if (comName.Contains("Controller"))
        {
            return comName.Replace("Controller", "PanelComponent");
        }

        if (comName.Contains("UnitComponentData"))
        {
            return comName.Replace("Data", "");
        }

        return null;
    }

    /// <summary>
    /// 生成ET 组件系统代码
    /// </summary>
    /// <param name="ClassName"></param>
    /// <param name="comName"></param>
    /// <param name="packgeName"></param>
    public static void GenerateETComponentSystemCode(string ClassName, string comName, string packgeName,string extensionClass)
    {
        string path = Application.dataPath.Replace("Unity/Assets", "Unity/Codes/HotfixView/Demo/FGUI/ComSystems/");

        StringBuilder code = new StringBuilder();
        string etClass = null;
        etClass = (string)ClassName.Clone();
        etClass = GetClassName(etClass);
        
        code.Append("using FairyGUI;\n");
        code.Append("namespace ET\n{");
        if (ClassName.Contains("UnitComponentData")) //如果是扩展组件
        {
            GenerateETUnitComponentSystemCode(code, etClass, ClassName,extensionClass);
            code.Append("}\n");
            File.WriteAllText(path + $"{etClass}System.cs", code.ToString());
            return;
        }

        string InheritClassName = $"AwakeSystem<{etClass}>";
        code.Append($"public class {etClass}AwakeSystem :{InheritClassName}\n");
        code.Append("{\n");

        code.Append($"public override void Awake({etClass} self)\n");
        code.Append("{\n");
        code.Append("self.Awake();\n");
        code.Append("}\n");

        code.Append("}\n");

        code.Append($"public static class {etClass}System\n");
        code.Append("{\n");

        code.Append($"public static void Awake(this {etClass} self)\n");
        code.Append("{\n");
        code.Append($"string name= UIHelperComponentSystem.GetClassName(UIEnum.{comName}.ToString());");
        code.Append($"self.{ClassName} =({ClassName}) UIHelperComponentSystem.LoadWindow(name);");
        code.Append($" self.{ClassName}.SelfUIPackageEnum = UIPackageEnum.{packgeName};");
        code.Append($" self.{ClassName}.UiEnum = UIEnum.{comName};");
        code.Append("}\n");

        code.Append($"public static void Show(this {etClass} self)\n");
        code.Append("{\n");
        code.Append($"Game.Scene.GetComponent<UIManagerComponent>().OpenWindow(self.{ClassName});");
        code.Append("self.OnShow();");
        code.Append("}\n");

        code.Append($"public static void OnShow(this {etClass} self)\n");
        code.Append("{\n");
        code.Append("self.AddEvent();");
        code.Append("}\n");

        code.Append($"public static void AddEvent(this {etClass} self)\n");
        code.Append("{\n");

        code.Append("}\n");

        code.Append($"public static void DelEvent(this {etClass} self)\n");
        code.Append("{\n");

        code.Append("}\n");

        code.Append($"public static void Hide(this {etClass} self)\n");
        code.Append("{\n");
        code.Append($"Game.Scene.GetComponent<UIManagerComponent>().CloseWindow(self.{ClassName});");
        code.Append("self.DelEvent();");
        code.Append("}\n");

        code.Append(
            $"public static void TurnToOpenPanel(this {etClass} self, UIEnum className, UiLayer layer, UIPackageEnum packageName = UIPackageEnum.Null, bool isModal = false,params object[] values)\n");
        code.Append("{\n");
        code.Append(
            $"Game.Scene.GetComponent<UIManagerComponent>().TurnToOpenPanel(self.{ClassName}, className, layer,packageName, isModal, values);\n");

        code.Append("}\n");

        code.Append("}\n");

        code.Append("}\n");

        File.WriteAllText(path + $"{etClass}System.cs", code.ToString());
    }

    public static void GenerateETUnitComponentSystemCode(StringBuilder code, string etClass, string ClassName,string extensionClass)
    {
        string InheritClassName = $"AwakeSystem<{etClass},{extensionClass}>";
        code.Append($"public class {etClass}AwakeSystem :{InheritClassName}\n");
        code.Append("{\n");

        code.Append($"public override void Awake({etClass} self, {extensionClass} a)\n");
        code.Append("{\n");
        code.Append("self.Awake(a);\n");
        code.Append("}\n");

        code.Append("}\n");

        code.Append($"public static class {etClass}System\n");
        code.Append("{\n");
        code.Append("//todo 扩展组件的awake组件 并没有自动添加实现 因为组件的awake 往往是跟随ui的开的 根据实际情况自行添加扩展组件即可 这个时候数据已经被准备好了");
        code.Append("\n");
        code.Append($"public static void Awake(this {etClass} self, {extensionClass} gComponent)\n");
        code.Append("{\n");
        code.Append($"self.{ClassName} = ({ClassName})gComponent;\n");

        code.Append("}\n");

        code.Append("}\n");
    }
}