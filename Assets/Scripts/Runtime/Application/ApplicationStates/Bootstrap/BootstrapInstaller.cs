﻿using Core.StateMachine;
using Runtime.Application.ApplicationStates.Game.Controllers;
using UnityEngine;
using Zenject;

namespace Application.GameStateMachine
{
    [CreateAssetMenu(fileName = "BootstrapInstaller", menuName = "Installers/BootstrapInstaller")]
    public class BootstrapInstaller : ScriptableObjectInstaller<BootstrapInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Bootstrapper>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BootstrapState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameState>().AsSingle();
            Container.Bind<StateMachine>().AsSingle();

            Container.Bind<AudioSettingsBootstrapController>().AsSingle();
            Container.Bind<UserDataStateChangeController>().AsSingle();
        }
    }
}