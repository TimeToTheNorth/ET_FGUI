using System.IO;
using System.Text;
using UnityEngine;


    public static class GenerateETCode
    {
        public static void GenerateCode(string ClassName)
        {
            
          string  path = Application.dataPath.Replace("Unity/Assets", "Unity/Codes/Model/Demo/FGUI/Coms/");
            
            StringBuilder code = new StringBuilder();

            string etClass = null;
            etClass = (string)ClassName.Clone();
            etClass=   GetClassName(etClass);
            code.Append("namespace ET\n{");
            string extentionClass = "Entity," + "IAwake";
            code.Append($"public class {etClass} :{extentionClass}\n");
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
            
            return null;
        }


    }
