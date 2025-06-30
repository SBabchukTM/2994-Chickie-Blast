using UnityEngine;

namespace Runtime.Game.Gameplay
{
    public class GameSetupController
    {
        private readonly LevelSpawner _levelSpawner;
        private readonly ChickenSpawner _chickenSpawner;
        private readonly TilemapToDataConverter _tilemapToDataConverter;
        private readonly TilemapModel _tilemapModel;
        private readonly TilePainter _tilePainter;
        private readonly GameplayData _gameplayData;
        private readonly GameplayBgSetter _gameplayBgSetter;
        private readonly LevelDataProvider _levelDataProvider;

        public GameSetupController(LevelSpawner levelSpawner, ChickenSpawner chickenSpawner, 
            GameplayData gameplayData, TilePainter tilePainter,
            TilemapToDataConverter tilemapToDataConverter, TilemapModel tilemapModel,
            GameplayBgSetter gameplayBgSetter, LevelDataProvider levelDataProvider)
        {
            _levelSpawner = levelSpawner;
            _chickenSpawner = chickenSpawner;
            _gameplayData = gameplayData;
            _tilePainter = tilePainter;
            _tilemapToDataConverter = tilemapToDataConverter;
            _tilemapModel = tilemapModel;
            _gameplayBgSetter = gameplayBgSetter;
            _levelDataProvider = levelDataProvider;
        }

        public void SetupLevel()
        {
            CreateMap();
            SpawnInitialChicken();
            ResetGameData();
        }

        public void EndLevel()
        {
            _gameplayBgSetter.Enable(false);
            _levelSpawner.DestroyLevel();

            for (int i = 0; i < _gameplayData.ChickensSpawned.Count; i++)
                Object.Destroy(_gameplayData.ChickensSpawned[i].gameObject);
            
            Cone[] cones = GameObject.FindObjectsOfType<Cone>();
            for (int i = 0; i < cones.Length; i++)
                Object.Destroy(cones[i].gameObject);
        }

        private void CreateMap()
        {
            var tilemap = _levelSpawner.SpawnLevel();
            var map = _tilemapToDataConverter.GetTilePresenceArray(tilemap, out Vector2Int offset);
            _tilemapModel.SetData(tilemap, offset, map);
            
            _tilePainter.SetData(tilemap, offset);
            
            _gameplayBgSetter.SetVisuals(_levelDataProvider.GetCurrentSectionVisuals());
            _gameplayBgSetter.Enable(true);
        }

        private void SpawnInitialChicken()
        {
            var chicken = _chickenSpawner.SpawnChicken();
            chicken.position = _tilemapModel.GetChickenWorldPos(0);
            _gameplayData.ChickensSpawned = new();
            _gameplayData.ChickensSpawned.Add(chicken);
        }

        private void ResetGameData()
        {
            _gameplayData.Steps = 0;
        }
    }
}