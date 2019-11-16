using System;
using System.Collections.Generic;
using System.Linq;
using App.Component;
using App.Util;
using LeanCloud;
using LeanCloud.Play;
using UnityEngine;
using UnityEngine.TaskExtension;
using UnityEngine.UI;

namespace App.UI
{
    /// <summary>
    /// 房间大厅窗口
    /// </summary>
    public class GameLobbyView :BaseView
    {
        #region private fields

        #region ui

        private Transform _roomItem;
        private Transform _grid;
        private InputField _searchInput;

        private Button _backBtn;
        private Button _createBtn;
        private Button _renovateBtn;
        private Button _searchBtn;


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
            _roomItem.transform.SetParent(_viewRoot);
            _roomItem.gameObject.SetActive(false);
            _netComponent.WebClient.Connect().ContinueWith(a =>
            {
                a.Result.JoinLobby().ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        Debug.LogError("加入游戏大厅失败,无法获取房间列表,原因" + t.Exception);
                    }
                });
            });

        }

        /// <summary>
        /// 同步开启(窗口每次显示都会执行)
        /// </summary>
        protected override void SyncOnOpen()
        {
            for (int i = 0; i < _grid.childCount; i++)
            {
                _grid.GetChild(i).gameObject.SetActive(false);
            }

            OnRenovateBtnClick();
        }


        /// <summary>
        /// 添加监听
        /// </summary>
        protected override void AddDelegates()
        {
            _backBtn.onClick.AddListener(OnBackBtnClick);
            _createBtn.onClick.AddListener(OnCreateBtnClick);
            _renovateBtn.onClick.AddListener(OnRenovateBtnClick);
            _netComponent.WebClient.OnLobbyRoomListUpdated += UpdateRoomItem;
        }

        #endregion

        #region private funcs

        /// <summary>
        /// 点击返回按钮
        /// </summary>
        private void OnBackBtnClick()
        {
            _uiComponent.CloseView<GameLobbyView>();
            _uiComponent.SyncOpenView<LeisurePattenView>();
        }

        /// <summary>
        /// 点击刷新按钮
        /// </summary>
        private void OnRenovateBtnClick()
        {
            UpdateRoomItem(_netComponent.WebClient.LobbyRoomList);
        }

        /// <summary>
        /// 刷新房间列表
        /// </summary>
        /// <param name="lobbyRooms"></param>
        private void UpdateRoomItem(List<LobbyRoom> lobbyRooms)
        {
            if (lobbyRooms == null || lobbyRooms.Count ==0)
            {
                return;
            }
            var gridChildren = _grid.GetChildrenT();
            gridChildren.ForEach(t=>t.gameObject.SetActive(true));
            var delete = gridChildren.Select(p => p.name).Except(lobbyRooms.Select(t => t.RoomName)).ToList();
            var create = lobbyRooms.Select(p => p.RoomName).Except(gridChildren.Select(t => t.name)).ToList();

            for (int i = 0; i < delete.Count; i++)
            {
                GameObject.DestroyImmediate(_grid.Find<Transform>(delete[i]));
            }

            foreach (var t in create)
            {
                var item = GameObject.Instantiate(_roomItem, _grid);
                item.gameObject.SetActive(true);
                item.name = t;
                var room = lobbyRooms.FirstOrDefault(p => p.RoomName == t);
                if (room==null)
                {
                    GameObject.DestroyImmediate(item.gameObject);
                    continue;
                }
                var roomText = item.GetComponentInChild<Text>("RoomText");
                roomText.text = room.CustomRoomProperties.GetString("RoomName");
                var playerText = item.GetComponentInChild<Text>("PlayerText");
                playerText.text = room.CustomRoomProperties.GetString("PetName");
                var joinBtn = item.GetComponentInChild<Button>("JoinBtn");
                _uiComponent.AddListener(joinBtn, btn =>
                    {
                        _netComponent.WebClient.JoinRoom(btn.transform.parent.name).ContinueToForeground(n =>
                        {
                            _uiComponent.CloseView<GameLobbyView>();
                            _uiComponent.SyncOpenView<RoomView>();
                        });
                    });
            }
        }

        /// <summary>
        /// 点击创建房间按钮
        /// </summary>
        private void OnCreateBtnClick()
        {
            _uiComponent.SyncOpenView<CreateRoomView>();
        }

        #endregion
    }
}


