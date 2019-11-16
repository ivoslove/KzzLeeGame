using System;
using System.Collections;
using System.Linq;
using System.Threading;

namespace UnityEngine.TaskExtension
{
    /// <summary>
    /// 表示一个可以返回值的Unity主线程异步操作。
    /// </summary>
    /// <typeparam name="T"> 此 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 生成的结果的类型。</typeparam>
    public sealed class UnityTask<T>:UnityTask
    {
        #region private fields

        private T _result;           //结果

        #endregion

        #region public properties

        /// <summary>
        /// Gets the result of the task. If the task is not complete, this property blocks
        /// until the task is complete. If the task has an Exception or was cancelled, this
        /// property will rethrow the exception.
        /// </summary>
        public T Result => _result;

        public static UnityTask<T> None => FromResult(default(T));


        #endregion

        #region public funcs

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
        public UnityTask<UnityTask<T2>> ContinueWith<T2>(Func<UnityTask<T>, UnityTask<T2>> continuation)
        {
            return base.ContinueWith(t => continuation((UnityTask<T>) t));
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
        public UnityTask<UnityTask> ContinueWith(Func<UnityTask<T>, UnityTask> continuation)
        {
            return base.ContinueWith(t => continuation((UnityTask<T>) t));
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
        public UnityTask ContinueWith(Action<UnityTask<T>> continuation)
        {
            return base.ContinueWith(t =>
            {
                try
                {
                    continuation((UnityTask<T>) t);
                    return FromResult(0);
                }
                catch (Exception e)
                {
                   Debug.LogError(e);
                   return FromException<int>(e);
                }
            });
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
        public UnityTask<T2> ContinueWith<T2>(Func<UnityTask<T>, T2> continuation)
        {
            return base.ContinueWith(t =>
            {
                try
                {
                    T2 result = continuation((UnityTask<T>) t);
                    return FromResult(result);
                }
                catch (Exception e)
                {
                    return FromException<T2>(e);
                }
            }).Unwrap();
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
        public UnityTask<UnityTask<TResult>> ContinueWith<TResult>(Func<UnityTask<T>, IEnumerator> continuation)
        {
            return base.ContinueWith<TResult>(t => continuation((UnityTask<T>) t));
        }

        #endregion

        #region internal funcs

        /// <summary>
        /// try to cancel
        /// </summary>
        /// <returns></returns>
        internal bool TrySetCanceled()
        {
            if (_isCompleted)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(CoroutineCode))
            {
               UnityTaskScheduler.Cancel(CoroutineCode);
            }
            _isCompleted = true;
            _isCanceled = true;
            RunContinuations();
            return true;
        }

        /// <summary>
        /// try to set specified exception
        /// </summary>
        /// <param name="exception">异常</param>
        /// <returns></returns>
        internal bool TrySetException(AggregateException exception)
        {
            if (_isCompleted)
            {
                return false;
            }
            _isCompleted = true;
            _exception = exception;
            RunContinuations();
            return true;
        }

        /// <summary>
        /// try to set specified result
        /// </summary>
        /// <param name="result">返回结果</param>
        /// <returns></returns>
        internal bool TrySetResult(T result)
        {
            if (_isCompleted)
            {
                return false;
            }
            _isCompleted = true;
            _result = result;
            RunContinuations();
            return true;
        }

        #endregion

        #region private funcs

        /// <summary>
        /// run tasks in continuation list
        /// </summary>
        private void RunContinuations()
        {
            if (Thread.CurrentThread.IsBackground)
            {
                var continueActions = _continuationActions.ToList();
                UnityTaskScheduler.Post(() =>
                {
                    continueActions.ForEach(p=>p(this));
                });
                _continuationActions.Clear();
            }
            else
            {
                _continuationActions.ForEach(p=>p(this));
                _continuationActions.Clear();
            }
        }

        #endregion
    }
}