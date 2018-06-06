using System.Security;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Physics;
using SpaceTaxi_1.Collision;
using SpaceTaxi_1.Movement;


namespace SpaceTaxi_1.SpaceTaxiEntities {
    /// <summary>
    /// Customer class which should be use for any customer of Space Taxi.
    /// </summary>
    public class Customer : Entity, ICollision {


        private string customername;
        public string Destination;
        public string Location;
        public int Pay;
        public int Spawntime;
        private Platform plane;
        
        private IMovement movement;

        public Customer(DynamicShape shape, Image image, string name) 
            : base(shape, image) {
            plane = null;
            movement = new CustomerMoveLeft((DynamicShape) Shape);
            customername = name;
            
        }

        public void SetPlatform(Platform platform) {
            plane = platform;
        }

        public bool CollidWith() {
            var player = Player.GetInstance();
            var data = CollisionDetection.Aabb((DynamicShape) player.Entity.Shape, Shape);
            
            // check if customer collid with player.
            if (data.Collision) {
                DeleteEntity();
            }
            
            // check if the customer collid with platform edges.
            if (plane.Shape.Position.X >= Shape.Position.X){
                movement = new CustomerMoveRight((DynamicShape) Shape);              
            }

            if (Shape.Position.X >= plane.Shape.Position.X + plane.Shape.Extent.X) {
                movement = new CustomerMoveLeft((DynamicShape) Shape);
            }

            return data.Collision;
        }


        public void Move() {
           movement.Move();
        }
    }
}