using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.GameStateMachine;
using Runtime.Game.Gameplay;
using Runtime.Game.Leaderboard;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UI;
using Runtime.Game.Shop;
using Runtime.Game.UI.Screen;
using UnityEngine;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class GameplayStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly GameSetupController _gameSetupController;
        private readonly ChickenMovementController _chickenMovementController;
        private readonly TilemapModel _tilemapModel;
        private readonly GameplayData _gameplayData;
        private readonly PausePopupStateController _pausePopupController;
        private readonly WinPopupStateController _winPopupController;
        private readonly RewardCalculator _rewardCalculator;
        private readonly ClearRatingCalculator _clearRatingCalculator;
        private readonly UserProgressService _userProgressService;
        private readonly InventoryService _inventoryService;
        private readonly IAudioService _audioService;

        private GameplayScreen _screen;

        public GameplayStateController(ILogger logger, IUiService uiService, 
            GameSetupController gameSetupController, TilemapModel tilemapModel,
            ChickenMovementController chickenMovementController, GameplayData gameplayData,
            PausePopupStateController pausePopupStateController,
            WinPopupStateController winPopupStateController,
            RewardCalculator rewardCalculator,
            ClearRatingCalculator clearRatingCalculator, UserProgressService userProgressService,
            InventoryService inventoryService, IAudioService audioService) : base(logger)
        {
            _uiService = uiService;
            _gameSetupController = gameSetupController;
            _tilemapModel = tilemapModel;
            _chickenMovementController = chickenMovementController;
            _gameplayData = gameplayData;
            _pausePopupController = pausePopupStateController;
            _winPopupController = winPopupStateController;
            _rewardCalculator = rewardCalculator;
            _clearRatingCalculator = clearRatingCalculator;
            _userProgressService = userProgressService;
            _inventoryService = inventoryService;
            _audioService = audioService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            _gameSetupController.SetupLevel();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            _gameSetupController.EndLevel();
            await _uiService.HideScreen(ConstScreens.GameplayScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<GameplayScreen>(ConstScreens.GameplayScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnPausePressed += () => _pausePopupController.Enter().Forget();
            _screen.OnMovement += ProcessMovement;
        }

        private async void ProcessMovement(Vector2Int dir)
        {
            _gameplayData.Steps++;
            
            List<List<Vector3>> allChickenTravelPoints = new();

            int highestStepsChickenId = 0;
            int highestStepsAmount = -1;

            for (int i = 0; i < _gameplayData.ChickensSpawned.Count; i++)
            {
                List<Vector3> chickenTravelPoints = _tilemapModel.GetChickenTravelDistance(dir, i);
                allChickenTravelPoints.Add(chickenTravelPoints);

                if (chickenTravelPoints.Count > highestStepsAmount)
                {
                    highestStepsAmount = chickenTravelPoints.Count;
                    highestStepsChickenId = i;
                }
            }
            
            _screen.EnableMovement(false);
            
            UniTask[] moveTasks = new UniTask[allChickenTravelPoints.Count];
            for (int i = 0; i < allChickenTravelPoints.Count; i++)
            {
                moveTasks[i] = _chickenMovementController.MoveChicken(_gameplayData.ChickensSpawned[i], allChickenTravelPoints[i], i == highestStepsChickenId);
            }
            
            await UniTask.WhenAll(moveTasks);
            
            _screen.EnableMovement(true);

            if (_tilemapModel.IsSolved())
                ProcessGameEnd();
        }

        private void ProcessGameEnd()
        {
            _audioService.PlaySound(ConstAudio.VictorySound);
            int reward = _rewardCalculator.CalculateReward();
            int clearRating = _clearRatingCalculator.CalculateClearRating();
            
            _gameplayData.ClearReward = reward;
            _gameplayData.ClearRating = clearRating;
            
            _inventoryService.AddBalance(reward);
            _userProgressService.RecordClearData(clearRating);
            
            _winPopupController.Enter().Forget();
        }
    }
}