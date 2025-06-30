using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _starsText;
    [SerializeField] private Image _image;

    public void Initialize(string name, int stars)
    {
        _nameText.text = name;
        _starsText.text = stars.ToString();
    }

    public void Initialize(string name, int stars, Sprite icon)
    {
        _nameText.text = name;
        _starsText.text = stars.ToString();
        
        if(icon)
            _image.sprite = icon;
    }
}
