using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapToDataConverter
{
    public bool[,] GetTilePresenceArray(Tilemap tilemap, out Vector2Int offset)
    {
        BoundsInt bounds = tilemap.cellBounds;

        offset = new Vector2Int(bounds.xMin, bounds.yMin);

        int width = bounds.size.x;
        int height = bounds.size.y;

        bool[,] result = new bool[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3Int tilePos = new Vector3Int(bounds.xMin + x, bounds.yMin + y, 0);
                result[y, x] = tilemap.HasTile(tilePos);
            }
        }

        return result;
    }
}
