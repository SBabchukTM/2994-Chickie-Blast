using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Game.Services.SettingsProvider;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class TilePainter : IInitializable
{
    private readonly IAssetProvider _assetProvider;
    
    private Tile _paintedTile;
    
    private Tilemap _tilemap;
    private Vector2Int _offset;

    public TilePainter(IAssetProvider assetProvider)
    {
        _assetProvider = assetProvider;
    }

    public async void Initialize()
    {
        _paintedTile = await _assetProvider.Load<Tile>(ConstPrefabs.PaintedTile);
    }

    public void SetData(Tilemap tilemap, Vector2Int offset)
    {
        _tilemap = tilemap;
        _offset = offset;
    }

    public void PaintTile(Vector3 position)
    {
        Vector3Int cellPos = _tilemap.WorldToCell(position);
        _tilemap.SetTile(cellPos, _paintedTile);
    }
}
