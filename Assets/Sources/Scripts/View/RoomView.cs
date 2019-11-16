
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Component;
using LeanCloud;
using LeanCloud.Play;
using UnityEngine;
using UnityEngine.TaskExtension;
using UnityEngine.UI;
using Player = App.LeanCloud.Player;

namespace App.UI
{
    /// <summary>
    /// 房间窗口
    /// </summary>
    public class RoomView : BaseView
    {
        #region private fields

        #region ui

        private Button _backBtn;
        private Button _sendBtn;
        private Button _playBtn;
        private Button _inviteBtn;

        private Text _titleText;
        private Text _contentText;

        private Transform _seatToggles;
        private Transform _grid;

        private InputField _talkInput;
        private Toggle _seatToggle;

        #endregion

        #region other

        [Inject]
        private UIComponent _uiComponent;

        [Inject]
        private NetComponent _netComponent;

        private string _userPetName;                         //昵称
        private List<Tuple<Toggle, Text>> _seats;            //座位列表
        private Toggle _currentSeat;                         //当前自己所在的座位

        #endregion

        #endregion

        #region baseView

        /// <summary>
        /// 初始化窗口(仅当窗口建立时执行,且最先执行)
        /// </summary>
        /// <returns></returns>
        protected override void OnAwake()
        {

            CreateSeat();
            //获取并显示当前玩家的昵称
            new AVQuery<Player>().WhereEqualTo(_netComponent.GetProperty<Player>(p => p.UserId), AVUser.CurrentUser)
                .FirstOrDefaultAsync().ContinueToForeground(
                    t =>
                    {
                        _userPetName = t.Result.PetName;
                        _titleText.text = $"{t.Result.PetName}的房间";
                        InitSeat().ContinueWith(p =>
                        {
                            var freeSeat = _seats.FirstOrDefault(n => string.IsNullOrEmpty(n.Item2.text))?.Item1;
                            //找到第一个空位,坐下去
                            if (freeSeat != null) freeSeat.isOn = true;
                        });
                    });
        }


        /// <summary>
        /// 同步开启(窗口每次显示都会执行)
        /// </summary>
        protected override void SyncOnOpen()
        {
            base.SyncOnOpen();
        }

        /// <summary>
        /// 添加监听
        /// </summary>
        protected override void AddDelegates()
        {
            _backBtn.onClick.AddListener(OnBackBtnClick);
            _netComponent.WebClient.OnPlayerCustomPropertiesChanged += OnPlayerCustomPropertiesChanged;
        }

        /// <summary>
        /// 销毁(删除)
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _seats.ForEach(p=>p.Item1.onValueChanged.RemoveAllListeners());
            _backBtn.onClick.RemoveListener(OnBackBtnClick);
            _netComponent.WebClient.OnPlayerCustomPropertiesChanged -= OnPlayerCustomPropertiesChanged;
        }

        #endregion

        #region private funcs

        /// <summary>
        /// 点击返回按钮
        /// </summary>
        private void OnBackBtnClick()
        {
            _netComponent.WebClient.LeaveRoom().ContinueToForeground(t =>
            {
                _uiComponent.DestroyView<RoomView>();
                _uiComponent.SyncOpenView<GameLobbyView>();
            });
        }

        /// <summary>
        /// 创建座位
        /// </summary>
        private void CreateSeat()
        {
            _seats = new List<Tuple<Toggle, Text>>();
            for (int i = 1; i < _netComponent.WebClient.Room.MaxPlayerCount; i++)
            {
                GameObject.Instantiate(_seatToggle, _seatToggles);
                //此时创建完毕，但是在UI中顺序极有可能是乱的，这就导致第N个位置,实际上我们无法知晓它是第几个生成的了。
            }
            //所以在此通过UI顺序,我们重新找一次.
            for (int i = 0; i < _seatToggles.childCount; i++)
            {
                var tempToggle = _seatToggles.transform.GetChild(i).GetComponent<Toggle>();
                tempToggle.name = i.ToString();
                _seats.Add(new Tuple<Toggle, Text>(tempToggle, tempToggle.GetComponentInChildren<Text>()));
                _uiComponent.AddListener(tempToggle, OnSeatToggleSelect);
            }
        }

        /// <summary>
        /// 分配座位
        /// </summary>
        private UnityTask InitSeat()
        {
            //假设目前是进入的一个房间，房间中是已经有其它玩家的，此时需要先根据情况分配他们的座位在自己客户端上
            var players = _netComponent.WebClient.Room.PlayerList;
      
            List<UnityTask> tasks = (players.Select(player =>
                    {
                        player.CustomProperties.TryGetString("Seat", out var seat);
                        return new {player, seat};
                    } )
                    .Where(@t1 => !string.IsNullOrEmpty(@t1.seat))
                    .Select(@t1 => new AVQuery<Player>()
                        .WhereEqualTo(_netComponent.GetProperty<Player>(p => p.UserId), @t1.player.UserId)
                        .FirstOrDefaultAsync()
                        .ContinueToForeground(t =>
                        {
                            _seats[int.Parse(@t1.seat)].Item2.text = t.Result.PetName;
                            //_playerToggle.transform.GetChild(int.Parse(@t1.seat)).GetComponentInChildren<Text>().text =
                            //    t.Result.PetName;
                            return 0;
                        }))).Cast<UnityTask>()
                .ToList();

            return UnityTask.WhenAll(tasks);
        }

        /// <summary>
        /// 当某个座位被选中时
        /// </summary>
        /// <param name="seat"></param>
        /// <param name="isOn"></param>
        private void OnSeatToggleSelect(Toggle seat,bool isOn)
        {
            if (isOn)
            {
                //当被选中,需要设置玩家到该座位上
                var freeText = _seats.FirstOrDefault(p => p.Item1 == seat)?.Item2;
                if (freeText != null && string.IsNullOrEmpty(freeText.text))
                {
                    freeText.text = _userPetName;
                    PlayObject po = new PlayObject()
                    {
                        ["Seat"] = freeText.transform.parent.name
                    };
                    _netComponent.WebClient.Player.SetCustomProperties(po).ContinueWith(t =>
                    {
                        var aa = _netComponent.WebClient.Player.CustomProperties;
                    });

                    _currentSeat = seat;
                }
                else
                {
                    //因为这个选中的座位,是被其它人坐了的，所以并不能真正坐上去，于是,还是得回到原位上去
                    seat.SetIsOnWithoutNotify(false);
                    _currentSeat.isOn = true;
                }
            }
            else
            {
                //当取消选中,那么需要复原该座位
                var tuple = _seats.FirstOrDefault(p => p.Item1 == seat);
                if (tuple!=null)
                {
                    tuple.Item2.text = "";
                }
            }
        }

        private void OnPlayerCustomPropertiesChanged(global::LeanCloud.Play.Player player, PlayObject playObject)
        {
            if (player.UserId == _netComponent.WebClient.Player.UserId)
            {
                //如果是自己，那么不处理
                return;
            }
            player.CustomProperties.TryGetString("Seat", out var seat);
            new AVQuery<Player>()
                .WhereEqualTo(_netComponent.GetProperty<Player>(p => p.UserId), player.UserId)
                .FirstOrDefaultAsync().ContinueToForeground(t =>
                    {
                        //先修改该用户之前所在的座位为空
                       var tuple = _seats.FirstOrDefault(p => p.Item2.text == t.Result.PetName);
                       if (tuple==null)
                       {
                           return;
                       }
                       tuple.Item2.text = "";
                       //再修改该用户目前所在的座位为其昵称
                       tuple = _seats.FirstOrDefault(p => p.Item1.name == seat);
                       if (tuple==null)
                       {
                           return;
                       }
                       tuple.Item2.text = t.Result.PetName;
                    });
        }
        #endregion
    }
}

