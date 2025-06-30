using Runtime.Game.Bonus;
using Runtime.Game.Gameplay;
using Runtime.Game.GameStates.Game.Controllers;
using Runtime.Game.GameStates.Game.Menu;
using Runtime.Game.Shop;
using UnityEngine;
using Zenject;

namespace Runtime.Game.GameStates.Game
{
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "Installers/GameInstaller")]
    public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
    {
        [SerializeField] private GameplayBgSetter _gameplayBgSetter;
        
        public override void InstallBindings()
        {
            BindStateControllers();
            BindShop();
            BindBonuses();
            BindGameplay();
            Container.BindInterfacesAndSelfTo<RecordsFactory>().AsSingle();
        }

        private void BindStateControllers()
        {
            Container.Bind<BonusStateController>().AsSingle();
            Container.Bind<GameplayStateController>().AsSingle();
            Container.Bind<HowToPlayStateController>().AsSingle();
            Container.Bind<LeaderboardStateController>().AsSingle();
            Container.Bind<LevelSelectStateController>().AsSingle();
            Container.Bind<MenuStateController>().AsSingle();
            Container.Bind<PrivacyPolicyStateController>().AsSingle();
            Container.Bind<ProfileStateController>().AsSingle();
            Container.Bind<SectionSelectStateController>().AsSingle();
            Container.Bind<SettingsStateController>().AsSingle();
            Container.Bind<ShopStateController>().AsSingle();
            Container.Bind<TermsOfUseStateController>().AsSingle();
            Container.Bind<MainScreenStateController>().AsSingle();
            Container.Bind<DailyRewardPopupStateController>().AsSingle();
            Container.Bind<PausePopupStateController>().AsSingle();
            Container.Bind<WinPopupStateController>().AsSingle();
        }

        private void BindShop()
        {
            Container.Bind<InventoryService>().AsSingle();
            Container.Bind<ShopService>().AsSingle();
            Container.Bind<SkinService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShopItemsFactory>().AsSingle();
        }

        private void BindBonuses()
        {
            Container.Bind<BonusRewardGenerator>().AsSingle();
            Container.Bind<BonusRouletteSpinner>().AsSingle();
            Container.Bind<LoginService>().AsSingle();
            Container.Bind<StartSettingsController>().AsSingle();
        }

        private void BindGameplay()
        {
            Container.Bind<GameplayData>().AsSingle();
            Container.Bind<TilemapToDataConverter>().AsSingle();
            Container.Bind<GameSetupController>().AsSingle();
            Container.Bind<TilemapModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<TilePainter>().AsSingle();
            Container.Bind<ChickenMovementController>().AsSingle();
            Container.Bind<RewardCalculator>().AsSingle();
            Container.Bind<ClearRatingCalculator>().AsSingle();
            Container.Bind<LevelDataProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<ChickenSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<ConeSpawner>().AsSingle();

            Container.Bind<GameplayBgSetter>().FromComponentInNewPrefab(_gameplayBgSetter).AsSingle();
        }
    }
}