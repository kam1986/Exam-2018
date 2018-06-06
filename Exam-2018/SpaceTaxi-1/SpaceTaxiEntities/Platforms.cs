using System;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using SpaceTaxi_1.Assets.GameConstants;
using SpaceTaxi_1.Collision;
using SpaceTaxi_1.Movement;


namespace SpaceTaxi_1.SpaceTaxiEntities {
    /// <summary>
    /// Platform class which should be used for any platform in Space Taxi.
    /// OBS the Platform should not be rendered, and should be placed just above where it,
    /// to help collision detection for customers and taxi.
    /// </summary>
    public class Platform : Entity, ICollision {
        private char ID;

        public Platform(StationaryShape shape, Image image, char id) : base(shape, image) {
            ID = id;
            
        }
        
        public bool CollidWith() {
            
            var player = (DynamicShape) Player.GetInstance().Entity.Shape;
            var pl = Player.GetInstance();
            // drop of customer if the ID match 
            /*if (Customers.GetInstance().CustomerCount > 0 && pl.Destination == "^" ||
                pl.Destination == ID.ToString() &&
                player.Position.Y <= Shape.Position.Y + Shape.Extent.Y) {

                Player.GetInstance().DropOfCustomer();
                
            }*/
            
            // Player collision
            var data = CollisionDetection.Aabb(player, Shape);
            if (data.Collision && data.CollisionDir != CollisionDirection.CollisionDirDown
                || (player.Position.X >= Shape.Position.X && player.Position.X <= Shape.Position.X + Shape.Extent.X 
                && player.Position.Y >= Shape.Position.Y && player.Position.Y <= Shape.Position.Y + Shape.Extent.Y)){
                var p = Player.GetInstance();
                // calculation speed
                var speed = Math.Sqrt(
                    player.Direction.X * player.Direction.X +
                    player.Direction.Y * player.Direction.Y)/
                    Game.ScreenTimer.CapturedUpdates;
                    
                
                
                if (speed > 0.00004f) {
                    p.Explode();
                    player.Position.Y = Shape.Position.Y + Shape.Extent.Y;
                } else
                {
                    p.Landed = true;
                    if (p.Movement is TrivialMovement) {
                        p.Movement = new OnPlatform(Shape);
                        ((DynamicShape) p.Entity.Shape).Direction.X = 0.0f;
                    }

                    p.Location = ID.ToString();
                }
                
            } 
            
            return data.Collision && data.CollisionDir != CollisionDirection.CollisionDirDown; 
        }
    }
}