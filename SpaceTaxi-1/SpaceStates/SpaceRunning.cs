using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;
using DIKUArcade.Entities;
using SpaceTaxi_1.SpaceStates;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;
using System.Xml;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using SpaceTaxi_1.SpaceStates;

namespace SpaceTaxi_1.SpaceStates {
    public class SpaceRunning {

        private Window win;
        private Player player;
        private GameEventBus<object> eventBus;

        private IBaseImage gameShotImage;
        private GameTimer _gameTimer;
        private  DynamicShape shape;
        private  Image _taxiBoosterOffImageLeft;
        private  Image _taxiBoosterOffImageRight;
        public Entity Entity { get; private set; }
        public Image Background;


        public void GameLoop() {
            while (win.IsRunning()) {
                _gameTimer.MeasureTime();
                while (_gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    eventBus.ProcessEvents();
                    SpaceMachine.ActivateState.UpdateGameLogic();


                }

                if (_gameTimer.ShouldRender()) {
                    win.Clear();
                    SpaceMachine.ActivateState.RenderState();
                    win.SwapBuffers();
                }

                if (_gameTimer.ShouldRender()) {
                    win.Title = "Space Taxi | UPS: " + _gameTimer.CapturedUpdates + ", FPS: " +
                                _gameTimer.CapturedFrames;
                }
            }
        }
        

        public class SpaceRunning : IGameState {

            // There should only be one game running at anytime, hence static.
            private static SpaceRunning gameRunning;

            private GameRunning() {
                InitializeGameState();
            }

            public static SpaceRunning NewGame() {
                SpaceRunning.gameRunning = new SpaceRunning();
                return SpaceRunning.gameRunning;
            }

            public static SpaceRunning GetInstance() {
                return SpaceRunning.gameRunning ?? (SpaceRunning.gameRunning = new SpaceRunning());
            }

            public void GameLoop() { }

            public void UpdateGameLogic() {
            private Player player;


        }

            public void InitializeGameState() {
                Background = new Entity(
                    new StationaryShape(
                        new Vec2F(0f, 0f),
                        new Vec2F(1f, 1f)),
                    new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));

                // Enemies

                // Player
                shape = new DynamicShape(new Vec2F(), new Vec2F());
                _taxiBoosterOffImageLeft = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
                _taxiBoosterOffImageRight = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));

                Entity = new Entity(shape, _taxiBoosterOffImageLeft);
            
            }


            public void HandleKeyEvent(string keyValue, string keyAction) {
                switch (keyValue) {
                // create a playerEvent of the with keyValue = messege and KeyAction = Parameter1
                // in case of the to cases belove.
                case "KEY_LEFT":
                case "KEY_RIGHT":
                    StateBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, keyValue, keyAction, ""));
                    break;

                case "KEY_SPACE":
                    if (keyAction == "KEY_RELEASE") {

                        StateBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, "CHANGE_STATE", "GAME_PAUSED",
                                ""));
                    }

                    break;

                }
            }
        }
    }
}