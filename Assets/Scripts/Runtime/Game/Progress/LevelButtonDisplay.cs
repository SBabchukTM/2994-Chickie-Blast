using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Button _button;
    [SerializeField] private Image _lockImage;
    [SerializeField] private Image[] _stars;

    public event Action<int> OnClicked;
    
    public void Initialize(int levelId)
    {
        _levelText.text = (levelId + 1).ToString();
        
        _button.onClick.AddListener(() =>
        {
            OnClicked?.Invoke(levelId);
        });
    }

    public void SetLocked(Sprite lockedSprite)
    {
        _button.image.sprite = lockedSprite;
        _button.interactable = false;
        _levelText.gameObject.SetActive(false);
        _lockImage.gameObject.SetActive(true);
    }

    public void SetStars(int stars, Sprite starSprite)
    {
        for (int i = 0; i < stars; i++)
            _stars[i].sprite = starSprite;
    }
}
