using System;
using Exiled.API.Features;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;

namespace Dx.Events
{
    public class Plugin : Plugin<Config>
    {
        public static Config Config => _config;

        public static Hint EventInfoHint { get; private set; }

        public static string EventName { get; internal set; }
        
        public static string EventRpLevel { get; internal set; }
        
        public static string EventHost { get; internal set; }
        
        public static DateTime? EventStartTime { get; internal set; }

        private static Config _config;

        public override void OnEnabled()
        {
            _config = base.Config;
            
            Events.Internal.Player.Register();

            EventInfoHint = new Hint
            {
                AutoText = Hints.EventInfoHint.OnRender,
                XCoordinate = Config.EventInfoHint.Position.x,
                YCoordinate = Config.EventInfoHint.Position.y,
                FontSize = Config.EventInfoHint.Size
            };
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Events.Internal.Player.Unregister();
            
            base.OnDisabled();
        }
    }
}