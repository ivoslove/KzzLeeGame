
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Component
{
    /// <summary>
    /// 组件获取器 -- 世界
    /// </summary>
    public static class World
    {

        #region private fields

        private static RepositoryComponent<string, BaseComponent> _cache;                          //仓储组件[组件ID , 组件对象]

        #endregion

        #region ctor

        /// <summary>
        /// 构造世界获取器
        /// </summary>
        static World()
        {
            _cache = new RepositoryComponent<string, BaseComponent>();
        }

        #endregion

        #region public funcs

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <typeparam name="TComponent">组件类型</typeparam>
        /// <param name="target">目标对象</param>
        /// <param name="tag">组件标识</param>
        /// <returns>添加的或者已存在的对象</returns>
        public static TComponent CreateComponent<TComponent>(string tag = null) where TComponent : BaseComponent, new()
        {
            var component = new TComponent(){ComponentTag = tag};
            _cache.Set(component.ComponentId,component);
            return component;
        }

        /// <summary>
        /// 获取组件列表
        /// </summary>
        /// <typeparam name="TComponent">要获取的组件类型</typeparam>
        /// <returns>组件列表</returns>
        public static List<TComponent> GetComponents<TComponent>() where TComponent : BaseComponent
        {
            var components = _cache.FindValuesFromValue(p => p.GetType() == typeof(TComponent));
            return components.Select(p => p as TComponent).ToList();
        }

        /// <summary>
        /// 获取组件列表
        /// </summary>
        /// <param name="type">要获取的组件类型</param>
        /// <returns>组件列表</returns>
        public static List<BaseComponent> GetComponents(Type type)
        {
            if (type.BaseType != typeof(BaseComponent))
            {
                return new List<BaseComponent>();
            }
            return _cache.FindValuesFromValue(p => p.GetType() == type).ToList();
        }

        #endregion

    }
}

