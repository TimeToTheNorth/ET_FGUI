using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

public static class FairyCodeHelper
{
    //界面组件命名
    public static string PanelTag = "Panel_";

    //单元组件命名
    public static string UnitTag = "Unit_";

    //按钮命名标记
    public static string BtnTag = "Btn_";

    //文本命名标记
    public static string TextTag = "Text_";

    //富文本
    public static string RichTextTag = "RichText_";

    //装载器
    public static string LoaderTag = "Loader_";

    //进度条
    public static string ProgressBarTag = "Pro_";

    //图片
    public static string ImageTag = "Image_";

    //component
    public static string ComponentTag = "Com_";

    //列表
    public static string ListTag = "List_";
    
    public static string TextInputTag = "TextInput_";
    //组
    public static string GrougTag = "Group_";
    public static string GraphTag = "Graph_";
    public static string SliderTag = "Slider_";


    /// <summary>
    /// 根据标签获取类型
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetType(string name, GObject obj)
    {
        if (name.StartsWith(BtnTag))
            return "GButton";
        else if (name.StartsWith(TextTag))
            return "GTextField";
        else if (name.StartsWith(LoaderTag))
            return "GLoader";
        else if (name.StartsWith(ProgressBarTag))
            return "GProgressBar";
        else if (name.StartsWith(ImageTag))
            return "GImage";
        else if (name.StartsWith(ComponentTag))
            return "GComponent";
        else if (name.StartsWith(ListTag))
            return "GList";
        else if (name.StartsWith(GrougTag))
            return "GGroup";
        else if (name.StartsWith(GraphTag))
            return "GGraph";
        else if (name.StartsWith(SliderTag))
            return "GSlider";
        else if (name.StartsWith(RichTextTag))
            return "GRichTextField";
        else if (name.StartsWith(TextInputTag))
            return "GTextInput";
        else if (name.StartsWith(UnitTag))
        {
            
            return obj.GetType().ToString();
        }

        return "";
    }

    public static string GetEnd(string name)
    {
        if (name.StartsWith(BtnTag))
            return ".asButton";
        else if (name.StartsWith(TextTag))
            return ".asTextField";
        else if (name.StartsWith(LoaderTag))
            return ".asLoader";
        else if (name.StartsWith(ProgressBarTag))
            return ".asProgress";
        else if (name.StartsWith(ImageTag))
            return ".asImage";
        else if (name.StartsWith(ComponentTag))
            return ".asCom";
        else if (name.StartsWith(ListTag))
            return ".asList";
        else if (name.StartsWith(GrougTag))
            return ".asGroup";
        else if (name.StartsWith(GraphTag))
            return ".asGraph";
        else if (name.StartsWith(SliderTag))
            return ".asSlider";
        return "";
    }

    public static string GetClassName(string comName)
    {
        if (comName.StartsWith(PanelTag))
        {
            return comName.Replace(PanelTag, "UI") + "Controller";
        }
        else if (comName.StartsWith(UnitTag))
        {
            return comName.Replace(UnitTag, "UI") + "Compoment";
        }

        return null;
    }

    public static string GetExtentionClassName(string comName, GObject com = null)
    {
        if (comName.StartsWith(PanelTag))
        {
            return "BaseUI";
        }
        else if (comName.StartsWith(UnitTag))
        {
            if (com != null)
            {
                return com.GetType().ToString();
            }

            return "GCompoment";
        }

        return null;
    }

    

    /// <summary>
    /// 获取花图片URL
    /// </summary>
    /// <returns></returns>
    public static string GetHuaUrl()
    {
        return "[img]ui://i0438osrpll3m2[/img]";
    }
}