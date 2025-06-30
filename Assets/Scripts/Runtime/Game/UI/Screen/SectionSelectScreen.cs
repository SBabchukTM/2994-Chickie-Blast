using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class SectionSelectScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private GameLevelSection _sectionOne;
        [SerializeField] private GameLevelSection _sectionTwo;
        [SerializeField] private GameLevelSection _sectionThree;
        [SerializeField] private Sprite _starSprite;
        
        public event Action OnBackPressed;
        public event Action OnMenuPressed;
        public event Action OnSectionOnePressed;
        public event Action OnSectionTwoPressed;
        public event Action OnSectionThreePressed;

        public void Initialize(int starsOne, int starsTwo, int starsThree)
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _menuButton.onClick.AddListener(() => OnMenuPressed?.Invoke());
            
            _sectionOne.SetStars(starsOne, _starSprite);
            _sectionTwo.SetStars(starsTwo, _starSprite);
            _sectionThree.SetStars(starsThree, _starSprite);
            
            _sectionOne.Button.onClick.AddListener(() => OnSectionOnePressed?.Invoke());
            _sectionTwo.Button.onClick.AddListener(() => OnSectionTwoPressed?.Invoke());
            _sectionThree.Button.onClick.AddListener(() => OnSectionThreePressed?.Invoke());
        }
        
        [Serializable]
        private class GameLevelSection
        {
            public Button Button;
            public Image[] Stars;

            public void SetStars(int stars, Sprite activeStar)
            {
                int levelsAmount = 7;
                for (int i = 0; i < stars / levelsAmount; i++)
                    Stars[i].sprite = activeStar;
            }
        }
    }
}