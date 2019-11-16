using System;

namespace App.UI
{
    /// <summary>
    /// 组件对象自动注入标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class InjectAttribute : Attribute
    {
        /// <summary>
        /// 组件标签
        /// </summary>
        public string ComponentTag { get; set; }

        public InjectAttribute(string componentTag = null)
        {
            ComponentTag = componentTag;
        }
    }
}

