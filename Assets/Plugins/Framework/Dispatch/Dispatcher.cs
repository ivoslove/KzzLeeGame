using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Component;

namespace App.Dispatch
{
    /// <summary>
    /// 调度者
    /// </summary>
    public abstract class Dispatcher
    {
        #region private static fields

        protected static RepositoryComponent<string, Delegate> _cache;     //任务缓存[任务ID,待处理回调]

        #endregion

        #region delegates

        /// <summary>
        /// 工作
        /// </summary>
        public delegate void Work();

        /// <summary>
        /// 工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <param name="arg1">参数1</param>
        public delegate void Work<in TArg1>(TArg1 arg1);

        /// <summary>
        /// 工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        public delegate void Work<in TArg1, in TArg2>(TArg1 arg1, TArg2 arg2);

        /// <summary>
        /// 工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="arg3">参数3</param>
        public delegate void Work<in TArg1, in TArg2, in TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3);

        /// <summary>
        /// 工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <typeparam name="TArg4">参数类型4</typeparam>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="arg3">参数3</param>
        /// <param name="arg4">参数4</param>
        public delegate void Work<in TArg1, in TArg2, in TArg3, in TArg4>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4);

        /// <summary>
        /// 工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <typeparam name="TArg4">参数类型4</typeparam>
        /// <typeparam name="TArg5">参数类型5</typeparam>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="arg3">参数3</param>
        /// <param name="arg4">参数4</param>
        /// <param name="arg5">参数5</param>
        public delegate void Work<in TArg1, in TArg2, in TArg3, in TArg4, in TArg5>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5);

        /// <summary>
        /// 工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <typeparam name="TArg4">参数类型4</typeparam>
        /// <typeparam name="TArg5">参数类型5</typeparam>
        /// <typeparam name="TArg6">参数类型6</typeparam>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="arg3">参数3</param>
        /// <param name="arg4">参数4</param>
        /// <param name="arg5">参数5</param>
        /// <param name="arg6">参数6</param>
        public delegate void Work<in TArg1, in TArg2, in TArg3, in TArg4, in TArg5,in TArg6>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5,TArg6 arg6);

        #endregion

        #region ctor

        static Dispatcher()
        {
            _cache = _cache ?? new RepositoryComponent<string, Delegate>();
        }

        #endregion

        #region public static funcs

        #region Listener


        /// <summary>
        /// 监听工作
        /// </summary>
        /// <param name="workId">工作ID</param>
        /// <param name="work">工作事务</param>
        public static void Listener(string workId, Work work)
        {
            var handle = _cache.Get(workId);
            if (handle == null)
            {
                _cache.Set(workId, null);
            }
            var value = (Work)_cache.Get(workId) + work;
            _cache.Set(workId,value);
        }

        /// <summary>
        /// 监听工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="work">工作事务</param>
        public static void Listener<TArg1>(string workId, Work<TArg1> work)
        {
            var handle = _cache.Get(workId);
            if (handle == null)
            {
                _cache.Set(workId, null);
            }
            var value = (Work<TArg1>)_cache.Get(workId) + work;
            _cache.Set(workId, value);
        }

        /// <summary>
        /// 监听工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="work">工作事务</param>
        public static void Listener<TArg1, TArg2>(string workId, Work<TArg1, TArg2> work)
        {
            var handle = _cache.Get(workId);
            if (handle == null)
            {
                _cache.Set(workId, null);
            }
            var value = (Work<TArg1, TArg2>)_cache.Get(workId) + work;
            _cache.Set(workId, value);
        }

        /// <summary>
        /// 监听工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="work">工作事务</param>
        public static void Listener<TArg1, TArg2, TArg3>(string workId, Work<TArg1, TArg2, TArg3> work)
        {
            var handle = _cache.Get(workId);
            if (handle == null)
            {
                _cache.Set(workId, null);
            }
            var value = (Work<TArg1, TArg2, TArg3>)_cache.Get(workId) + work;
            _cache.Set(workId, value);
        }

        /// <summary>
        /// 监听工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <typeparam name="TArg4">参数类型4</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="work">工作事务</param>
        public static void Listener<TArg1, TArg2, TArg3, TArg4>(string workId, Work<TArg1, TArg2, TArg3, TArg4> work)
        {
            var handle = _cache.Get(workId);
            if (handle == null)
            {
                _cache.Set(workId, null);
            }
            var value = (Work<TArg1, TArg2, TArg3, TArg4>)_cache.Get(workId) + work;
            _cache.Set(workId, value);
        }

        /// <summary>
        /// 监听工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <typeparam name="TArg4">参数类型4</typeparam>
        /// <typeparam name="TArg5">参数类型5</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="work">工作事务</param>
        public static void Listener<TArg1, TArg2, TArg3, TArg4, TArg5>(string workId, Work<TArg1, TArg2, TArg3, TArg4, TArg5> work)
        {
            var handle = _cache.Get(workId);
            if (handle == null)
            {
                _cache.Set(workId, null);
            }
            var value = (Work<TArg1, TArg2, TArg3, TArg4, TArg5>)_cache.Get(workId) + work;
            _cache.Set(workId, value);
        }

        /// <summary>
        /// 监听工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <typeparam name="TArg4">参数类型4</typeparam>
        /// <typeparam name="TArg5">参数类型5</typeparam>
        /// <typeparam name="TArg6">参数类型6</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="work">工作事务</param>
        public static void Listener<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(string workId, Work<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> work)
        {
            var handle = _cache.Get(workId);
            if (handle == null)
            {
                _cache.Set(workId, null);
            }
            var value = (Work<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>)_cache.Get(workId) + work;
            _cache.Set(workId, value);
        }

        #endregion

        #region Remove

        /// <summary>
        /// 移除工作
        /// </summary>
        /// <param name="workId">工作ID</param>
        public static void Remove(string workId)
        {
            _cache.RemoveAllFromKey(p => p == workId);
        }

        /// <summary>
        /// 移除工作
        /// </summary>
        /// <param name="workId">工作ID</param>
        /// <param name="work"></param>
        public static void Remove(string workId,Work work)
        {
            var handle = _cache.Get(workId);
            if (handle == null)
            {
                Remove(workId);
                return;
            }
            _cache.Set(workId, (Work)handle - work);
        }

        /// <summary>
        /// 移除工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="work">工作事务</param>
        public static void Remove<TArg1>(string workId, Work<TArg1> work)
        {
            var handle = _cache.Get(workId);
            if (handle == null)
            {
                Remove(workId);
                return;
            }
            _cache.Set(workId, (Work<TArg1>)handle - work);
        }

        /// <summary>
        /// 移除工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="work">工作事务</param>
        public static void Remove<TArg1, TArg2>(string workId, Work<TArg1, TArg2> work)
        {
            var handle = _cache.Get(workId);
            if (handle == null)
            {
                Remove(workId);
                return;
            }
            _cache.Set(workId, (Work<TArg1, TArg2>)handle - work);
        }

        /// <summary>
        /// 移除工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="work">工作事务</param>
        public static void Remove<TArg1, TArg2, TArg3>(string workId, Work<TArg1, TArg2, TArg3> work)
        {
            var handle = _cache.Get(workId);
            if (handle == null)
            {
                Remove(workId);
                return;
            }
            _cache.Set(workId, (Work<TArg1, TArg2, TArg3>)handle - work);
        }

        /// <summary>
        /// 移除工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <typeparam name="TArg4">参数类型4</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="work">工作事务</param>
        public static void Remove<TArg1, TArg2, TArg3, TArg4>(string workId, Work<TArg1, TArg2, TArg3, TArg4> work)
        {
            var handle = _cache.Get(workId);
            if (handle == null)
            {
                Remove(workId);
                return;
            }
            _cache.Set(workId, (Work<TArg1, TArg2, TArg3, TArg4>)handle - work);
        }

        /// <summary>
        /// 移除工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <typeparam name="TArg4">参数类型4</typeparam>
        /// <typeparam name="TArg5">参数类型5</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="work">工作事务</param>
        public static void Remove<TArg1, TArg2, TArg3, TArg4, TArg5>(string workId, Work<TArg1, TArg2, TArg3, TArg4, TArg5> work)
        {
            var handle = _cache.Get(workId);
            if (handle == null)
            {
                Remove(workId);
                return;
            }
            _cache.Set(workId, (Work<TArg1, TArg2, TArg3, TArg4, TArg5>)handle - work);
        }

        /// <summary>
        /// 移除工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <typeparam name="TArg4">参数类型4</typeparam>
        /// <typeparam name="TArg5">参数类型5</typeparam>
        /// <typeparam name="TArg6">参数类型6</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="work">工作事务</param>
        public static void Remove<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(string workId, Work<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> work)
        {
            var handle = _cache.Get(workId);
            if (handle == null)
            {
                Remove(workId);
                return;
            }
            _cache.Set(workId, (Work<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>)handle - work);
        }

        #endregion

        #region DoWork

        /// <summary>
        /// 执行工作
        /// </summary>
        /// <param name="workId">工作ID</param>
        public static void DoWork(string workId)
        {
            var handle = _cache.Get(workId);
            (handle as Work)?.Invoke();
        }

        /// <summary>
        /// 执行工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="arg1">参数1</param>
        public static void DoWork<TArg1>(string workId, TArg1 arg1)
        {
            var handle = _cache.Get(workId);
            (handle as Work<TArg1>)?.Invoke(arg1);
        }

        /// <summary>
        /// 执行工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        public static void DoWork<TArg1, TArg2>(string workId, TArg1 arg1, TArg2 arg2)
        {
            var handle = _cache.Get(workId);
            (handle as Work<TArg1, TArg2>)?.Invoke(arg1, arg2);
        }

        /// <summary>
        /// 执行工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="arg3">参数3</param>
        public static void DoWork<TArg1, TArg2, TArg3>(string workId, TArg1 arg1, TArg2 arg2,
            TArg3 arg3)
        {
            var handle = _cache.Get(workId);
            (handle as Work<TArg1, TArg2, TArg3>)?.Invoke(arg1, arg2, arg3);
        }

        /// <summary>
        /// 执行工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <typeparam name="TArg4">参数类型4</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="arg3">参数3</param>
        /// <param name="arg4">参数4</param>
        public static void DoWork<TArg1, TArg2, TArg3, TArg4>(string workId, TArg1 arg1, TArg2 arg2,
            TArg3 arg3, TArg4 arg4)
        {
            var handle = _cache.Get(workId);
            (handle as Work<TArg1, TArg2, TArg3, TArg4>)?.Invoke(arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// 执行工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <typeparam name="TArg4">参数类型4</typeparam>
        /// <typeparam name="TArg5">参数类型5</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="arg3">参数3</param>
        /// <param name="arg4">参数4</param>
        /// <param name="arg5">参数5</param>
        public static void DoWork<TArg1, TArg2, TArg3, TArg4, TArg5>(string workId, TArg1 arg1, TArg2 arg2,
            TArg3 arg3, TArg4 arg4, TArg5 arg5)
        {
            var handle = _cache.Get(workId);
            (handle as Work<TArg1, TArg2, TArg3, TArg4, TArg5>)?.Invoke(arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// 执行工作
        /// </summary>
        /// <typeparam name="TArg1">参数类型1</typeparam>
        /// <typeparam name="TArg2">参数类型2</typeparam>
        /// <typeparam name="TArg3">参数类型3</typeparam>
        /// <typeparam name="TArg4">参数类型4</typeparam>
        /// <typeparam name="TArg5">参数类型5</typeparam>
        /// <typeparam name="TArg6">参数类型6</typeparam>
        /// <param name="workId">工作ID</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="arg3">参数3</param>
        /// <param name="arg4">参数4</param>
        /// <param name="arg5">参数5</param>
        /// <param name="arg6">参数6</param>
        public static void DoWork<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(string workId, TArg1 arg1, TArg2 arg2,
            TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
        {
            var handle = _cache.Get(workId);
            (handle as Work<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>)?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
        }


        #endregion

        #endregion


    }
}

