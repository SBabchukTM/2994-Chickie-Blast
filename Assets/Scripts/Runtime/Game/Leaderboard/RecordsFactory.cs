using System.Collections.Generic;
using System.Linq;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Game.Leaderboard;
using Runtime.Game.Services.SettingsProvider;
using Runtime.Game.Services.UserData;
using Runtime.Game.UserAccountSystem;
using UnityEngine;
using Zenject;

public class RecordsFactory : IInitializable
{
    private readonly IAssetProvider _assetProvider;
    private readonly ProfileService _profileService;
    private readonly UserDataService _userDataService;
    private readonly GameObjectFactory _gameObjectFactory;
    private UserProgressService _userProgressService;

    private GameObject _prefab;

    public RecordsFactory(ProfileService profileService, UserDataService userDataService,
        GameObjectFactory gameObjectFactory, IAssetProvider assetProvider, UserProgressService userProgressService)
    {
        _assetProvider = assetProvider;
        _userDataService = userDataService;
        _gameObjectFactory = gameObjectFactory;
        _profileService = profileService;
        _userProgressService = userProgressService;
    }
    
    public async void Initialize()
    {
        _prefab = await _assetProvider.Load<GameObject>(ConstPrefabs.RecordPrefab);
    }

    public List<RecordDisplay> CreateRecordDisplayList()
    {
        List<RecordDisplay> recordDisplayList = new List<RecordDisplay>();

        var records = CreateRecords();

        for (int i = 0; i < records.Count; i++)
        {
            var display = _gameObjectFactory.Create<RecordDisplay>(_prefab);
            var data = records[i];

            if (data.Player)
                display.Initialize(data.Name, data.Stars, _profileService.GetUsedAvatarSprite());
            else
                display.Initialize(data.Name, data.Stars);
            
            recordDisplayList.Add(display);
        }
        
        return recordDisplayList;
    }

    
    private List<RecordData> CreateRecords()
    {
        List<RecordData> recordDataList = new List<RecordData>()
        {
            new (){Name = "Alexander", Stars = 63},
            new (){Name = "Caleb", Stars = 60},
            new (){Name = "Grace", Stars = 59},
            new (){Name = "Daniel", Stars = 50},
            new (){Name = "John", Stars = 42},
            new (){Name = "Isaac", Stars = 33},
            new (){Name = "Marcus", Stars = 28},
            new (){Name = "Felix", Stars = 26},
            new (){Name = "Benjamin", Stars = 23},
            new (){Name = "Hannah", Stars = 19},
            new (){Name = "Fiona", Stars = 18},
            new (){Name = "Lily", Stars = 17},
            new (){Name = "Oliver", Stars = 15},
            new (){Name = "Dahlia", Stars = 12},
            new (){Name = "Chloe", Stars = 9},
            new (){Name = "Sophia", Stars = 10},
            new (){Name = "Isla", Stars = 6},
            new (){Name = "Tobias", Stars = 3},
            new (){Name = "Kayla", Stars = 1},
        };
        
        recordDataList.Add(new ()
        {
            Name = _userDataService.GetUserData().ProfileData.Username,
            Stars = _userProgressService.GetScoresSum(),
            Player = true,
        });

        recordDataList = recordDataList.OrderByDescending(x => x.Stars).ToList();
        
        return recordDataList;
    }
    
    private class RecordData
    {
        public string Name;
        public int Stars;
        public bool Player;
    }
}
