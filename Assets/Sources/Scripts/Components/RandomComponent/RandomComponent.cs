
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace App.Component
{
    /// <summary>
    /// 随机组件
    /// </summary>
    public class RandomComponent : BaseComponent
    {

        #region public funcs

        /// <summary>
        /// 生成指定数量组的指定长度的随机中文
        /// </summary>
        /// <param name="groupLength">指定的组</param>
        /// <param name="contentLength">指定长度的中文</param>
        /// <returns></returns>
        public Task<List<string>> RandomChinese(int groupLength, int contentLength)
        {
            List<Task<object[]>> tasks = new List<Task<object[]>>();
            for (int i = 0; i < groupLength; i++)
            {
                tasks.Add(Task.Run(() => CreateRegionCode(contentLength)));
            }

            return Task.WhenAll(tasks).ContinueWith(t =>
            {
                List<string> result = new List<string>();
                result.AddRange(t.Result.Select(@group => @group.Aggregate("", (current, code) =>
                    current + Encoding.GetEncoding("gb2312")
                        .GetString((byte[]) Convert.ChangeType(code, typeof(byte[]))))));
                return result;
            });
        }

        /// <summary>
        /// 生成指定数量组的长度不等的随机中文
        /// </summary>
        /// <param name="groupLength">指定的组</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Task<List<string>> RandomChinese(int groupLength,int min ,int max)
        {
            List<Task<object[]>> tasks = new List<Task<object[]>>();
            for (int i = 0; i < groupLength; i++)
            {
                var num = UnityEngine.Random.Range(min < 1 ? 1 : min, max + 1);
                tasks.Add(Task.Run(() => CreateRegionCode(num)));
            }
            return Task.WhenAll(tasks).ContinueWith(t =>
            {
                List<string> result = new List<string>();
                result.AddRange(t.Result.Select(@group => @group.Aggregate("", (current, code) =>
                    current + Encoding.GetEncoding("gb2312")
                        .GetString((byte[])Convert.ChangeType(code, typeof(byte[]))))));
                return result;
            });
        }

        /// <summary>
        /// 生成一组指定长度的随机中文
        /// </summary>
        /// <param name="contentLength">指定的汉字长度</param>
        /// <returns></returns>
        public string RandomChinese(int contentLength)
        {
            var array = CreateRegionCode(contentLength);
            return array.Aggregate(string.Empty,
                (current, t) => current + Encoding.GetEncoding("gb2312")
                                    .GetString((byte[]) Convert.ChangeType(t, typeof(byte[]))));
        }

        #endregion


        #region private funcs

        /// <summary>
        /// 创建随机的字符区域编码
        /// </summary>
        /// <param name="length">要随机的长度</param>
        /// <returns></returns>
        private object[] CreateRegionCode(int length)
        {
            //定义一个字符串数组储存汉字编码的组成元素
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
            var rnd = new System.Random();
            //定义一个object数组用来
            object[] bytes = new object[length];
            /*每循环一次产生一个含两个元素的十六进制字节数组，并将其放入byte数组中
             每个汉字有四个区位码组成
             区位码第1位和区位码第2位作为字节数组第一个元素
             区位码第3位和区位码第4位作为字节数组第二个元素
            */
            for (int i = 0; i < length; i++)
            {
                //区位码第1位
                int r1 = rnd.Next(11, 14);
                string str_r1 = rBase[r1].Trim();
                //区位码第2位
                rnd = new Random(r1 * Guid.NewGuid().GetHashCode() + i);//更换随机数发生器的种子避免产生重复值
                var r2 = rnd.Next(0, r1 == 13 ? 7 : 16);
                string str_r2 = rBase[r2].Trim();
                //区位码第3位
                rnd = new Random(r2 * Guid.NewGuid().GetHashCode() + i);
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3].Trim();
                //区位码第4位
                rnd = new Random(r3 * Guid.NewGuid().GetHashCode() + i);
                int r4;
                switch (r3)
                {
                    case 10:
                        r4 = rnd.Next(1, 16);
                        break;
                    case 15:
                        r4 = rnd.Next(0, 15);
                        break;
                    default:
                        r4 = rnd.Next(0, 16);
                        break;
                }

                string str_r4 = rBase[r4].Trim();
                //定义两个字节变量存储产生的随机汉字区位码
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中
                byte[] str_r = { byte1, byte2 };
                //将产生的一个汉字的字节数组放入object数组中
                bytes.SetValue(str_r, i);
            }
            return bytes;
        }

        #endregion


    }

}

