using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

[CreateAssetMenu(fileName = "ChickenPrefabsConfig", menuName = "Config/ChickenPrefabsConfig")]
public class ChickenPrefabsConfig : BaseSettings
{
    public List<GameObject> prefabs = new List<GameObject>();
}
