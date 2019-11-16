using System;
using System.Threading.Tasks;
using App.Dispatch;
using UnityEngine;
using UnityEngine.Events;

namespace App.UI
{
    /// <summary>
    /// UI窗体基类
    /// </summary>
    public abstract class BaseView
    {

        #region private fields

        private string _typeName;                   //View名称
        protected Transform _viewRoot;              //窗口根节点

        #endregion

        #region ctor

        protected BaseView()
        {
            _typeName = GetType().Name;
            Dispatcher.Listener($"OnInitView_{_typeName}", OnAwake);
            Dispatcher.Listener($"AddDelegates_{_typeName}", AddDelegates);
            Dispatcher.Listener($"SyncOpenView_{_typeName}", SyncOnOpen);
            Dispatcher.Listener($"CloseView_{_typeName}", Close);
            Dispatcher.Listener($"DestroyView_{_typeName}", OnDestroy);
            Dispatcher<Task>.Listener($"AsyncOpenView_{_typeName}", AsyncOnOpen);
        }

        #endregion

        /// <summary>
        /// 初始化窗口(仅当窗口建立时执行,且最先执行)
        /// </summary>
        /// <returns></returns>
        protected virtual void OnAwake()
        {

        }

        /// <summary>
        /// 添加监听
        /// </summary>
        protected virtual void AddDelegates()
        {

        }

        /// <summary>
        /// 同步开启(窗口每次显示都会执行)
        /// </summary>
        protected virtual void SyncOnOpen()
        {

        }

        /// <summary>
        /// 异步开启(窗口每次开启都会执行)
        /// </summary>
        /// <returns></returns>
        protected virtual Task AsyncOnOpen() => Task.FromResult(0);


        /// <summary>
        /// 关闭(仅隐藏)
        /// </summary>
        protected virtual void Close()
        {
            Dispatcher.Remove($"OnInitView_{_typeName}", OnAwake);
        }

        /// <summary>
        /// 销毁(删除)
        /// </summary>
        protected virtual void OnDestroy()
        {
            Close();
            Dispatcher.Remove($"SyncOpenView_{_typeName}", SyncOnOpen);
            Dispatcher<Task>.Remove($"AsyncOpenView_{_typeName}", AsyncOnOpen);
        }
    }
}

