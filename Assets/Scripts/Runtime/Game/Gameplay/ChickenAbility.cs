using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Audio;
using Runtime.Game.Gameplay;
using Runtime.Game.Services.Audio;
using Runtime.Game.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class ChickenAbility : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI _amountText;
    
    private InventoryService _inventoryService;
    private ChickenSpawner _chickenSpawner;
    private GameplayData _gameplayData;
    private TilemapModel _tilemapModel;
    private IAudioService _audioService;

    private Camera _camera;
    private Transform _spawnedChicken;
    
    [Inject]
    private void Construct(InventoryService inventoryService, ChickenSpawner chickenSpawner, 
        TilemapModel tilemapModel,GameplayData gameplayData, IAudioService audioService)
    {
        _camera = Camera.main;
        
        _inventoryService = inventoryService;
        _chickenSpawner = chickenSpawner;
        _tilemapModel = tilemapModel;
        _gameplayData = gameplayData;
        _audioService = audioService;
        
        _amountText.text = _inventoryService.GetSpawnChicksAmount().ToString();
        _inventoryService.OnChickUsed += UpdateChickAmount;
    }

    private void OnDestroy() => _inventoryService.OnChickUsed -= UpdateChickAmount;

    private void UpdateChickAmount(int amount) => _amountText.text = amount.ToString();
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(_inventoryService.GetSpawnChicksAmount() <= 0)
            return;
        
        _spawnedChicken = _chickenSpawner.SpawnChicken();
        _spawnedChicken.transform.position = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_spawnedChicken == null)
            return;
        
        Vector3 worldPos = _camera.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;
        _spawnedChicken.transform.position = worldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(_spawnedChicken == null)
            return;
        
        if (!_tilemapModel.IsItemPlacementValid(_spawnedChicken.position))
        {
            Destroy(_spawnedChicken.gameObject);
            return;
        }
        
        _tilemapModel.PlaceAdditionalChicken(_spawnedChicken);
        _gameplayData.ChickensSpawned.Add(_spawnedChicken);
        _inventoryService.UseChick();
        _audioService.PlaySound(ConstAudio.ChickenSound);
        _spawnedChicken = null;
    }
}
