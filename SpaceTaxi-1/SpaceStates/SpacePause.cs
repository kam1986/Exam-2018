using DIKUArcade.State;
using DIKUArcade;
using DIKUArcade.State;
using System;
using System.Drawing;
using System.Linq.Expressions;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;


namespace SpaceTaxi_1.SpaceStates {
    /// <summary>
    /// Handles game pausing and resuming.
    /// </summary>
    public class SpacePause : IGameState {
        
        public static SpacePaused gamePaused;

        private Text help;



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
            "                         Game Roles\n\n" +
            "      1. Shoot as many enemies as you can\n"+
            "           by tapping space\n\n" +
            "      2. Move from side to side by pressing\n"+
            "           the left or right key\n\n" +
            "    To quit press \"Q\"\n"+
            "    and to play pres ESC\n"+
            "    For new game press \"N\"",
            new Vec2F(0f, 0f),
            new Vec2F(1f, 0.5f));
            help.SetColor(new Vec3I(255, 165, 0));
            help.SetFontSize(20);
        }

        public void UpdateGameLogic() {
        }

        public void RenderState() {
            var run = GameRunning.GetInstance();
            run.RenderState();
            help.RenderText();
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {

            switch (keyValue) {
                case "KEY_Q":
                    StateBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent,this,"CLOSE_WINDOW","",""));
                    break;
                   
                case "KEY_ESCAPE":
                    if (keyAction == "KEY_RELEASE")
                    StateBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent,this,"CHANGE_STATE","GAME_RUNNING",""));
                    
                    break;
                
                case "KEY_N":
                    StateBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent,this,"CHANGE_STATE","NEW_GAME",""));
                    break;
            }
            
            
        }

        

    }

}
