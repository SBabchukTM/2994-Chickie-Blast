using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Runtime.Game.Gameplay
{
    public class TilemapModel
    {
        private ChickenTravelController _chickenTravelController;
        
        private Tilemap _tilemap;
        private Vector2Int _offset;
        
        private bool[,] _map;
        private bool[,] _visitedMap;

        private int _rows;
        private int _cols;
        
        private List<Vector2Int> _chickensCoordinates = new();
        
        public void SetData(Tilemap tilemap, Vector2Int offset, bool[,] map)
        {
            _tilemap = tilemap;
            _offset = offset;
            _map = map;

            _rows = _map.GetLength(0);
            _cols = _map.GetLength(1);

            PlaceInitialChicken();
            CopyArray();

            _chickenTravelController = new();
            _chickenTravelController.SetData(this, _map, _visitedMap);
        }

        public Vector3 GetChickenWorldPos(int index) => GetWorldPos(_chickensCoordinates[index]);

        public Vector3 GetWorldPos(Vector2Int itemPos)
        {
            Vector3Int cellPos = new Vector3Int(itemPos.x + _offset.x, itemPos.y + _offset.y, 0);
            return _tilemap.GetCellCenterWorld(cellPos);
        }

        public List<Vector3> GetChickenTravelDistance(Vector2Int direction, int index)
        {
            if (direction.x != 0)
                return _chickenTravelController.TravelHorizontal(_chickensCoordinates, index, direction.x);

            return _chickenTravelController.TravelVertical(_chickensCoordinates, index, direction.y);
        }

        public bool IsSolved()
        {
            for (int y = 0; y < _rows; y++)
            {
                for (int x = 0; x < _cols; x++)
                {
                    if(!_visitedMap[y, x])
                        return false;
                }
            }
            
            return true;
        }

        public bool IsItemPlacementValid(Vector3 worldPos)
        {
            Vector3Int cellPos = _tilemap.WorldToCell(worldPos);
            return _tilemap.HasTile(cellPos);
        }

        public void PlaceCone(Transform cone)
        {
            Vector3Int cellPos = _tilemap.WorldToCell(cone.position);
            cone.position = _tilemap.GetCellCenterWorld(cellPos);

            int x = cellPos.x - _offset.x;
            int y = cellPos.y - _offset.y;

            if (x >= 0 && x < _cols && y >= 0 && y < _rows)
                _map[y, x] = false;
        }

        public void PlaceAdditionalChicken(Transform chicken)
        {
            Vector3Int cellPos = _tilemap.WorldToCell(chicken.position);
            chicken.position = _tilemap.GetCellCenterWorld(cellPos);
            
            int x = cellPos.x - _offset.x;
            int y = cellPos.y - _offset.y;
            
            _chickensCoordinates.Add(new Vector2Int(x, y));
        }

        private void PlaceInitialChicken()
        {
            _chickensCoordinates.Clear();
            _chickensCoordinates.Add(Vector2Int.zero);
            
            for (int y = 0; y < _rows; y++)
            {
                for (int x = 0; x < _cols; x++)
                {
                    if (_map[y, x])
                    {
                        _chickensCoordinates[0] = new Vector2Int(x, y);
                        return;
                    }
                }
            }
        }
        
        private void CopyArray()
        {
            _visitedMap = new bool[_rows, _cols];

            for (int y = 0; y < _rows; y++)
            {
                for (int x = 0; x < _cols; x++)
                    _visitedMap[y, x] = !_map[y, x];
            }
        }
    }
}