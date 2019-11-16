using System.Collections.Generic;
using App.Component;
using LeanCloud;
using LeanCloud.Play;
using UnityEngine;
using UnityEngine.TaskExtension;
using UnityEngine.UI;
using Player = App.LeanCloud.Player;
using RoomOptions = LeanCloud.Play.RoomOptions;

namespace App.UI
{
    /// <summary>
    /// 创建房间窗口
    /// </summary>
    public class CreateRoomView :BaseView
    {
        #region private fields

        #region ui

        private Text _pattenText;

        private InputField _nameInput;
        private InputField _passwordInput;

        private Button _backBtn;
        private Button _createBtn;

        private Toggle _onePlayerToggle;


        #endregion

        #region other

        [Inject]
        private UIComponent _uiComponent;

        [Inject]
        private NetComponent _netComponent;

        private string _petName;   //用户昵称
        #endregion

        #endregion

        #region baseView

        /// <summary>
        /// 同步开启(窗口每次显示都会执行)
        /// </summary>
        protected override void SyncOnOpen()
        {
            _onePlayerToggle.isOn = true;
            _nameInput.text = "";
            _passwordInput.text = "";
            //获取并显示当前玩家的昵称
            new AVQuery<Player>().WhereEqualTo(_netComponent.GetProperty<Player>(p => p.UserId), AVUser.CurrentUser)
                .FirstOrDefaultAsync().ContinueToForeground(
                    t =>
                    {
                        _petName = t.Result.PetName;
                        _nameInput.text = $"{t.Result.PetName}的房间";
                    });
        }

        /// <summary>
        /// 添加监听
        /// </summary>
        protected override void AddDelegates()
        {
           _onePlayerToggle.onValueChanged.AddListener(OnOnePlayToggleSelect);
           _backBtn.onClick.AddListener(OnBackBtnClick);
           _createBtn.onClick.AddListener(OnCreateBtnClick);
        }

        #endregion

        #region private funcs

        /// <summary>
        /// 选择单人模式
        /// </summary>
        private void OnOnePlayToggleSelect(bool isOn)
        {
            if (_onePlayerToggle.isOn && isOn)
            {
                return;
            }
            _pattenText.text = _onePlayerToggle.transform.Find("Label").GetComponent<Text>().text + "模式";
        }

        /// <summary>
        /// 点击返回按钮
        /// </summary>
        private void OnBackBtnClick()
        {
            _uiComponent.CloseView<CreateRoomView>();
        }

        /// <summary>
        /// 点击创建房间按钮
        /// </summary>
        private void OnCreateBtnClick()
        {

            var roomOptions = new RoomOptions
            {
                Visible = true,
                MaxPlayerCount = 8,
                EmptyRoomTtl = 0, //房间人数为0,即刻销毁
                CustomRoomProperties = new PlayObject()
                {
                    ["PetName"] = _petName,
                    ["RoomName"] = _nameInput.text
                },
                CustoRoomPropertyKeysForLobby = new List<string>{ "PetName","RoomName"}
            };

            _netComponent.WebClient.CreateRoom(null, roomOptions).ContinueToForeground(t =>
            {
                if (t.IsFaulted)
                {
                    Debug.LogError($"创建房间失败,原因:{t.Exception}");
                    return;
                }

                _uiComponent.CloseView<CreateRoomView>();
                _uiComponent.CloseView<GameLobbyView>();
                _uiComponent.SyncOpenView<RoomView>();
            });

        }
        #endregion

    }
}
