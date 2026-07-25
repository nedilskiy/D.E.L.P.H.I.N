using Delphin.Core;
using Delphin.Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Delphin.UI
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private string gameSceneName = "Game";

        private void Awake()
        {
            startButton.onClick.AddListener(OnStartClicked);
            exitButton.onClick.AddListener(OnExitClicked);
        }

        private void OnStartClicked()
        {
            var gameState = ServiceLocator.Get<IGameStateService>();
            gameState.SetState(GameState.Loading);

            ServiceLocator.Get<ISceneLoaderService>()
                .LoadScene(gameSceneName, LoadSceneMode.Single, () => gameState.SetState(GameState.Playing));
        }

        private void OnExitClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
