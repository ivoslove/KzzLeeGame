using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.TaskExtension
{
    /// <summary>
    /// <summary>作为执行者，在Unity中提供各种同步模型中传播同步上下文的基本功能。</summary>
    /// </summary>
    public class UnitySynchronizationContextExecutor : MonoBehaviour
    {
        public Action UpdateAction { get; set; }

        private void Update()
        {
            UpdateAction?.Invoke();
        }
    }
}
