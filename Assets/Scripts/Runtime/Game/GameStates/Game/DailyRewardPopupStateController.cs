using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.UI.Popup;
using Runtime.Game.GameStates.Game.Menu;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using Runtime.Game.Shop;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

public class DailyRewardPopupStateController : StateController
{
    private readonly IUiService _uiService;
    private readonly InventoryService _inventoryService;
    
    private BonusReward _bonusReward;
    
    public DailyRewardPopupStateController(ILogger logger, IUiService uiService, InventoryService inventoryService) : base(logger)
    {
        _uiService = uiService;
        _inventoryService = inventoryService;
    }

    public override async UniTask Enter(CancellationToken cancellationToken = default)
    {
        DailyItemPopup popup = await _uiService.ShowPopup(ConstPopups.DailyItemPopup) as DailyItemPopup;
        popup.SetData(_bonusReward);

        popup.OnClaimed += async () =>
        {
            popup.DestroyPopup();
            await GoTo<MainScreenStateController>();
        };
    }
    
    public void SetBonusReward(BonusReward bonusReward)
    {
        _bonusReward = bonusReward;
        
        if(bonusReward.RewardType == RewardType.Coin)
            _inventoryService.AddBalance(bonusReward.RewardValue);

        if (bonusReward.RewardType == RewardType.Item)
        {
            if(bonusReward.RewardValue == 0)
                _inventoryService.AddChick();
            else
                _inventoryService.AddCone();
        }
    }
}
