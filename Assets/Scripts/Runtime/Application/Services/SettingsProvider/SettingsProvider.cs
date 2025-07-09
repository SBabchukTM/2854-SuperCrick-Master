using System;
using System.Collections.Generic;
using Application.Game;
using Core;
using Core.Services.Audio;
using Core.Services.ScreenOrientation;
using Cysharp.Threading.Tasks;
using Runtime.Application.ApplicationStates.Game.Features.Bat;
using Runtime.Application.ShopSystem;

namespace Application.Services
{
    public class SettingsProvider : ISettingProvider
    {
        private readonly IAssetProvider _assetProvider;
        private readonly Dictionary<Type, BaseSettings> _settings = new();
        private readonly List<BaseSettings> _configs = new();

        public SettingsProvider(IAssetProvider assetProvider) =>
                _assetProvider = assetProvider;

        public async UniTask Initialize()
        {
            await LoadConfig<ScreenOrientationConfig>(ConstConfigs.ScreenOrientationConfig);
            await LoadConfig<AudioConfig>(ConstConfigs.AudioConfig);
            await LoadConfig<ShopSetup>(ConstConfigs.ShopSetup);
            await LoadConfig<TeamConfig>(ConstConfigs.TeamConfig);
            await LoadConfig<BatConfig>(ConstConfigs.BatConfig);
            await LoadConfig<EnemyConfig>(ConstConfigs.EnemyConfig);
            await LoadConfig<BallConfig>(ConstConfigs.BallConfig);
            await LoadConfig<GameResultSetup>(ConstConfigs.GameResultSetup);
            await LoadConfig<GameConfig>(ConstConfigs.GameConfig);

            foreach (var config in _configs)
                Set(config);
        }

        private async UniTask LoadConfig<T>(string address) where T : BaseSettings =>
                _configs.Add(await _assetProvider.Load<T>(address));

        public T Get<T>() where T : BaseSettings
        {
            if (_settings.ContainsKey(typeof(T)))
            {
                var setting = _settings[typeof(T)];
                return setting as T;
            }

            throw new Exception("No setting found");
        }

        public void Set(BaseSettings config)
        {
            if (_settings.ContainsKey(config.GetType()))
                return;

            _settings.Add(config.GetType(), config);
        }
    }
}