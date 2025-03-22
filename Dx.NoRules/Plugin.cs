using System;
using Dx.Core.API.Features;
using Dx.NoRules.API.Features.CustomItems.SniperRifle;
using Exiled.API.Features;
using Exiled.CustomItems.API;
using Respawning.Waves;

namespace Dx.NoRules
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance => _instance;

        public static Config Config => _config;

        public static Coroutines Coroutines => _coroutines;

        public static TimeBasedWave LastSpawnedWave { get; internal set; }
        
        public static SniperRifleItem SniperRifleItem { get; internal set; }
        
        public static DateTime LastSpawnedTime { get; internal set; }

        private static Plugin _instance;

        private static Coroutines _coroutines;

        private static Config _config;

        public override void OnEnabled()
        {
            _instance = this;
            _config = base.Config;
            _coroutines = new Coroutines();

            SniperRifleItem = new SniperRifleItem();
            SniperRifleItem.Register();

            Events.Internal.Player.Register();
            Events.Internal.Map.Register();
            Events.Internal.Server.Register();

            //API.Features.ShootingEffects.Events.Internal.Player.Register();
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            SniperRifleItem.Unregister();

            Events.Internal.Player.Unregister();
            Events.Internal.Map.Unregister();
            Events.Internal.Server.Unregister();

            //API.Features.ShootingEffects.Events.Internal.Player.Unregister();
            
            base.OnDisabled();
        }
    }
}