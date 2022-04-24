using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

/// <summary>
/// 所有FGUI 包的注入
/// </summary>
public class UIPackageManager
{
    public string packagePath = "FGUI/";

    public void InitAll()
    {
        UIPackage.AddPackage(packagePath + UIPackageEnum.Main);
        UIPackage.AddPackage(packagePath + UIPackageEnum.Public);
       
    }
}