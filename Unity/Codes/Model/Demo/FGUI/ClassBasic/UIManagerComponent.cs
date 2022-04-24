using System;
using System.Collections.Generic;
using System.Reflection;
using FairyGUI;
using UnityEngine;

namespace ET
{
    public class UIManagerComponent: Entity, IAwake
    {
        //游戏中界面
        //public PlayingPanel PlayingPanel;

        //是否是全面屏
        public bool IsFullScreen = false;

        //界面移动距离
        public int UiMoveDis = 0;

        //全面屏下移距离
        public int FullScreenMove = 60;
        public bool IsPreload = false;

        /// <summary>
        /// 所有ui的管理
        /// </summary>
        public readonly Dictionary<UIEnum, BaseUI> _uiDictionary = new Dictionary<UIEnum, BaseUI>();

        /// <summary>
        /// 所有被打开的ui的集合
        /// </summary>
        public readonly Dictionary<UIEnum, BaseUI> _openedUiDictionary = new Dictionary<UIEnum, BaseUI>();

        /// <summary>
        /// 等待的弹窗集合
        /// </summary>
        public readonly Stack<BaseUI> _waitOpenUiStack = new Stack<BaseUI>();

        public BaseUI _openedPopupUi;
        public BlackPanelComponent _blackPanel = null;

        public int openedUiDictionaryCount => _openedUiDictionary.Count;


    }

    //ui层级
    public enum UiLayer
    {
        //最底层
        Lowest = 0,

        //常驻的ui界面使用，home界面或者游戏中的界面
        Low = 20,

        Effect = 30,

        //一般弹窗次级ui使用
        Middle = 40,

        //主界面金币上方金币显示
        MiddleHigh = 50,

        //通讯、加载等遮罩界面使用
        High = 60,

        //最高层
        Highest = 100
    }
}