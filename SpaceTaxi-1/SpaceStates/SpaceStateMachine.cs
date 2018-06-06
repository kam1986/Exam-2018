using DIKUArcade.EventBus;
using DIKUArcade.State;
using SpaceTaxi_1.SpaceStates;
using System;

namespace SpaceTaxi_1.SpaceStates {
    /// <summary>
    /// SpaceMachine processes the messages from Statebus and updates the game accordingly.
    /// </summary>
    public class SpaceMachine : IGameEventProcessor<object> {
        private IGameEventProcessor<object> _gameEventProcessorImplementation;

        public SpaceMachine() {
            StateBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            StateBus.GetBus().Subscribe(GameEventType.InputEvent, this);

            ActiveState = MainMenu.GetInstance();
        }

        public IGameState ActiveState { get; }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            ActiveState.UpdateGameLogic();
        }

        private void SwitchState(GameSpaceType stateType) {
            switch (stateType) {
            //case ActiveState.HandleKeyEvent("KEY_UP", "KEY_RELEASE"):
            case GameSpaceType.GamePaused:

                break;

            case GameSpaceType.GameRunning:

                break;

            case GameSpaceType.MainMenu:
                MainMenu.GetInstance();
                ActiveState.RenderState();
                break;

            default:
             throw new ArgumentException("NO GAMESTATE");

                break;

            }
        }
    }

    public class StateMachine : IGameEventProcessor<object> {
        public StateMachine() {
            ActiveState = MainMenu.GetInstance();

            StateBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            StateBus.GetBus().Subscribe(GameEventType.InputEvent, this);
        }

        public IGameState ActiveState { get; private set; }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            switch (eventType) {
            case GameEventType.InputEvent:
                ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
                break;

            case GameEventType.GameStateEvent:
                switch (gameEvent.Parameter1) {
                case "GAME_RUNNING":
                    SwitchState(GameSpaceType.GameRunning);
                    break;

                case "QUIT":
                    StateBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.WindowEvent, this,
                            "CLOSE_WINDOW", "", ""));
                    break;

                case "GAME_PAUSED":
                    SwitchState(GameSpaceType.GamePaused);
                    break;

                case "NEW_GAME":
                    ActiveState = SpaceRunning.NewGame();
                    break;
                }

                break;
            }
        }

        private void SwitchState(GameSpaceType stateType) {
            switch (stateType) {
            case GameSpaceType.MainMenu:
                ActiveState = MainMenu.GetInstance();
                break;
            case GameSpaceType.GameRunning:
                ActiveState = SpaceRunning.GetInstance();
                break;
            case GameSpaceType.GamePaused:
                ActiveState = SpacePause.GetInstance();
                break;
            }
        }
    }
}