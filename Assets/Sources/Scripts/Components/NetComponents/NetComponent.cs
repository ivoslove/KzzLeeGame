
using System;
using System.Linq.Expressions;
using System.Reflection;
using LeanCloud;
using LeanCloud.Play;
using UnityEngine;
using Logger = LeanCloud.Play.Logger;

namespace App.Component
{
    /// <summary>
    /// 网络组件
    /// </summary>
    public class NetComponent : BaseComponent
    {
        #region private fields

        private readonly string _appId = "8aKTp1kEwkOV861hvjOh8gKK-9Nh9j0Va";          //AppId为该项目唯一标识符        

        private readonly string _appKey = "SnD2n9qKRgvtbW95t0VgEb8U";                  //AppKey 是公开的访问密钥，适用于在公开的客户端中使用。使用 AppKey 进行的访问受到 ACL 的限制。

        private readonly string _tempUrl = @"https://8aktp1ke.lc-cn-e1-shared.com";  //临时域名

        #endregion

        #region public properties

        public Client WebClient { get; }


        #endregion

        #region ctor

        public NetComponent()
        {
            Logger.LogDelegate = (level, log) =>
            {
                if (level == LogLevel.Debug)
                {
                    Debug.LogFormat("[DEBUG] {0}", log);
                }
                else if (level == LogLevel.Warn)
                {
                    Debug.LogWarningFormat("[WARN] {0}", log);
                }
                else if (level == LogLevel.Error)
                {
                    Debug.LogErrorFormat("[ERROR] {0}", log);
                }
            };

            AVClient.HttpLog(Debug.Log);
            AVClient.Initialize(_appId, _appKey, _tempUrl);
            WebClient = new Client(_appId,_appKey,Guid.NewGuid().ToString());
        }


        #endregion

        #region public funcs

        /// <summary>
        /// 获取参数名称
        /// </summary>
        /// <typeparam name="TTable">LeanCloud表类型</typeparam>
        /// <param name="expression">表达式</param>
        /// <returns>要获取的表名称</returns>
        public string GetProperty<TTable>(Expression<Func<TTable, object>> expression) where TTable : AVObject, new()
        {
            var body = (MemberExpression)expression.Body;
            var attribute = body.Member.GetCustomAttribute<AVFieldNameAttribute>();
            return attribute == null ? "" : attribute.FieldName;
        }

        #endregion
    }

}
