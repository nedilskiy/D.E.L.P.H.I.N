using System.Collections.Generic;
using Delphin.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Delphin.Core
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private string firstSceneName = "Menu";

        private readonly List<IGameService> services = new();

        private void Awake()
        {
            RegisterServices();

            foreach (var service in services)
                service.Initialize();
        }

        private void Start()
        {
            var gameState = ServiceLocator.Get<IGameStateService>();
            gameState.SetState(GameState.Loading);

            ServiceLocator.Get<ISceneLoaderService>()
                .LoadScene(firstSceneName, LoadSceneMode.Single, OnFirstSceneLoaded);
        }

        private void OnApplicationQuit()
        {
            for (var i = services.Count - 1; i >= 0; i--)
                services[i].Shutdown();
        }

        private void RegisterServices()
        {
            RegisterService<IGameStateService>(new GameStateService());
            RegisterService<IAudioService>(new AudioService());
            RegisterService<IInputService>(new InputService(inputActions));
            RegisterService<ISceneLoaderService>(new SceneLoaderService());
        }

        private void RegisterService<TInterface>(TInterface instance) where TInterface : class, IGameService
        {
            ServiceLocator.Register(instance);
            services.Add(instance);
        }

        private void OnFirstSceneLoaded()
        {
            ServiceLocator.Get<IGameStateService>().SetState(GameState.MainMenu);
        }
    }
}
