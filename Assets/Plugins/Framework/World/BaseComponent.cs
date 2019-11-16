
using System;

namespace App.Component
{
    /// <summary>
    /// 组件基类
    /// </summary>
    public class BaseComponent
    {
        #region private fields

        private string _componentTag;              //组件标识
        private string _componentId;               //组件ID

        #endregion

        #region ctor

        /// <summary>
        /// 构造组件基类
        /// </summary>
        public BaseComponent()
        {
            _componentId = Guid.NewGuid().ToString();
        }

        #endregion

        #region public properties

        /// <summary>
        /// 获取组件ID
        /// </summary>
        public string ComponentId => _componentId;

        /// <summary>
        /// 设置或获取组件标识
        /// </summary>
        public string ComponentTag
        {
            get => _componentTag;
            set => _componentTag = value;
        }

        #endregion

    }
}
