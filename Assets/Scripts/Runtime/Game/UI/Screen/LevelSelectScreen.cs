using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class LevelSelectScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private LevelButtonDisplay[] _levelButtons;

        [SerializeField] private Sprite _lockedLevelSprite;
        [SerializeField] private Sprite _starSprite;
        
        public event Action OnBackPressed;
        public event Action OnMenuPressed;
        public event Action<int> OnLevelSelected;

        public void InitializeScreen()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _menuButton.onClick.AddListener(() => OnMenuPressed?.Invoke());
        }

        public void InitializeButtons(List<int> clearData)
        {
            int lastUnlockedLevel = clearData.Count;
            for (int i = 0; i < _levelButtons.Length; i++)
            {
                var button = _levelButtons[i];
                button.Initialize(i);
                
                button.SetStars(GetStarsAmount(i, clearData), _starSprite);
                
                bool locked = i > lastUnlockedLevel;
                if(locked)
                    button.SetLocked(_lockedLevelSprite);
                
                button.OnClicked += (level) => OnLevelSelected?.Invoke(level);
            }
        }

        private int GetStarsAmount(int level, List<int> clearData)
        {
            if(level >= clearData.Count)
                return 0;
            
            return clearData[level];
        }
    }
}