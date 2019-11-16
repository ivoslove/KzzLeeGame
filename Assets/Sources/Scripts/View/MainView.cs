using App.Component;
using App.LeanCloud;
using LeanCloud;
using UnityEngine.TaskExtension;
using UnityEngine.UI;

namespace App.UI
{
    /// <summary>
    /// 主界面窗口
    /// </summary>
    public class MainView : BaseView
    {
        #region private fields

        #region ui

        private Text _userIdText;
        private Button _matchBtn;
        private Button _leisureBtn;

        #endregion

        #region other

        [Inject]
        private UIComponent _uiComponent;

        [Inject]
        private NetComponent _netComponent;
        #endregion

        #endregion

        #region BaseView

        /// <summary>
        /// 初始化窗口(仅当窗口建立时执行,且最先执行)
        /// </summary>
        /// <returns></returns>
        protected override void OnAwake()
        {
            //获取并显示当前玩家的昵称
            new AVQuery<Player>().WhereEqualTo(_netComponent.GetProperty<Player>(p => p.UserId), AVUser.CurrentUser)
                .FirstOrDefaultAsync().ContinueToForeground(
                    t => { _userIdText.text = t.Result.PetName; });
        }

        /// <summary>
        /// 添加监听
        /// </summary>
        protected override void AddDelegates()
        {
            _leisureBtn.onClick.AddListener(OnLeisureBtnClick);
        }

        #endregion


        #region private funcs

        /// <summary>
        /// 点击休闲模式按钮
        /// </summary>
        private void OnLeisureBtnClick()
        {
            _uiComponent.CloseView<MainView>();
            _uiComponent.SyncOpenView<LeisurePattenView>();
        }

        #endregion
    }
}

