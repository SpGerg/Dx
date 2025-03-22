using Dx.Core.API.Features;
using Dx.Scp575.API.Features.CustomRoles;
using Exiled.API.Features;
using Exiled.CustomRoles.API;

namespace Dx.Scp575
{
    public class Plugin : Plugin<Config>
    {
        public static Coroutines Coroutines { get; private set; } = new();
        
        public static Config Config { get; private set; }

        private readonly Scp575Role _scp575Role = new();

        public override void OnEnabled()
        {
            Config = base.Config;
            
            Events.Internal.Server.Register();
            
            _scp575Role.Register();
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Events.Internal.Server.Unregister();
            
            _scp575Role.Unregister();
            
            base.OnDisabled();
        }
    }
}