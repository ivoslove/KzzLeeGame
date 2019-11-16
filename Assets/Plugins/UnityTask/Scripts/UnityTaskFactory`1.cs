

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UnityEngine.TaskExtension
{
    /// <summary>
    ///   提供对创建和计划 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 对象的支持。
    /// </summary>
    /// <typeparam name="TResult">
    ///   此类的方法创建的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 对象的返回值。
    /// </typeparam>
    public class UnityTaskFactory<TResult>
    {
        #region private fields

        private readonly UnityTaskFactory _factory;

        #endregion

        #region ctor

        /// <summary>
        ///   使用默认配置初始化 <see cref="T:UnityEngine.TaskExtension.UnityTaskFactory`1" /> 实例。
        /// </summary>
        public UnityTaskFactory() : this(CancellationToken.None)
        {
            
        }

        /// <summary>
        ///   使用默认配置初始化 <see cref="T:UnityEngine.TaskExtension.UnityTaskFactory`1" /> 实例。
        /// </summary>
        /// <param name="cancellationToken">
        ///   将指派给由此 <see cref="T:UnityEngine.TaskExtension.UnityTaskFactory`1" /> 创建的任务的默认取消标记（除非在调用工厂方法时显式指定另一个取消标记）。
        /// </param>
        internal UnityTaskFactory(CancellationToken cancellationToken)
        {
            _factory = new UnityTaskFactory(cancellationToken);
        }

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
        public UnityTask<TResult> StartNew(Func<IEnumerator> function)
        {
            return _factory.StartNew<TResult>(function);
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
        public UnityTask<TResult> StartNew(Func<object, IEnumerator> function, object state)
        {
            return _factory.StartNew<TResult>(function, state);
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
        public UnityTask<TResult> StartNew<TArg1>(Func<TArg1, IEnumerator> function, TArg1 arg1)
        {
            return _factory.StartNew<TArg1, TResult>(function, arg1);
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
        public UnityTask<TResult> StartNew<TArg1, TArg2>(Func<TArg1, TArg2, IEnumerator> function, TArg1 arg1, TArg2 arg2)
        {
            return _factory.StartNew<TArg1, TArg2, TResult>(function, arg1,arg2);
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
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, IEnumerator> function, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            return _factory.StartNew<TArg1, TArg2, TArg3, TResult>(function, arg1, arg2, arg3);
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
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4>(Func<TArg1, TArg2, TArg3, TArg4, IEnumerator> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
        {
            return _factory.StartNew<TArg1, TArg2, TArg3, TArg4, TResult>(function, arg1, arg2, arg3, arg4);
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
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4, TArg5>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, IEnumerator> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
        {
            return _factory.StartNew<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(function, arg1, arg2, arg3, arg4, arg5);
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
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IEnumerator> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
        {
            return _factory.StartNew<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(function, arg1, arg2, arg3, arg4, arg5, arg6);
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
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IEnumerator> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7)
        {
            return _factory.StartNew<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
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
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IEnumerator> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8)
        {
            return _factory.StartNew<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
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
        public UnityTask<UnityTask<T>> ContinueWhenAll<T>(IList<UnityTask> tasks,
            Func<IList<UnityTask>, IEnumerator> continuation)
        {
            return _factory.ContinueWhenAll<T>(tasks, continuation);
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
        public UnityTask ContinueWhenAll(IList<UnityTask> tasks, Func<IList<UnityTask>, IEnumerator> continuation)
        {
            return _factory.ContinueWhenAll(tasks, continuation);
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
            _factory.ContinueWhenAll(tasks, continuation);
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
        public void ContinueWhenAll<T>(IList<UnityTask<T>> tasks, Action<Dictionary<string, T>> continuation)
        {
            _factory.ContinueWhenAll(tasks, continuation);
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
            return _factory.ContinueWhenAll(tasks, continuation);
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
        public UnityTask<TResult> StartNew(Func<TResult> function)
        {
            return _factory.StartNew(function);
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
        public UnityTask<TResult> StartNew(Func<object, TResult> function, object state)
        {
            return _factory.StartNew(function, state);
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
        public UnityTask<TResult> StartNew<TArg1>(Func<TArg1, TResult> function, TArg1 arg1)
        {
            return _factory.StartNew(function, arg1);
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
            return _factory.StartNew(function, arg1,arg2);
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
            return _factory.StartNew(function, arg1, arg2, arg3);
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
            return _factory.StartNew(function, arg1, arg2, arg3, arg4);
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
        public UnityTask<TResult> StartNew<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult> function, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
        {
            return _factory.StartNew(function, arg1, arg2, arg3, arg4, arg5);
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
            return _factory.StartNew(function, arg1, arg2, arg3, arg4, arg5, arg6);
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
            return _factory.StartNew(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
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
            return _factory.StartNew(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
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
        public UnityTask<TResult> FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod,
            Func<IAsyncResult, TResult> endMethod, object state)
        {
            return _factory.FromAsync(beginMethod, endMethod, state);
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
        public UnityTask<TResult> FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state)
        {
            return _factory.FromAsync(beginMethod, endMethod, arg1, state);
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
        public UnityTask<TResult> FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
        {
            return _factory.FromAsync(beginMethod, endMethod, arg1, arg2, state);
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
        public UnityTask<TResult> FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
        {
            return _factory.FromAsync(beginMethod, endMethod, arg1, arg2, arg3, state);
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
        public UnityTask<TResult> FromAsync<TArg1, TArg2, TArg3, TArg4>(Func<TArg1, TArg2, TArg3, TArg4, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state)
        {
            return _factory.FromAsync(beginMethod, endMethod, arg1, arg2, arg3, arg4, state);
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
        public UnityTask<TResult> FromAsync<TArg1, TArg2, TArg3, TArg4, TArg5>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state)
        {
            return _factory.FromAsync(beginMethod, endMethod, arg1, arg2, arg3, arg4, arg5, state);
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
        public UnityTask<TResult> FromAsync<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state)
        {
            return _factory.FromAsync(beginMethod, endMethod, arg1, arg2, arg3, arg4, arg5, arg6, state);
        }
        #endregion
    }
}