using App.Component;
using UnityEngine.UI;

namespace App.UI
{
    /// <summary>
    /// 娱乐模式窗口
    /// </summary>
    public class LeisurePattenView : BaseView
    {
        #region private fields

        #region ui

        private Button _customizeRoomBtn;
        private Button _backBtn;

        #endregion

        #region other

        [Inject]
        private UIComponent _uiComponent;

        #endregion

        #endregion

        #region BaseView

        /// <summary>
        /// 添加监听
        /// </summary>
        protected override void AddDelegates()
        {
            _customizeRoomBtn.onClick.AddListener(OnCustomizeRoomBtnClick);
            _backBtn.onClick.AddListener(OnBackBtnClick);
        }

        #endregion

        #region private funcs

        /// <summary>
        /// 点击自定义房间按钮
        /// </summary>
        private void OnCustomizeRoomBtnClick()
        {
            _uiComponent.CloseView<LeisurePattenView>();
            _uiComponent.SyncOpenView<GameLobbyView>();
        }

        private void OnBackBtnClick()
        {
            _uiComponent.CloseView<LeisurePattenView>();
            _uiComponent.SyncOpenView<MainView>();
        }

        #endregion
    }
}


