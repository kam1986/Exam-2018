using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Physics;
using SpaceTaxi_1.Collision;

namespace SpaceTaxi_1.SpaceTaxiEntities {
    /// <summary>
    /// Portal Class which is intended for level portaling and in latter level between different
    /// protals.
    /// </summary>
    public class Portal : Entity, ICollision{
        
        public Portal (Shape shape, Image image) : base(shape, image){}

        public bool CollidWith() {
            var player = Player.GetInstance();
            if (player.Destination.Length > 1) {
                // remove stamp for next level maker if it is for specific platform.
                player.SetDestination(player.Destination[1].ToString());
            }
            
            return CollisionDetection.Aabb(
                (DynamicShape) Player.GetInstance().Entity.Shape, Shape).Collision;
            
        }
    }
}