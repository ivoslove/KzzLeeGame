using LeanCloud;

namespace App.LeanCloud
{
    [AVClassName("Player")]
    public class Player : AVObject
    {

        /// <summary>
        /// 玩家昵称
        /// </summary>
        [AVFieldName("petName")]
        public string PetName
        {
            get => GetProperty<string>("PetName");
            set => SetProperty(value,"PetName");
        }


        /// <summary>
        /// User表唯一ID
        /// </summary>
        [AVFieldName("userId")]
        public AVObject UserId
        {
            get => GetProperty<AVObject>("UserId");
            set => SetProperty(value, "UserId");
        }
    }
}
