using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Dispatch;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("2S");
        Dispatcher<Task<int>>.Listener<int>("123",BBB);
        Debug.Log("2E");

        for (int i = 0; i < 5; i++)
        {
            Dispatcher<Task<int>>.DoWork("123",i).ContinueWith(t =>
            {
                Debug.Log(t.Result);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Task<int> BBB(int a)
    {
        return Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith(t => 10);
    } 
}
