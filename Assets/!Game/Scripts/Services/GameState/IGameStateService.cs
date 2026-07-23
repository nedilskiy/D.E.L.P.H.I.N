using System;
using Delphin.Core;

namespace Delphin.Services
{
    public interface IGameStateService : IGameService
    {
        GameState CurrentState { get; }
        event Action<GameState> StateChanged;
        void SetState(GameState newState);
    }
}
