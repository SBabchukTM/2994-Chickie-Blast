using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : BaseSettings
{
    public SectionVisuals SectionOneBgVisuals;
    public SectionVisuals SectionTwoBgVisuals;
    public SectionVisuals SectionThreeBgVisuals;
    
    public List<MapConfig> SectionOneMaps = new ();
    public List<MapConfig> SectionTwoMaps = new ();
    public List<MapConfig> SectionThreeMaps = new ();
}

[Serializable]
public class MapConfig
{
    public GameObject Prefab;
    public int ThreeStarCondition;
    public int TwoStarCondition;
}

[Serializable]
public class SectionVisuals
{
    public Sprite SectionBG;
    public Sprite SectionMap;
}