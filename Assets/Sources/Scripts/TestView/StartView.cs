
using App.Component;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI
{
    public class StartView : BaseView
    {

        #region private fields

        #region ui

        private Button _startGameBtn;                          //开始游戏按钮

        #endregion

        #region components

        [Inject]
        private UIComponent _uiComponent;                     //UI组件

        #endregion

        #endregion

        #region BaseView

        /// <summary>
        /// 同步开启(窗口每次显示都会执行)
        /// </summary>
        protected override void SyncOnOpen()
        {
            Debug.Log(_uiComponent + "......." + _viewRoot);
        }

        #endregion

    }

    
}

