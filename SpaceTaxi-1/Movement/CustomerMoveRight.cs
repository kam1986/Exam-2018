using DIKUArcade.Entities;
using DIKUArcade.Math;


namespace SpaceTaxi_1.Movement {
    public class CustomerMoveRight : IMovement {
        
        private DynamicShape customer;
        
        public CustomerMoveRight(DynamicShape shape) {
            customer = shape;
            shape.Direction = new Vec2F(0.0005f, 0.0f);
        }
        
        public void Move() {
            customer.Move();
        }
    }
}