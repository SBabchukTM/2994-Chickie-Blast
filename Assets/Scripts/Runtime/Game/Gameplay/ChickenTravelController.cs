using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Game.Gameplay
{
    public class ChickenTravelController
    {
        private bool[,] _map;
        private bool[,] _visitedMap;

        private TilemapModel _tilemapModel;

        private int _cols;
        private int _rows;
        
        public void SetData(TilemapModel tilemapModel, bool[,] map, bool[,] visitedMap)
        {
            _tilemapModel = tilemapModel;
            _map = map;
            _visitedMap = visitedMap;
            
            _rows = map.GetLength(0);
            _cols = map.GetLength(1);
        }
        
        public List<Vector3> TravelHorizontal(List<Vector2Int> chickenCoordinates, int index, int directionX)
        {
            List<Vector3> result = new();
            
            var chickenPos = chickenCoordinates[index];
            int resultXPos = chickenPos.x;
            
            _visitedMap[chickenPos.y, chickenPos.x] = true;
            
            result.Add(_tilemapModel.GetWorldPos(chickenPos));
            while (HorizontalMovementValid(chickenPos, resultXPos + directionX))
            {
                resultXPos += directionX;
                _visitedMap[chickenPos.y, resultXPos] = true;
                result.Add(_tilemapModel.GetWorldPos(new Vector2Int(resultXPos, chickenPos.y)));
            }
                
            chickenCoordinates[index] = new Vector2Int(resultXPos, chickenPos.y);

            return result;
        }

        public List<Vector3> TravelVertical(List<Vector2Int> chickenCoordinates, int index, int directionY)
        {
            List<Vector3> result = new();

            var chickenPos = chickenCoordinates[index];
            int resultYPos = chickenPos.y;
                
            _visitedMap[chickenPos.y, chickenPos.x] = true;
            
            result.Add(_tilemapModel.GetWorldPos(chickenPos));
            while (VerticalMovementValid(chickenPos, resultYPos + directionY))
            {
                resultYPos += directionY;
                
                _visitedMap[resultYPos, chickenPos.x] = true;
                result.Add(_tilemapModel.GetWorldPos(new Vector2Int(chickenPos.x, resultYPos)));
            }

            chickenCoordinates[index] = new Vector2Int(chickenPos.x ,resultYPos);
            return result;
        }

        private bool HorizontalMovementValid(Vector2Int chickenCoordinates, int xPos)
        {
            bool insideBounds = xPos >= 0 && xPos < _cols;
            if (!insideBounds)
                return false;

            return _map[chickenCoordinates.y, xPos];
        }

        private bool VerticalMovementValid(Vector2Int chickenCoordinates, int yPos)
        {
            bool insideBounds = yPos >= 0 && yPos < _rows;
            if (!insideBounds)
                return false;

            return _map[yPos, chickenCoordinates.x];
        }
    }
}