using System;
using UnityEngine.SceneManagement;

namespace Delphin.Services
{
    public sealed class SceneLoaderService : ISceneLoaderService
    {
        public event Action<string> SceneLoaded;

        public void Initialize()
        {
        }

        public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, Action onLoaded = null)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName, mode);
            if (operation == null)
                return;

            operation.completed += _ =>
            {
                SceneLoaded?.Invoke(sceneName);
                onLoaded?.Invoke();
            };
        }

        public void Shutdown()
        {
            SceneLoaded = null;
        }
    }
}
