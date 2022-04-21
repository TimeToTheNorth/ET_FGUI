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
        private int FullScreenMove = 60;
        public bool IsPreload = false;
        
        /// <summary>
        /// 所有ui的管理
        /// </summary>
        private readonly Dictionary<UIEnum, BaseUI> _uiDictionary = new Dictionary<UIEnum, BaseUI>();
        /// <summary>
        /// 所有被打开的ui
        /// </summary>
        private readonly Dictionary<UIEnum, BaseUI> _openedUiDictionary = new Dictionary<UIEnum, BaseUI>();
        private readonly Stack<BaseUI> _waitOpenUiStack = new Stack<BaseUI>();

        private BaseUI _openedPopupUi;
        private BlackPanelCom _blackPanel = null;

        public int openedUiDictionaryCount => _openedUiDictionary.Count;

        // public static UIManagerComponent Instance
        // {
        //     get
        //     {
        //         return Instance = new UIManagerComponent();
        //     }
        //     private set
        //     {
        //     }
        // }

        public UIManagerComponent()
        {
            if (Screen.height * 1.0f / Screen.width > 2)
            {
                IsFullScreen = true;
                UiMoveDis += FullScreenMove;
            }
            else
            {
                IsFullScreen = false;
                UiMoveDis = 0;
            }

            UIConfig.bringWindowToFrontOnClick = false;
            UIConfig.modalLayerColor = new Color(0f, 0f, 0f, 0.5f);
            GRoot.inst.SetContentScaleFactor(1920, 1080, UIContentScaler.ScreenMatchMode.MatchWidth);
            //        GRoot.inst.scaleY = 1;
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="className"></param>
        /// <param name="layer"></param>
        /// <param name="packageName"></param>
        /// <param name="values"></param>
        public BaseUI OpenWindow(UIEnum className, UIPackageEnum packageName, UiLayer layer = UiLayer.Middle,
        bool isBlack = true,
        params object[] values)
        {
            BaseUI baseUi = GetBaseUI(className, packageName);
            if (baseUi == null) return null;
            if (!baseUi.isShowing)
            {
                if (!_openedUiDictionary.ContainsKey(className))
                    _openedUiDictionary.Add(className, baseUi);
                baseUi.IsPopupWindow = false;
                baseUi.modal = false;
                baseUi.sortingOrder = (int)layer + _openedUiDictionary.Count;
                baseUi.isBlack = isBlack;
                baseUi.SetValues(values);
                baseUi.OpenPanel();
                // if (isBlack) //todo 黑色背景适配暂停使用
                // {
                //     if (_blackPanel == null)
                //     {
                //         _blackPanel = new BlackPanelCom();
                //         _blackPanel.Init();
                //     }
                //
                //     _blackPanel.Push(baseUi);
                // }
            }

            return baseUi;
        }

        /// <summary>
        /// 弹出界面
        /// </summary>
        /// <param name="className"></param>
        /// <param name="layer"></param>
        /// <param name="packageName"></param>
        /// <param name="values"></param>
        public BaseUI PopupWindow(UIEnum className, UIPackageEnum packageName, UiLayer layer = UiLayer.High,
        params object[] values)
        {
            BaseUI baseUi = GetBaseUI(className, packageName);
            if (baseUi == null) return null;
            if (!baseUi.isShowing)
            {
                baseUi.IsPopupWindow = true;
                baseUi.modal = false;
                baseUi.sortingOrder = (int)layer;
                baseUi.SetValues(values);
                _waitOpenUiStack.Push(baseUi);
            }

            if (_waitOpenUiStack.Count > 0)
            {
                ShowPopupWindow();
            }

            return baseUi;
        }

        /// <summary>
        /// 打开弹出界面
        /// </summary>
        private void ShowPopupWindow()
        {
            if (_openedPopupUi == null)
            {
                _openedPopupUi = _waitOpenUiStack.Pop();
                if (_openedPopupUi != null)
                {
                    (_openedPopupUi as BaseUI).OpenPanel();
                }
            }
        }

        /// <summary>
        /// 关闭弹出界面
        /// </summary>
        public void ClosePopupWindow()
        {
            _openedPopupUi = null;
            if (_waitOpenUiStack.Count > 0)
            {
                ShowPopupWindow();
            }
        }

        //关闭界面
        public void CloseWindow(UIEnum className)
        {
            BaseUI window = null;
            if (_uiDictionary.TryGetValue(className, out window))
            {
                BaseUI baseUi = (BaseUI)window;
                if (baseUi.isBlack && _blackPanel != null)
                {
                    _blackPanel.Pop();
                }

                if (baseUi.isShowing)
                {
                    _openedUiDictionary.Remove(className);
                }
                else Debug.LogWarning("CloseWindow window " + className + " not in showing");
            }
            else Debug.LogWarning("CloseWindow window " + className + " not created");
        }

        //获取界面
        public T GetWindow<T>(UIEnum className) where T : BaseUI
        {
            BaseUI window = null;
            if (_uiDictionary.TryGetValue(className, out window))
                return (T)window;

            Debug.LogWarning("GetWindow window " + className + " not created");
            return (T)window;
        }

        //获取打开中的界面
        public bool TryGetOpeningWindow<T>(UIEnum className, out T ui) where T : BaseUI
        {
            BaseUI baseUi;
            if (_openedUiDictionary.TryGetValue(className, out baseUi))
            {
                ui = (T)baseUi;
                return true;
            }

            ui = default (T);
            return false;
        }

        //获取打开中的界面
        public T GetOpeningWindow<T>(UIEnum className) where T : BaseUI
        {
            BaseUI baseUi;
            if (_openedUiDictionary.TryGetValue(className, out baseUi))
                return (T)baseUi;
            Debug.LogError("GetOpenedWindow window " + className + " not opened");
            return (T)baseUi;
        }

        //获取打开过的界面
        public T GetOpenedWindow<T>(UIEnum className) where T : BaseUI
        {
            BaseUI baseUi;
            if (_uiDictionary.TryGetValue(className, out baseUi))
                return (T)baseUi;
            Debug.LogError("GetOpenedWindow window " + className + " not opened");
            return (T)baseUi;
        }

        public bool IsWindowOpen(UIEnum className)
        {
            return _openedUiDictionary.ContainsKey(className);
        }

        //所有打开的界面调用方法
        public void CallAction(string actName)
        {
            foreach (KeyValuePair<UIEnum, BaseUI> valuePair in _openedUiDictionary)
            {
                ((BaseUI)valuePair.Value).CallAction(actName);
            }
        }

        //是否是全屏界面
        private bool IsFullPanel(UIEnum uiEnum)
        {
            return true;
            //return uiEnum == UiEnum.LoadPanel || uiEnum == UiEnum.ChangeSceneMaskPanel;
        }

        //获取UI资源
        private BaseUI GetBaseUI(UIEnum className, UIPackageEnum packageName)
        {
            BaseUI window = null;
            if (!_uiDictionary.TryGetValue(className, out window))
            {
                string name = GetClassName(className.ToString());
                UIPackage.AddPackage("FGUI/" + packageName);
                window = LoadWindow(name);
                window.contentPane = UIPackage.CreateObject(packageName.ToString(), className.ToString()).asCom;
                if (window == null)
                {
                    Debug.LogWarning("OpenWindow create window " + name + " failed");
                    return null;
                }

                window.UiEnum = className;
                _uiDictionary.Add(className, window);
            }

            return (BaseUI)window;
        }

        public static string GetClassName(string comName)
        {
            if (comName.StartsWith("Panel_"))
            {
                return comName.Replace("Panel_", "UI") + "Controller";
            }
            else if (comName.StartsWith("Unit_"))
            {
                return comName.Replace("Unit_", "UI") + "Compoment";
            }

            return null;
        }

        //加载界面
        private BaseUI LoadWindow(string name)
        {
            Type t = CodeLoader.Instance.GetAssembly().GetType("ET." + name);

            ET.BaseUI baseUI = (ET.BaseUI)System.Activator.CreateInstance(t);

            return baseUI;
        }

        public void ShowToast(string str)
        {
            GComponent view = UIPackage.CreateObject("PublicPackage", "ToastWindow").asCom;
            view.GetChild("infoText").asTextField.text = str;
            GRoot.inst.AddChild(view);
            view.Center();
            view.sortingOrder = (int)UiLayer.Highest;
            view.GetTransitionAt(0).Play(() => { GRoot.inst.RemoveChild(view, true); });
        }

        public void PreSetText(Dictionary<int, long> currencies = null)
        {
            //        foreach (var item in currencies)
            //        {
            //            GComponent gc = AssetController.Instance.ShowComponentDic[item.Key];
            //            gc.visible = false;
            //            if (Enum.IsDefined(typeof(CurrencyType),item.Key))
            //            {
            //                if ((CurrencyType) item.Key == CurrencyType.Gold)
            //                {
            //                    gc.GetChild("NumValue").text =
            //                        Global.SetNum((currencies[item.Key] * RoleDataManager.Instance.GoldScale).ToString());
            //                }
            //                else
            //                {
            //                    gc.GetChild("NumValue").text = Global.SetNum(currencies[item.Key].ToString());
            //                }           
            //            }
            //            else
            //            {
            //                gc.GetChild("NumValue").text = "X" + currencies[item.Key].ToString();
            //            }  
            //            gc.position = new Vector2(2000f, 2000f);
            //        }
        }

        //预加载
        public void PreloadUi()
        {
            IsPreload = true;
            IsPreload = false;
        }

        public class BlackPanelCom
        {
            public List<GObject> parents = new List<GObject>();
            public GComponent blackPanel;

            public void Init()
            {
                blackPanel = UIPackage.CreateObject(UIPackageEnum.Public.ToString(), "Com_Black") as GComponent;

                //全屏界面适配
                blackPanel.SetSize(GRoot.inst.width, GRoot.inst.height);

                GRoot.inst.AddChild(blackPanel);
            }

            public void Push(GObject p)
            {
                parents.Add(p);
                blackPanel.visible = true;
                blackPanel.sortingOrder = p.sortingOrder - 1;
            }

            public void Pop()
            {
                if (parents.Count > 0)
                {
                    parents.RemoveAt(parents.Count - 1);
                    if (parents.Count > 0)
                    {
                        blackPanel.sortingOrder = parents[parents.Count - 1].sortingOrder - 1;
                    }
                    else
                    {
                        blackPanel.visible = false;
                    }
                }
                else
                {
                    blackPanel.visible = false;
                }
            }
        }
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