using Application.GameStateMachine;
using Application.Services.CameraProvider;
using Application.Services.UserData;
using Application.UI;
using Runtime.Application.ApplicationStates.Game.Controllers;
using Runtime.Application.ApplicationStates.Game.Features.Bat;
using Runtime.Application.ApplicationStates.Game.Features.Bat.Factory;
using Runtime.Application.ApplicationStates.Game.Features.Enemy;
using Runtime.Application.ApplicationStates.Game.Features.Enemy.Factory;
using Runtime.Application.Services.Chart.Provider;
using Runtime.Application.Services.Shop;
using Runtime.Application.ShopSystem;
using Runtime.Application.UI.LevelSelectionButtonHandlers;
using UnityEngine;
using Zenject;

namespace Application.Game
{
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "Installers/GameInstaller")]
    public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
    {
        [SerializeField] private SettingPopupSpritesData _settingPopupSpritesData;
        [SerializeField] private Sprite _lockedLevel;

        public override void InstallBindings()
        {
            BindControllers();
            BindServices();
            BindData();
            BindProviders();
            BindFactories();
        }

        private void BindFactories()
        {
            Container.Bind<IBatLauncherFactory>().To<BatLauncherFactory>().AsSingle();
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.Bind<IBallFactory>().To<BallFactory>().AsSingle();
        }

        private void BindServices()
        {
            BindGameServices();
            BindLevelSelectionServices();
            BindShopServices();
        }

        private void BindGameServices()
        {
            Container.Bind<ICleanupGame>().To<CleanupGame>().AsSingle();
            Container.Bind<IChoosedTeamProvider>().To<ChoosedTeamProvider>().AsSingle();
        }

        private void BindControllers()
        {
            Container.BindInterfacesAndSelfTo<EnemyAttackController>().AsSingle();
            Container.Bind<BatLifeCycleController>().AsSingle();
            Container.Bind<GameResultController>().AsSingle();
            Container.Bind<TitleStateController>().AsSingle();
            Container.Bind<LevelSelectionController>().AsSingle();
            Container.Bind<InitGameController>().AsSingle();
            Container.Bind<GameStateController>().AsSingle();
            Container.Bind<ChooseTeamState>().AsSingle();
            Container.Bind<ShopStateController>().AsSingle();
            Container.Bind<InitShopState>().AsSingle();
            Container.Bind<FinishGameState>().AsSingle();
            Container.Bind<StartSettingsController>().AsSingle();
            Container.Bind<InfoPopupController>().AsSingle();
            Container.Bind<LevelProgressController>().AsSingle();
            Container.Bind<EmitPauseController>().AsSingle();
            Container.Bind<PauseController>().AsSingle();
            Container.Bind<ShopItemDisplayController>().AsSingle();
        }

        private void BindProviders()
        {
            Container.Bind<ICameraProvider>().To<CameraProvider>().AsSingle();
            Container.Bind<ILevelPointProvider>().To<LevelPointProvider>().AsSingle();
            Container.Bind<IBallScaleProvider>().To<BallScaleProvider>().AsSingle();
            Container.Bind<IChartProvider>().To<ChartProvider>().AsSingle();
            Container.Bind<IGameResultProvider>().To<GameResultProvider>().AsSingle();
            Container.Bind<IBallLaunchProvider>().To<BallLaunchProvider>().AsSingle();
        }

        private void BindData()
        {
            Container.Bind<ChoosedTeamModel>().AsSingle();
            
            BindSettingsStorage();
            BindLevelModel();
            BindShopData();
        }

        private void BindShopData()
        {
            Container.Bind<IShopItemsStorage>().To<ShopItemsStorage>().AsSingle();
            Container.Bind<ShopItemDisplayModel>().AsTransient();
        }

        private void BindLevelModel()
        {
            var levelSelectionModel = new LevelSelectionModel(_lockedLevel);
            Container.Bind<LevelSelectionModel>().FromInstance(levelSelectionModel).AsSingle();
        }

        private void BindSettingsStorage()
        {
            var settingsPopupToggleSpritesStorage = new SettingsPopupSpritesStorage(
                    _settingPopupSpritesData.MusicActiveSprite,
                    _settingPopupSpritesData.MusicDisactiveSprite,
                    _settingPopupSpritesData.SoundActiveSprite,
                    _settingPopupSpritesData.SoundDisactiveSprite);

            Container.BindInstance(settingsPopupToggleSpritesStorage).AsSingle();
            Container.Bind<SettingsPopupData>().AsSingle();
        }

        private void BindShopServices()
        {
            Container.Bind<IPurchaseEffectsService>().To<PurchaseEffectsService>().AsSingle();
            Container.Bind<IShopItemsDisplayService>().To<ShopItemsDisplayService>().AsSingle();
            Container.Bind<ISelectPurchaseItemService>().To<SelectPurchaseItemService>().AsSingle();
            Container.Bind<IProcessPurchaseService>().To<ProcessPurchaseService>().AsSingle();
            Container.Bind<IUserInventoryService>().To<UserInventoryService>().AsSingle();
        }

        private void BindLevelSelectionServices()
        {
            Container.Bind<ILevelSelectionButtonDestroyer>().To<LevelSelectionButtonDestroyer>().AsSingle();
            Container.Bind<ILevelSelectionButtonAdjuster>().To<LevelSelectionButtonAdjuster>().AsSingle();
            Container.Bind<ILevelSelectionButtonCreator>().To<LevelSelectionButtonCreator>().AsSingle();
            Container.Bind<ISetupGameState>().To<SetupGameState>().AsSingle();
        }
    }
}