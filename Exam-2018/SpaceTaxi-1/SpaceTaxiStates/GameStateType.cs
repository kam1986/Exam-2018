using System;

namespace SpaceTaxi_1.SpaceTaxiStates
{
    public enum GameStateType {
        GameRunning,
        GamePaused,
        MainMenu,
        NewGame,
        NextLevel
    };

    public class StateTransformer
    {
        public static GameStateType TransformStringToState(string state)
        {
            switch (state)
            {
                case "PAUSE_GAME":
                    return GameStateType.GamePaused;
                 
                case "MAIN_MENU":
                    return GameStateType.MainMenu;
                
                default:
                    return GameStateType.GameRunning;
                
            }
        }

        public static string TransformStateToString(GameStateType state)
        {
            switch (state)
            {
                case  GameStateType.GamePaused:
                    return "PAUSE_GAME";
                    
                
                case GameStateType.MainMenu:
                    return "MAIN_MENU";
                
                case GameStateType.GameRunning:
                    return "GAME_RUNNING";
                
                default:
                    throw new ArgumentException("NO GAMESTATE");
            }
        }
    }
}