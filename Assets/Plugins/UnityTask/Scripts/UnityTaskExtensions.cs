using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UnityEngine.TaskExtension
{
    /// <summary>
    ///  提供了一套用于处理特定类型的静态实例。
    /// </summary>
    public static class UnityTaskExtensions
    {
        /// <summary>
        ///   创建一个代理 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 表示异步操作的 <see cref="M:UnityEngine.TaskExtension.UnityTaskScheduler.Post(System.Action)" />。
        /// </summary>
        /// <param name="task">
        ///  进行解包的任务。
        /// </param>
        /// <returns>
        ///   表示所提供的异步操作的任务 <see langword="UnityEngine.TaskExtension.UnityTask" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   如果引发的异常 <paramref name="task" /> 参数为 null。
        /// </exception>
        public static UnityTask Unwrap(this UnityTask<UnityTask> task)
        {
            var uTcs = new UnityTaskCompletionSource<object>();
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    uTcs.TrySetException(t.Exception);
                }else if (t.IsCanceled)
                {
                    uTcs.TrySetCanceled();
                }
                else
                {
                    task.Result.ContinueWith(inner =>
                    {
                        if (inner.IsFaulted)
                        {
                            uTcs.TrySetException(inner.Exception);
                        }
                        else if (inner.IsCanceled)
                        {
                            uTcs.TrySetCanceled();
                        }
                        else
                        {
                            uTcs.TrySetResult(null);
                        }
                    });
                }
            });
            return uTcs.Task;
        }

        /// <summary>
        ///   创建一个代理 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 表示异步操作的Task
        /// </summary>
        /// <param name="task">
        ///   进行解包的任务。
        /// </param>
        /// <typeparam name="T">该任务的结果的类型。</typeparam>
        /// <returns>
        ///   一个 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 表示所提供的异步操作Task
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   如果引发的异常 <paramref name="task" /> 参数为 null。
        /// </exception>
        public static UnityTask<T> Unwrap<T>(this UnityTask<UnityTask<T>> task)
        {
            var uTcs = new UnityTaskCompletionSource<T>();
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    uTcs.TrySetException(t.Exception);
                }
                else if (t.IsCanceled)
                {
                    uTcs.TrySetCanceled();
                }
                else
                {
                    t.Result.ContinueWith(inner =>
                    {
                        if (inner.IsFaulted)
                        {
                            uTcs.TrySetException(inner.Exception);
                        }
                        else if (inner.IsCanceled)
                        {
                            uTcs.TrySetCanceled();
                        }
                        else
                        {
                            uTcs.TrySetResult(inner.Result);
                        }
                    });
                }
            });
            return uTcs.Task;
        }

        /// <summary>
        /// 空任务
        /// </summary>
        /// <typeparam name="TResult">该任务的结果的类型。</typeparam>
        /// <returns></returns>
        public static UnityTask<IEnumerable<TResult>> EndForNull<TResult>()
        {
            UnityTaskCompletionSource<IEnumerable<TResult>> uTcs = new UnityTaskCompletionSource<IEnumerable<TResult>>();
            uTcs.SetResult(null);
            return uTcs.Task;
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 成功执行时异步执行的延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:UnityEngine.TaskExtension.UnityTask" /></param>
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
        public static UnityTask OnSuccess(this UnityTask task, Action<UnityTask> continuation)
        {
            UnityTaskCompletionSource<object> uTcs = new UnityTaskCompletionSource<object>();
            task.ContinueWith(t =>
            {
                if (t.IsFaulted || t.IsCanceled)
                {
                    uTcs.TrySetException(t.Exception);
                }
                else
                {
                    continuation(t);
                    uTcs.TrySetResult(null);
                }
            });
            return uTcs.Task;
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 成功执行时异步执行的延续任务。
        /// </summary>
        /// <typeparam name="TResult">该任务的结果的类型。</typeparam>
        /// <param name="task">目标<see cref="T:UnityEngine.TaskExtension.UnityTask`1" /></param>
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
        public static UnityTask<TResult> OnSuccess<TResult>(this UnityTask task, Func<UnityTask, TResult> continuation)
        {
            UnityTaskCompletionSource<TResult> uTcs = new UnityTaskCompletionSource<TResult>();
            task.ContinueWith(t =>
            {
                if (t.IsFaulted || t.IsCanceled)
                {
                    uTcs.TrySetException(t.Exception);                   
                }
                else
                {
                    TResult result = continuation(t);
                    uTcs.TrySetResult(result);
                }
            });
            return uTcs.Task;
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 成功执行时异步执行的延续任务。
        /// </summary>
        /// <typeparam name="TIn">该任务的参数的类型。</typeparam>
        /// <param name="task">目标<see cref="T:UnityEngine.TaskExtension.UnityTask`1" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <returns>
        ///   一个新的延续 <see cref="T:UnityEngine.TaskExtension.UnityTask" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static UnityTask OnSuccess<TIn>(this UnityTask<TIn> task, Action<UnityTask<TIn>> continuation)
        {
            return task.OnSuccess((UnityTask t) => continuation((UnityTask<TIn>) t));
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 成功执行时异步执行的延续任务。
        /// </summary>
        /// <typeparam name="TIn">该任务的参数的类型。</typeparam>
        /// <typeparam name="TResult">该任务的结果的类型。</typeparam>
        /// <param name="task">目标<see cref="T:UnityEngine.TaskExtension.UnityTask`1" /></param>
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
        public static UnityTask<TResult> OnSuccess<TIn, TResult>(this UnityTask<TIn> task,
            Func<UnityTask<TIn>, TResult> continuation)
        {
            return task.OnSuccess((UnityTask t) => continuation((UnityTask<TIn>)t));
        }


        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 成功执行时异步执行的<see cref="T:System.Threading.Tasks.Task" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:UnityEngine.TaskExtension.UnityTask" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static Task ContinueToBackground(this UnityTask task, Action<UnityTask> continuation)
        {
            return task.ContinueToBackground(continuation, CancellationToken.None);
        }
    

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 成功执行时异步执行的<see cref="T:System.Threading.Tasks.Task" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:UnityEngine.TaskExtension.UnityTask" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <param name="cancellationToken">将指派给新的延续任务的 <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" />。</param>
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static Task ContinueToBackground(this UnityTask task, Action<UnityTask> continuation,
            CancellationToken cancellationToken)
        {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            task.ContinueWith(t =>
            {
                if (t.IsFaulted || t.IsCanceled)
                {
                    tcs.TrySetException(t.Exception);

                }
                else
                {
                    Task.Run(() => { continuation(t); }, cancellationToken)
                        .ContinueWith(tt =>
                        {
                            if (tt.IsFaulted || tt.IsCanceled)
                            {
                               tcs.TrySetException(tt.Exception ?? tt.DefaultException());
                            }
                            else
                            {
                                tcs.TrySetResult(0);
                            }
                        }, cancellationToken);
                }
            });
            return tcs.Task;
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 成功执行时异步执行的<see cref="T:System.Threading.Tasks.Task`1" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:UnityEngine.TaskExtension.UnityTask" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static Task<TResult> ContinueToBackground<TResult>(this UnityTask task,
            Func<UnityTask, TResult> continuation)
        {
            return task.ContinueToBackground(continuation, CancellationToken.None);
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 成功执行时异步执行的<see cref="T:System.Threading.Tasks.Task`1" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:UnityEngine.TaskExtension.UnityTask" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <param name="cancellationToken">将指派给新的延续任务的 <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" />。</param>
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static Task<TResult> ContinueToBackground<TResult>(this UnityTask task,
            Func<UnityTask, TResult> continuation, CancellationToken cancellationToken)
        {
            TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
            task.ContinueWith(t =>
            {
                if (t.IsFaulted || t.IsCanceled)
                {
                    tcs.TrySetException(t.Exception);
                }
                else
                {
                    Task.Run(() => continuation(t), cancellationToken).ContinueWith(a =>
                    {
                        if (a.IsFaulted || a.IsCanceled)
                        {
                            tcs.TrySetException(a.Exception ?? a.DefaultException());
                        }
                        else
                        {
                            tcs.TrySetResult(a.Result);
                        }
                    }, cancellationToken);
                }
            });
            return tcs.Task;
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 成功执行时异步执行的<see cref="T:System.Threading.Tasks.Task" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:UnityEngine.TaskExtension.UnityTask`1" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>        
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static Task ContinueToBackground<T>(this UnityTask<T> task, Action<UnityTask<T>> continuation)
        {
            return task.ContinueToBackground(continuation, CancellationToken.None);
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 成功执行时异步执行的<see cref="T:System.Threading.Tasks.Task" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:UnityEngine.TaskExtension.UnityTask`1" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <param name="cancellationToken">将指派给新的延续任务的 <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" />。</param>
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static Task ContinueToBackground<T>(this UnityTask<T> task, Action<UnityTask<T>> continuation,
            CancellationToken cancellationToken)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            task.ContinueWith(p =>
            {
                if (p.IsFaulted || p.IsCanceled)
                {
                    tcs.TrySetException(p.Exception);
                }
                else
                {
                    Task.Run(() => continuation(p), cancellationToken).ContinueWith(t =>
                    {
                        if (t.IsFaulted || t.IsCanceled)
                        {
                            tcs.TrySetException(t.Exception ?? t.DefaultException());
                        }
                        else
                        {
                            tcs.TrySetResult(0);
                        }
                    }, cancellationToken);
                }
            });
            return tcs.Task;
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 成功执行时异步执行的<see cref="T:System.Threading.Tasks.Task`1" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:UnityEngine.TaskExtension.UnityTask`1" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static Task<TResult> ContinueToBackground<TIn, TResult>(this UnityTask<TIn> task,
            Func<UnityTask<TIn>, TResult> continuation)
        {
            return task.ContinueToBackground(continuation, CancellationToken.None);
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 成功执行时异步执行的<see cref="T:System.Threading.Tasks.Task`1" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:UnityEngine.TaskExtension.UnityTask`1" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <param name="cancellationToken">将指派给新的延续任务的 <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" />。</param>
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static Task<TResult> ContinueToBackground<TIn, TResult>(this UnityTask<TIn> task,
            Func<UnityTask<TIn>, TResult> continuation, CancellationToken cancellationToken)
        {
            TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
            task.ContinueWith(p =>
            {
                if (p.IsFaulted || p.IsCanceled)
                {
                    tcs.TrySetException(p.Exception);
                }
                else
                {
                    Task.Run(() => continuation(p), cancellationToken).ContinueWith(t =>
                    {
                        if (t.IsFaulted || t.IsCanceled)
                        {
                            tcs.TrySetException(t.Exception ?? t.DefaultException());
                        }
                        else
                        {
                            tcs.TrySetResult(t.Result);
                        }
                    }, cancellationToken);
                }
            });
            return tcs.Task;

        }

        /// <summary>
        /// 作为后台线程
        /// </summary>
        /// <param name="task">前台线程</param>
        /// <returns></returns>
        public static Task AsBackground(this UnityTask task)
        {
            return task.ContinueToBackground(p => p.IsFaulted ? Task.FromException(p.Exception) : Task.FromResult(0))
                .Unwrap();
        }

        /// <summary>
        /// 作为后台线程
        /// </summary>
        /// <param name="task">前台线程</param>
        /// <returns></returns>
        public static Task<T> AsBackground<T>(this UnityTask<T> task)
        {
            return task.ContinueToBackground(p =>
                p.IsFaulted ? Task.FromException<T>(p.Exception) : Task.FromResult(p.Result)).Unwrap();
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:System.Threading.Tasks.Task" /> 成功执行时异步执行的<see cref="T:UnityEngine.TaskExtension.UnityTask" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:System.Threading.Tasks.Task" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:System.Threading.Tasks.Task" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static UnityTask ContinueToForeground(this Task task, Action<Task> continuation)
        {
           return task.ContinueToForeground(continuation, CancellationToken.None);
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:System.Threading.Tasks.Task" /> 成功执行时异步执行的<see cref="T:UnityEngine.TaskExtension.UnityTask" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:System.Threading.Tasks.Task" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:System.Threading.Tasks.Task" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <param name="cancellationToken">将指派给新的延续任务的 <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" />。</param>
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static UnityTask ContinueToForeground(this Task task, Action<Task> continuation,
            CancellationToken cancellationToken)
        {
            UnityTaskCompletionSource<object> uTcs = new UnityTaskCompletionSource<object>();
            var cancellation = cancellationToken.Register(() => uTcs.TrySetCanceled());
            task.ContinueWith(p =>
            {
                UnityTaskScheduler.Post(() =>
                {
                    try
                    {
                        continuation(p);
                        uTcs.TrySetResult(0);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        uTcs.TrySetException(e);
                    }
                    finally
                    {
                        cancellation.Dispose();
                    }
                });
            }, cancellationToken);
            return uTcs.Task;
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:System.Threading.Tasks.Task" /> 成功执行时异步执行的<see cref="T:UnityEngine.TaskExtension.UnityTask`1" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:System.Threading.Tasks.Task" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:System.Threading.Tasks.Task" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>      
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static UnityTask<TResult> ContinueToForeground<TResult>(this Task task, Func<Task, TResult> continuation)
        {
            return task.ContinueToForeground(continuation, CancellationToken.None);
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:System.Threading.Tasks.Task" /> 成功执行时异步执行的<see cref="T:UnityEngine.TaskExtension.UnityTask`1" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:System.Threading.Tasks.Task" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:System.Threading.Tasks.Task" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <param name="cancellationToken">将指派给新的延续任务的 <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" />。</param>
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static UnityTask<TResult> ContinueToForeground<TResult>(this Task task, Func<Task,TResult> continuation,
            CancellationToken cancellationToken)
        {
            UnityTaskCompletionSource<TResult> uTcs = new UnityTaskCompletionSource<TResult>();
            var cancellation = cancellationToken.Register(() => uTcs.TrySetCanceled());
            task.ContinueWith(p =>
            {
                UnityTaskScheduler.Post(() =>
                {
                    try
                    {
                        var result = continuation(p);
                        uTcs.TrySetResult(result);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        uTcs.TrySetException(e);
                    }
                    finally
                    {
                        cancellation.Dispose();
                    }
                });
            }, cancellationToken);
            return uTcs.Task;
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:System.Threading.Tasks.Task`1" /> 成功执行时异步执行的<see cref="T:UnityEngine.TaskExtension.UnityTask" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:System.Threading.Tasks.Task`1" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:System.Threading.Tasks.Task`1" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static UnityTask ContinueToForeground<TIn>(this Task<TIn> task, Action<Task<TIn>> continuation)
        {
            return task.ContinueToForeground(continuation,CancellationToken.None);
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:System.Threading.Tasks.Task`1" /> 成功执行时异步执行的<see cref="T:UnityEngine.TaskExtension.UnityTask" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:System.Threading.Tasks.Task`1" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:System.Threading.Tasks.Task`1" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <param name="cancellationToken">将指派给新的延续任务的 <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" />。</param>
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static UnityTask ContinueToForeground<TIn>(this Task<TIn> task, Action<Task<TIn>> continuation,
            CancellationToken cancellationToken)
        {
            UnityTaskCompletionSource<int> uTcs = new UnityTaskCompletionSource<int>();
            var cancellation = cancellationToken.Register(() => uTcs.TrySetCanceled());
            task.ContinueWith(p =>
            {
                UnityTaskScheduler.Post(() =>
                {
                    try
                    {
                        continuation(p);
                        uTcs.TrySetResult(0);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        uTcs.TrySetException(e);
                    }
                    finally
                    {
                        cancellation.Dispose();
                    }
                });
            }, cancellationToken);
            return uTcs.Task;
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:System.Threading.Tasks.Task`1" /> 成功执行时异步执行的<see cref="T:UnityEngine.TaskExtension.UnityTask`1" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:System.Threading.Tasks.Task`1" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:System.Threading.Tasks.Task`1" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>      
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static UnityTask<TResult> ContinueToForeground<TIn, TResult>(this Task<TIn> task, Func<Task<TIn>, TResult> continuation)
        {
            return task.ContinueToForeground(continuation, CancellationToken.None);
        }

        /// <summary>
        ///   创建一个在目标 <see cref="T:System.Threading.Tasks.Task`1" /> 成功执行时异步执行的<see cref="T:UnityEngine.TaskExtension.UnityTask`1" />延续任务。
        /// </summary>
        /// <param name="task">目标<see cref="T:System.Threading.Tasks.Task`1" /></param>
        /// <param name="continuation">
        ///   在 <see cref="T:System.Threading.Tasks.Task`1" /> 完成时要运行的操作。
        ///    在运行时，委托将作为一个参数传递给完成的任务。
        /// </param>
        /// <param name="cancellationToken">将指派给新的延续任务的 <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" />。</param>
        /// <returns>
        ///   一个新的延续 <see cref="T:System.Threading.Tasks.Task" />。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="continuation" /> 参数为 <see langword="null" />。
        /// </exception>
        public static UnityTask<TResult> ContinueToForeground<TIn,TResult>(this Task<TIn> task, Func<Task<TIn>,TResult> continuation,
            CancellationToken cancellationToken)
        {
            UnityTaskCompletionSource<TResult> uTcs = new UnityTaskCompletionSource<TResult>();
            var cancellation = cancellationToken.Register(() => uTcs.TrySetCanceled());
            task.ContinueWith(p =>
            {
                UnityTaskScheduler.Post(() =>
                {
                    try
                    {
                       var result = continuation(p);
                        uTcs.TrySetResult(result);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        uTcs.TrySetException(e);
                    }
                    finally
                    {
                        cancellation.Dispose();
                    }
                });
            }, cancellationToken);
            return uTcs.Task;
        }

        /// <summary>
        /// 作为Unity主线程
        /// </summary>
        /// <param name="task">一个<see cref="T:System.Threading.Tasks.Task" />任务</param>
        /// <returns></returns>
        public static UnityTask AsForeground(this Task task)
        {
            return task.ContinueToForeground(p =>
                p.IsFaulted ? UnityTask.FromException<int>(p.Exception) : UnityTask.FromResult(0)).Unwrap();
        }

        /// <summary>
        /// 作为Unity主线程
        /// </summary>
        /// <param name="task">一个<see cref="T:System.Threading.Tasks.Task`1" />任务</param>
        /// <returns></returns>
        public static UnityTask<TResult> AsForeground<TResult>(this Task<TResult> task)
        {
            return task.ContinueToForeground(p =>
                p.IsFaulted ? UnityTask.FromException<TResult>(p.Exception) : UnityTask.FromResult(p.Result)).Unwrap();
        }

        #region private funcs

        /// <summary>
        /// 返回一个默认的Exception
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns></returns>
        private static Exception DefaultException(this Task task)
        {
            return new Exception(
                $"Task Id:{task.Id} Error,IsFaulted:{task.IsFaulted} ,IsCanceled:{task.IsCanceled} ,Status:{task.Status}");
        }

        #endregion
    }

}