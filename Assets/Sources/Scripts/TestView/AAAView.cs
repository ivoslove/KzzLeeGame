


using UnityEngine;

namespace App.UI
{
    public class AAAView : BaseView
    {
        protected override void OnAwake()
        {
            Debug.Log("AAA Awake");
        }

        protected override void SyncOnOpen()
        {
            Debug.Log("AAA Open");
        }
    }
}
