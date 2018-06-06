using DIKUArcade.EventBus;

namespace SpaceTaxi_1 {
    /// <summary>
    /// Designated Event Handler that can be called in different sections.
    /// </summary>
    public class StateBus {
        
        private static GameEventBus<object> eventBus;

        public static GameEventBus<object> GetBus()
        {
            return StateBus.eventBus ?? (StateBus.eventBus = new GameEventBus<object>());
        }

    }
}
