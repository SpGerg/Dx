using Exiled.API.Features;

namespace Dx.Events
{
    public class Plugin : Plugin<Config>
    {
        public static Config Config => _config;
        
        public static string EventName { get; internal set; }
        
        public static string EventRpLevel { get; internal set; }

        private static Config _config;

        public override void OnEnabled()
        {
            _config = base.Config;
            
            base.OnEnabled();
        }
    }
}