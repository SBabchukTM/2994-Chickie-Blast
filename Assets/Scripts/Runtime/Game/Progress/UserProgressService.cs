using System.Collections.Generic;
using System.Linq;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;

namespace Runtime.Game.Leaderboard
{
    public class UserProgressService
    {
        private readonly UserDataService _userDataService;
        private readonly GameplayData _gameplayData;
        private readonly LevelDataProvider _levelDataProvider;

        public UserProgressService(UserDataService userDataService, GameplayData gameplayData, 
            LevelDataProvider levelDataProvider)
        {
            _userDataService = userDataService;
            _gameplayData = gameplayData;
            _levelDataProvider = levelDataProvider;
        }

        public bool NextLevelExists() => _levelDataProvider.GetCurrentSectionMaps().Count > _gameplayData.LevelId + 1;

        public void RecordClearData(int stars)
        {
            int levelId = _gameplayData.LevelId;
            List<int> clearData = GetCurrentSectionClearData();

            if (levelId == clearData.Count)
                clearData.Add(stars);
            else
            {
                int lastClear = clearData[levelId];
                if(stars > lastClear)
                    clearData[levelId] = stars;
            }
        }

        public int GetScoresSum() => GetSectionOneStars() + GetSectionTwoStars() + GetSectionThreeStars();

        public int GetSectionOneStars() => GetSectionOneClearData().Sum();
        public int GetSectionTwoStars() => GetSectionTwoClearData().Sum();
        public int GetSectionThreeStars() => GetSectionThreeClearData().Sum();

        public List<int> GetSectionOneClearData() => GetUserProgressData().SectionOneStars;
        public List<int> GetSectionTwoClearData() => GetUserProgressData().SectionTwoStars;
        public List<int> GetSectionThreeClearData() => GetUserProgressData().SectionThreeStars;

        private UserProgressData GetUserProgressData() => _userDataService.GetUserData().UserProgressData;

        private List<int> GetCurrentSectionClearData()
        {
            var progressData = GetUserProgressData();
            
            switch (_gameplayData.SectionID)
            {
                case 0:
                    return progressData.SectionOneStars;
                case 1:
                    return progressData.SectionTwoStars;
                case 2:
                    return progressData.SectionThreeStars;
            }

            return null;
        }
    }
}