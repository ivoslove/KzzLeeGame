
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using App.Component;
using App.Dispatch;
using App.UI;
using SqlCipher4Unity3D;
using SQLite.Attribute;
using UnityEngine;
using Random = System.Random;


public class Test : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
       // UIComponent ui = new UIComponent();
       // ui.SyncOpenView<StartView>();

        //var path = @"C:\Users\Xqh\Desktop\test.db";
        //var connect = new SQLiteConnection(path, "123");

        //connect.CreateTable<AAAAA>();
        //connect.InsertAll(new[]
        //{
        //    new AAAAA()
        //    {
        //        Id = 1,
        //        abc = Guid.NewGuid().ToString(),
        //        x_y = 6
        //    },
        //});
        //new SqlCipherComponent();

        RandomComponent aa = new RandomComponent();

        aa.RandomChinese(20, 1,6).ContinueWith(t =>
        {
            foreach (var temp in t.Result)
            {
                Debug.Log(temp);
            }
        });
    }

    public class AAAAA
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string abc { get; set; }

        public int x_y { get; set; }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int AAA(int a)
    {
        Debug.Log($"执行了{a}次AAA");
        return 888;
    }



}
