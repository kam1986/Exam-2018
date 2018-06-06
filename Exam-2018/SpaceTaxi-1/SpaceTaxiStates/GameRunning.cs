using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Math;
using DIKUArcade.State;
using SpaceTaxi_1.LevelBuilder;
using SpaceTaxi_1.SpaceTaxiEntities;
using Image = DIKUArcade.Graphics.Image;

namespace SpaceTaxi_1.SpaceTaxiStates {
    public class GameRunning : IGameState {
        
        // There should only be one game running at anytime, hence static.
        private static GameRunning gameRunning;

        private List<Level> levels;

        private int currentlevel = 0;

        public Player taxi = new Player();

        // level handler
        // private int currentLevelIndicator = 0;
        // private int LastLevel = 2;
        public Entity Background;
        
        
        
        
        private GameRunning() {
            InitializeGameState();
        }

        public static GameRunning NewGame() {
            GameRunning.gameRunning = new GameRunning();
            return GameRunning.gameRunning;
        }

        public static GameRunning GetInstance() {
            return GameRunning.gameRunning ?? (GameRunning.gameRunning = new GameRunning());
        }
        

        private void NextLevel() {
            currentlevel++;
        }

        private void Render(Entity e) {
            e.RenderEntity();
        }
        
        
        public void GameLoop() {
            
        }

        public void UpdateGameLogic() {
            
        }
        
          
        public void RenderState() {
            Background.RenderEntity();
            //levels[currentlevel].Obstacles.Iterate(Render);
        }

        public void InitializeGameState() {
            Background = new Entity(
                new StationaryShape(
                    new Vec2F(0f, 0f), 
                    new Vec2F(1f, 1f)),
                    new Image("Assets/Images/Background.jpeg"));
            
            // fetching levelfiles.
            var paths = Directory.GetFiles("Levels", ".txt");
            foreach (var path in paths) {
                levels.Add(new Level(new LevelParser(new Loader(path))));
            }
            
            
        }

      
        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyValue) {
            // create a playerEvent of the with keyValue = messege and KeyAction = Parameter1
            // in case of the to cases belove.
            
            
            case "KEY_UP":
            case "KEY_LEFT":
            case "KEY_RIGHT":
                SpaceTaxiBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, keyValue, keyAction, ""));
                break;

            


            case "KEY_ESCAPE":
                if (keyAction == "KEY_RELEASE") {

                    SpaceTaxiBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, this, "CHANGE_STATE", "GAME_PAUSED", ""));
                }

                break;
               
            }
        }
        
       
    }
}