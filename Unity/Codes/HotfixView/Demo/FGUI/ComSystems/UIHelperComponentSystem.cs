using System;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace ET
{
    public class UIHelperComponentAwakeSystem: AwakeSystem<UIManagerComponent>
    {
        public override void Awake(UIManagerComponent self)
        {
            self.Awake();
        }
    }

    public static class UIHelperComponentSystem
    {
        public static void Awake(this UIManagerComponent self)
        {
            if (Screen.height * 1.0f / Screen.width > 2)
            {
                self.IsFullScreen = true;
                self.UiMoveDis += self.FullScreenMove;
            }
            else
            {
                self.IsFullScreen = false;
                self.UiMoveDis = 0;
            }

            UIConfig.bringWindowToFrontOnClick = false;
            UIConfig.modalLayerColor = new Color(0f, 0f, 0f, 0.5f);
            GRoot.inst.SetContentScaleFactor(1920, 1080, UIContentScaler.ScreenMatchMode.MatchWidth); //设置设计分辨率
            UIFGUIHelper.RegisterCompoment();
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="className"></param>
        /// <param name="layer"></param>
        /// <param name="packageName"></param>
        /// <param name="values"></param>
        public static BaseUI OpenWindow(this UIManagerComponent self, UIEnum className, UIPackageEnum packageName, UiLayer layer = UiLayer.Middle,
        bool isBlack = false,
        params object[] values)
        {
            BaseUI baseUi = self.GetBaseUI(className, packageName);
            if (baseUi == null) return null;
            if (!baseUi.isShowing)
            {
                if (!self._openedUiDictionary.ContainsKey(className))
                    self._openedUiDictionary.Add(className, baseUi);
                baseUi.IsPopupWindow = false;
                baseUi.modal = false;
                baseUi.sortingOrder = (int)layer + self._openedUiDictionary.Count;
                baseUi.isBlack = isBlack;
                baseUi.SetValues(values);
                baseUi.OpenPanel();

                if (isBlack)
                {
                    if (self._blackPanel == null)
                    {
                        self._blackPanel = self.AddComponent<BlackPanelComponent>();
                    }

                    self._blackPanel.Push(baseUi);
                }
            }

            return baseUi;
        }

        public static BaseUI OpenWindow(this UIManagerComponent self, BaseUI baseUI, UiLayer layer = UiLayer.Middle)
        {
            self.GetUIAsset(baseUI, baseUI.UiEnum, baseUI.SelfUIPackageEnum);
            if (baseUI == null) return null;
            if (!baseUI.isShowing)
            {
                if (!self._openedUiDictionary.ContainsKey(baseUI.UiEnum))
                    self._openedUiDictionary.Add(baseUI.UiEnum, baseUI);
                baseUI.IsPopupWindow = false;
                baseUI.modal = false;
                baseUI.sortingOrder = (int)layer + self._openedUiDictionary.Count;

                baseUI.OpenPanel();
              
            }

            return baseUI;
        }

        //获取UIbase
        public static BaseUI GetBaseUI(this UIManagerComponent self, UIEnum className, UIPackageEnum packageName)
        {
            BaseUI window = null;
            if (!self._uiDictionary.TryGetValue(className, out window))
            {
                string name = GetClassName(className.ToString());
                UIPackage.AddPackage("FGUI/" + packageName);
                window = LoadWindow(name);
                window.contentPane = UIPackage.CreateObject(packageName.ToString(), className.ToString()).asCom;

                if (window.contentPane.GetChild("AdaptivePanel") != null && self.IsFullScreen)
                {
                    window.contentPane.GetChild("AdaptivePanel").y += self.UiMoveDis;
                    window.contentPane.GetChild("AdaptivePanel").height -= self.UiMoveDis;
                } //全面屏适配

                if (window == null)
                {
                    Debug.LogWarning("OpenWindow create window " + name + " failed");
                    return null;
                }

                window.UiEnum = className;
                self._uiDictionary.Add(className, window);
            }

            return (BaseUI)window;
        }

        //获取UI资源
        public static BaseUI GetUIAsset(this UIManagerComponent self, BaseUI baseUI, UIEnum className, UIPackageEnum packageName)
        {
            BaseUI window = null;
            if (!self._uiDictionary.TryGetValue(className, out window))
            {
                UIPackage.AddPackage("FGUI/" + packageName);
                window = baseUI;
                window.contentPane = UIPackage.CreateObject(packageName.ToString(), className.ToString()).asCom;

                if (window.contentPane.GetChild("AdaptivePanel") != null && self.IsFullScreen)
                {
                    window.contentPane.GetChild("AdaptivePanel").y += self.UiMoveDis;
                    window.contentPane.GetChild("AdaptivePanel").height -= self.UiMoveDis;
                } //全面屏适配

                self._uiDictionary.Add(className, window);
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
        public static BaseUI LoadWindow(string name)
        {
            Type t = CodeLoader.Instance.GetAssembly().GetType("ET." + name);

            ET.BaseUI baseUI = (ET.BaseUI)System.Activator.CreateInstance(t);

            return baseUI;
        }

        //关闭界面
        public static void CloseWindow(this UIManagerComponent self, UIEnum className)
        {
            BaseUI window = null;
            if (self._uiDictionary.TryGetValue(className, out window))
            {
                BaseUI baseUi = (BaseUI)window;
                if (baseUi.isBlack && self._blackPanel != null)
                {
                    self._blackPanel.Pop();
                }

                if (baseUi.isShowing)
                {
                    baseUi.ClosePanel();
                    self._openedUiDictionary.Remove(className);
                }
                else Debug.LogWarning("CloseWindow window " + className + " not in showing");
            }
            else Debug.LogWarning("CloseWindow window " + className + " not created");
        }

        public static void CloseWindow(this UIManagerComponent self, BaseUI baseUi)
        {
            if (baseUi.isBlack && self._blackPanel != null)
            {
                self._blackPanel.Pop();
            }

            if (baseUi.isShowing)
            {
                baseUi.ClosePanel();
                self._openedUiDictionary.Remove(baseUi.UiEnum);
            }
        }

        /// <summary>
        /// 弹出界面
        /// </summary>
        /// <param name="className"></param>
        /// <param name="layer"></param>
        /// <param name="packageName"></param>
        /// <param name="values"></param>
        public static BaseUI PopupWindow(this UIManagerComponent self, UIEnum className, UIPackageEnum packageName, UiLayer layer = UiLayer.High,
        params object[] values)
        {
            BaseUI baseUi = self.GetBaseUI(className, packageName);
            if (baseUi == null) return null;
            if (!baseUi.isShowing)
            {
                baseUi.IsPopupWindow = true;
                baseUi.modal = false;
                baseUi.sortingOrder = (int)layer;
                baseUi.SetValues(values);
                self._waitOpenUiStack.Push(baseUi);
            }

            if (self._waitOpenUiStack.Count > 0)
            {
                self.ShowPopupWindow();
            }

            return baseUi;
        }

        /// <summary>
        /// 打开弹出界面
        /// </summary>
        private static void ShowPopupWindow(this UIManagerComponent self)
        {
            if (self._openedPopupUi == null)
            {
                self._openedPopupUi = self._waitOpenUiStack.Pop();
                if (self._openedPopupUi != null)
                {
                    (self._openedPopupUi as BaseUI).OpenPanel();
                }
            }
        }

        /// <summary>
        /// 关闭弹出界面
        /// </summary>
        public static void ClosePopupWindow(this UIManagerComponent self)
        {
            self._openedPopupUi = null;
            if (self._waitOpenUiStack.Count > 0)
            {
                self.ShowPopupWindow();
            }
        }

        //获取界面
        public static T GetWindow<T>(this UIManagerComponent self, UIEnum className) where T : BaseUI
        {
            BaseUI window = null;
            if (self._uiDictionary.TryGetValue(className, out window))
                return (T)window;

            Debug.LogWarning("GetWindow window " + className + " not created");
            return (T)window;
        }

        //获取打开中的界面
        public static bool TryGetOpeningWindow<T>(this UIManagerComponent self, UIEnum className, out T ui) where T : BaseUI
        {
            BaseUI baseUi;
            if (self._openedUiDictionary.TryGetValue(className, out baseUi))
            {
                ui = (T)baseUi;
                return true;
            }

            ui = default (T);
            return false;
        }

        //获取打开中的界面
        public static T GetOpeningWindow<T>(this UIManagerComponent self, UIEnum className) where T : BaseUI
        {
            BaseUI baseUi;
            if (self._openedUiDictionary.TryGetValue(className, out baseUi))
                return (T)baseUi;
            Debug.LogError("GetOpenedWindow window " + className + " not opened");
            return (T)baseUi;
        }

        //获取打开过的界面
        public static T GetOpenedWindow<T>(this UIManagerComponent self, UIEnum className) where T : BaseUI
        {
            BaseUI baseUi;
            if (self._uiDictionary.TryGetValue(className, out baseUi))
                return (T)baseUi;
            Debug.LogError("GetOpenedWindow window " + className + " not opened");
            return (T)baseUi;
        }

        public static bool IsWindowOpen(this UIManagerComponent self, UIEnum className)
        {
            return self._openedUiDictionary.ContainsKey(className);
        }

        //所有打开的界面调用方法
        public static void CallAction(this UIManagerComponent self, string actName)
        {
            foreach (KeyValuePair<UIEnum, BaseUI> valuePair in self._openedUiDictionary)
            {
                ((BaseUI)valuePair.Value).CallAction(actName);
            }
        }

        //是否是全屏界面
        private static bool IsFullPanel(this UIManagerComponent self, UIEnum uiEnum)
        {
            return true;
            //  return uiEnum ==  uiEnum.LoadPanel || uiEnum == UiEnum.ChangeSceneMaskPanel;
        }

        //预加载
        public static void PreloadUi(this UIManagerComponent self)
        {
            self.IsPreload = true;
            self.IsPreload = false;
        }

        /// <summary>
        /// 显示文本
        /// </summary>
        /// <param name="self"></param>
        /// <param name="str"></param>
        public static void ShowToast(this UIManagerComponent self, string str)
        {
            GComponent view = UIPackage.CreateObject("PublicPackage", "ToastWindow").asCom;
            view.GetChild("infoText").asTextField.text = str;
            GRoot.inst.AddChild(view);
            view.Center();
            view.sortingOrder = (int)UiLayer.Highest;
            view.GetTransitionAt(0).Play(() => { GRoot.inst.RemoveChild(view, true); });
        }

        /// <summary>
        /// 资源获得ui提示 连续弹出
        /// </summary>
        /// <param name="self"></param>
        /// <param name="currencies"></param>
        public static void PreSetText(this UIManagerComponent self, Dictionary<int, long> currencies = null)
        {
        }

        public static void TurnToOpenPanel(this UIManagerComponent self, BaseUI baseUI, UIEnum className, UiLayer layer = UiLayer.Middle,
        UIPackageEnum packageName = UIPackageEnum.Null, bool isModal = false,
        params object[] values)
        {
            baseUI.children = Game.Scene.GetComponent<UIManagerComponent>().OpenWindow(className, packageName, layer, isModal, values);
            if (baseUI.children != null)
            {
                baseUI.children.Selfparent = baseUI.children;
            }
        }
    }
}