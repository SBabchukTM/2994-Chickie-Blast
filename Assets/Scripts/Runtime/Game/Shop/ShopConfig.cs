using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopConfig", menuName = "Configs/ShopConfig")]
public class ShopConfig : BaseSettings
{
    public List<SkinConfig> Skins = new();
    public int ConePrice = 0;
    public int SpawnChicksPrice = 0;
}

[Serializable]
public class SkinConfig
{
    public Sprite ItemSprite;
    public int Price;
}
