using System;
namespace SpaceTaxi_1.SpaceStates {
    public enum GameSpaceType {
        GameRunning,
        GamePaused,
        MainMenu,
        ChooseLevel,
        NewGame
    };
/// <summary>
/// Takes a string
/// And then returns a state 
/// </summary>
    public static class SpaceTransformer {
        public static GameSpaceType TransformStringToSState(string state) {
            switch (state) {
            case "PAUSE_GAME":
                return GameSpaceType.GamePaused;
            case "MAIN_MENU":
                return GameSpaceType.MainMenu;
            case "CHOOSE_LEVEL":
                return GameSpaceType.ChooseLevel;
            case "NEW_GAME":
                return GameSpaceType.NewGame;
            default:
                return GameSpaceType.GameRunning;
            }

        }
    /// <summary>
    /// Takes a state and returns a string
    /// Otherwise throws an exception if the state is not found.
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
        public static string TransformStateToString(GameSpaceType state)
        {
            switch (state)
            {
            case  GameSpaceType.GamePaused:
                return "PAUSE_GAME";
                    
                
            case GameSpaceType.MainMenu:
                return "MAIN_MENU";
            
            case GameSpaceType.NewGame:
                return "NEW_GAME";
                
            case GameSpaceType.GameRunning:
                return "GAME_RUNNING";
            case GameSpaceType.ChooseLevel:
                return "CHOOSE_LEVEL";
                
            default:
                throw new ArgumentException("NO GAMESTATE");
            }
        }
    }
}