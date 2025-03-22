using Dx.Core.API.Features;
using Exiled.API.Features;

namespace Dx.ProximityChat
{
    public class Plugin : Plugin<Config>
    {
        public static Coroutines Coroutines { get; private set; }
        
        public static API.Features.ProximityChat.ProximityChat ProximityChat { get; private set; }
        
        public static Config Config { get; private set; }

        public override void OnEnabled()
        {
            Config = base.Config;
            
            Events.Internal.Server.Register();
            API.Features.ProximityChat.Events.Internal.Player.Register();
            
            Coroutines = new Coroutines();
            ProximityChat = new API.Features.ProximityChat.ProximityChat();

            Config = base.Config;
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Events.Internal.Server.Unregister();
            API.Features.ProximityChat.Events.Internal.Player.Unregister();
            
            ProximityChat.Toggled.Clear();
            
            base.OnDisabled();
        }
    }
}