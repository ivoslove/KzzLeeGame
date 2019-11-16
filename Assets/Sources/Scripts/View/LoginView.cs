
using System;
using System.Threading.Tasks;
using App.Component;
using App.LeanCloud;
using LeanCloud;
using UnityEngine;
using UnityEngine.TaskExtension;
using UnityEngine.UI;
using Random = System.Random;

namespace App.UI
{
    /// <summary>
    /// 用户登录窗口
    /// </summary>
    public class LoginView : BaseView
    {
        #region private fields

        #region ui

        private Transform _loginEmpty;
        private Transform _registerEmpty;

        private Text _loginErrorText;
        private Text _registerErrorText;

        private Toggle _rememberPasswordToggle;

        private InputField _accountInput;
        private InputField _passwordInput;
        private InputField _captchaInput;

        private Button _loginBtn;
        private Button _registerBtn;
        private Button _forgetPasswordBtn;
        private Button _captchaBtn;
        private Button _registerConfirmBtn;
        private Button _registerBackBtn;


        #endregion

        #region other

        [Inject]
        private UIComponent _uiComponent;             //UI组件

        [Inject]
        private NetComponent _netComponent;           //网络组件

        private readonly string _isRememberPassword = "IsRememberPassword";          //是否记住密码,1：记住   0：不记住
        private readonly string _theLastLoginUser = "TheLastLoginUser";              //最后一次登录的用户的用户名
        private readonly string _showPasswordKey = "showPassword";             //AVUser表中自己新增的密码字段

        #endregion

        #endregion

        #region BaseView

        /// <summary>
        /// 初始化窗口(仅当窗口建立时执行,且最先执行)
        /// </summary>
        /// <returns></returns>
        protected override void OnAwake()
        {
            _loginErrorText.text = "";
            _registerErrorText.text = "";
            _registerEmpty.gameObject.SetActive(false);
            _rememberPasswordToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt(_isRememberPassword) == 1);
        }

        /// <summary>
        /// 添加监听
        /// </summary>
        protected override void AddDelegates()
        {
            _loginBtn.onClick.AddListener(OnLoginBtnClick);
            _registerBtn.onClick.AddListener(OnRegisterBtnClick);
            _registerBackBtn.onClick.AddListener(OnRegisterBackBtnClick);
            _registerConfirmBtn.onClick.AddListener(OnRegisterConfirmBtnClick);
        }


        /// <summary>
        /// 同步开启(窗口每次显示都会执行)
        /// </summary>
        protected override void SyncOnOpen()
        {
            if (_rememberPasswordToggle.isOn)
            {
                _accountInput.text = PlayerPrefs.GetString(_theLastLoginUser);
                AVUser.Query.WhereEqualTo(_netComponent.GetProperty<AVUser>(t=>t.Username), _accountInput.text).FirstOrDefaultAsync().ContinueToForeground(t =>
                {
                    _passwordInput.text = t.Result["showPassword"].ToString();
                    OnLoginBtnClick();
                });
            }
        }

        /// <summary>
        /// 销毁(删除)
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _loginBtn.onClick.RemoveListener(OnLoginBtnClick);
            _registerBtn.onClick.RemoveListener(OnRegisterBtnClick);
            _registerBackBtn.onClick.RemoveListener(OnRegisterBackBtnClick);
            _registerConfirmBtn.onClick.RemoveListener(OnRegisterConfirmBtnClick);
        }

        #endregion

        #region private funcs

        /// <summary>
        /// 点击登录按钮
        /// </summary>
        private void OnLoginBtnClick()
        {
            if (string.IsNullOrEmpty(_accountInput.text) || string.IsNullOrEmpty(_passwordInput.text))
            {
                _loginErrorText.text = "账号或密码不能为空!";
                return;
            }

            //是否存在该用户
            AVUser.Query.WhereEqualTo(_netComponent.GetProperty<AVUser>(t => t.Username), _accountInput.text).CountAsync().ContinueWith(t =>
            {
                if (t.Result == 0)
                {
                    return;
                }
                Login(_accountInput.text, _passwordInput.text).ContinueToForeground(a =>
                    {
                        _uiComponent.SyncOpenView<MainView>();
                        _uiComponent.DestroyView<LoginView>();
                    });
            });
        }

        /// <summary>
        /// 点击注册按钮
        /// </summary>
        private void OnRegisterBtnClick()
        {
            _loginErrorText.text = "";
            _registerErrorText.text = "";
            _loginEmpty.gameObject.SetActive(false);
            _registerEmpty.gameObject.SetActive(true);
        }

        /// <summary>
        /// 点击返回登录按钮
        /// </summary>
        private void OnRegisterBackBtnClick()
        {
            _loginErrorText.text = "";
            _registerErrorText.text = "";
            _loginEmpty.gameObject.SetActive(true);
            _registerEmpty.gameObject.SetActive(false);
        }

        /// <summary>
        /// 点击确认注册按钮
        /// </summary>
        private void OnRegisterConfirmBtnClick()
        {

            if (string.IsNullOrEmpty(_accountInput.text) || string.IsNullOrEmpty(_passwordInput.text))
            {
                _registerErrorText.text = "账号或密码不能为空!";
                return;
            }

            //是否存在该用户
            AVUser.Query.WhereEqualTo(_netComponent.GetProperty<AVUser>(t => t.Username), _accountInput.text)
                .CountAsync().ContinueWith(t =>
                {
                    if (t.Result != 0)
                    {
                        _registerErrorText.text = "该账号已注册!";
                        return;
                    }

                    //不存在该用户，可以注册
                    var user = new AVUser()
                    {
                        Username = _accountInput.text,
                        Password = _passwordInput.text,
                        [_showPasswordKey] = _passwordInput.text
                    };
                    user.SignUpAsync().ContinueToForeground(p =>
                    {

                        string petName = new RandomComponent().RandomChinese(UnityEngine.Random.Range(2, 5));
                        var player = new Player
                        {
                            PetName = petName,
                            UserId = AVUser.CurrentUser
                        };

                        player.SaveAsync().ContinueWith(n =>
                            {
                                Login(_accountInput.text, _passwordInput.text).ContinueToForeground(a =>
                                {
                                    _uiComponent.SyncOpenView<MainView>();
                                    _uiComponent.DestroyView<LoginView>();
                                });
                            }
                        );
                    });
                });

        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        private Task Login(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                _loginErrorText.text = "账号或密码不能为空";
                return Task.FromResult(0);
            }

            //存在该用户，进行登录
            return AVUser.LogInAsync(_accountInput.text, _passwordInput.text).ContinueToForeground(p =>
            {
                if (p.IsFaulted)
                {
                    //TODO
                    return UnityTask.FromException(p.Exception);
                }

                PlayerPrefs.SetString(_theLastLoginUser, _accountInput.text);
                PlayerPrefs.SetInt(_isRememberPassword, _rememberPasswordToggle.isOn ? 1 : 0);
                Debug.Log("登录成功!");
                return UnityTask.FromResult(0);
            }).AsBackground();
        }
       

        #endregion
    }
}

