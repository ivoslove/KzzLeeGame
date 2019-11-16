//using System;
//using System.Threading.Tasks;

//using System;

//namespace UnityEngine.TaskExtension
//{
//    /// <summary>
//    ///   表示未绑定到委托的 <see cref="T:UnityEngine.TaskExtension.UnityTask" /> 的制造者方，并通过 <see cref="P:UnityEngine.TaskExtension.UnityTaskCompletionSource`1.Task" /> 属性提供对使用者方的访问。
//    /// </summary>
//    /// <typeparam name="TResult">
//    ///   与此 <see cref="T:UnityEngine.TaskExtension.UnityTaskCompletionSource`1" /> 关联的结果值的类型。
//    /// </typeparam>
//    public class UnityTaskCompletionSource<TResult>
//    {
//        #region public properties
//        public UnityTask<TResult> Task { get; private set; }

//        #endregion

//        #region ctor
//        public UnityTaskCompletionSource()
//        {
//            Task = new UnityTask<TResult>();
//        }

//        #endregion

//        #region public funcs

//        /// <summary>
//        /// Transitions the underlying Task&lt;TResult&gt; into the Canceled state
//        /// </summary>
//        public void SetCanceled()
//        {
//            if (!TrySetCanceled())
//            {
//                throw new InvalidOperationException("Cannot cancel a completed task.");
//            }

//        }

//        /// <summary>
//        /// Attempts to transition the underlying Task&lt;TResult&gt; into the Canceled state.
//        /// </summary>
//        /// <returns></returns>
//        public bool TrySetCanceled()
//        {
//            return Task.TrySetCanceled();
//        }

//        /// <summary>
//        /// Transitions the underlying Task&lt;TResult&gt; into the Faulted state and binds it to a specified exception.
//        /// </summary>
//        /// <param name="exception"></param>
//        public void SetException(AggregateException exception)
//        {
//            if (!TrySetException(exception))
//            {
//                throw new InvalidOperationException("Cannot set the exception of a completed task.");
//            }
//        }

//        /// <summary>
//        /// Transitions the underlying Task&lt;TResult&gt; into the Faulted state and binds it to a specified exception.
//        /// </summary>
//        /// <param name="exception"></param>
//        public void SetException(Exception exception)
//        {
//            if (!TrySetException(exception))
//            {
//                throw new InvalidOperationException("Cannot set the exception of a completed task.");
//            }
//        }

//        /// <summary>
//        /// Attempts to transition the underlying Task&lt;TResult&gt; into the Faulted state and binds it to a specified exception.
//        /// </summary>
//        /// <param name="exception">exception</param>
//        /// <returns></returns>
//        public bool TrySetException(AggregateException exception)
//        {
//            return Task.TrySetException(exception);
//        }

//        /// <summary>
//        ///  Attempts to transition the underlying Task&lt;TResult&gt; into the Faulted state and binds it to a specified exception.
//        /// </summary>
//        /// <param name="exception">exception</param>
//        /// <returns></returns>
//        public bool TrySetException(Exception exception)
//        {
//            if (exception is AggregateException aggregateException)
//            {
//                return Task.TrySetException(aggregateException);
//            }
//            UnityTask<TResult> task = Task;
//            Exception[] innerExceptions = { exception };
//            return task.TrySetException(new AggregateException(innerExceptions).Flatten());
//        }

//        /// <summary>
//        /// Transitions the underlying Task&lt;TResult&gt; into the RanToCompletion state.
//        /// </summary>
//        /// <param name="result">result</param>
//        public void SetResult(TResult result)
//        {
//            if (!this.TrySetResult(result))
//            {
//                throw new InvalidOperationException("Cannot set the result of a completed task.");
//            }
//        }

//        /// <summary>
//        /// Attempts to transition the underlying Task&lt;TResult&gt; into the RanToCompletion state.
//        /// </summary>
//        /// <param name="result">result</param>
//        /// <returns></returns>
//        public bool TrySetResult(TResult result)
//        {
//            return this.Task.TrySetResult(result);
//        }

//        #endregion
//    }
//}


using System;
using System.Collections.Generic;
using System.Threading;

namespace UnityEngine.TaskExtension
{
    /// <summary>
    ///   表示未绑定到委托的 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 的制造者方，并通过 <see cref="P:UnityEngine.TaskExtension.UnityTaskCompletionSource`1.Task" /> 属性提供对使用者方的访问。
    /// </summary>
    /// <typeparam name="TResult">
    ///   与此 <see cref="T:UnityEngine.TaskExtension.UnityTaskCompletionSource`1" /> 关联的结果值的类型。
    /// </typeparam>
    public class UnityTaskCompletionSource<TResult>
    {
        /// <summary>
        ///   创建一个 <see cref="T:UnityEngine.TaskExtension.UnityTaskCompletionSource`1" />。
        /// </summary>
        public UnityTaskCompletionSource()
        {
            Task = new UnityTask<TResult>();
        }

        /// <summary>
        ///   获取由此 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 创建的 <see cref="T:UnityEngine.TaskExtension.UnityTaskCompletionSource`1" />。
        /// </summary>
        /// <returns>
        ///   返回由此 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 创建的 <see cref="T:UnityEngine.TaskExtension.UnityTaskCompletionSource`1" />。
        /// </returns>
        public UnityTask<TResult> Task { get; }

        private void SpinUntilCompleted()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                SpinWait spinWait = new SpinWait();
                while (!Task.IsCompleted)
                    spinWait.SpinOnce();
            });
        }

        /// <summary>
        ///   尝试将基础 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 转换为 <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> 状态，并将其绑定到一个指定异常上。
        /// </summary>
        /// <param name="exception">
        ///   要绑定到此 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 的异常。
        /// </param>
        /// <returns>如果操作成功，则为 true；否则为 false。</returns>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   已释放 <see cref="P:UnityEngine.TaskExtension.UnityTaskCompletionSource`1.Task" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="exception" /> 参数为 null。
        /// </exception>
        public bool TrySetException(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));
            bool flag = this.Task.TrySetException(new AggregateException(exception));
            if (!flag && !this.Task.IsCompleted)
                this.SpinUntilCompleted();
            return flag;
        }

        /// <summary>
        ///   尝试将基础 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 转换为 <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> 状态，并对其绑定一些异常对象。
        /// </summary>
        /// <param name="exceptions">
        ///   要绑定到此 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 的异常的集合。
        /// </param>
        /// <returns>如果操作成功，则为 true；否则为 false。</returns>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   已释放 <see cref="P:UnityEngine.TaskExtension.UnityTaskCompletionSource`1.Task" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="exceptions" /> 参数为 null。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="exceptions" /> 中有一个或多个 null 元素。
        /// 
        ///   - 或 -
        /// 
        ///   <paramref name="exceptions" /> 集合为空。
        /// </exception>
        public bool TrySetException(IEnumerable<Exception> exceptions)
        {
            if (exceptions == null)
                throw new ArgumentNullException(nameof(exceptions));
            List<Exception> exceptionList = new List<Exception>();
            foreach (Exception exception in exceptions)
            {
                if (exception == null)
                    throw new ArgumentException(nameof(exceptions) + "TaskCompletionSourceT_TrySetException_NullException");
                exceptionList.Add(exception);
            }
            if (exceptionList.Count == 0)
                throw new ArgumentException(nameof(exceptions) + "TaskCompletionSourceT_TrySetException_NoExceptions");
            bool flag = this.Task.TrySetException(new AggregateException(exceptionList));
            if (!flag && !this.Task.IsCompleted)
                this.SpinUntilCompleted();
            return flag;
        }


        /// <summary>
        ///   将基础 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 转换为 <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> 状态，并将其绑定到一个指定异常上。
        /// </summary>
        /// <param name="exception">
        ///   要绑定到此 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 的异常。
        /// </param>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   已释放 <see cref="P:UnityEngine.TaskExtension.UnityTaskCompletionSource`1.Task" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="exception" /> 参数为 null。
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   基础 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 已处于以下三种最终状态的其中一种：<see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" />、<see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> 或 <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" />。
        /// </exception>
        public void SetException(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));
            if (!this.TrySetException(exception))
                throw new InvalidOperationException("TaskT_TransitionToFinal_AlreadyCompleted");
        }

        /// <summary>
        ///   将基础 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 转换为 <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> 状态，并对其绑定一些异常对象。
        /// </summary>
        /// <param name="exceptions">
        ///   要绑定到此 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 的异常的集合。
        /// </param>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   已释放 <see cref="P:UnityEngine.TaskExtension.UnityTaskCompletionSource`1.Task" />。
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="exceptions" /> 参数为 null。
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="exceptions" /> 中有一个或多个 null 元素。
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   基础 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 已处于以下三种最终状态的其中一种：<see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" />、<see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> 或 <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" />。
        /// </exception>
        public void SetException(IEnumerable<Exception> exceptions)
        {
            if (!this.TrySetException(exceptions))
                throw new InvalidOperationException("TaskT_TransitionToFinal_AlreadyCompleted");
        }

        /// <summary>
        ///   尝试将基础 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 转换为 <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" /> 状态。
        /// </summary>
        /// <param name="result">
        ///   要绑定到此 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 的结果值。
        /// </param>
        /// <returns>如果操作成功，则为 true；否则为 false。</returns>
        public bool TrySetResult(TResult result)
        {
            bool flag = this.Task.TrySetResult(result);
            if (!flag && !this.Task.IsCompleted)
                this.SpinUntilCompleted();
            return flag;
        }

        /// <summary>
        ///   将基础 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 转换为 <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" /> 状态。
        /// </summary>
        /// <param name="result">
        ///   要绑定到此 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 的结果值。
        /// </param>
        /// <exception cref="T:System.ObjectDisposedException">
        ///   <see cref="P:UnityEngine.TaskExtension.UnityTaskCompletionSource`1.Task" /> 已被释放。
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   基础 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 已在三种的最终状态之一 ︰ <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" />, ，<see cref="F:System.Threading.Tasks.TaskStatus.Faulted" />, ，或 <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" />。
        /// </exception>
        public void SetResult(TResult result)
        {
            if (!this.TrySetResult(result))
                throw new InvalidOperationException("TaskT_TransitionToFinal_AlreadyCompleted");
        }

        /// <summary>
        ///   尝试将基础 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 转换为 <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" /> 状态并启用要存储在取消的任务中的取消标记。
        /// </summary>
        /// <returns>
        ///   如果操作成功，则为 <see langword="true" />；否则为 <see langword="false" />。
        /// </returns>
        public bool TrySetCanceled()
        {
            bool flag = this.Task.TrySetCanceled();
            if (!flag && !this.Task.IsCompleted)
                this.SpinUntilCompleted();
            return flag;
        }

        /// <summary>
        ///   将基础 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 转换为 <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" /> 状态。
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">
        ///   基础 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 已在三种的最终状态之一︰ <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" />, ，<see cref="F:System.Threading.Tasks.TaskStatus.Faulted" />, ，或 <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" />, ，或者，如果基础 <see cref="T:UnityEngine.TaskExtension.UnityTask`1" /> 被释放。
        /// </exception>
        public void SetCanceled()
        {
            if (!this.TrySetCanceled())
                throw new InvalidOperationException("TaskT_TransitionToFinal_AlreadyCompleted");
        }
    }
}
