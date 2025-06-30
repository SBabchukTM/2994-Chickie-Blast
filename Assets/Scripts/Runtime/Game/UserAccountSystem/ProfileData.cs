using System;

namespace Runtime.Game.UserAccountSystem
{
    [Serializable]
    public class ProfileData
    {
        public string Username = "Player";
        public string AvatarBase64 = String.Empty;

        public ProfileData Copy()
        {
            return (ProfileData)MemberwiseClone();
        }
    }
}