using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;

namespace Dx.NoRules
{
    public class Plugin : Plugin<Config>
    {
        public override void OnEnabled()
        {
            API.Features.ShootingEffects.Events.Internal.Player.Register();
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            API.Features.ShootingEffects.Events.Internal.Player.Unregister();
            
            base.OnDisabled();
        }
    }
}