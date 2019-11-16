using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using App.Database;
using SqlCipher4Unity3D;
using UnityEngine;

namespace App.Component
{
    /// <summary>
    /// 数据库组件
    /// </summary>
    public class SqlCipherComponent : BaseComponent
    {
        private readonly SQLiteConnection _connection;

        public SqlCipherComponent()
        {
            Debug.Log(new SHA1Component().GetFromContent("K_WarArtist_L"));
            var cachePath = $@"{Application.streamingAssetsPath}\Database";
            Directory.CreateDirectory(cachePath);
            var dbPath = $@"{cachePath}\WarArtist.db";
            if (File.Exists(dbPath))
            {
                _connection = new SQLiteConnection(dbPath, new SHA1Component().GetFromContent("K_WarArtist_L"));
                return;
            }
            _connection = new SQLiteConnection(dbPath, new SHA1Component().GetFromContent("K_WarArtist_L"));
        }
    }
}

