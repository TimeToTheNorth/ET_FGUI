using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FairyGUI;
using FairyGUIEditor;
using UnityEditor;
using UnityEngine;

public class FairyCodeTool: BaseWindow
{
    int selectedPackage = -1;
    string selectedPackageName;
    string selectedComponentName;
    private Vector2 _scrollPos;

    private Vector2 _scrollPos2;

    //  private string generateScriptPath = "Assets/Scripts/UI/";
    //  private string generateScriptPath1 = "/.../Scripts/UI/";
    // private string baseScriptPath = "Assets/Scripts/UI/BaseClass/";
    private string packageResPath = "Assets/Resources/FGUI/";

    //    private static  string GenSystemCodePath =  Application.dataPath.Replace("Unity/Assets", "Unity/Codes/HotfixView/Demo/FGUI/");
    private static string GenComponetCodePath;
    //  private static  string GenFguiinitCodePath = Application.dataPath + "/../Codes/HotfixView/Demo/FGUI";

    private List<UIPackage> pkgs;
    private int cnt;
    private string packageDes;

    [MenuItem("Custom Editor/FairyGUI代码生成")]
    static void Init()
    {
        UnityEditor.EditorWindow window = GetWindow(typeof (FairyCodeTool));
        window.Show();
    }

    private void Awake()
    {
        EditorToolSet.ReloadPackages();
        pkgs = UIPackage.GetPackages();
        cnt = pkgs.Count;
    }

    private void OnGUI()
    {
        if (pkgs == null)
        {
            EditorToolSet.LoadPackages();
            pkgs = UIPackage.GetPackages();
            cnt = pkgs.Count;
        }

        GUILayout.BeginVertical(GUILayout.Width(500));
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(GUI.skin.box);
        if (cnt == 0)
        {
            selectedPackage = -1;
            selectedPackageName = null;
        }
        else
        {
            _scrollPos2 = GUILayout.BeginScrollView(_scrollPos2, GUILayout.Width(250));
            for (int i = 0; i < cnt; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(4);

                packageDes = pkgs[i].name + "---依赖:";
                for (int j = 0; j < pkgs[i].dependencies.Length; j++)
                {
                    packageDes += $"[{pkgs[i].dependencies[j]["name"]}]";
                }

                if (GUILayout.Toggle(selectedPackageName == pkgs[i].name, packageDes))
                {
                    selectedPackage = i;
                    selectedPackageName = pkgs[i].name;
                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal(GUILayout.Width(250));
            if (GUILayout.Button("生成所有组件枚举脚本"))
            {
                GenerateEnumCode(pkgs);
            }

            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
        GUILayout.BeginVertical(GUI.skin.box);
        _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Width(250));
        if (selectedPackage >= 0)
        {
            foreach (PackageItem pi in pkgs[selectedPackage].GetItems())
            {
                if (pi.type == PackageItemType.Component && pi.exported &&
                    (pi.name.StartsWith(FairyCodeHelper.PanelTag) || pi.name.StartsWith(FairyCodeHelper.UnitTag)))
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(4);
                    if (GUILayout.Toggle(selectedComponentName == pi.name, pi.name))
                    {
                        selectedComponentName = pi.name;
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        GUILayout.EndScrollView();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("导入UI脚本") && selectedPackage >= 0)
        {
            UIPackage selectedPkg = pkgs[selectedPackage];
            GObject view = CreateCom(pkgs, selectedPkg.name, selectedComponentName);
            GenerateCode(selectedComponentName, selectedPkg.name, view, false);
            view.Dispose();
        }

        EditorGUILayout.EndHorizontal();
        // EditorGUILayout.BeginHorizontal();
        // if (GUILayout.Button("生成可替换代码到剪切板"))
        // {
        //     UIPackage selectedPkg = pkgs[selectedPackage];
        //     GObject view = CreateCom(pkgs, selectedPkg.name, selectedComponentName);
        //     GenerateCode(selectedComponentName, selectedPkg.name, view, true);
        //     view.Dispose();
        // }
        //
        // EditorGUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("命名规则");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("主界面  \"Panel_\"\n装载器  \"Loader_\"\n进度条  \"Pro_\"\n按钮     \"Btn_\"\n文本     \"Text_\"\n图片     \"Image_\"\n组件   " +
            "  \"Com_\"\n列表     \"List_\"\n图像     \"Graph_\"\n组       \"Group_\" \n输入文本       \"TextInput_\"    \n扩展组件  \"Unit_");
        EditorGUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private GObject CreateCom(List<UIPackage> pkgs, string packageName, string componentName)
    {
        int cnt = pkgs.Count;
        for (int i = 0; i < cnt; i++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(4);
            if (GUILayout.Toggle(selectedPackageName == pkgs[i].name, pkgs[i].name))
            {
                selectedPackage = i;
                selectedPackageName = pkgs[i].name;
            }

            EditorGUILayout.EndHorizontal();
        }

        UIPackage.AddPackage(packageResPath + packageName);
        GObject view = UIPackage.CreateObject(packageName, componentName).asCom;
        return view;
    }

    private Dictionary<string, StringBuilder> _nameCodeDic = new Dictionary<string, StringBuilder>();
    private Dictionary<string, StringBuilder> _getDic = new Dictionary<string, StringBuilder>();
    private Dictionary<string, StringBuilder> _setTxtDic = new Dictionary<string, StringBuilder>();
    private Dictionary<string, StringBuilder> _btnClickAddDic = new Dictionary<string, StringBuilder>();
    private Dictionary<string, StringBuilder> _btnClickRemoveDic = new Dictionary<string, StringBuilder>();
    private List<StringBuilder> btnClickMethod = new List<StringBuilder>();

    /// <summary>
    /// 生成代码
    /// </summary>
    /// <param name="comName"></param>
    /// <param name="packgeName"></param>
    /// <param name="view"></param>
    /// <param name="isReGet"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private string GenerateCode(string comName, string packgeName, GObject view, bool isReGet)
    {
        GenComponetCodePath = Application.dataPath.Replace("Unity/Assets", "Unity/Codes/Model/Demo/FGUI/ComBasics/");
        _nameCodeDic = new Dictionary<string, StringBuilder>();
        _getDic = new Dictionary<string, StringBuilder>();
        _setTxtDic = new Dictionary<string, StringBuilder>();
        _btnClickAddDic = new Dictionary<string, StringBuilder>();
        _btnClickRemoveDic = new Dictionary<string, StringBuilder>();
        btnClickMethod = new List<StringBuilder>();

        StringBuilder code = new StringBuilder();

        //  string path = generateScriptPath + packgeName + "/";
        string path = GenComponetCodePath + packgeName + "/";
        string className = FairyCodeHelper.GetClassName(comName);
        string extentionClass = FairyCodeHelper.GetExtentionClassName(comName, view);
        ReadAllChild(view, 0, "", extentionClass);

        if (className == null)
        {
            throw new Exception($"组件名格式错误{comName}");
        }

        isReGet = File.Exists(path + $"{className}.cs");

        if (!isReGet)
        {
            code.Append("using FairyGUI;\n");
            // code.Append("using BaseClass;\n");
            code.Append("using FairyGUI.Utils;\n\n");
            code.Append("namespace ET\n{");
            code.Append($"public class {className} : {extentionClass}\n");
            code.Append("{\n");
        }

        code.Append("\t#region 自动生成可替换代码\n\n");
        code.Append("\t#region FairyGUI控件\n\n");
        foreach (var keyValuePair in _nameCodeDic)
        {
            code.Append(keyValuePair.Value);
        }

        code.Append("\n\t#endregion\n\n");

        if (extentionClass == "BaseUI")
        {
            // code.Append($"\tpublic static {className} GetInstance()\n");
            // code.Append("\t{\n");
            // code.Append($"\t\treturn UIManagerComponent.GetWindow<{className}>(UIEnum.{comName});\n");
            // code.Append("\t}\n\n");
        }

        if (extentionClass == "BaseUI")
        {
            code.Append("\tpublic override void GetAllComponent()\n");
        }
        else
        {
            code.Append("\tpublic override void ConstructFromXML(XML xml)\n");
            code.Append("\t{\n");
            code.Append("\t\tbase.ConstructFromXML(xml);\n\n");
            code.Append("\t\tGetAllComponent();\n");
            code.Append("\t}\n\n");

            code.Append("\tpublic void GetAllComponent()\n");
        }

        code.Append("\t{\n");
        if (extentionClass == "BaseUI")
        {
            code.Append("\t\tbase.GetAllComponent();\n\n");
        }

        code.Append("\t\t#region 自动获取FairyGUI控件\n\n");
        //先写入组件类型
        if (_getDic.TryGetValue("GComponent", out StringBuilder stringBuilder))
        {
            code.Append(stringBuilder);
            _getDic.Remove("GComponent");
        }

        //先写入按钮类型
        if (_getDic.TryGetValue("GButton", out StringBuilder stringBuilderButton))
        {
            code.Append(stringBuilderButton);
            _getDic.Remove("GButton");
        }

        //先写入进度条类型
        if (_getDic.TryGetValue("GProgressBar", out StringBuilder stringBuilderPro))
        {
            code.Append(stringBuilderPro);
            _getDic.Remove("GProgressBar");
        }

        foreach (var keyValuePair in _getDic)
        {
            code.Append(keyValuePair.Value);
        }

        code.Append("\n");
        code.Append("\t\t#endregion\n");
        code.Append("\t}\n\n");

        // if (_btnClickAddDic.Count > 0)
        // {
        //     code.Append("\t#region 自动注册\\销毁按钮事件\n\n");
        //     if (extentionClass == "BaseUI")
        //     {
        //         code.Append("\tpublic override void AddEvent()\n");
        //     }
        //     else
        //     {
        //         code.Append("\tpublic void AddEvent()\n");
        //     }
        //
        //     code.Append("\t{\n");
        //     foreach (var keyValuePair in _btnClickAddDic)
        //     {
        //         code.Append(keyValuePair.Value);
        //     }
        //
        //     code.Append("\t}\n\n");
        //
        //     if (extentionClass == "BaseUI")
        //     {
        //         code.Append("\tpublic override void DelEvent()\n");
        //     }
        //     else
        //     {
        //         code.Append("\tpublic void DelEvent()\n");
        //     }
        //
        //     code.Append("\t{\n");
        //     foreach (var keyValuePair in _btnClickRemoveDic)
        //     {
        //         code.Append(keyValuePair.Value);
        //     }
        //
        //     code.Append("\t}\n\n");
        //     code.Append("\t#endregion\n\n");
        // }

        code.Append("\n\t#endregion 自动生成可替换代码结束\n\n");

        // if (extentionClass != "BaseUI" && !isReGet)
        // {
        //     code.Append("\tpublic void Init()\n");
        //     code.Append("\t{\n");
        //     code.Append("\t\tDelEvent();\n");
        //     code.Append("\t\tAddEvent();\n");
        //     code.Append("\t}\n");
        // }

        if (!isReGet)
        {
            foreach (var item in btnClickMethod)
            {
                code.Append("\t");
                code.Append(item);
            }

            code.Append("}\n\n");
            code.Append("}\n");
        }

        if (!string.IsNullOrEmpty(code.ToString()))
        {
            DirectoryInfo info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }

            if (isReGet)
            {
                StringBuilder ss = new StringBuilder();

                using (FileStream stream = File.OpenRead(path + $"{className}.cs"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        bool isIgnor = false;
                        bool isAdd = false;

                        while (!reader.EndOfStream)
                        {
                            string s = reader.ReadLine() + "\n";
                            if (s.Contains("自动生成可替换代码"))
                            {
                                isIgnor = true;
                                if (!isAdd)
                                {
                                    ss.Append(code);
                                }

                                isAdd = true;
                            }

                            if (!isIgnor)
                            {
                                ss.Append(s);
                            }

                            if (s.Contains("自动生成可替换代码结束"))
                            {
                                isIgnor = false;
                            }
                        }
                    }
                }

                File.WriteAllText(path + $"{className}.cs", ss.ToString());

                Debug.LogError("脚本已存在，重新导入成功");
            }
            else
            {
                File.WriteAllText(path + $"{className}.cs", code.ToString());
                GenerateETCode.GenerateETComponentCode(className,extentionClass);
                GenerateETCode.GenerateETComponentSystemCode(className, comName, packgeName,extentionClass);
                Debug.Log("代码自动生成成功");
                AssetDatabase.Refresh();
            }
        }
        else
        {
            Debug.LogError("生成代码有误");
        }

        return code.ToString();
    }

    private string GenerateEnumCode(List<UIPackage> packages)
    {
        string path = Application.dataPath.Replace("Unity/Assets", "Unity/Codes/Model/Demo/FGUI/ClassBasic/");
        DirectoryInfo info = new DirectoryInfo(path);
        if (!info.Exists)
        {
            Directory.CreateDirectory(path);
        }

        string filePath = path + "UIFGUIHelper.cs";

        StringBuilder code = new StringBuilder();

        code.Append("using FairyGUI;\n");
        code.Append("namespace ET");
        code.Append("{\n");
        code.Append("public static class UIFGUIHelper\n");
        code.Append("{\n");
        code.Append("\tpublic static void RegisterCompoment()\n");
        code.Append("\t{");

        foreach (var package in packages)
        {
            foreach (var item in package.GetItems())
            {
                if (item.type == PackageItemType.Component && item.name.StartsWith(FairyCodeHelper.UnitTag) &&
                    item.exported)
                {
                    code.Append("\n");
                    code.Append(
                        $"\t\tUIObjectFactory.SetPackageItemExtension(\"ui://{package.name}/{item.name}\", typeof({FairyCodeHelper.GetClassName(item.name)}));");
                }
            }
        }

        code.Append("\n\t}\n\n");

        code.Append("\tpublic static void SetIcon(this GLoader loader,UIPackageEnum packageEnum, string resName)\n");
        code.Append("\t{\n");
        code.Append("\t\tloader.icon = GetUrl(packageEnum,resName);\n");
        code.Append("\t}\n");

        code.Append("\tpublic static string GetUrl(UIPackageEnum packageEnum, string resName)\n");
        code.Append("\t{\n");
        code.Append("\t\treturn $\"ui://{packageEnum.ToString()}/{resName}\";\n");
        code.Append("\t}\n");

        code.Append("}\n\n");

        code.Append("#region 自动生成FGUI枚举代码\n\n");

        code.Append("public enum UIPackageEnum\n");
        code.Append("{\n");

        code.Append("\t");
        code.Append("Null,\n");
        foreach (var package in packages)
        {
            code.Append("\t");
            code.Append(package.name + ",\n");
        }

        code.Append("}\n\n");

        code.Append("public enum UIEnum\n");
        code.Append("{\n");

        foreach (var package in packages)
        {
            foreach (var item in package.GetItems())
            {
                if (item.type == PackageItemType.Component && item.name.StartsWith(FairyCodeHelper.PanelTag))
                {
                    code.Append("\t");
                    code.Append(item.name + ",\n");
                }
            }
        }

        code.Append("}\n\n");
        code.Append("#endregion\n\n");

        code.Append("}\n\n");
        File.WriteAllText(filePath, code.ToString());
        AssetDatabase.Refresh();
        Debug.Log("代码自动生成成功");
        return null;
    }

    /// <summary>
    /// _nameCodeDic获取名字  _getDic获取类型
    /// </summary>
    /// <param name="view"></param>
    /// <param name="index"></param>
    /// <param name="parentName"></param>
    /// <param name="extention"></param>
    private void ReadAllChild(GObject view, int index = 0, string parentName = "", string extention = "")
    {
        string typeName = FairyCodeHelper.GetType(view.name, view);
        if (typeName != "" && view.name != "")
        {
            string comName = view.name;
            if (!_nameCodeDic.TryGetValue(typeName, out StringBuilder stringBuilder))
            {
                stringBuilder = new StringBuilder();
                _nameCodeDic.Add(typeName, stringBuilder);
            }

            stringBuilder.Append("\t" + "public " + typeName + " " + GetName(comName, parentName) + ";\n");

            if (!_getDic.TryGetValue(typeName, out StringBuilder getBuilder))
            {
                getBuilder = new StringBuilder();
                _getDic.Add(typeName, getBuilder);
            }

            getBuilder.Append("\t\t" + GetName(comName, parentName) + " = ");
            getBuilder.Append(parentName + ".GetChild(\"" + view.name + "\") as " + typeName + ";\n");

            if (string.Equals(typeName, "GTextField") || string.Equals(typeName, "RichTextField"))
            {
                if (!_setTxtDic.TryGetValue(typeName, out StringBuilder setTxtBuilder))
                {
                    setTxtBuilder = new StringBuilder();
                    _setTxtDic.Add(typeName, setTxtBuilder);
                }

                setTxtBuilder.Append("\t\t" + GetName(comName, parentName) + ".text" + " = \"\";\n");
            }

            if (string.Equals(typeName, "GTextInput")) //输入文本
            {
                if (!_setTxtDic.TryGetValue(typeName, out StringBuilder setTxtBuilder))
                {
                    setTxtBuilder = new StringBuilder();
                    _setTxtDic.Add(typeName, setTxtBuilder);
                }

                setTxtBuilder.Append("\t\t" + GetName(comName, parentName) + ".text" + " = \"\";\n");
            }

            if (string.Equals(typeName, "GButton"))
            {
                if (!_btnClickAddDic.TryGetValue(typeName, out StringBuilder btnClickBuilder))
                {
                    btnClickBuilder = new StringBuilder();
                    _btnClickAddDic.Add(typeName, btnClickBuilder);
                }

                btnClickBuilder.Append("\t\t" + GetName(comName, parentName) + $".onClick.Add(OnClick{comName});\n");

                if (!_btnClickRemoveDic.TryGetValue(typeName, out StringBuilder btnClickRemoveBuilder))
                {
                    btnClickRemoveBuilder = new StringBuilder();
                    _btnClickRemoveDic.Add(typeName, btnClickRemoveBuilder);
                }

                btnClickRemoveBuilder.Append("\t\t" + GetName(comName, parentName) +
                    $".onClick.Remove(OnClick{comName});\n");

                StringBuilder methodBuilder = new StringBuilder();
                methodBuilder.Append(GetMethodStr("OnClick" + comName));
                btnClickMethod.Add(methodBuilder);
            }
        }

        if (view is GComponent)
        {
            GComponent com = view as GComponent;
            if (com.numChildren > 0 && (FairyCodeHelper.GetType(com.name, view) == "GButton" ||
                FairyCodeHelper.GetType(com.name, view) == "GProgressBar" ||
                FairyCodeHelper.GetType(com.name, view) == "GComponent" || com.name == ""))
            {
                GetTransition(com, parentName, extention);
                GetController(com, parentName, extention);
                for (int i = 0; i < com.numChildren; i++)
                {
                    if (parentName == "")
                    {
                        if (extention == "BaseUI")
                        {
                            ReadAllChild(com.GetChildAt(i), i, "contentPane");
                        }
                        else
                        {
                            ReadAllChild(com.GetChildAt(i), i, "this");
                        }
                    }
                    else
                    {
                        ReadAllChild(com.GetChildAt(i), i, GetName(com.name, parentName));
                    }
                }
            }
        }
    }

    private void GetTransition(GComponent com, string parentName, string extention)
    {
        for (int i = 0; i < com.Transitions.Count; i++)
        {
            if (!_nameCodeDic.TryGetValue("Transition", out StringBuilder stringBuilder))
            {
                stringBuilder = new StringBuilder();
                _nameCodeDic.Add("Transition", stringBuilder);
            }

            stringBuilder.Append("private Transition " + GetName(com.Transitions[i].name, "_" + com.name) + ";\n");

            if (!_getDic.TryGetValue("Transition", out StringBuilder getBuilder))
            {
                getBuilder = new StringBuilder();
                _getDic.Add("Transition", getBuilder);
            }

            getBuilder.Append("\t\t" + GetName(com.Transitions[i].name, "_" + com.name) + " = ");
            if (com.name == "")
            {
                string s = "";
                if (extention == "BaseUI")
                {
                    s = "contentPane.";
                }
                else if (extention == "GCompoment")
                {
                    s = "";
                }

                getBuilder.Append($"{s}GetTransition(\"" + com.Transitions[i].name + "\")" + ";\n");
            }
            else
            {
                getBuilder.Append(GetName(com.name, parentName) + ".GetTransition(\"" + com.Transitions[i].name +
                    "\")" + ";\n");
            }
        }
    }

    private void GetController(GComponent com, string parentName, string extention)
    {
        for (int i = 0; i < com.Controllers.Count; i++)
        {
            if (com.Controllers[i].name.StartsWith("~") || com.Controllers[i].name == "button")
            {
                continue;
            }

            if (!_nameCodeDic.TryGetValue("Controller", out StringBuilder stringBuilder))
            {
                stringBuilder = new StringBuilder();
                _nameCodeDic.Add("Controller", stringBuilder);
            }

            stringBuilder.Append("private Controller " + GetName(com.Controllers[i].name, "_" + com.name) + ";\n");

            if (!_getDic.TryGetValue("Controller", out StringBuilder getBuilder))
            {
                getBuilder = new StringBuilder();
                _getDic.Add("Controller", getBuilder);
            }

            getBuilder.Append("\t\t" + GetName(com.Controllers[i].name, "_" + com.name) + " = ");
            if (com.name == "")
            {
                string s = "";
                if (extention == "BaseUI")
                {
                    s = "contentPane.";
                }
                else if (extention == "GCompoment")
                {
                    s = "";
                }

                getBuilder.Append($"{s}GetController(\"" + com.Controllers[i].name + "\")" + ";\n");
            }
            else
            {
                getBuilder.Append(GetName(com.name, parentName) + ".GetController(\"" + com.Controllers[i].name +
                    "\")" + ";\n");
            }
        }
    }

    private string GetName(string name, string parentName = "")
    {
        if (parentName != "_" && parentName != "contentPane" && parentName != "this")
        {
            string[] strings = parentName.Split('_');
            return name + "_" + strings[1];
        }
        else if (name.Contains("_"))
        {
            return name;
        }

        return "_" + name;
    }

    private StringBuilder GetMethodStr(string methodName)
    {
        StringBuilder str = new StringBuilder();
        if (methodName != "")
        {
            str.Append($"public void {methodName}()\n");
            str.Append("\t{\n");
            str.Append("\t}\n\n");
        }

        return str;
    }

    public void CopyToClipboard(string input)
    {
        TextEditor t = new TextEditor();
        t.text = input;
        t.OnFocus();
        t.Copy();
    }
}