using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayData
{
    private int _steps = 0;
    public event Action<int> OnStepsChanged;

    public int Steps
    {
        get => _steps;
        set
        {
            _steps = value;
            OnStepsChanged?.Invoke(value);
        }
    }
    
    public int SectionID { get; set; }
    public int LevelId { get; set; }

    public List<Transform> ChickensSpawned;
    
    public int ClearRating { get; set; }
    public int ClearReward { get; set; }
}
