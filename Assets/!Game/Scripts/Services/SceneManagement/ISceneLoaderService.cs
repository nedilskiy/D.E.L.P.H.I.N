using System;
using Delphin.Core;
using UnityEngine.SceneManagement;

namespace Delphin.Services
{
    public interface ISceneLoaderService : IGameService
    {
        event Action<string> SceneLoaded;
        void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, Action onLoaded = null);
    }
}
