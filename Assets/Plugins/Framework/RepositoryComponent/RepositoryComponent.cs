
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Component
{

    /// <summary>
    /// 仓储组件(用于存储数据)
    /// </summary>
    /// <typeparam name="TKey">存储键</typeparam>
    /// <typeparam name="TValue">存储值</typeparam>
    public class RepositoryComponent<TKey,TValue> : BaseComponent
    {
        #region private fields
        private Dictionary<TKey, TValue> _cache;                //缓存
        #endregion

        #region ctor

        /// <summary>
        /// 构造仓储组件
        /// </summary>
        public RepositoryComponent()
        {
            _cache = new Dictionary<TKey, TValue>();
        }

        #endregion

        #region public funcs

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="isCover">是否覆盖原有数据</param>
        public TValue Set(TKey key, TValue value, bool isCover = true)
        {
            if (_cache.ContainsKey(key))
            {
                if (isCover)
                {
                    _cache[key] = value;
                }
                else
                {
                    return _cache[key];
                }
            }
            else
            {
                _cache.Add(key, value);
            }
            return value;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key">需要获取的键</param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            _cache.TryGetValue(key, out var value);
            return value;
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(TKey key)
        {
            if (_cache.ContainsKey(key))
            {
                _cache.Remove(key);
            }
        }

        /// <summary>
        /// 获取是否包含满足条件的键
        /// </summary>
        /// <param name="match">键条件</param>
        /// <returns>存在</returns>
        public bool IsContainsKey(Func<TKey,bool> match)
        {
            return FirstKeyFromKey(match) != null;
        }


        /// <summary>
        /// 获取所有满足键条件的键
        /// </summary>
        /// <param name="match">键条件</param>
        /// <returns>Key集合</returns>
        public List<TKey> FindKeysFromKey(Func<TKey, bool> match)
        {
            return FindKeys().FindAll(t => match(t));
        }

        /// <summary>
        /// 获取首个满足键条件的键
        /// </summary>
        /// <param name="match">键条件</param>
        /// <returns>满足条件的键</returns>
        public TKey FirstKeyFromKey(Func<TKey, bool> match)
        {
            return FindKeys().FirstOrDefault(match);
        }

        /// <summary>
        /// 获取所有满足键条件的键
        /// </summary>
        /// <param name="match">值条件</param>
        /// <returns>Key集合</returns>
        public List<TKey> FindKeysFromValue(Func<TValue,bool> match)
        {
            return FindAllFromValue(match).Select(p => p.Item1).ToList();
        }
        /// <summary>
        /// 获取首个满足键条件的键
        /// </summary>
        /// <param name="match">值条件</param>
        /// <returns>满足条件的键</returns>
        public TKey FirstKeyFromValue(Func<TValue, bool> match)
        {
            return FirstAllFromValue(match).Item1;
        }

        /// <summary>
        /// 获取所有的键
        /// </summary>
        /// <returns>Key集合</returns>
        public List<TKey> FindKeys()
        {
            return _cache.Keys.ToList();
        }

        /// <summary>
        /// 获取所有满足键条件的值
        /// </summary>
        /// <param name="match">键条件</param>
        /// <returns>Value集合</returns>
        public List<TValue> FindValuesFromKey(Func<TKey, bool> match)
        {
            return FindAllFromKey(match).Select(p => p.Item2).ToList();
        }

        /// <summary>
        /// 获取所有满足键条件的值
        /// </summary>
        /// <param name="match">键条件</param>
        /// <returns>Value集合</returns>
        public TValue FirstValueFromKey(Func<TKey, bool> match)
        {
            return FirstAllFromKey(match).Item2;
        }

        /// <summary>
        /// 获取所有满足值条件的值
        /// </summary>
        /// <param name="match">值条件</param>
        /// <returns></returns>
        public List<TValue> FindValuesFromValue(Func<TValue, bool> match)
        {
           return FindValues().FindAll(p => match(p));
        }

        /// <summary>
        /// 获取首个满足值条件的值
        /// </summary>
        /// <param name="match">值条件</param>
        /// <returns></returns>
        public TValue FirstValueFromValue(Func<TValue, bool> match)
        {
            return FindValues().FirstOrDefault(match);
        }

        /// <summary>
        /// 获取所有的值
        /// </summary>
        /// <returns>Value集合</returns>
        public List<TValue> FindValues()
        {
            return _cache.Values.ToList();
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<TKey, TValue> FindAll()
        {
            return _cache;
        }

        /// <summary>
        /// 获取所有满足键条件的键值对列表
        /// </summary>
        /// <param name="match">键条件</param>
        /// <returns>键值对列表</returns>
        public List<Tuple<TKey,TValue>> FindAllFromKey(Func<TKey, bool> match)
        {
            return _cache.ToList().FindAll(p => match(p.Key)).Select(t => new Tuple<TKey, TValue>(t.Key, t.Value))
                .ToList();
        }

        /// <summary>
        /// 获取首个满足键条件的键值对列表
        /// </summary>
        /// <param name="match">键条件</param>
        /// <returns>键值对列表</returns>
        public Tuple<TKey, TValue> FirstAllFromKey(Func<TKey, bool> match)
        {
            var pair = _cache.ToList().FirstOrDefault(p => match(p.Key));
            return new Tuple<TKey, TValue>(pair.Key,pair.Value);
        }

        /// <summary>
        /// 获取所有满足值条件的键值对列表
        /// </summary>
        /// <param name="match">值条件</param>
        /// <returns>键值对列表</returns>
        public List<Tuple<TKey, TValue>> FindAllFromValue(Func<TValue, bool> match)
        {
            return _cache.ToList().FindAll(p => match(p.Value)).Select(t => new Tuple<TKey, TValue>(t.Key, t.Value))
                .ToList();
        }

        /// <summary>
        /// 获取首个满足值条件的键值对列表
        /// </summary>
        /// <param name="match">值条件</param>
        /// <returns>键值对列表</returns>
        public Tuple<TKey, TValue> FirstAllFromValue(Func<TValue, bool> match)
        {
            var pair = _cache.ToList().FirstOrDefault(p => match(p.Value));
            return new Tuple<TKey, TValue>(pair.Key,pair.Value);
        }

        /// <summary>
        /// 移除所有满足键条件的数据
        /// </summary>
        /// <param name="match">键条件</param>
        public void RemoveAllFromKey(Func<TKey,bool> match)
        {
            var key = FindKeysFromKey(match);
            key.ForEach(p => { _cache.Remove(p); });
        }

        /// <summary>
        /// 移除所有满足值条件的数据
        /// </summary>
        /// <param name="match">值条件</param>
        public void RemoveAllFromValue(Func<TValue, bool> match)
        {
            var key = FindKeysFromValue(match);
            key.ForEach(p => { _cache.Remove(p); });
        }
        #endregion

    }
}

