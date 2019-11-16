
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UnityEngine.TaskExtension
{
    /// <summary>
    ///   提供对创建和计划 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 对象的支持。
    /// </summary>
    public class UnityTaskFactory 
    {
        #region private fields

        private CancellationToken _cancellationToken;                   //取消操作的通知
        private static readonly object _lock = new object();            //锁

        #endregion

        #region ctor

        /// <summary>
        ///   使用指定配置初始化 <see cref="T:UnityEngine.TaskExtension.UnityTaskFactory" /> 实例。
        /// </summary>
        /// <param name="cancellationToken">
        ///   在使用此 TaskFactory 创建任务时要使用的默认 <see cref="T:System.Threading.CancellationToken" />。
        /// </param>
        public UnityTaskFactory(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
        }

        /// <summary>
        ///   使用默认配置初始化 <see cref="T:UnityEngine.TaskExtension.UnityTaskFactory" /> 实例。
        /// </summary>
        public UnityTaskFactory() : this(CancellationToken.None) { }

        #endregion

        #region public funcs

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">要异步执行的操作委托。</param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   已释放提供的 <see cref="T:System.Threading.CancellationToken" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="function" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> StartNew<TResult>(Func<IEnumerator> function)
        {
            var tcs = new UnityTaskCompletionSource<TResult>();
            var cancelCode = UnityTaskScheduler.Post(function(), result =>
            {
                try
                {
                    tcs.SetResult((TResult) result);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            tcs.Task.CoroutineCode = cancelCode;
            return tcs.Task;
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">要异步执行的操作委托。</param>
        /// <param name="state">
        ///   一个包含由 <paramref name="function" /> 委托使用的数据的对象。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="function" /> 参数为 <see langword="null" />。
        /// </exception>
        public UnityTask<TResult> StartNew<TResult>(Func<object, IEnumerator> function, object state)
        {
            return StartNew<TResult>(() => function(state));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">要异步执行的操作委托。</param>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="function" /> 参数为 <see langword="null" />。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TResult>(Func<TArg1, IEnumerator> function, TArg1 arg1)
        {
            return StartNew<TResult>(() => function(arg1));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">要异步执行的操作委托。</param>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///  传递给 <paramref name="function" /> 委托的第二个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="function" /> 参数为 <see langword="null" />。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TArg2,TResult>(Func<TArg1, TArg2, IEnumerator> function, TArg1 arg1, TArg2 arg2)
        {
            return StartNew<TResult>(() => function(arg1, arg2));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">要异步执行的操作委托。</param>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///  传递给 <paramref name="function" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///  传递给 <paramref name="function" /> 委托的第三个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="function" /> 参数为 <see langword="null" />。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3,IEnumerator> function, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            return StartNew<TResult>(() => function(arg1, arg2, arg3));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">要异步执行的操作委托。</param>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///  传递给 <paramref name="function" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///  传递给 <paramref name="function" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///  传递给 <paramref name="function" /> 委托的第四个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="function" /> 参数为 <see langword="null" />。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4, TResult>(Func<TArg1, TArg2, TArg3, TArg4,IEnumerator> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
        {
            return StartNew<TResult>(() => function(arg1, arg2, arg3, arg4));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">要异步执行的操作委托。</param>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///  传递给 <paramref name="function" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///  传递给 <paramref name="function" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///  传递给 <paramref name="function" /> 委托的第四个参数。
        /// </param>
        /// <param name="arg5">
        ///  传递给 <paramref name="function" /> 委托的第五个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="function" /> 参数为 <see langword="null" />。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, IEnumerator> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
        {
            return StartNew<TResult>(() => function(arg1, arg2, arg3, arg4, arg5));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">要异步执行的操作委托。</param>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///  传递给 <paramref name="function" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///  传递给 <paramref name="function" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///  传递给 <paramref name="function" /> 委托的第四个参数。
        /// </param>
        /// <param name="arg5">
        ///  传递给 <paramref name="function" /> 委托的第五个参数。
        /// </param>
        /// <param name="arg6">
        ///  传递给 <paramref name="function" /> 委托的第六个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="function" /> 参数为 <see langword="null" />。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IEnumerator> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
        {
            return StartNew<TResult>(() => function(arg1, arg2, arg3, arg4, arg5, arg6));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">要异步执行的操作委托。</param>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///  传递给 <paramref name="function" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///  传递给 <paramref name="function" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///  传递给 <paramref name="function" /> 委托的第四个参数。
        /// </param>
        /// <param name="arg5">
        ///  传递给 <paramref name="function" /> 委托的第五个参数。
        /// </param>
        /// <param name="arg6">
        ///  传递给 <paramref name="function" /> 委托的第六个参数。
        /// </param>
        /// <param name="arg7">
        ///  传递给 <paramref name="function" /> 委托的第七个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="function" /> 参数为 <see langword="null" />。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IEnumerator> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7)
        {
            return StartNew<TResult>(() => function(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">要异步执行的操作委托。</param>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///  传递给 <paramref name="function" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///  传递给 <paramref name="function" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///  传递给 <paramref name="function" /> 委托的第四个参数。
        /// </param>
        /// <param name="arg5">
        ///  传递给 <paramref name="function" /> 委托的第五个参数。
        /// </param>
        /// <param name="arg6">
        ///  传递给 <paramref name="function" /> 委托的第六个参数。
        /// </param>
        /// <param name="arg7">
        ///  传递给 <paramref name="function" /> 委托的第七个参数。
        /// </param>
        /// <param name="arg8">
        ///  传递给 <paramref name="function" /> 委托的第八个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="function" /> 参数为 <see langword="null" />。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IEnumerator> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8)
        {
            return StartNew<TResult>(() => function(arg1, arg2, arg3,arg4,arg5, arg6,arg7,arg8));
        }

        /// <summary>创建一个延续任务，该任务在一组指定的任务完成后开始。</summary>
        /// <param name="tasks">继续执行的任务所在的数组。</param>
        /// <param name="continuation">
        ///   在 <paramref name="tasks" /> 数组中的所有任务完成时要执行的操作委托。
        /// </param>
        /// <returns>新的延续任务。</returns>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   中的某个元素 <paramref name="tasks" /> 数组已被释放。
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="tasks" /> 数组是 <see langword="null" />。
        /// 
        ///   - 或 -
        /// 
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="tasks" /> 数组为空或包含一个 null 值。
        /// </exception>
        public UnityTask<UnityTask<TResult>> ContinueWhenAll<TResult>(IList<UnityTask> tasks,
            Func<IList<UnityTask>, IEnumerator> continuation)
        {
            var count = tasks.Count;
            var uTcs = new UnityTaskCompletionSource<IList<UnityTask>>();
            if (count == 0)
            {
                uTcs.TrySetResult(tasks);
            }
            foreach (var task in tasks)
            {
                task.ContinueWith(t =>
                {
                    count--;
                    if (count == 0)
                    {
                        uTcs.TrySetResult(tasks);
                    }
                });
            }
            return uTcs.Task.ContinueWith<TResult>(t => continuation(t.Result));
        }

        /// <summary>创建一个延续任务，该任务在一组指定的任务完成后开始。</summary>
        /// <param name="tasks">继续执行的任务所在的数组。</param>
        /// <param name="continuation">
        ///   在 <paramref name="tasks" /> 数组中的所有任务完成时要执行的操作委托。
        /// </param>
        /// <returns>新的延续任务。</returns>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   中的某个元素 <paramref name="tasks" /> 数组已被释放。
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="tasks" /> 数组是 <see langword="null" />。
        /// 
        ///   - 或 -
        /// 
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="tasks" /> 数组为空或包含一个 null 值。
        /// </exception>
        public UnityTask ContinueWhenAll(IList<UnityTask> tasks,Func<IList<UnityTask>, IEnumerator> continuation)
        {
            var count = tasks.Count;
            var uTcs = new UnityTaskCompletionSource<IList<UnityTask>>();
            if (count == 0)
            {
                uTcs.TrySetResult(tasks);
            }
            foreach (var task in tasks)
            {
                task.ContinueWith(t =>
                {
                    count--;
                    if (count == 0)
                    {
                        uTcs.TrySetResult(tasks);
                    }
                });
            }
            return uTcs.Task.ContinueWith(t => continuation(t.Result));
        }

        /// <summary>创建一个延续任务，该任务在一组指定的任务完成后开始。</summary>
        /// <param name="tasks">继续执行的任务所在的数组。</param>
        /// <param name="continuation">
        ///   在 <paramref name="tasks" /> 数组中的所有任务完成时要执行的操作委托。
        /// </param>
        /// <returns>新的延续任务。</returns>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   中的某个元素 <paramref name="tasks" /> 数组已被释放。
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="tasks" /> 数组是 <see langword="null" />。
        /// 
        ///   - 或 -
        /// 
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="tasks" /> 数组为空或包含一个 null 值。
        /// </exception>
        public void ContinueWhenAll(IList<UnityTask> tasks,
            Action<IList<UnityTask>> continuation)
        {
            var count = tasks.Count;
            if (count == 0)
            {
                continuation(tasks);
            }

            foreach (var task in tasks)
            {
                task.ContinueWith(t =>
                {
                    count--;
                    if (count == 0)
                    {
                        continuation(tasks);
                    }
                });
            }
        }

        /// <summary>创建一个延续任务，该任务在一组指定的任务完成后开始。</summary>
        /// <param name="tasks">继续执行的任务所在的数组。</param>
        /// <param name="continuation">
        ///   在 <paramref name="tasks" /> 数组中的所有任务完成时要执行的操作委托。
        /// </param>
        /// <returns>新的延续任务。</returns>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   中的某个元素 <paramref name="tasks" /> 数组已被释放。
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="tasks" /> 数组是 <see langword="null" />。
        /// 
        ///   - 或 -
        /// 
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="tasks" /> 数组为空或包含一个 null 值。
        /// </exception>
        public void ContinueWhenAll<TResult>(IList<UnityTask<TResult>> tasks, Action<Dictionary<string, TResult>> continuation)
        {
            var count = tasks.Count;          
            if (count == 0)
            {
               continuation(new Dictionary<string, TResult>());
            }
            Dictionary<string, TResult> dictionary = new Dictionary<string, TResult>();
            tasks.ToList().ForEach(p =>
            {
                p.ContinueWith(t =>
                {
                    lock (_lock)
                    {
                        dictionary[p.GetHashCode().ToString()] = t.Result;
                        if (dictionary.Count == tasks.Count)
                        {
                            continuation(dictionary);
                        }
                    }               
                });
            });
        }

        /// <summary>创建一个延续任务，该任务在一组指定的任务完成后开始。</summary>
        /// <param name="tasks">继续执行的任务所在的数组。</param>
        /// <param name="continuation">
        ///   在 <paramref name="tasks" /> 数组中的所有任务完成时要执行的操作委托。
        /// </param>
        /// <returns>新的延续任务。</returns>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   中的某个元素 <paramref name="tasks" /> 数组已被释放。
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="tasks" /> 数组是 <see langword="null" />。
        /// 
        ///   - 或 -
        /// 
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="tasks" /> 数组为空或包含一个 null 值。
        /// </exception>
        public UnityTask ContinueWhenAll(IList<Task> tasks, Action<IList<Task>> continuation)
        {
            int remaining = tasks.Count;
            UnityTaskCompletionSource<IList<Task>> uTcs = new UnityTaskCompletionSource<IList<Task>>();
            if (remaining == 0)
            {
                uTcs.TrySetResult(tasks);
            }
            else
            {
                tasks.ToList().ForEach(p =>
                {
                    p.ContinueWith(t =>
                    {
                        if (Interlocked.Decrement(ref remaining) == 0)
                        {
                            uTcs.TrySetResult(tasks);
                        }
                    }, _cancellationToken);
                });
            }
            return uTcs.Task.ContinueWith(p => continuation);
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">
        ///   一个函数委托，可返回能够通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的将来结果。
        /// </param>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <returns>
        ///   已启动的 <see cref="T:ST:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="function" /> 参数为 <see langword="null" />。
        /// </exception>
        public UnityTask<TResult> StartNew<TResult>(Func<TResult> function)
        {
            var uTcs = new UnityTaskCompletionSource<TResult>();
            UnityTaskScheduler.Post(() =>
            {
                try
                {
                    uTcs.SetResult(function());
                }
                catch (Exception e)
                {
                    uTcs.TrySetException(e);
                }
            });
            return uTcs.Task;
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">
        ///   一个函数委托，可返回能够通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的将来结果。
        /// </param>
        /// <param name="state">
        ///   一个包含由 <paramref name="function" /> 委托使用的数据的对象。
        /// </param>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="function" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> StartNew<TResult>(Func<object, TResult> function, object state)
        {
            return StartNew(() => function(state));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">
        ///   一个函数委托，可返回能够通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的将来结果。
        /// </param>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="function" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="function" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TResult>(Func<TArg1, TResult> function, TArg1 arg1)
        {
            return StartNew(() => function(arg1));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">
        ///   一个函数委托，可返回能够通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的将来结果。
        /// </param>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="function" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="function" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///  传递给 <paramref name="function" /> 委托的第二个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="function" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TArg2, TResult>(Func<TArg1, TArg2, TResult> function, TArg1 arg1, TArg2 arg2)
        {
            return StartNew(() => function(arg1, arg2));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">
        ///   一个函数委托，可返回能够通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的将来结果。
        /// </param>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="function" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="function" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg3">
        ///   传递给 <paramref name="function" /> 委托的第三个参数的类型。
        /// </typeparam>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///  传递给 <paramref name="function" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///  传递给 <paramref name="function" /> 委托的第三个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="function" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, TResult> function, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            return StartNew(() => function(arg1, arg2, arg3));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">
        ///   一个函数委托，可返回能够通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的将来结果。
        /// </param>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="function" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="function" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg3">
        ///   传递给 <paramref name="function" /> 委托的第三个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg4">
        ///   传递给 <paramref name="function" /> 委托的第四个参数的类型。
        /// </typeparam>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///  传递给 <paramref name="function" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///  传递给 <paramref name="function" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///  传递给 <paramref name="function" /> 委托的第四个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="function" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TResult> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
        {
            return StartNew(() => function(arg1, arg2, arg3, arg4));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">
        ///   一个函数委托，可返回能够通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的将来结果。
        /// </param>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="function" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="function" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg3">
        ///   传递给 <paramref name="function" /> 委托的第三个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg4">
        ///   传递给 <paramref name="function" /> 委托的第四个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg5">
        ///   传递给 <paramref name="function" /> 委托的第五个参数的类型。
        /// </typeparam>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///  传递给 <paramref name="function" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///  传递给 <paramref name="function" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///  传递给 <paramref name="function" /> 委托的第四个参数。
        /// </param>
        /// <param name="arg5">
        ///  传递给 <paramref name="function" /> 委托的第五个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="function" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4, TArg5,  TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
        {
            return StartNew(() => function(arg1, arg2, arg3, arg4, arg5));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">
        ///   一个函数委托，可返回能够通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的将来结果。
        /// </param>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="function" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="function" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg3">
        ///   传递给 <paramref name="function" /> 委托的第三个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg4">
        ///   传递给 <paramref name="function" /> 委托的第四个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg5">
        ///   传递给 <paramref name="function" /> 委托的第五个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg6">
        ///   传递给 <paramref name="function" /> 委托的第六个参数的类型。
        /// </typeparam>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///  传递给 <paramref name="function" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///  传递给 <paramref name="function" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///  传递给 <paramref name="function" /> 委托的第四个参数。
        /// </param>
        /// <param name="arg5">
        ///  传递给 <paramref name="function" /> 委托的第五个参数。
        /// </param>
        /// <param name="arg6">
        ///  传递给 <paramref name="function" /> 委托的第六个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="function" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
        {
            return StartNew(() => function(arg1, arg2, arg3, arg4, arg5, arg6));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">
        ///   一个函数委托，可返回能够通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的将来结果。
        /// </param>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="function" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="function" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg3">
        ///   传递给 <paramref name="function" /> 委托的第三个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg4">
        ///   传递给 <paramref name="function" /> 委托的第四个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg5">
        ///   传递给 <paramref name="function" /> 委托的第五个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg6">
        ///   传递给 <paramref name="function" /> 委托的第六个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg7">
        ///   传递给 <paramref name="function" /> 委托的第七个参数的类型。
        /// </typeparam>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///  传递给 <paramref name="function" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///  传递给 <paramref name="function" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///  传递给 <paramref name="function" /> 委托的第四个参数。
        /// </param>
        /// <param name="arg5">
        ///  传递给 <paramref name="function" /> 委托的第五个参数。
        /// </param>
        /// <param name="arg6">
        ///  传递给 <paramref name="function" /> 委托的第六个参数。
        /// </param>
        /// <param name="arg7">
        ///  传递给 <paramref name="function" /> 委托的第七个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="function" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7)
        {
            return StartNew(() => function(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
        }

        /// <summary>
        ///   创建并启动 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="function">
        ///   一个函数委托，可返回能够通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的将来结果。
        /// </param>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="function" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="function" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg3">
        ///   传递给 <paramref name="function" /> 委托的第三个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg4">
        ///   传递给 <paramref name="function" /> 委托的第四个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg5">
        ///   传递给 <paramref name="function" /> 委托的第五个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg6">
        ///   传递给 <paramref name="function" /> 委托的第六个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg7">
        ///   传递给 <paramref name="function" /> 委托的第七个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg8">
        ///   传递给 <paramref name="function" /> 委托的第八个参数的类型。
        /// </typeparam>
        /// <param name="arg1">
        ///  传递给 <paramref name="function" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///  传递给 <paramref name="function" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///  传递给 <paramref name="function" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///  传递给 <paramref name="function" /> 委托的第四个参数。
        /// </param>
        /// <param name="arg5">
        ///  传递给 <paramref name="function" /> 委托的第五个参数。
        /// </param>
        /// <param name="arg6">
        ///  传递给 <paramref name="function" /> 委托的第六个参数。
        /// </param>
        /// <param name="arg7">
        ///  传递给 <paramref name="function" /> 委托的第七个参数。
        /// </param>
        /// <param name="arg8">
        ///  传递给 <paramref name="function" /> 委托的第八个参数。
        /// </param>
        /// <returns>
        ///   已启动的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="function" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8)
        {
            return StartNew(() => function(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
        }

        /// <summary>
        ///   创建一个 <see cref="T:UnityEngine.TaskExtension.UnityTask" />，表示符合异步编程模型模式的成对的开始和结束方法。
        /// </summary>
        /// <param name="beginMethod">用于启动异步操作的委托。</param>
        /// <param name="endMethod">用于结束异步操作的委托。</param>
        /// <param name="state">
        ///   一个包含由 <paramref name="beginMethod" /> 委托使用的数据的对象。
        /// </param>
        /// <returns>
        ///   创建的表示异步操作的 <see cref="T:UnityEngine.TaskExtension.UnityTask" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="beginMethod" /> 参数为 null。
        /// 
        ///   - 或 -
        /// 
        ///   时，将引发的异常 <paramref name="endMethod" /> 参数为 null。
        /// </exception>
        public UnityTask FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod,
            Action<IAsyncResult> endMethod, object state)
        {
            return FromAsync(beginMethod, result =>
            {
                endMethod(result);
                return 0;
            },state);
        }

        /// <summary>
        ///   创建一个 <see cref="T:UnityEngine.TaskExtension.UnityTask" />，表示符合异步编程模型模式的成对的开始和结束方法。
        /// </summary>
        /// <param name="beginMethod">用于启动异步操作的委托。</param>
        /// <param name="endMethod">用于结束异步操作的委托。</param>
        /// <param name="arg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数。
        /// </param>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <returns>
        ///   创建的表示异步操作的 <see cref="T:UnityEngine.TaskExtension.UnityTask" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="beginMethod" /> 参数为 null。
        /// 
        ///   - 或 -
        /// 
        ///   时，将引发的异常 <paramref name="endMethod" /> 参数为 null。
        /// </exception>
        public UnityTask FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, object state)
        {
            return FromAsync((callback, _) => beginMethod(arg1, callback, state), endMethod, state);
        }

        /// <summary>
        ///   创建一个 <see cref="T:UnityEngine.TaskExtension.UnityTask" />，表示符合异步编程模型模式的成对的开始和结束方法。
        /// </summary>
        /// <param name="beginMethod">用于启动异步操作的委托。</param>
        /// <param name="endMethod">用于结束异步操作的委托。</param>
        /// <param name="arg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数。
        /// </param>
        /// <param name="state">
        ///   一个包含由 <paramref name="beginMethod" /> 委托使用的数据的对象。
        /// </param>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <returns>
        ///   创建的表示异步操作的 <see cref="T:UnityEngine.TaskExtension.UnityTask" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="beginMethod" /> 参数为 null。
        /// 
        ///   - 或 -
        /// 
        ///   时，将引发的异常 <paramref name="endMethod" /> 参数为 null。
        /// </exception>
        public UnityTask FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
        {
            return FromAsync((callback, _) => beginMethod(arg1, arg2, callback, state), endMethod, state);
        }

        /// <summary>
        ///   创建一个 <see cref="T:UnityEngine.TaskExtension.UnityTask" />，表示符合异步编程模型模式的成对的开始和结束方法。
        /// </summary>
        /// <param name="beginMethod">用于启动异步操作的委托。</param>
        /// <param name="endMethod">用于结束异步操作的委托。</param>
        /// <param name="arg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数。
        /// </param>
        /// <param name="state">
        ///   一个包含由 <paramref name="beginMethod" /> 委托使用的数据的对象。
        /// </param>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数的类型。
        /// </typeparam>
        /// <returns>
        ///   创建的表示异步操作的 <see cref="T:UnityEngine.TaskExtension.UnityTask" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="beginMethod" /> 参数为 null。
        /// 
        ///   - 或 -
        /// 
        ///   时，将引发的异常 <paramref name="endMethod" /> 参数为 null。
        /// </exception>
        public UnityTask FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
        {
            return FromAsync((callback, _) => beginMethod(arg1, arg2, arg3, callback, state), endMethod, state);
        }

        /// <summary>
        ///   创建一个 <see cref="T:UnityEngine.TaskExtension.UnityTask" />，表示符合异步编程模型模式的成对的开始和结束方法。
        /// </summary>
        /// <param name="beginMethod">用于启动异步操作的委托。</param>
        /// <param name="endMethod">用于结束异步操作的委托。</param>
        /// <param name="arg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///   传递给 <paramref name="beginMethod" /> 委托的第四个参数。
        /// </param>
        /// <param name="state">
        ///   一个包含由 <paramref name="beginMethod" /> 委托使用的数据的对象。
        /// </param>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg4">
        ///   传递给 <paramref name="beginMethod" /> 委托的第四个参数的类型。
        /// </typeparam>
        /// <returns>
        ///   创建的表示异步操作的 <see cref="T:UnityEngine.TaskExtension.UnityTask" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="beginMethod" /> 参数为 null。
        /// 
        ///   - 或 -
        /// 
        ///   时，将引发的异常 <paramref name="endMethod" /> 参数为 null。
        /// </exception>
        public UnityTask FromAsync<TArg1, TArg2, TArg3, TArg4>(Func<TArg1, TArg2, TArg3, TArg4, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state)
        {
            return FromAsync((callback, _) => beginMethod(arg1, arg2, arg3, arg4, callback, state), endMethod, state);
        }

        /// <summary>
        ///   创建一个 <see cref="T:UnityEngine.TaskExtension.UnityTask" />，表示符合异步编程模型模式的成对的开始和结束方法。
        /// </summary>
        /// <param name="beginMethod">用于启动异步操作的委托。</param>
        /// <param name="endMethod">用于结束异步操作的委托。</param>
        /// <param name="arg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///   传递给 <paramref name="beginMethod" /> 委托的第四个参数。
        /// </param>
        /// <param name="arg5">
        ///   传递给 <paramref name="beginMethod" /> 委托的第五个参数。
        /// </param>
        /// <param name="state">
        ///   一个包含由 <paramref name="beginMethod" /> 委托使用的数据的对象。
        /// </param>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg4">
        ///   传递给 <paramref name="beginMethod" /> 委托的第四个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg5">
        ///   传递给 <paramref name="beginMethod" /> 委托的第五个参数的类型。
        /// </typeparam>
        /// <returns>
        ///   创建的表示异步操作的 <see cref="T:UnityEngine.TaskExtension.UnityTask" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="beginMethod" /> 参数为 null。
        /// 
        ///   - 或 -
        /// 
        ///   时，将引发的异常 <paramref name="endMethod" /> 参数为 null。
        /// </exception>
        public UnityTask FromAsync<TArg1, TArg2, TArg3, TArg4, TArg5>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state)
        {
            return FromAsync((callback, _) => beginMethod(arg1, arg2, arg3, arg4, arg5, callback, state), endMethod, state);
        }

        /// <summary>
        ///   创建一个 <see cref="T:UnityEngine.TaskExtension.UnityTask" />，表示符合异步编程模型模式的成对的开始和结束方法。
        /// </summary>
        /// <param name="beginMethod">用于启动异步操作的委托。</param>
        /// <param name="endMethod">用于结束异步操作的委托。</param>
        /// <param name="arg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///   传递给 <paramref name="beginMethod" /> 委托的第四个参数。
        /// </param>
        /// <param name="arg5">
        ///   传递给 <paramref name="beginMethod" /> 委托的第五个参数。
        /// </param>
        /// <param name="arg6">
        ///   传递给 <paramref name="beginMethod" /> 委托的第六个参数。
        /// </param>
        /// <param name="state">
        ///   一个包含由 <paramref name="beginMethod" /> 委托使用的数据的对象。
        /// </param>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg4">
        ///   传递给 <paramref name="beginMethod" /> 委托的第四个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg5">
        ///   传递给 <paramref name="beginMethod" /> 委托的第五个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg6">
        ///   传递给 <paramref name="beginMethod" /> 委托的第六个参数的类型。
        /// </typeparam>
        /// <returns>
        ///   创建的表示异步操作的 <see cref="T:UnityEngine.TaskExtension.UnityTask" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="beginMethod" /> 参数为 null。
        /// 
        ///   - 或 -
        /// 
        ///   时，将引发的异常 <paramref name="endMethod" /> 参数为 null。
        /// </exception>
        public UnityTask FromAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state)
        {
            return FromAsync((callback, _) => beginMethod(arg1, arg2, arg3, arg4, arg5, arg6, callback, state), endMethod, state);
        }

        /// <summary>
        ///   创建一个 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />，表示符合异步编程模型模式的成对的开始和结束方法。
        /// </summary>
        /// <param name="beginMethod">用于启动异步操作的委托。</param>
        /// <param name="endMethod">用于结束异步操作的委托。</param>
        /// <param name="state">
        ///   一个包含由 <paramref name="beginMethod" /> 委托使用的数据的对象。
        /// </param>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <returns>
        ///   创建的表示异步操作的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="beginMethod" /> 参数为 null。
        /// 
        ///   - 或 -
        /// 
        ///   时，将引发的异常 <paramref name="endMethod" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> FromAsync<TResult>(Func<AsyncCallback, object, IAsyncResult> beginMethod,
            Func<IAsyncResult, TResult> endMethod, object state)
        { 
            var cancellation = _cancellationToken.Register(() => UnityTask.FromCanceled<TResult>());
            if (_cancellationToken.IsCancellationRequested)
            {
                cancellation.Dispose();
                return UnityTask.FromCanceled<TResult>();
            }
            var uTcs = new UnityTaskCompletionSource<TResult>();
            try
            {
                beginMethod(result =>
                {
                    try
                    {
                        var value = endMethod(result);
                        uTcs.TrySetResult(value);
                    }
                    catch (Exception e)
                    {
                        uTcs.TrySetException(e);
                    }
                    finally
                    {
                        cancellation.Dispose();
                    }
                },state);
            }
            catch(Exception e)
            {
                uTcs.TrySetException(e);
                cancellation.Dispose();
            }
            return uTcs.Task;
        }

        /// <summary>
        ///   创建一个 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />，表示符合异步编程模型模式的成对的开始和结束方法。
        /// </summary>
        /// <param name="beginMethod">用于启动异步操作的委托。</param>
        /// <param name="endMethod">用于结束异步操作的委托。</param>
        /// <param name="arg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数。
        /// </param>
        /// <param name="state">
        ///   一个包含由 <paramref name="beginMethod" /> 委托使用的数据的对象。
        /// </param>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <returns>
        ///   创建的表示异步操作的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="beginMethod" /> 参数为 null。
        /// 
        ///   - 或 -
        /// 
        ///   时，将引发的异常 <paramref name="endMethod" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> FromAsync<TArg1, TResult>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state)
        {
            return FromAsync((callback, _) => beginMethod(arg1, callback, state), endMethod, state);
        }

        /// <summary>
        ///   创建一个 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />，表示符合异步编程模型模式的成对的开始和结束方法。
        /// </summary>
        /// <param name="beginMethod">用于启动异步操作的委托。</param>
        /// <param name="endMethod">用于结束异步操作的委托。</param>
        /// <param name="arg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数。
        /// </param>
        /// <param name="state">
        ///   一个包含由 <paramref name="beginMethod" /> 委托使用的数据的对象。
        /// </param>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <returns>
        ///   创建的表示异步操作的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="beginMethod" /> 参数为 null。
        /// 
        ///   - 或 -
        /// 
        ///   时，将引发的异常 <paramref name="endMethod" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> FromAsync<TArg1, TArg2, TResult>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
        {
            return FromAsync((callback, _) => beginMethod(arg1, arg2, callback, state), endMethod, state);
        }

        /// <summary>
        ///   创建一个 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />，表示符合异步编程模型模式的成对的开始和结束方法。
        /// </summary>
        /// <param name="beginMethod">用于启动异步操作的委托。</param>
        /// <param name="endMethod">用于结束异步操作的委托。</param>
        /// <param name="arg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数。
        /// </param>
        /// <param name="state">
        ///   一个包含由 <paramref name="beginMethod" /> 委托使用的数据的对象。
        /// </param>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数的类型。
        /// </typeparam>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <returns>
        ///   创建的表示异步操作的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="beginMethod" /> 参数为 null。
        /// 
        ///   - 或 -
        /// 
        ///   时，将引发的异常 <paramref name="endMethod" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
        {
            return FromAsync((callback, _) => beginMethod(arg1, arg2, arg3, callback, state), endMethod, state);
        }

        /// <summary>
        ///   创建一个 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />，表示符合异步编程模型模式的成对的开始和结束方法。
        /// </summary>
        /// <param name="beginMethod">用于启动异步操作的委托。</param>
        /// <param name="endMethod">用于结束异步操作的委托。</param>
        /// <param name="arg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///   传递给 <paramref name="beginMethod" /> 委托的第四个参数。
        /// </param>
        /// <param name="state">
        ///   一个包含由 <paramref name="beginMethod" /> 委托使用的数据的对象。
        /// </param>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg4">
        ///   传递给 <paramref name="beginMethod" /> 委托的第四个参数的类型。
        /// </typeparam>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <returns>
        ///   创建的表示异步操作的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="beginMethod" /> 参数为 null。
        /// 
        ///   - 或 -
        /// 
        ///   时，将引发的异常 <paramref name="endMethod" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> FromAsync<TArg1, TArg2, TArg3, TArg4, TResult>(Func<TArg1, TArg2, TArg3, TArg4, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state)
        {
            return FromAsync((callback, _) => beginMethod(arg1, arg2, arg3, arg4, callback, state), endMethod, state);
        }

        /// <summary>
        ///   创建一个 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />，表示符合异步编程模型模式的成对的开始和结束方法。
        /// </summary>
        /// <param name="beginMethod">用于启动异步操作的委托。</param>
        /// <param name="endMethod">用于结束异步操作的委托。</param>
        /// <param name="arg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///   传递给 <paramref name="beginMethod" /> 委托的第四个参数。
        /// </param>
        /// <param name="arg5">
        ///   传递给 <paramref name="beginMethod" /> 委托的第五个参数。
        /// </param>
        /// <param name="state">
        ///   一个包含由 <paramref name="beginMethod" /> 委托使用的数据的对象。
        /// </param>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg4">
        ///   传递给 <paramref name="beginMethod" /> 委托的第四个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg5">
        ///   传递给 <paramref name="beginMethod" /> 委托的第五个参数的类型。
        /// </typeparam>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <returns>
        ///   创建的表示异步操作的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="beginMethod" /> 参数为 null。
        /// 
        ///   - 或 -
        /// 
        ///   时，将引发的异常 <paramref name="endMethod" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> FromAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state)
        {
            return FromAsync((callback, _) => beginMethod(arg1, arg2, arg3, arg4, arg5, callback, state), endMethod, state);
        }

        /// <summary>
        ///   创建一个 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />，表示符合异步编程模型模式的成对的开始和结束方法。
        /// </summary>
        /// <param name="beginMethod">用于启动异步操作的委托。</param>
        /// <param name="endMethod">用于结束异步操作的委托。</param>
        /// <param name="arg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数。
        /// </param>
        /// <param name="arg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数。
        /// </param>
        /// <param name="arg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数。
        /// </param>
        /// <param name="arg4">
        ///   传递给 <paramref name="beginMethod" /> 委托的第四个参数。
        /// </param>
        /// <param name="arg5">
        ///   传递给 <paramref name="beginMethod" /> 委托的第五个参数。
        /// </param>
        /// <param name="arg6">
        ///   传递给 <paramref name="beginMethod" /> 委托的第六个参数。
        /// </param>
        /// <param name="state">
        ///   一个包含由 <paramref name="beginMethod" /> 委托使用的数据的对象。
        /// </param>
        /// <typeparam name="TArg1">
        ///   传递给 <paramref name="beginMethod" /> 委托的第一个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg2">
        ///   传递给 <paramref name="beginMethod" /> 委托的第二个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg3">
        ///   传递给 <paramref name="beginMethod" /> 委托的第三个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg4">
        ///   传递给 <paramref name="beginMethod" /> 委托的第四个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg5">
        ///   传递给 <paramref name="beginMethod" /> 委托的第五个参数的类型。
        /// </typeparam>
        /// <typeparam name="TArg6">
        ///   传递给 <paramref name="beginMethod" /> 委托的第六个参数的类型。
        /// </typeparam>
        /// <typeparam name="TResult">
        ///   可通过 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 获得的结果的类型。
        /// </typeparam>
        /// <returns>
        ///   创建的表示异步操作的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   时，将引发的异常 <paramref name="beginMethod" /> 参数为 null。
        /// 
        ///   - 或 -
        /// 
        ///   时，将引发的异常 <paramref name="endMethod" /> 参数为 null。
        /// </exception>
        public UnityTask<TResult> FromAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state)
        {
            return FromAsync((callback, _) => beginMethod(arg1, arg2, arg3, arg4, arg5, arg6, callback, state), endMethod, state);
        }
        #endregion
    }
}

