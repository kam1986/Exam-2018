using DIKUArcade.Entities;
using DIKUArcade.Math;
using SpaceTaxi_1.Assets.GameConstants;
using SpaceTaxi_1.SpaceTaxiEntities;

namespace SpaceTaxi_1.Movement {
    public class TrivialMovement : IMovement {

       
        public void Move() {

            var shape = (DynamicShape) Player.GetInstance().Entity.Shape;

            var updates = Game.ScreenTimer.CapturedUpdates;
            
            Vec2F thrusterDirection = Player.GetInstance().Thruster;
            Vec2F gravityDirection = new Vec2F(0.0f, GameConstants.GRAVITY);


            float relativeSpeed = 1.0f;

            // Using Timer.CapturedUpdates to calculate how many
            // updates happened during the lastsecond.
            if (updates > 0) {
                relativeSpeed = 60.0f / updates;
            }

            

            shape.Direction.X += (thrusterDirection.X + gravityDirection.X) * relativeSpeed;
            shape.Direction.Y += (thrusterDirection.Y + gravityDirection.Y) * relativeSpeed;

            shape.Move();
            

            if (shape.Position.X < 0.0f) {
                shape.Position.X = 0.0f;
            }

            if (shape.Position.X > GameConstants.SCREEN_WIDTH - GameConstants.PLAYER_WIDTH) {
                shape.Position.X = GameConstants.SCREEN_WIDTH - GameConstants.PLAYER_WIDTH;
            }

            if (shape.Position.Y < 0.0f) {
                shape.Position.Y = 0.0f;
            }
            
            if (shape.Position.Y > GameConstants.SCREEN_HEIGHT - GameConstants.PLAYER_HEIGHT) {
                shape.Position.Y = GameConstants.SCREEN_HEIGHT - GameConstants.PLAYER_HEIGHT;
            }
        }

        public void Move(DynamicShape shape) {
            throw new System.NotImplementedException();
        }
    }
}
