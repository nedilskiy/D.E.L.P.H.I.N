using System;

namespace Delphin.Services
{
    public sealed class GameStateService : IGameStateService
    {
        public GameState CurrentState { get; private set; } = GameState.Bootstrapping;
        public event Action<GameState> StateChanged;

        public void Initialize()
        {
        }

        public void SetState(GameState newState)
        {
            if (newState == CurrentState)
                return;

            CurrentState = newState;
            StateChanged?.Invoke(newState);
        }

        public void Shutdown()
        {
            StateChanged = null;
        }
    }
}
