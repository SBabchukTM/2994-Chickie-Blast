using Runtime.Core.Audio;
using Runtime.Game.Gameplay;
using Runtime.Game.Services.Audio;
using Runtime.Game.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ConeAbility : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI _amountText;
    
    private InventoryService _inventoryService;
    private TilemapModel _tilemapModel;
    private ConeSpawner _coneSpawner;
    private IAudioService _audioService;

    private Transform _spawnedCone;
    private Camera _camera;
    
    [Inject]
    private void Construct(InventoryService inventoryService, TilemapModel tilemapModel, ConeSpawner coneSpawner, IAudioService audioService)
    {
        _camera = Camera.main;
        _inventoryService = inventoryService;
        _tilemapModel = tilemapModel;
        _coneSpawner = coneSpawner;
        _audioService = audioService;

        _amountText.text = _inventoryService.GetConesAmount().ToString();
        _inventoryService.OnConeUsed += UpdateConesAmount;
    }

    private void OnDestroy() => _inventoryService.OnConeUsed -= UpdateConesAmount;

    private void UpdateConesAmount(int amount) => _amountText.text = amount.ToString();

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(_inventoryService.GetConesAmount() <= 0)
            return;
        
        _spawnedCone = _coneSpawner.SpawnCone();
        _spawnedCone.transform.position = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_spawnedCone == null)
            return;
        
        Vector3 worldPos = _camera.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;
        _spawnedCone.transform.position = worldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(_spawnedCone == null)
            return;
        
        if (!_tilemapModel.IsItemPlacementValid(_spawnedCone.position))
        {
            Destroy(_spawnedCone.gameObject);
            return;
        }
        
        _tilemapModel.PlaceCone(_spawnedCone);
        _inventoryService.UseCone();
        _audioService.PlaySound(ConstAudio.ConeSound);
        _spawnedCone = null;
    }
}
