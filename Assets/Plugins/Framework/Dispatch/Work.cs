using System;
using System.Collections.Generic;

namespace App.Dispatch
{
    /// <summary>
    /// 事务
    /// </summary>
    public struct Work
    {
        /// <summary>
        /// 工作ID
        /// </summary>
        public string WorkId;

        /// <summary>
        /// 工作具体任务
        /// </summary>
        public Delegate WorkEvent;
    }
}


