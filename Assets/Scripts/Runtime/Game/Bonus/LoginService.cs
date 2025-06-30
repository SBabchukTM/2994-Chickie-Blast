using System;
using Runtime.Game.Services.UserData;

namespace Runtime.Game.Bonus
{
    public class LoginService
    {
        private readonly UserDataService _userDataService;

        public LoginService(UserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        public bool CanSpin()
        {
            var lastLoginDateString  = GetSavedTime();
            if (lastLoginDateString == String.Empty)
                return true;
         
            var lastLoginDate = Convert.ToDateTime(lastLoginDateString);
            return DateTime.Now.Date > lastLoginDate.Date;
        }
    
        public void RecordSpinDate()
        {
            _userDataService.GetUserData().LoginData.LastLoginDateString = DateTime.Now.ToString();
            _userDataService.SaveUserData();
        }
    
        private string GetSavedTime() => _userDataService.GetUserData().LoginData.LastLoginDateString;
    }
}