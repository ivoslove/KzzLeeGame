using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace App.Component
{
    /// <summary>
    /// MD5组件
    /// </summary>
    public class SHA1Component
    {

        //暂时为简单实现,必须保证所有传入内容编码格式一致,此处全部采用UTF-8为默认格式.
        //后期需要可以支持任何编码格式的传入

        #region public funcs

        /// <summary>
        /// 从文本加密
        /// </summary>
        /// <param name="content">需要加密的文本</param>
        /// <returns></returns>
        public string GetFromContent(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return content;
            }
            var res = Encoding.UTF8.GetBytes(content);
            return GetFromBytes(res);
        }


        /// <summary>
        /// 从二进制加密
        /// </summary>
        /// <param name="bytes">需要加密的二进制</param>
        /// <returns></returns>
        public string GetFromBytes(IEnumerable<byte> bytes)
        {
            if (bytes==null )
            {
                return "";
            }
            var array = bytes.ToArray();
            HashAlgorithm sha = new SHA1CryptoServiceProvider();
            array = sha.ComputeHash(array);
            var result = new StringBuilder();
            foreach (var temp in array)
            {
                result.AppendFormat("{0:x2}", temp);
            }
            return result.ToString();
        }

        #endregion
    }
}

