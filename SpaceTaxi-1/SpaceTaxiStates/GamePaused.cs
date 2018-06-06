using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;

namespace SpaceTaxi_1.SpaceTaxiStates {
    public class GamePaused : IGameState {

        public static GamePaused gamePaused;

        private Text help;

        private Entity backGroundImage =
            new Entity(
                new StationaryShape(
                    new Vec2F(0f, 0f),
                    new Vec2F(1f, 1f)),
                    new Image(Path.Combine("Assets","Images","SpaceBackground.png")));

        private GamePaused() {
            InitializeGameState();
            
        }

        public static GamePaused GetInstance() {
            return GamePaused.gamePaused ?? (GamePaused.gamePaused = new GamePaused());
        }


        public void GameLoop() {
        }

        public void InitializeGameState() {
             help = new Text(
            " Game Paused\n\n" +
            "    To quit press \"Q\"\n"+
            "    and to play pres \"P\"\n"+
            "    For new game press \"N\"",
            new Vec2F(0f, 0f),
            new Vec2F(1f, 0.5f));
            help.SetColor(new Vec3I(255, 165, 0));
            help.SetFontSize(20);
        }

        public void UpdateGameLogic() {
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            help.RenderText();
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {

            switch (keyValue) {
                case "KEY_Q":
                    SpaceTaxiBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent,this,"CLOSE_WINDOW","",""));
                    break;
                   
                case "KEY_P":
                    if (keyAction == "KEY_RELEASE")
                    SpaceTaxiBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent,this,"GAME_RUNNING", "", ""));
                    
                    break;
                
                case "KEY_N":
                    SpaceTaxiBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent,this,"NEW_GAME", "CHANGE_STATE", ""));
                    break;
            }
            
            
        }

        

    }

}