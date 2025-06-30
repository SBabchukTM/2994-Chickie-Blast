using System;
using System.Collections.Generic;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class UserProgressData
    {
        public List<int> SectionOneStars = new();
        public List<int> SectionTwoStars = new();
        public List<int> SectionThreeStars = new();
    }
}