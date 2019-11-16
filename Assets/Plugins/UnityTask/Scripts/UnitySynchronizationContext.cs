
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;

namespace UnityEngine.TaskExtension
{

    /// <summary>
    /// <summary>在Unity中提供各种同步模型中传播同步上下文的基本功能。</summary>
    /// </summary>
#if UNITY_TASK_EDITOR
    [InitializeOnLoad]
#endif
    public sealed class UnitySynchronizationContext : SynchronizationContext
    {

        private sealed class Message
        {
            public Message(SendOrPostCallback callback, object state)
            {
                Callback = callback;
                State = state;
            }

            public SendOrPostCallback Callback { get; set; }

            public event Action CallbackEvent;

            public void CallbackEventTrigger()
            {
                CallbackEvent?.Invoke();
            }

            public object State { get; set; }
        }


        private readonly Queue<Message> _queue;

        private const int MaxCount = 88;

        public UnitySynchronizationContext()
        {
            _queue = new Queue<Message>();
#if UNITY_TASK_EDITOR
            EditorApplication.update += Update;
#else
            GameObject go = new GameObject("UnitySynchronizationContextExecutor");
            go.AddComponent<UnitySynchronizationContextExecutor>().UpdateAction = Update;
            Object.DontDestroyOnLoad(go);
#endif
        }


        private void Update()
        {
            int currentCount = 0;
            Message message = null;
            while (_queue.Count != 0 && currentCount <= MaxCount)
            {
                currentCount++;
                lock (this)
                {
                    if (_queue.Count != 0)
                    {
                        message = _queue.Dequeue();
                    }
                }

                if (message != null)
                {
                    message.Callback(message.State);
                    message.CallbackEventTrigger();
                }
            }
        }

        /// <summary>在派生类中重写时，创建同步上下文的副本。</summary>
        /// <returns>
        ///   一个新 <see cref="T:System.Threading.SynchronizationContext" /> 对象。
        /// </returns>
        public override SynchronizationContext CreateCopy()
        {
            return new UnitySynchronizationContext();
        }

        /// <summary>在派生类中重写时，将异步消息分派到同步上下文。</summary>
        /// <param name="d">
        ///   要调用的 <see cref="T:System.Threading.SendOrPostCallback" /> 委托。
        /// </param>
        /// <param name="state">传递给委托的对象。</param>
        public override void Post(SendOrPostCallback d, object state)
        {
            lock (this)
            {
                _queue.Enqueue(new Message(d, state));
            }
        }

        /// <summary>在派生类中重写时，将同步消息分派到同步上下文。</summary>
        /// <param name="d">
        ///   要调用的 <see cref="T:System.Threading.SendOrPostCallback" /> 委托。
        /// </param>
        /// <param name="state">传递给委托的对象。</param>
        /// <exception cref="T:System.NotSupportedException">
        ///   该方法是在 Windows 应用商店应用程序中调用的。
        ///    实现 <see cref="T:System.Threading.SynchronizationContext" /> for Windows 应用商店应用程序不支持 <see cref="M:System.Threading.SynchronizationContext.Send(System.Threading.SendOrPostCallback,System.Object)" /> 方法。
        /// </exception>
        public override void Send(SendOrPostCallback d, object state)
        {
            Message message = new Message(d, state);
            bool hasComplete = false;
            ManualResetEvent waiter = new ManualResetEvent(false);
            message.CallbackEvent += () =>
            {
                hasComplete = true;
                waiter.Set();
            };
            lock (this)
            {
                _queue.Enqueue(message);
            }

            if (!hasComplete)
            {
                waiter.WaitOne();
            }
        }
    }
}

