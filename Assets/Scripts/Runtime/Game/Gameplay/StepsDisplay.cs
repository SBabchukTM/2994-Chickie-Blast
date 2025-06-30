using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class StepsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _stepsText;

    private GameplayData _gameplayData;
    
    [Inject]
    private void Construct(GameplayData gameplayData)
    {
        _gameplayData = gameplayData;
        
        _gameplayData.OnStepsChanged += UpdateSteps;
    }

    private void OnDestroy()
    {
        _gameplayData.OnStepsChanged -= UpdateSteps;
    }

    private void UpdateSteps(int steps) => _stepsText.text = steps.ToString(); 
}
