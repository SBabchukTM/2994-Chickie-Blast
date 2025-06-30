using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class GameplayScreen : UiScreen
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _upButton;
        [SerializeField] private Button _downButton;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        
        public event Action OnPausePressed;
        public event Action<Vector2Int> OnMovement;

        public void Initialize()
        {
            _pauseButton.onClick.AddListener(() => OnPausePressed?.Invoke());
            
            _upButton.onClick.AddListener(() => OnMovement?.Invoke(Vector2Int.up));
            _downButton.onClick.AddListener(() => OnMovement?.Invoke(Vector2Int.down));
            _leftButton.onClick.AddListener(() => OnMovement?.Invoke(Vector2Int.left));
            _rightButton.onClick.AddListener(() => OnMovement?.Invoke(Vector2Int.right));
        }

        public void EnableMovement(bool enable)
        {
            _upButton.interactable = enable;
            _downButton.interactable = enable;
            _leftButton.interactable = enable;
            _rightButton.interactable = enable;
        }
    }
}