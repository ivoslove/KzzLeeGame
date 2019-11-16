
using SQLite.Attribute;

namespace App.Database
{
    /// <summary>
    /// 异常码对应表
    /// </summary>
    public class ExceptionCodeTable
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// 异常码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 异常含义
        /// </summary>
        public string Meaning { get; set; }

        /// <summary>
        /// 异常码所属类型
        /// </summary>
        public string ExceptionType { get; set; }

        /// <summary>
        /// 异常码所属类型
        /// </summary>
        public enum ExceptionTableType
        {
            /// <summary>
            /// LeanCloud异常
            /// </summary>
            LeanCloudException = 0,
        }
    }
}

