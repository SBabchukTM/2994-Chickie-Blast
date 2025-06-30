using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;

    [Inject]
    private void Construct(GameplayData gameplayData)
    {
        int levelId = gameplayData.LevelId;
        _levelText.text = $"Level {levelId + 1}";
    }
}
