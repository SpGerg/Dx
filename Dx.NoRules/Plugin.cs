using System;
using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;

namespace Dx.NoRules
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance => _instance;

        public static Config Config => _config;

        private static Plugin _instance;

        private static Config _config;
        
        public override void OnEnabled()
        {
            _instance = this;
            _config = Config;
            
            Events.Internal.Player.Register();
            Events.Internal.Map.Register();
            Events.Internal.Server.Register();
            
            API.Features.ShootingEffects.Events.Internal.Player.Register();
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Events.Internal.Player.Unregister();
            Events.Internal.Map.Unregister();
            Events.Internal.Server.Unregister();
            
            API.Features.ShootingEffects.Events.Internal.Player.Unregister();
            
            base.OnDisabled();
        }
    }
}