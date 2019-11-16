using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UnityEngine.TaskExtension
{
    /// <summary>
    /// 表示一个Unity主线程异步操作。
    /// </summary>
    public abstract class UnityTask
    {
        #region protected members

        protected List<Action<UnityTask>> _continuationActions;
        protected AggregateException _exception;
        protected bool _isCompleted; //是否完成
        protected bool _isCanceled; //是否取消

        #endregion

        #region ctor

        protected UnityTask()
        {
            _continuationActions =_continuationActions ?? new List<Action<UnityTask>>();
        }

        #endregion

        #region internal properties

        /// <summary>
        /// 协同编码
        /// </summary>
        internal string CoroutineCode { get; set; }

        #endregion

        #region public properties

        /// <summary>
        /// 获取工厂
        /// </summary>
        internal static UnityTaskFactory Factory => new UnityTaskFactory();

        /// <summary>
        /// 获取应用程序执行过程中发生的错误
        /// </summary>
        public AggregateException Exception => _exception;

        /// <summary>
        /// 获取任务是否失败
        /// </summary>
        public bool IsFaulted => Exception != null;

        /// <summary>
        /// 获取任务是否完成
        /// </summary>
        public bool IsCompleted => _isCompleted;

        /// <summary>
        /// 获取任务是否被取消
        /// </summary>
        public bool IsCanceled => _isCanceled;

        #endregion
         
        #region public functions

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 完成时异步执行的延续任务。
        /// </summary>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <returns>
        ///   一个新的延续 <see cref="T:UnityEngine.TaskExtension.UnityTask" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public UnityTask ContinueWith(Action<UnityTask> continuation)
        {
            return ContinueWith(continuation, CancellationToken.None);
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 完成时可接收取消标记并以异步方式执行的延续任务。
        /// </summary>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个自变量传递给完成的任务。
        /// </param>
        /// <param name="cancellationToken">
        ///   将指派给新的延续任务的 <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" />。
        /// </param>
        /// <returns>
        ///   一个新的延续 <see cref="T:UnityEngine.TaskExtension.UnityTask" />。
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   <see cref="T:System.Threading.CancellationTokenSource" /> 创建该标记已被释放。
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 null。
        /// </exception>
        public UnityTask ContinueWith(Action<UnityTask> continuation, CancellationToken cancellationToken)
        {
            bool completed = false;
            UnityTaskCompletionSource<int> uTcs = new UnityTaskCompletionSource<int>();
            CancellationTokenRegistration cancellationTokenRegistration =
                cancellationToken.Register(() => uTcs.TrySetCanceled());
            completed = _isCompleted;
            if (!completed)
            {
                _continuationActions.Add(t =>
                {
                    try
                    {
                        continuation(this);
                        uTcs.TrySetResult(0);
                    }
                    catch (Exception e)
                    {
                        uTcs.TrySetException(e);
                    }
                    finally
                    {
                        cancellationTokenRegistration.Dispose();
                    }
                });
            }
            else
            {
                UnityTaskScheduler.Post(() =>
                {
                    try
                    {
                        continuation(this);
                        uTcs.TrySetResult(0);
                    }
                    catch (Exception e)
                    {
                        uTcs.TrySetException(e);
                    }
                    finally
                    {
                        cancellationTokenRegistration.Dispose();
                    }
                });
            }
            return uTcs.Task;
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 完成时异步执行的延续任务。
        /// </summary>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <returns>
        ///   一个新的延续 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public UnityTask<UnityTask<TResult>> ContinueWith<TResult>(Func<UnityTask, UnityTask<TResult>> continuation)
        {
            return ContinueWith(continuation, CancellationToken.None);
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 完成时可接收取消标记并以异步方式执行的延续任务。
        /// </summary>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个自变量传递给完成的任务。
        /// </param>
        /// <param name="cancellationToken">
        ///   将指派给新的延续任务的 <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" />。
        /// </param>
        /// <returns>
        ///   一个新的延续 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   <see cref="T:System.Threading.CancellationTokenSource" /> 创建该标记已被释放。
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 null。
        /// </exception>
        public UnityTask<UnityTask<TResult>> ContinueWith<TResult>(Func<UnityTask, UnityTask<TResult>> continuation,
            CancellationToken cancellationToken)
        {
            bool completed = false;
            UnityTaskCompletionSource<UnityTask<TResult>> uTcs = new UnityTaskCompletionSource<UnityTask<TResult>>();
            CancellationTokenRegistration cancellationTokenRegistration =
                cancellationToken.Register(() => uTcs.TrySetCanceled());
            completed = _isCompleted;
            if (!completed)
            {
                _continuationActions.Add(t =>
                {
                    try
                    {
                        var result = continuation(t);
                        uTcs.TrySetResult(result);
                    }
                    catch (Exception e)
                    {
                        uTcs.TrySetException(e);
                    }
                    finally
                    {
                        cancellationTokenRegistration.Dispose();
                    }
                });
            }
            else
            {
                UnityTaskScheduler.Post(() =>
                {
                    try
                    {
                        var result = continuation(this);
                        uTcs.TrySetResult(result);
                    }
                    catch (Exception e)
                    {
                        uTcs.TrySetException(e);
                    }
                    finally
                    {
                        cancellationTokenRegistration.Dispose();
                    }
                });
            }

            return uTcs.Task;
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 完成时异步执行的延续任务。
        /// </summary>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <returns>
        ///   一个新的延续 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public UnityTask<UnityTask> ContinueWith(Func<UnityTask, UnityTask> continuation)
        {
            return ContinueWith(continuation, CancellationToken.None);
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 完成时可接收取消标记并以异步方式执行的延续任务。
        /// </summary>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个自变量传递给完成的任务。
        /// </param>
        /// <param name="cancellationToken">
        ///   将指派给新的延续任务的 <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" />。
        /// </param>
        /// <returns>
        ///   一个新的延续 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   <see cref="T:System.Threading.CancellationTokenSource" /> 创建该标记已被释放。
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 null。
        /// </exception>
        public UnityTask<UnityTask> ContinueWith(Func<UnityTask, UnityTask> continuation,
            CancellationToken cancellationToken)
        {        
            bool completed = false;
            UnityTaskCompletionSource<UnityTask> uTcs = new UnityTaskCompletionSource<UnityTask>();
            CancellationTokenRegistration cancellationTokenRegistration =
                cancellationToken.Register(() => uTcs.TrySetCanceled());
            completed = _isCompleted;
            if (!completed)
            {
                _continuationActions.Add(t =>
                {
                    try
                    {
                        var result = continuation(t);
                        uTcs.TrySetResult(result);
                    }
                    catch (Exception e)
                    {
                        uTcs.TrySetException(e);
                    }
                    finally
                    {
                        cancellationTokenRegistration.Dispose();
                    }
                });
            }
            else
            {
                UnityTaskScheduler.Post(() =>
                {
                    try
                    {
                        var result = continuation(this);
                        uTcs.TrySetResult(result);
                    }
                    catch (Exception e)
                    {
                        uTcs.TrySetException(e);
                    }
                    finally
                    {
                        cancellationTokenRegistration.Dispose();
                    }
                });
            }
            return uTcs.Task;
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 完成时可接收取消标记并以异步方式执行的延续任务。
        /// </summary>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个自变量传递给完成的任务。
        /// </param>
        /// <returns>
        ///   一个新的延续 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   <see cref="T:System.Threading.CancellationTokenSource" /> 创建该标记已被释放。
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 null。
        /// </exception>
        public UnityTask<UnityTask<TResult>> ContinueWith<TResult>(Func<UnityTask, IEnumerator> continuation)
        {
            return ContinueWith<TResult>(continuation, CancellationToken.None);
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 完成时可接收取消标记并以异步方式执行的延续任务。
        /// </summary>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个自变量传递给完成的任务。
        /// </param>
        /// <param name="cancellationToken">
        ///   将指派给新的延续任务的 <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" />。
        /// </param>
        /// <returns>
        ///   一个新的延续 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   <see cref="T:System.Threading.CancellationTokenSource" /> 创建该标记已被释放。
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 null。
        /// </exception>
        public UnityTask<UnityTask<TResult>> ContinueWith<TResult>(Func<UnityTask, IEnumerator> continuation, CancellationToken cancellationToken)
        {
            return ContinueWith(t=>Run<TResult>(() => continuation(t)), cancellationToken);
        }

        #endregion

        #region public static funcs

        /// <summary>
        ///   将在线程池上运行的指定工作排队，并返回代表该工作的 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 对象。
        /// </summary>
        /// <param name="action">以异步方式执行的工作量。</param>
        /// <returns>表示在主线程执行的队列的任务。</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="action" /> 参数是 <see langword="null" />。
        /// </exception>
        public static UnityTask Run(Action action)
        {
            return FromResult(0).ContinueWith(t => action() );
        }

        /// <summary>
        ///   将在线程池上运行的指定工作排队，并返回代表该工作的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 对象。
        /// </summary>
        /// <param name="function">以异步方式执行的工作。</param>
        /// <typeparam name="TResult">任务的返回类型。</typeparam>
        /// <returns>表示在主线程中排队执行的工作的任务对象。</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="function" /> 参数为 <see langword="null" />。
        /// </exception>
        public static UnityTask<TResult> Run<TResult>(Func<TResult> function)
        {
            return FromResult(0).ContinueWith(t => function());
        }

        /// <summary>
        ///   将在线程池上运行的指定工作排队，并返回代表该工作的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 对象。
        /// </summary>
        /// <param name="function">以异步方式执行的工作。</param>
        /// <typeparam name="TResult">任务的返回类型。</typeparam>
        /// <returns>表示在主线程中排队执行的工作的任务对象。</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="function" /> 参数为 <see langword="null" />。
        /// </exception>
        public static UnityTask<TResult> Run<TResult>(Func<IEnumerator> function)
        {
            return Factory.StartNew<TResult>(function);
        }

        /// <summary>
        ///   将在线程池上运行的指定工作排队，并返回代表该工作的 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 对象。
        /// </summary>
        /// <param name="function">以异步方式执行的工作量。</param>
        /// <returns>表示在主线程执行的队列的任务。</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="function" /> 参数是 <see langword="null" />。
        /// </exception>
        public static UnityTask Run(Func<IEnumerator> function)
        {
            return Factory.StartNew<object>(function);
        }

        /// <summary>
        ///   创建一个任务，该任务将在数组中的所有 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 对象都完成时完成。
        /// </summary>
        /// <param name="tasks">等待完成的任务。</param>
        /// <returns>表示所有提供的任务的完成情况的任务。</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="tasks" /> 参数为 <see langword="null" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="tasks" /> 数组包含 <see langword="null" /> 任务。
        /// </exception>
        public static UnityTask WhenAll(params UnityTask[] tasks)
        {
            return WhenAll(tasks.ToList());
        }

        /// <summary>
        ///   创建一个任务，该任务将在可枚举集合中的所有 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 对象都完成时完成。
        /// </summary>
        /// <param name="tasks">等待完成的任务。</param>
        /// <returns>表示所有提供的任务的完成情况的任务。</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="tasks" /> 参数为 <see langword="null" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   此 <paramref name="tasks" /> 集合包含 <see langword="null" /> 任务。
        /// </exception>
        public static UnityTask WhenAll(IEnumerable<UnityTask> tasks)
        {
            var taskArray = tasks.ToArray();
            if (taskArray.Length == 0)
            {
                return FromResult(0);
            }
            var uTcs = new UnityTaskCompletionSource<object>();
            Factory.ContinueWhenAll(taskArray, t =>
            {
                Exception[] exceptions = taskArray.Where(p => p.IsFaulted).Select(p => p.Exception).ToArray();
                if (exceptions.Length>0)
                {
                    uTcs.SetException(new AggregateException(exceptions));
                }
                else if (taskArray.Any(p=>p.IsCanceled))
                {
                    uTcs.SetCanceled();
                }
                else
                {
                    uTcs.SetResult(0);
                }
            });
            return uTcs.Task;
        }

        /// <summary>
        /// 创建一个任务，该任务将在可枚举集合中的所有 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 对象都完成时完成。
        /// </summary>
        /// <param name="tasks">等待完成的任务。</param>
        /// <returns>表示所有提供的任务的完成情况的任务。</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="tasks" /> 参数为 <see langword="null" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   此 <paramref name="tasks" /> 集合包含 <see langword="null" /> 任务。
        /// </exception>
        public static UnityTask<List<TResult>> WhenAll<TResult>(params UnityTask<TResult>[] tasks)
        {
            return WhenAll(tasks.ToList());
        }

        /// <summary>
        /// 创建一个任务，该任务将在可枚举集合中的所有 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 对象都完成时完成。
        /// </summary>
        /// <param name="tasks">等待完成的任务。</param>
        /// <returns>表示所有提供的任务的完成情况的任务。</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="tasks" /> 参数为 <see langword="null" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   此 <paramref name="tasks" /> 集合包含 <see langword="null" /> 任务。
        /// </exception>
        public static UnityTask<List<TResult>> WhenAll<TResult>(IEnumerable<UnityTask<TResult>> tasks)
        {    
            var unityTasks = tasks.ToList();
            var uTcs = new UnityTaskCompletionSource<List<TResult>>();
            Factory.ContinueWhenAll(unityTasks, p =>
            {
                var listResult = unityTasks.Select(t => p[t.GetHashCode().ToString()]).ToList();
                uTcs.TrySetResult(listResult);
            });
            return uTcs.Task;
        }

        /// <summary>任何提供的任务已完成时，创建将完成的任务。</summary>
        /// <param name="tasks">等待完成的任务。</param>
        /// <returns>
        ///   表示提供的任务之一已完成的任务。
        ///     返回任务的结果是完成的任务。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="tasks" /> 参数为 <see langword="null" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="tasks" />数组包含 null 的任务，或者为空。
        /// </exception>
        internal static UnityTask<UnityTask> WhenAny(params UnityTask[] tasks)
        {
            return WhenAny((IEnumerable<UnityTask>)tasks);
        }

        /// <summary>任何提供的任务已完成时，创建将完成的任务。</summary>
        /// <param name="tasks">等待完成的任务。</param>
        /// <returns>
        ///   表示提供的任务之一已完成的任务。
        ///     返回任务的结果是完成的任务。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="tasks" /> 参数为 <see langword="null" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="tasks" />数组包含 null 的任务，或者为空。
        /// </exception>
        internal static UnityTask<UnityTask> WhenAny(IEnumerable<UnityTask> tasks)
        {
            var uTcs = new UnityTaskCompletionSource<UnityTask>();
            foreach (var task in tasks)
            {
                task.ContinueWith(p => uTcs.TrySetResult(p));
            }
            return uTcs.Task;
        }

        /// <summary>
        ///   创建指定结果的、成功完成的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />。
        /// </summary>
        /// <param name="result">存储入已完成任务的结果。</param>
        /// <typeparam name="T">任务返回的结果的类型。</typeparam>
        /// <returns>已成功完成的任务。</returns>
        public static UnityTask<T> FromResult<T>(T result)
        {
            var uTcs = new UnityTaskCompletionSource<T>();
            uTcs.SetResult(result);
            return uTcs.Task;
        }

        /// <summary>
        /// 创建 <see cref="T:UnityEngine.TaskExtension.UnityTask" />，它是以指定的异常来完成的。
        /// </summary>
        /// <param name="exception">完成任务的异常</param>
        /// <returns>出错的任务</returns>
        public static UnityTask FromException(Exception exception)
        {
            var uTcs = new UnityTaskCompletionSource<int>();
            uTcs.SetException(exception);
            return uTcs.Task;
        }

        /// <summary>
        /// 创建 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />，它是以指定的异常来完成的。
        /// </summary>
        /// <typeparam name="T">任务返回的结果的类型</typeparam>
        /// <param name="exception">完成任务的异常</param>
        /// <returns>出错的任务</returns>
        public static UnityTask<T> FromException<T>(Exception exception)
        {
            var uTcs = new UnityTaskCompletionSource<T>();
            uTcs.SetException(exception);
            return uTcs.Task;
        }

        /// <summary>
        ///   创建 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" />取消操作。
        /// </summary>
        /// <returns>取消的任务。</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">    
        /// </exception>
        public static UnityTask<T> FromCanceled<T>()
        {
            var uTcs = new UnityTaskCompletionSource<T>();
            uTcs.SetCanceled();
            return uTcs.Task;
        }

        /// <summary>
        ///   创建 <see cref="T:UnityEngine.TaskExtension.UnityTask" />，取消操作。
        /// </summary>
        /// <returns>取消的任务。</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">    
        /// </exception>
        public static UnityTask FromCanceled()
        {
            var uTcs = new UnityTaskCompletionSource<int>();
            uTcs.SetCanceled();
            return uTcs.Task;
        }

        /// <summary>创建一个在指定的时间间隔后完成的可取消任务。</summary>
        /// <param name="delay">
        ///   在完成返回的任务前等待的时间跨度；如果无限期等待，则为 <see langword="TimeSpan.FromMilliseconds(-1)" />。
        /// </param>
        /// <returns>表示时间延迟的任务。</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="delay" /> 表示负时间间隔以外 <see langword="TimeSpan.FromMillseconds(-1)" />。
        /// 
        ///   - 或 -
        /// 
        ///   <paramref name="delay" /> 参数的 <see cref="P:System.TimeSpan.TotalMilliseconds" /> 属性大于 <see cref="F:System.Int32.MaxValue" />。
        /// </exception>
        /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">
        ///   该任务已取消。
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        /// </exception>
        public static UnityTask RunDelay(TimeSpan delay)
        {
            return Task.Delay(delay,CancellationToken.None).AsForeground();
        }

        /// <summary>创建一个在指定的时间间隔后完成的可取消任务。</summary>
        /// <param name="delay">
        ///   在完成返回的任务前等待的时间跨度；如果无限期等待，则为 <see langword="TimeSpan.FromMilliseconds(-1)" />。
        /// </param>
        /// <param name="cancellationToken">将在完成返回的任务之前选中的取消标记。</param>
        /// <returns>表示时间延迟的任务。</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="delay" /> 表示负时间间隔以外 <see langword="TimeSpan.FromMillseconds(-1)" />。
        /// 
        ///   - 或 -
        /// 
        ///   <paramref name="delay" /> 参数的 <see cref="P:System.TimeSpan.TotalMilliseconds" /> 属性大于 <see cref="F:System.Int32.MaxValue" />。
        /// </exception>
        /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">
        ///   该任务已取消。
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   已释放提供的 <paramref name="cancellationToken" />。
        /// </exception>
        public static UnityTask RunDelay(TimeSpan delay, CancellationToken cancellationToken)
        {
            return Task.Delay(delay, cancellationToken).AsForeground();
        }

        #endregion
    }
}




