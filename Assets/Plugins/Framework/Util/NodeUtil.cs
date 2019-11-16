using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.Util
{
    /// <summary>
    /// 节点工具
    /// </summary>
    public static class NodeUtil 
    {
        #region public static funcs

        /// <summary>
        /// 获取子节点集
        /// </summary>
        /// <param name="from">目标</param>
        /// <returns>子节点集(仅自身的子节点,不获取所有子节点)</returns>
        public static List<Transform> GetChildrenT(this GameObject from)
        {
            return from == null ? new List<Transform>() : from.transform.GetChildrenT();
        }

        /// <summary>
        /// 获取子节点集
        /// </summary>
        /// <typeparam name="TUnityComponent">目标类型</typeparam>
        /// <param name="from">目标</param>
        /// <returns>子节点集(仅自身的子节点,不获取所有子节点)</returns>
        public static List<Transform> GetChildrenT<TUnityComponent>(this TUnityComponent from)
            where TUnityComponent : UnityEngine.Component
        {
            if (from == null)
            {
                return new List<Transform>();
            }
            List<Transform> result = new List<Transform>();
            for (int i = 0; i < from.transform.childCount; i++)
            {
                result.Add(from.transform.GetChild(i));
            }
            return result;
        }

        /// <summary>
        /// 获取子节点集
        /// </summary>
        /// <param name="from">目标</param>
        /// <returns>子节点集(仅自身的子节点,不获取所有子节点)</returns>
        public static List<GameObject> GetChildrenG(this GameObject from)
        {
            return from == null ? new List<GameObject>() : from.transform.GetChildrenG();
        }

        /// <summary>
        /// 获取子节点集
        /// </summary>
        /// <typeparam name="TUnityComponent">目标类型</typeparam>
        /// <param name="from">目标</param>
        /// <returns>子节点集(仅自身的子节点,不获取所有子节点)</returns>
        public static List<GameObject> GetChildrenG<TUnityComponent>(this TUnityComponent from)
            where TUnityComponent : UnityEngine.Component
        {
            return from.GetChildrenT().Select(t => t.gameObject).ToList();
        }

        /// <summary>
        /// 获取目标子节点,并同时返回需要获取的组件
        /// </summary>
        /// <typeparam name="TTargetComponent">需要获取到的组件类型</typeparam>
        /// <typeparam name="TFromComponent">查找节点的目标类型</typeparam>
        /// <param name="from">从该节点开始查询</param>
        /// <param name="target">目标节点名称</param>
        /// <returns>一个目标组件对象</returns>
        public static TTargetComponent GetComponentInChild<TTargetComponent, TFromComponent>(this TFromComponent from, string target)
            where TTargetComponent : UnityEngine.Component where TFromComponent : UnityEngine.Component
        {
            if (from == null || string.IsNullOrEmpty(target))
            {
                return default;
            }
            var child = from.transform.Find(target);
            return child == null ? default : child.GetComponent<TTargetComponent>();
        }

        /// <summary>
        /// 获取目标子节点,并同时返回需要获取的组件
        /// </summary>
        /// <typeparam name="TTargetComponent">需要获取到的组件类型</typeparam>
        /// <param name="from">从该节点开始查询</param>
        /// <param name="target">目标节点名称</param>
        /// <returns>一个目标组件对象</returns>
        public static TTargetComponent GetComponentInChild<TTargetComponent>(this GameObject from, string target)
            where TTargetComponent : UnityEngine.Component
        {
            return @from == null ? default : @from.transform.GetComponentInChild<TTargetComponent>(target);
        }

        /// <summary>
        /// 获取目标子节点,并同时返回需要获取的组件
        /// </summary>
        /// <typeparam name="TTargetComponent">需要获取到的组件类型</typeparam>
        /// <param name="from">从该节点开始查询</param>
        /// <param name="target">目标节点名称</param>
        /// <returns>一个目标组件对象</returns>
        public static TTargetComponent GetComponentInChild<TTargetComponent>(this Transform from, string target)
            where TTargetComponent : UnityEngine.Component
        {
            return @from.GetComponentInChild<TTargetComponent, Transform>(target);
        }


        /// <summary>
        /// 查找一个子节点,并返回其游戏对象
        /// </summary>
        /// <typeparam name="TFromComponent">查找节点的目标类型</typeparam>
        /// <param name="from">从该节点开始查询</param>
        /// <param name="target">目标节点类型</param>
        /// <returns>目标节点游戏对象</returns>
        public static GameObject Find<TFromComponent>(this TFromComponent from, string target)
            where TFromComponent : UnityEngine.Component
        {
            if (from == null || string.IsNullOrEmpty(target))
            {
                return null;
            }
            var targetNode = from.transform.Find(target);
            return targetNode == null ? null : targetNode.gameObject;
        }

        /// <summary>
        /// 查找一个子节点,并返回其游戏对象
        /// </summary>
        /// <param name="from">从该节点开始查询</param>
        /// <param name="target">目标节点类型</param>
        /// <returns>目标节点游戏对象</returns>
        public static GameObject Find(this GameObject from, string target)
        {
            return @from == null ? null : Find(from.transform, target);
        }

        /// <summary>
        /// 查找一个子节点,并返回其游戏对象
        /// </summary>
        /// <param name="from">从该节点开始查询</param>
        /// <param name="target">目标节点类型</param>
        /// <returns>目标节点游戏对象</returns>
        public static GameObject Find(this Transform from, string target)
        {
            return @from.Find<Transform>(target);
        }

        /// <summary>
        /// 获取相同层级目标,并同时返回需要获取的组件
        /// </summary>
        /// <typeparam name="TTargetComponent">需要获取到的组件类型</typeparam>
        /// <typeparam name="TFromComponent">查找节点的目标类型</typeparam>
        /// <param name="from">从该节点开始查询</param>
        /// <param name="target">目标节点名称</param>
        /// <returns>一个目标组件对象</returns>
        public static TTargetComponent GetComponentInFellow<TTargetComponent, TFromComponent>(this TFromComponent from, string target)
            where TTargetComponent : UnityEngine.Component where TFromComponent : UnityEngine.Component
        {
            if (from == null || string.IsNullOrEmpty(target))
            {
                return default;
            }
            var parent = from.transform.parent;
            Transform targetNode;
            if (parent == null)
            {
                var node = GameObject.Find(target);
                targetNode = node == null ? null : node.transform;
            }
            else
            {
                 targetNode = parent.Find(target);
            }
            return targetNode != null ? targetNode.GetComponent<TTargetComponent>() : default;
        }

        /// <summary>
        /// 获取相同层级目标,并同时返回需要获取的组件
        /// </summary>
        /// <typeparam name="TTargetComponent">需要获取到的组件类型</typeparam>
        /// <param name="from">从该节点开始查询</param>
        /// <param name="target">目标节点名称</param>
        /// <returns>一个目标组件对象</returns>
        public static TTargetComponent GetComponentInFellow<TTargetComponent>(this GameObject from, string target)
            where TTargetComponent : UnityEngine.Component
        {
            return @from == null ? default : @from.transform.GetComponentInFellow<TTargetComponent>(target);
        }

        /// <summary>
        /// 获取相同层级目标,并同时返回需要获取的组件
        /// </summary>
        /// <typeparam name="TTargetComponent">需要获取到的组件类型</typeparam>
        /// <param name="from">从该节点开始查询</param>
        /// <param name="target">目标节点名称</param>
        /// <returns>一个目标组件对象</returns>
        public static TTargetComponent GetComponentInFellow<TTargetComponent>(this Transform from, string target)
            where TTargetComponent : UnityEngine.Component
        {
            return @from == null ? default : @from.GetComponentInFellow<TTargetComponent, Transform>(target);
        }
        #endregion
    }
}

