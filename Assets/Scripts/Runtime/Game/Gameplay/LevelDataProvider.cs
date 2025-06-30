using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

public class LevelDataProvider
{
    private readonly ISettingProvider _settingProvider;
    private readonly GameplayData _gameplayData;

    public LevelDataProvider(ISettingProvider settingProvider, GameplayData gameplayData)
    {
        _settingProvider = settingProvider;
        _gameplayData = gameplayData;
    }
    
    public MapConfig GetCurrentMapConfig()
    {
        var maps = GetCurrentSectionMaps();
        return maps[_gameplayData.LevelId];
    }
    
    public List<MapConfig> GetCurrentSectionMaps()
    {
        switch (_gameplayData.SectionID)
        {
            case 0:
                return _settingProvider.Get<GameConfig>().SectionOneMaps;
            case 1:
                return _settingProvider.Get<GameConfig>().SectionTwoMaps;
            case 2:
                return _settingProvider.Get<GameConfig>().SectionThreeMaps;
        }
        
        return null;
    }

    public SectionVisuals GetCurrentSectionVisuals()
    {
        switch (_gameplayData.SectionID)
        {
            case 0:
                return _settingProvider.Get<GameConfig>().SectionOneBgVisuals;
            case 1:
                return _settingProvider.Get<GameConfig>().SectionTwoBgVisuals;
            case 2:
                return _settingProvider.Get<GameConfig>().SectionThreeBgVisuals;
        }
        
        return null;
    }
}
