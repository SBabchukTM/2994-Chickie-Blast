using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class ProfileScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _avatarButton;
        [SerializeField] private TMP_InputField _nameField;
        
        public event Action OnBackPressed;
        public event Action OnSavePressed;
        public event Action OnAvatarPressed;
        public event Action<string> OnNameChanged;

        public void Initialize()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _saveButton.onClick.AddListener(() => OnSavePressed?.Invoke());
            
            _avatarButton.onClick.AddListener(() => OnAvatarPressed?.Invoke());
            _nameField.onEndEdit.AddListener((value) => OnNameChanged?.Invoke(value));
        }

        public void SetAvatar(Sprite avatar)
        {
            if(avatar != null)
                _avatarButton.image.sprite = avatar;
        }

        public void SetName(string username)
        {
            _nameField.text = username;
        }
    }
}