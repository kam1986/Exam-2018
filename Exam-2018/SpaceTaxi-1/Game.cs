using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using SpaceTaxiTesting;
using SpaceTaxi_1.SpaceTaxiEntities;
using SpaceTaxi_1.SpaceTaxiStates;


namespace SpaceTaxi_1
{
    public class Game : IGameEventProcessor<object>
    {
        private Window _win;
        private GameEventBus<object> _eventBus;
        public static GameTimer ScreenTimer;
        public static Timer Timer;

        private Entity _backGroundImage;
        private Player _taxi;

        
        private StateMachine StateMachine;

        public Game()
        {
            
            // window
            _win = new Window("Space Taxi Game v0.1", 1000, AspectRatio.R4X3);
            
            // event bus
            _eventBus = SpaceTaxiBus.GetBus();
            _eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.GameStateEvent,  // control state event
                GameEventType.InputEvent,      // key press / key release
                GameEventType.WindowEvent,     // messages to the window, e.g. CloseWindow()
                GameEventType.PlayerEvent      // commands issued to the player object, e.g. move, destroy, receive health, etc.
            });
           
            _win.RegisterEventBus(_eventBus);

            // game timer
            Game.ScreenTimer = SpaceTimer.GetTimer;

            // game assets
            _backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );
            _backGroundImage.RenderEntity();

            // game entities
            _taxi = Player.GetInstance();
            
            
            // event delegation
           _eventBus.Subscribe(GameEventType.WindowEvent, this);
           _eventBus.Subscribe(GameEventType.PlayerEvent, _taxi);
            
            StateMachine = new StateMachine();
        }

        public void GameLoop()
        {
            while (_win.IsRunning())
            {
                Game.ScreenTimer.MeasureTime();

                while (Game.ScreenTimer.ShouldUpdate())
                {
                    _win.PollEvents();
                    _eventBus.ProcessEvents();

                    StateMachine.ActiveState.GameLoop();
                    
                    
                    
                }

                if (Game.ScreenTimer.ShouldRender())
                {
                    _win.Clear();
                    _backGroundImage.RenderEntity();
                    StateMachine.ActiveState.RenderState();
                   
                    _win.SwapBuffers();
                }

                if (Game.ScreenTimer.ShouldReset())
                {
                    // 1 second has passed - display last captured ups and fps from the timer
                    _win.Title = "Space Taxi | UPS: " + Game.ScreenTimer.CapturedUpdates + ", FPS: " +
                                Game.ScreenTimer.CapturedFrames;
                }

                if (Player.GetInstance().IsExploding) {
                    // TODO - needs delay
                    _eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, this, "MAIN_MENU", "", ""));
                    
                }
            }
        }

  

        

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent)
        {
            if (eventType == GameEventType.WindowEvent)
            {
                switch (gameEvent.Message)
                {
                    case "CLOSE_WINDOW":
                        _win.CloseWindow();
                        break;
                    
                }
            }
           
        }
    }
}
