using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Physics;
using SpaceTaxi_1.Collision;


namespace SpaceTaxi_1.SpaceTaxiEntities {
    public class Obstacle : Entity, ICollision{
        
        public Obstacle(Shape shape, Image image) : base(shape, image){}

        public bool CollidWith() {
            
            var player = (DynamicShape) Player.GetInstance().Entity.Shape;

            var data = CollisionDetection.Aabb(player, Shape);
            if (data.Collision) {
                Player.GetInstance().Explode();
            }

            return data.Collision;
            
        }
      
    }
}