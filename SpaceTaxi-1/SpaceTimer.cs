using DIKUArcade.Timers;

namespace SpaceTaxiTesting {
    public class SpaceTimer {
        
        
        private static GameTimer timer;


        public static GameTimer GetTimer
            => timer ?? (timer = new GameTimer(60));
        
    }
}