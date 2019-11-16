using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#if UNITY_TASK_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.TaskExtension
{
#if UNITY_TASK_EDITOR
    [InitializeOnLoad]
#endif
    /// <summary>
    /// 表示一个处理将任务排队到Unity主线程中的低级工作的对象。
    /// </summary>
    public static class UnityTaskScheduler
    {
        #region private fields

        private static SynchronizationContext _context;             //同步上下文
        private static bool _isExit;
        private static readonly Dictionary<string, IEnumeratorTaskStruct> _enumeratorDictionary = new Dictionary<string, IEnumeratorTaskStruct>();

        #endregion

        #region public funcs

#if UNITY_TASK_EDITOR
        static UnityTaskScheduler(){Init();}
#else
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
        public static void Init()
        {
            _context = new UnitySynchronizationContext();
            _isExit = false;
            ThreadPool.QueueUserWorkItem(a =>
            {
                while (!_isExit)
                {
                    Thread.Sleep(20);
                    _context.Send(b =>
                    {

                        for (int i = 0; i < _enumeratorDictionary.Count; i++)
                        {
                            var item = _enumeratorDictionary.ElementAt(i);

                            if (item.Value.IsSuspend)
                            {
                                continue;
                            }

                            bool isEnd = !item.Value.IEnumeratorObject.MoveNext();
                            if (item.Value.IEnumeratorObject.Current is IEnumerator current)
                            {
                                item.Value.IsSuspend = true;
                                Post(current, c =>
                                {
                                    if (isEnd)
                                    {
                                        _enumeratorDictionary.Remove(item.Key);
                                        item.Value.Callback(current);
                                    }
                                    else
                                    {
                                        item.Value.IsSuspend = false;
                                    }
                                });
                            }
                            else
                            {
                                if (isEnd)
                                {
                                    _enumeratorDictionary.Remove(item.Key);
                                    item.Value.Callback(item.Value.IEnumeratorObject.Current);
                                }
                            }
                        }
                    }, null);
                    if (_isExit)
                    {
                        break;
                    }
                }
            }, null);
        }


        /// <summary>
        /// Dispatches an asynchronous message to coroutine.
        /// </summary>
        /// <param name="action">task</param>
        /// <param name="completeCallback">callback</param>
        /// <returns></returns>
        public static string Post(IEnumerator action, Action<object> completeCallback)
        {
            IEnumeratorTaskStruct task = new IEnumeratorTaskStruct()
            {
                IsSuspend = false,
                IEnumeratorObject = action,
                Callback = completeCallback
            };
            var hashCode = task.GetHashCode().ToString();
            _enumeratorDictionary[hashCode] = task;
            return hashCode;
        }

        /// <summary>
        /// Dispatches an asynchronous message to a synchronization context.
        /// </summary>
        /// <param name="action">action</param>
        public static void Post(Action action)
        {
            _context.Post(p => action(), null);
        }


        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="code">任务id</param>
        public static void Cancel(string code)
        {
            if (_enumeratorDictionary.ContainsKey(code))
            {
                _enumeratorDictionary.Remove(code);
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public static void Disposed()
        {
            _isExit = true;
            _enumeratorDictionary.Clear();
        }

        #endregion

        #region class

        /// <summary>
        /// 任务结构
        /// </summary>
        internal sealed class IEnumeratorTaskStruct
        {
            /// <summary>
            /// 是否暂停
            /// </summary>
            public bool IsSuspend { set; get; }

            /// <summary>
            /// 回调
            /// </summary>
            public Action<object> Callback { set; get; }

            /// <summary>
            /// 任务
            /// </summary>
            public IEnumerator IEnumeratorObject { set; get; }
        }

        #endregion
    }
}
