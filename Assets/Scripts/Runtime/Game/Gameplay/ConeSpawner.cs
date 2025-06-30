using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Game.Services.SettingsProvider;
using UnityEngine;
using Zenject;

public class ConeSpawner : IInitializable
{
    private readonly IAssetProvider _assetProvider;
    private readonly GameObjectFactory _gameObjectFactory;

    private GameObject _conePrefab;
    
    public ConeSpawner(IAssetProvider assetProvider, GameObjectFactory gameObjectFactory)
    {
        _assetProvider = assetProvider;
        _gameObjectFactory = gameObjectFactory;
    }
    
    public async void Initialize()
    {
        _conePrefab = await _assetProvider.Load<GameObject>(ConstPrefabs.ConePrefab);
    }

    public Transform SpawnCone()
    {
        return _gameObjectFactory.Create(_conePrefab).transform;
    }
}
