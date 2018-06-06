using System.Runtime.Serialization.Formatters;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using SpaceTaxi_1.Collision;
using SpaceTaxi_1.Movement;
using SpaceTaxi_1.SpaceTaxiEntities;
using SpaceTaxi_1.SpaceTaxiStates;

namespace SpaceTaxi_1.LevelBuilder {
    public class Level : IGameState{
        
        private IParser data;

        
        private Customers customers = Customers.GetInstance();
        
        private EntityContainer<Obstacle> obstacles;
        private EntityContainer<Platform> platforms;


        private Portal portal;
        

        private string name;

        private GameEventBus<object> _eventBus = SpaceTaxiBus.GetBus();
       
        
        // Explosions
        public static AnimationContainer Explosions = new AnimationContainer(1);
        
        public Player Taxi { get; }

        public Level(IParser parser) {
            data = parser;
            Taxi = Player.GetInstance();
            
            InitializeGameState();
        }

        public Portal GetPortal() {
            return portal;
        }

        public Vec2F GetStartPosition { get; private set; }

        public void CollisionChecker(ICollision entity) {
            entity.CollidWith();
        }

       

        public void GameLoop()
        {
            Taxi.Landed = false;
            obstacles.Iterate(CollisionChecker);
            platforms.Iterate(CollisionChecker);
            if (portal.CollidWith()) {
                _eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent, this, "NEXT_LEVEL", "", ""));
            }
            if (!Taxi.Landed) {
                Taxi.Location = "";
                Taxi.Movement = new TrivialMovement();
            }
            
            Taxi.Move();
            

        }

        public void InitializeGameState() {

            GetStartPosition = data.GetTaxiPosition();
            Taxi.SetPosition(GetStartPosition);
            Taxi.SetDirection(new Vec2F());
            Taxi.SetExtent(new Vec2F(1.0f / 23.0f, 1.0f / 23.0f));
            
            obstacles = data.GetObstacles();
            platforms = data.GetPlatforms();
            portal = data.GetPortal();
            
            name = data.GetName();

            var parser = (LevelParser) data;
            customers.SetDestinations(parser.destinations);
            customers.SetLocations(parser.locations);
            customers.SetPoints(parser.points);
            customers.SetSpawntimer(parser.spawntimes);
            customers.SetTimeLimits(parser.timeLimits);
            customers.SetStartPositions(parser.StartingPositions);
        }

        
        
        public void UpdateGameLogic() {
            
            
        }

        public void RenderState() {
            obstacles.RenderEntities();
            platforms.RenderEntities();
            Taxi.RenderPlayer();
            customers.RenderCustomer();
            
        }

        
        public void KeyPress(string key)
        {
            switch (key)
            {
                case "KEY_UP":
                    _eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_UPWARDS", "", ""));
                    break;
                
                case "KEY_LEFT":
                    _eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_TO_LEFT", "", ""));
                    break;
                
                case "KEY_RIGHT":
                    _eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_TO_RIGHT", "", ""));
                    break;
                
                
            }
        }

        public void KeyRelease(string key)
        {
            switch (key)
            {
                case "KEY_UP":
                    _eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "STOP_ACCELERATE_UP", "", ""));
                    break;
                
                case "KEY_LEFT":
                    _eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "STOP_ACCELERATE_LEFT", "", ""));
                    break;
                
                case "KEY_RIGHT":
                    _eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "STOP_ACCELERATE_RIGHT", "", ""));
                    break;
                    
                case "KEY_P":
                    _eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, this, "GAME_PAUSED", "", ""));
                    break;
                
                
            }
        }
        
        
        
        
        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyAction) {
                case "KEY_PRESS":
                    KeyPress(keyValue);
                    break;
                    
                case "KEY_RELEASE":
                    KeyRelease(keyValue);
                    break;
            }
        }
    }
}