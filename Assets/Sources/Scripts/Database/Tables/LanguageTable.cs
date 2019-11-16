
using SQLite.Attribute;

namespace App.Database
{
    /// <summary>
    /// 语言
    /// </summary>
    public class LanguageTable
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// 中文
        /// </summary>
        public string Chinese { get; set; }

        /// <summary>
        /// 英文
        /// </summary>
        public string English { get; set; }
    }
}
