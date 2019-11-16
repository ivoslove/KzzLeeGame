using System;
using System.Linq;

namespace App.Plugins
{
    /// <summary>
    /// 反射
    /// </summary>
    public static class Reflect
    {
        /// <summary>
        /// 从...拷贝
        /// </summary>
        /// <typeparam name="TFrom">源类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="from">源对象</param>
        /// <returns></returns>
        public static TResult ReflectFrom<TFrom, TResult>(TFrom from) where TResult:new()
        {
            var fromProperties = from.GetType().GetProperties().ToList();
            var fromFields = from.GetType().GetFields().ToList();
            TResult result = new TResult();
            var resultProperties = result.GetType().GetProperties();
            var resultFields = result.GetType().GetFields();

            foreach (var resultTemp in resultFields)
            {
                foreach (var fromTemp in fromFields)
                {
                    if (resultTemp.Name == fromTemp.Name)
                    {
                        resultTemp.SetValue(result,fromTemp.GetValue(from));
                        break;
                    }
                }
            }

            foreach (var resultTemp in resultProperties)
            {
                foreach (var fromTemp in fromProperties)
                {
                    if (resultTemp.Name == fromTemp.Name)
                    {
                        resultTemp.SetValue(result, fromTemp.GetValue(from));
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 从...拷贝
        /// </summary>
        /// <typeparam name="TFrom">源类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="from">源对象</param>
        /// <returns></returns>
        public static TResult ReflectFrom<TFrom, TResult>(TFrom from,Func<TFrom,TResult,bool> func) where TResult : new()
        {
            var fromProperties = from.GetType().GetProperties().ToList();
            var fromFields = from.GetType().GetFields().ToList();
            TResult result = new TResult();
            var resultProperties = result.GetType().GetProperties();
            var resultFields = result.GetType().GetFields();

            foreach (var resultTemp in resultFields)
            {
                foreach (var fromTemp in fromFields)
                {
                    if (func(from,result))
                    {
                        resultTemp.SetValue(result, fromTemp.GetValue(from));
                        break;
                    }
                }
            }

            foreach (var resultTemp in resultProperties)
            {
                foreach (var fromTemp in fromProperties)
                {
                    if (func(from,result))
                    {
                        resultTemp.SetValue(result, fromTemp.GetValue(from));
                        break;
                    }
                }
            }

            return result;
        }


    }
}


