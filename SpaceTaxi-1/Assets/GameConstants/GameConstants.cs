namespace SpaceTaxi_1.Assets.GameConstants
{
    public static class GameConstants
    {
        // Screen
        public const float SCREEN_WIDTH = 1.0f;
        public const float SCREEN_HEIGHT = 1.0f;
        
       // Player
        public const float PLAYER_WIDTH = 1.0f/23.0f;
        public const float PLAYER_HEIGHT = 1.0f/23.0f;
        public const float PLAYER_STARTING_POSITION_X = 0.45f;
        public const float PLAYER_STARTING_POSITION_Y = 0.1f;
        
        // Physics
        public const float PLAYER_STILL = 0.0f;
        public const float PLAYER_VERTICAL_SPEED = 0.0001f;
        public const float PLAYER_HORISONTAL_SPEED = 0.00003f;
        public const float GRAVITY = -0.00003f;
        
        // Explosion
        public const int EXPLOSION_DURATION = 500;
        public const float EXPLOSION_WIDTH = 0.1f;
        public const float EXPLOSION_HEIGHT = 0.1f;
        public const int NUMBER_OF_IMAGE_EXPLOSIONS = 8;
        
        // gameTimer
        public const int UPS = 60;
        public const int FPS = 60;
    }
}
