using System.IO;
using DIKUArcade.EventBus;
using DIKUArcade.State;
using SpaceTaxi_1.LevelBuilder;
using SpaceTaxi_1.SpaceTaxiEntities;

namespace SpaceTaxi_1.SpaceTaxiStates {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }

        private GameEventBus<object> _eventBus;

        private IGameState currentLevel;
        

       
        
        public StateMachine() {
            ActiveState = MainMenu.GetInstance();
            _eventBus = SpaceTaxiBus.GetBus();
            _eventBus.Subscribe(GameEventType.InputEvent, this);
            _eventBus.Subscribe(GameEventType.GameStateEvent, this);        }
        
        private void SwitchState(GameStateType stateType) {
            switch (stateType) {
            case GameStateType.MainMenu:
                ActiveState = MainMenu.GetInstance();
                break;
                
            case GameStateType.NewGame:
                currentLevel = NewGame();
                ActiveState = currentLevel;
                Player.GetInstance().ResetPlayer();
                break;
                
            case GameStateType.GameRunning:
                ActiveState = currentLevel;
                break;
                
            case GameStateType.GamePaused:
                ActiveState = GamePaused.GetInstance();
                break;
                
            case GameStateType.NextLevel:
                currentLevel = NextLevel();
                ActiveState = currentLevel;
                break;
                
           
            }
        }

        public Level NewGame() {
            
            return new Level(
                new LevelParser(
                    new Loader(Path.Combine("Levels", "short-n-sweet.txt"))));
            
        }

        private IGameState NextLevel() {
            Player.GetInstance().ResetPlayer();
            return new Level(
                new LevelParser(
                    new Loader(Path.Combine("Levels", "the-beach.txt"))));
            
        }
        
        
        
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            switch (eventType) {
            
            // All key inputs are been handle by the active state.
            case GameEventType.InputEvent:
                ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
                break;

            case GameEventType.GameStateEvent:
                switch (gameEvent.Message) {
                
                case "GAME_RUNNING":
                    SwitchState(GameStateType.GameRunning);
                    break;

                case "QUIT":
                    SpaceTaxiBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.WindowEvent, this,
                            "CLOSE_WINDOW", "", ""));
                    break;

                case "GAME_PAUSED":
                    SwitchState(GameStateType.GamePaused);
                    break;
                    
                case "NEXT_LEVEL":
                    SwitchState(GameStateType.NextLevel);
                    break;

                case "NEW_GAME":
                    SwitchState(GameStateType.NewGame);
                    break;
                    
                case "MAIN_MENU":
                    SwitchState(GameStateType.MainMenu);
                    break;
                }

                break;
            }
        }
    }
}