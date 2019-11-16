
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using App.Dispatch;
using App.UI;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace App.Component
{
    /// <summary>
    /// UI组件
    /// </summary>
    public sealed class UIComponent : BaseComponent
    {

        #region private fields

        private RepositoryComponent<BaseView, Transform> _cache;     //缓存[窗口对象，窗口游戏物体]

        #endregion

        #region public properties

        /// <summary>
        /// 画布
        /// </summary>
        public Transform Canvas { get; }

        #endregion

        #region ctor

        public UIComponent()
        {
            _cache = new RepositoryComponent<BaseView, Transform>();
            Canvas = GameObject.Find("Canvas").transform;
        }

        #endregion

        #region public funcs

        /// <summary>
        /// 开启窗口(同步)
        /// </summary>
        /// <typeparam name="TView">窗口类型</typeparam>
        /// <returns>窗口对象</returns>
        public TView SyncOpenView<TView>() where TView :BaseView,new()
        {
            try
            {
                var viewName = typeof(TView).Name;
                var view = _cache.FirstAllFromKey(p => p.GetType() == typeof(TView));
                if (view.Item1 == null)
                {
                    var initView = InitView<TView>();
                    view = new Tuple<BaseView, Transform>(initView.Item1, initView.Item2);
                    _cache.Set(view.Item1, view.Item2);
                    Dispatcher.DoWork($"OnInitView_{viewName}");  //执行初始化 
                    Dispatcher.DoWork($"AddDelegates_{viewName}"); //执行监听任务
                }
                else
                {
                    view.Item2.gameObject.SetActive(true);
                }
                Dispatcher.DoWork($"SyncOpenView_{viewName}"); //执行窗口开启
                return view.Item1 as TView;
            }
            catch (Exception e)
            {
                Debug.LogError($"同步开启窗口{typeof(TView)}失败,原因:{e}");
            }
            return null;
        }

        /// <summary>
        /// 开启窗口(异步)
        /// </summary>
        /// <typeparam name="TView">窗口类型</typeparam>
        /// <returns>包含窗口对象的任务</returns>
        public Task<TView> AsyncOpenView<TView>() where TView : BaseView, new()
        {
            var viewName = typeof(TView).Name;
            var view = _cache.FirstAllFromKey(p => p.GetType() == typeof(TView));

            if (view.Item1 == null)
            {
                var initView = InitView<TView>();
                view = new Tuple<BaseView, Transform>(initView.Item1, initView.Item2);
                _cache.Set(view.Item1, view.Item2);
                Dispatcher.DoWork($"OnInitView_{viewName}"); //执行初始化 
                Dispatcher.DoWork($"AddDelegates_{viewName}"); //执行监听任务
            }

            return Dispatcher<Task>.DoWork($"AsyncOpenView_{viewName}").ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Debug.LogError($"异步开启窗口{viewName}异常,原因:{t.Exception}");
                }
                view.Item2?.gameObject.SetActive(true);
                return view.Item1 as TView;
            });
        }

        /// <summary>
        /// 关闭窗口(仅隐藏)
        /// </summary>
        /// <typeparam name="TView">窗口类型</typeparam>
        public void CloseView<TView>() where TView : BaseView, new ()
        {
            var viewName = typeof(TView).Name;
            var view = _cache.FirstValueFromKey((p => p.GetType() == typeof(TView)));
            if (view!=null)
            {
                Dispatcher.DoWork($"CloseView_{viewName}"); //执行窗口关闭
                view.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 销毁窗口
        /// </summary>
        /// <typeparam name="TView">窗口类型</typeparam>
        public void DestroyView<TView>() where TView : BaseView, new()
        {
            var viewName = typeof(TView).Name;
            var view = _cache.FirstAllFromKey((p => p.GetType() == typeof(TView)));
            if (view != null)
            {
                Dispatcher.DoWork($"DestroyView_{viewName}"); //执行窗口关闭
                GameObject.DestroyImmediate(view.Item2.gameObject);
                _cache.Remove(view.Item1);
            }
        }


        /// <summary>
        /// UI控件反射赋值
        /// </summary>
        /// <typeparam name="TView">窗口类型</typeparam>
        /// <param name="from">哪个游戏物体作为赋值对象</param>
        /// <param name="view">要被赋值的窗口对象</param>
        public void ReflectFromTransform<TView>(Transform from, ref TView view) where TView : BaseView, new()
        {
            //该方法目前采用的是反射的方式，以后可写成表达式树,用以提升效率

            var viewType = view.GetType();
            var viewProperties = viewType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var viewFields = viewType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            viewFields.FirstOrDefault(t => t.Name == "_viewRoot")?.SetValue(view, from);
            var transforms = from.GetComponentsInChildren<Transform>(true);

            foreach (var tempTransform in transforms)
            {
                foreach (var tempField in viewFields)
                {
                    if (tempField.Name.Replace("_", string.Empty).ToUpper() == tempTransform.name.ToUpper())
                    {
                        tempField.SetValue(view, tempTransform.GetComponent(tempField.FieldType));
                        break;
                    }
                }

                foreach (var tempProperty in viewProperties)
                {
                    if (tempProperty.Name.ToUpper() == tempTransform.name.ToUpper())
                    {
                        tempProperty.SetValue(view, tempTransform.GetComponent(tempProperty.PropertyType));
                        break;
                    }
                }
            }

            foreach (var tempField in viewFields)
            {
                var inject = tempField.GetCustomAttribute<InjectAttribute>();
                if (inject == null)
                {
                    continue;
                }
                tempField.SetValue(view, World.GetComponents(tempField.FieldType).FirstOrDefault(p => p.ComponentTag == inject.ComponentTag));
            }

            foreach (var tempProperty in viewProperties)
            {
                var inject = tempProperty.GetCustomAttribute<InjectAttribute>();
                if (inject == null)
                {
                    continue;
                }
                tempProperty.SetValue(view, World.GetComponents(tempProperty.PropertyType).FirstOrDefault(p => p.ComponentTag == inject.ComponentTag));
            }
        }


        /// <summary>
        /// 添加一个带有被点击的按钮的监听
        /// </summary>
        /// <param name="btn">被点击的按钮</param>
        /// <param name="call">要执行的方法</param>
        public void AddListener(Button btn, Action<Button> call)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => call(btn));
        }

        /// <summary>
        /// 添加一个带有触发改变的Toggle的监听
        /// </summary>
        /// <param name="toggle">触发了改变的Toggle</param>
        /// <param name="call">要执行的方法</param>
        public void AddListener(Toggle toggle,Action<Toggle,bool> call)
        {
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(t=>call(toggle,t));
        }

        #endregion

        #region private funcs

        /// <summary>
        /// 创建窗口与实例化窗口对象
        /// </summary>
        /// <typeparam name="TView">窗口类型</typeparam>
        /// <returns>[窗口对象,窗口游戏物体]</returns>
        private Tuple<TView,Transform> InitView<TView>() where TView : BaseView, new()
        {
            var viewCache = Resources.Load($"Views/{typeof(TView).Name}");
            var viewGameObject = GameObject.Instantiate(viewCache, Canvas) as GameObject;
            if (viewGameObject == null)
            {
                return null;
            }
            var rectTransform = viewGameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            var view = new TView();
            ReflectFromTransform(viewGameObject.transform,ref view);
            return new Tuple<TView, Transform>(view,viewGameObject.transform);
        }


        #endregion
    }

}

