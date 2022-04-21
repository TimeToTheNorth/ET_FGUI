﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FairyGUI;
using UnityEngine;

namespace BaseClass
{
    public abstract class BaseUI : Window
    {
        //是否没有语言文本
        private bool _isNoLanguageText = false;

        public BaseUI parent;

        public BaseUI children;

        public UIEnum UiEnum;

        //是否正在播放ui进出动画
        public bool IsInUiTransition = false;

        public bool IsPlaySound = true;

        //是否属于弹出界面
        public bool IsPopupWindow;

        public bool isBlack;

        //添加事件监听
        public virtual void AddEvent()
        {
        }

        //移除事件监听
        public virtual void DelEvent()
        {
        }

        //获取所有组件
        public virtual void GetAllComponent()
        {
        }

        /// <summary>
        /// 初始化（只会调用一次）
        /// </summary>
        protected override void OnInit()
        {
            base.OnInit();
            //全屏界面适配
            contentPane.SetSize(GRoot.inst.width, GRoot.inst.height);

            //全面屏对顶部适配
            if (contentPane.GetChild("AdaptivePanel") != null && UIManager.Instance.IsFullScreen)
            {
                contentPane.GetChild("AdaptivePanel").y += UIManager.Instance.UiMoveDis;
                contentPane.GetChild("AdaptivePanel").height -= UIManager.Instance.UiMoveDis;
            }

            //获取组件
            GetAllComponent();
        }

        /// <summary>
        /// 跳转打开其他界面
        /// </summary>
        public virtual void TurnToOpenPanel(UIEnum className, UiLayer layer,
            UIPackageEnum packageName = UIPackageEnum.Null, bool isModal = false,
            params object[] values)
        {
            children = UIManager.Instance.OpenWindow(className, packageName, layer, isModal, values);
            if (children != null)
            {
                children.parent = this;
            }
        }

        //打开界面
        public virtual void OpenPanel()
        {
            Show();
            AddEvent();
            AutoSetText();
            if (IsPlaySound)
            {
                //SoundService.Instance.PlayCilp(SoundType.UI_open);
            }
        }

        public virtual void SetValues(params object[] values)
        {
        }

        //关闭界面
        public virtual void ClosePanel()
        {
            DelEvent();
            Hide();
            if (IsPlaySound)
            {
                //SoundService.Instance.PlayCilp(SoundType.UI_close);
            }
        }

        /// <summary>
        /// 打开界面后（界面动画结束后）
        /// </summary>
        protected override void OnShown()
        {
            base.OnShown();
            IsInUiTransition = false;
            //modal = false;
        }

        /// <summary>
        /// 关闭界面后（界面动画结束后）
        /// </summary>
        protected override void OnHide()
        {
            base.OnHide();
            IsInUiTransition = false;
            modal = false;
            if (IsPopupWindow)
            {
                UIManager.Instance.ClosePopupWindow();
            }
            else
            {
                UIManager.Instance.CloseWindow(UiEnum);
            }
        }

        /// <summary>
        /// 播放界面打开动画
        /// </summary>
        protected override void DoShowAnimation()
        {
//            SetScale(0.1f,0.1f);
//            this.SetPivot(0.5f, 0.5f);
//            TweenScale(new Vector2(1f, 1f), 0.5f).OnComplete(() => { OnShown(); });
            IsInUiTransition = true;
            Transition trans = contentPane.GetTransition("UiInTrans");

            if (trans != null)
            {
                trans.invalidateBatchingEveryFrame = true;
                trans.Play(OnShown);
            }
            else OnShown();
        }

        /// <summary>
        /// 播放界面关闭动画
        /// </summary>
        protected override void DoHideAnimation()
        {
            IsInUiTransition = true;
            modal = true;
//            SetScale(1f, 1f);
//            this.SetPivot(0.5f, 0.5f);
//            TweenScale(new Vector2(0.1f, 0.1f), 0.5f).OnComplete(() => { HideImmediately(); });
            Transition trans = contentPane.GetTransition("UiOutTrans");

            if (trans != null)
            {
                trans.invalidateBatchingEveryFrame = true;
                trans.PlayReverse(HideImmediately);
            }
            else HideImmediately();
        }

        public void AutoSetText()
        {
            if (_isNoLanguageText) return;
            GObject[] children = contentPane.GetChildren();
            for (int i = 0; i < children.Length; i++)
            {
                GObject child = children[i];
                if (child is GTextField)
                {
                    //if (child.data != null && child.data.ToString() != "")
                    //child.text = TableManager.Instance.LanguageTable.GetString(child.data.ToString());
                }
                else AutoSetText(child.asCom);
            }
        }

        private void AutoSetText(GComponent gComponent)
        {
            if (gComponent == null) return;
            GObject[] children = gComponent.GetChildren();
            for (int i = 0; i < children.Length; i++)
            {
                GObject child = children[i];
                if (child is GTextField)
                {
                    //if (child.data != null && child.data.ToString() != "")
                    //child.text = TableManager.Instance.LanguageTable.GetString(child.data.ToString());
                }
                else AutoSetText(child.asCom);
            }

            return;
        }

        //调用某个public方法
        public void CallAction(string funcName)
        {
            MethodInfo method = GetType().GetMethod(funcName);
            Action action = (Action) Delegate.CreateDelegate(typeof(Action), this, method);
            action();
        }
    }
}