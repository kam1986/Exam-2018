using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using SpaceTaxi_1.Assets.GameConstants;

using SpaceTaxi_1.Movement;

namespace SpaceTaxi_1.SpaceTaxiEntities
{
    public class Player : IGameEventProcessor<object> {

        private static Player taxi;

        private string destination;
        public string Location = "";

        public bool Landed { get; set; }

        public IMovement Movement;

        public Entity Entity { get; }
        private readonly DynamicShape shape;
        private readonly Image _taxiBoosterOffImageLeft;
        private readonly Image _taxiBoosterOffImageRight;

        private readonly ImageStride _taxiBoosterOnImageLeft;
        private readonly ImageStride _taxiBoosterOnImageRight;

        private readonly ImageStride _taxiBoosterOnImageBottomLeft;
        private readonly ImageStride _taxiBoosterOnImageBottomRight;

        private readonly ImageStride _taxiBoosterOnImageBottomAndLeft;
        private readonly ImageStride _taxiBoosterOnImageBottomAndRight;


        private List<Image> explosionStrides;

        private Orientation _taxiOrientation;
        private Orientation _thrusterOrientation;

        public void Explode (){
            IsExploding = true;
        }

        public bool IsExploding { get; private set; } 

        
        
        public void ResetPlayer() {
            SetDirection(new Vec2F());
            IsExploding = false;
        }
        
        // Physic vectors for thruster and gravitys
        public Vec2F Thruster { get; } = new Vec2F(0.0f, 0.0f);
        private Vec2F gravity = new Vec2F(0.0f, GameConstants.GRAVITY);

        private Player() {
            Movement = new TrivialMovement();

            destination = "";

            shape = new DynamicShape(new Vec2F(), new Vec2F());
            // thruster inactive.
            _taxiBoosterOffImageLeft =
                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            _taxiBoosterOffImageRight =
                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));

            // Create explosions
            explosionStrides = ImageStride.CreateStrides(
                GameConstants.NUMBER_OF_IMAGE_EXPLOSIONS, "Assets/Images/Explosion.png");

            var milsec = 60;
            
            
            // Back thruster active.
            _taxiBoosterOnImageLeft =
                new ImageStride(milsec,
                    ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Taxi_Thrust_Back.png"))
                );
            ;

            _taxiBoosterOnImageRight =
                new ImageStride(milsec,
                    ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Taxi_Thrust_Back_Right.png"))
                );
            ;

            // Bottom thrusters active.
            _taxiBoosterOnImageBottomLeft =
                new ImageStride(milsec,
                    ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom.png"))
                );
            _taxiBoosterOnImageBottomRight =
                new ImageStride(milsec,
                    ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Right.png"))
                );
            ;

            // all thrusters active.
            _taxiBoosterOnImageBottomAndLeft =
                new ImageStride(milsec,
                    ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back.png"))
                );
            ;

            _taxiBoosterOnImageBottomAndRight =
                new ImageStride(milsec,
                    ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back_Right.png"))
                );
            ;

            explosionStrides =
                ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));

            _taxiOrientation = Orientation.Right;
            _thrusterOrientation = Orientation.None;

            Entity = new Entity(shape, _taxiBoosterOffImageLeft);
        }

        public static Player GetInstance() {
            return Player.taxi ?? (Player.taxi = new Player());
        }


        /// <summary>
        /// Set the costumer
        /// </summary>
        /// <param name="destinationID"></param>
        public void SetDestination(string destinationID) {
            destination = destinationID;
        }

        /// <summary>
        /// Drop of the costumer
        /// </summary>
        public void DropOfCustomer() {
            destination = "";
        }

        /// <summary>
        /// Check if the taxi has a customer.
        /// </summary>
        public bool InService => destination != "";

        public string Destination => destination;

        

        /// <summary>
        /// Setting the taxi orientation at the start of a new level.
        /// </summary>
        /// <param name="orientation"></param>
        public void SetOrientation(Orientation orientation) {
            switch (orientation) {
            case Orientation.Left:
                _taxiOrientation = Orientation.Left;
                Entity.Image = _taxiBoosterOffImageLeft;
                break;

            case Orientation.Right:
                _taxiOrientation = Orientation.Right;
                Entity.Image = _taxiBoosterOffImageRight;
                break;
            }
        }

        /// <summary>
        /// Move the player accordingly to the pressed keys.
        /// </summary>
        public void Move() {
            if (!IsExploding) {
                Movement.Move();
            }
        }
    

    public void SetPosition(Vec2F pos) {
            shape.Position = pos;
        }

        public void SetExtent(Vec2F ext) {
            shape.Extent = ext;
        }

        public void SetDirection(Vec2F dir) {
            shape.Direction = dir;
        }

        public void RenderPlayer()
        {
            // Thruster checking.
            switch (_thrusterOrientation) {
            case Orientation.Left:
                Entity.Image = _taxiBoosterOnImageLeft;
                break;
                
            case Orientation.Right:
                Entity.Image = _taxiBoosterOnImageRight;
                break;
                
            case Orientation.Up:
                switch (_taxiOrientation) {
                    case Orientation.Left:
                        Entity.Image = _taxiBoosterOnImageBottomLeft;
                        break;
                        
                    case Orientation.Right:
                        Entity.Image = _taxiBoosterOnImageBottomRight;
                        break;

                }
                break;
                
            case Orientation.UpLeft:
                Entity.Image = _taxiBoosterOnImageBottomAndLeft;
                break;
                
            case Orientation.UpRight:
                Entity.Image = _taxiBoosterOnImageBottomAndRight;
                break;
                
            case Orientation.None:
                // no thrusters active.
                Entity.Image = _taxiOrientation == Orientation.Left
                    ? _taxiBoosterOffImageLeft
                    : _taxiBoosterOffImageRight;
                break;
            }
            // check if the taxi has collided
            switch (IsExploding) {
            case true :
                // render the explosion at the taxies location.
                var explosion = new ImageStride(120/8,explosionStrides);
                explosion.Render(shape);
                break;
            default:
                Entity.RenderEntity();
                break;
            }


        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent)
        {
            if (eventType == GameEventType.PlayerEvent)
            {
                switch (gameEvent.Message)
                {
                    // in the future, we will be handling movement here
                    
                    // Key Press
                    case "BOOSTER_UPWARDS":
                        Thruster.Y = GameConstants.PLAYER_VERTICAL_SPEED;
                        switch (_thrusterOrientation) {
                            case Orientation.Left:
                                _thrusterOrientation = Orientation.UpLeft;
                                // Movement = new TrivialMovement();
                                break;
                                
                            case Orientation.Right:
                                _thrusterOrientation = Orientation.UpRight;
                                break; 
                            
                            default:
                                _thrusterOrientation = Orientation.Up;
                                break;
                        }
                        break;
                    
                    case "BOOSTER_TO_LEFT":
                        Thruster.X = -GameConstants.PLAYER_HORISONTAL_SPEED;
                        _taxiOrientation = Orientation.Left;
                        switch (_thrusterOrientation) {
                            case Orientation.Up:
                                _thrusterOrientation = Orientation.UpLeft;
                                break;
                            
                            // does nothing.
                            case Orientation.UpLeft:
                                break;
                                
                            case Orientation.UpRight:
                                _thrusterOrientation = Orientation.UpLeft;
                                break;
                            
                            default:
                                _thrusterOrientation = Orientation.Left;
                                break;
                        }
                        break;
                    
                    case "BOOSTER_TO_RIGHT":
                        Thruster.X = GameConstants.PLAYER_HORISONTAL_SPEED;
                        _taxiOrientation = Orientation.Right;
                        switch (_thrusterOrientation) {
                            case Orientation.Up:
                                _thrusterOrientation = Orientation.UpRight;
                                break;
                            
                            // does nothing.
                            case Orientation.UpRight:
                                break;
                                
                            case Orientation.UpLeft:
                                _thrusterOrientation = Orientation.UpRight;
                                break;
                            
                            default:
                                _thrusterOrientation = Orientation.Right;
                                break;
                        }
                        break;
                    
                    // Key Release
                    case "STOP_ACCELERATE_UP":
                        Thruster.Y = GameConstants.PLAYER_STILL;
                        switch (_thrusterOrientation) {
                            case Orientation.UpLeft:
                                _thrusterOrientation = Orientation.Left;
                                break;
                                
                            case Orientation.UpRight:
                                _thrusterOrientation = Orientation.Right;
                                break;
                            
                            default:
                                _thrusterOrientation = Orientation.None;
                                break;
                        }
                        break;
                    
                    case "STOP_ACCELERATE_LEFT":
                        Thruster.X = GameConstants.PLAYER_STILL;
                        switch (_thrusterOrientation) {
                            case Orientation.UpLeft:
                                _thrusterOrientation = Orientation.Up;
                                break;
                                
                            default:
                                _thrusterOrientation = Orientation.None;
                                break;
                        }
                        break;
                    
                    case "STOP_ACCELERATE_RIGHT":
                        Thruster.X = GameConstants.PLAYER_STILL;
                        switch (_thrusterOrientation) {
                        case Orientation.UpRight:
                            _thrusterOrientation = Orientation.Up;
                            break;
                                
                        default:
                            _thrusterOrientation = Orientation.None;
                            break;
                        }
                        break;
                }
                
            }
        }
        
      
    }
}
