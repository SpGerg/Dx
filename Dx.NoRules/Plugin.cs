using System;
using Dx.Core.API.Features;
using Dx.NoRules.API.Features.CustomRoles;
using Dx.NoRules.API.Features.ProximityChat;
using Exiled.API.Features;
using Exiled.API.Features.Waves;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Respawning.Waves;
using Player = Exiled.Events.Handlers.Player;

namespace Dx.NoRules
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance => _instance;

        public static Config Config => _config;

        public static Coroutines Coroutines => _coroutines;

        public static ProximityChat ProximityChat { get; private set; }
        
        public static TimeBasedWave LastSpawnedWave { get; internal set; }
        
        public static DateTime LastSpawnedTime { get; internal set; }

        private static Plugin _instance;

        private static Coroutines _coroutines;

        private static Config _config;

        private Scp575 _scp575;
        
        public override void OnEnabled()
        {
            _instance = this;
            _config = base.Config;
            _coroutines = new Coroutines();

            ProximityChat = new ProximityChat();

            _scp575 = new Scp575();
            _scp575.Register();
            
            Events.Internal.Player.Register();
            Events.Internal.Map.Register();
            Events.Internal.Server.Register();
            
            API.Features.ProximityChat.Events.Internal.Player.Register();
            
            //API.Features.ShootingEffects.Events.Internal.Player.Register();
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            _scp575.Unregister();
            
            Events.Internal.Player.Unregister();
            Events.Internal.Map.Unregister();
            Events.Internal.Server.Unregister();
            
            API.Features.ProximityChat.Events.Internal.Player.Unregister();
            
            //API.Features.ShootingEffects.Events.Internal.Player.Unregister();
            
            base.OnDisabled();
        }
    }
}