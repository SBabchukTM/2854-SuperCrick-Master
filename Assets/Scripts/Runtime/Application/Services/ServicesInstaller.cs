using Application.Game;
using Application.IAP;
using Application.Services.ApplicationState;
using Application.Services.Audio;
using Application.Services.Network;
using Application.Services.UserData;
using Application.UI;
using Code.Gameplay.Common.Collisions;
using Code.Gameplay.Common.Physics;
using Code.Gameplay.Common.Random;
using Core;
using Core.Compressor;
using Core.Factory;
using Core.Services.Audio;
using Core.Services.ScreenOrientation;
using ImprovedTimers;
using Runtime.Application.ShopSystem;
using UnityEngine;
using Zenject;
using ILogger = Core.ILogger;

namespace Application.Services
{
    [CreateAssetMenu(fileName = "ServicesInstaller", menuName = "Installers/ServicesInstaller")]
    public class ServicesInstaller : ScriptableObjectInstaller<ServicesInstaller>
    {
        public override void InstallBindings()
        {
            BuildServices();
            BuildProviders();
            BuildLogger();
            BuildFileCleaner();
            BuildCompressor();
            BuildControllers();
            BindCommonServices();
        }

        private void BuildServices()
        {
            Container.Bind<IUiService>().To<UiService>().AsSingle();
            Container.Bind<IFileStorageService>().To<PersistentFileStorageService>().AsSingle();
            Container.Bind<IAudioService>().To<AudioService>().AsSingle();
            Container.Bind<INetworkConnectionService>().To<NetworkConnectionService>().AsSingle();
            Container.Bind<IIAPService>().To<IAPServiceMock>().AsSingle();
            Container.Bind<IFinishedComponentsService>().To<FinishedComponentsService>().AsSingle();
            Container.Bind<IDestroyableService>().To<DestroyableService>().AsSingle();
            Container.Bind<GameObjectFactory>().AsSingle();
            Container.Bind<ApplicationStateService>().AsSingle();
            Container.Bind<UserDataService>().AsSingle();
        }

        private void BuildFileCleaner() => Container.Bind<IFileCleaner>().To<FileCleaner>().AsSingle();

        private void BuildLogger() => Container.Bind<ILogger>().To<SimpleLogger>().AsSingle();

        private void BuildCompressor() => Container.Bind<BaseCompressor>().To<ZipCompressor>().AsSingle();

        private void BuildControllers()
        {
            Container.BindInterfacesAndSelfTo<SettingsController>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScreenOrientationAlertController>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShopService>().AsSingle();
        }

        private void BuildProviders()
        {
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<IPersistentDataProvider>().To<PersistantDataProvider>().AsSingle();
            Container.Bind<ISettingProvider>().To<SettingsProvider>().AsSingle();
            Container.Bind<ServerProvider>().AsSingle();
            Container.Bind<ISerializationProvider>().To<JsonSerializationProvider>().AsSingle();
        }

        private void BindCommonServices()
        {
            Container.Bind<IRandomService>().To<UnityRandomService>().AsSingle();
            Container.Bind<ICollisionRegistry>().To<CollisionRegistry>().AsSingle();
            Container.Bind<IPhysicsService>().To<PhysicsService>().AsSingle();
            Container.Bind<ITimeService>().To<TimeService>().AsSingle();
        }
    }
}