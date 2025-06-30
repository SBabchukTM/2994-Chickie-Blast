using Runtime.Core.Factory;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

public class LevelSpawner
{
    private readonly GameObjectFactory _factory;
    private readonly LevelDataProvider _levelDataProvider;

    private GameObject _spawnedMap;
    
    public LevelSpawner(GameObjectFactory factory, LevelDataProvider levelDataProvider)
    {
        _factory = factory;
        _levelDataProvider = levelDataProvider;
    }

    public Tilemap SpawnLevel()
    {
        var levelConfig = _levelDataProvider.GetCurrentMapConfig();
        
        _spawnedMap = _factory.Create(levelConfig.Prefab);
        
        _spawnedMap.transform.SetParent(Object.FindObjectOfType<Grid>().transform, false);
        return _spawnedMap.GetComponent<Tilemap>();
    }

    public void DestroyLevel()
    {
        Object.Destroy(_spawnedMap);
    }
}
