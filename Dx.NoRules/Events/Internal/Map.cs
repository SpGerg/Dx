using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Map;
using EventTarget = Exiled.Events.Handlers.Map;

namespace Dx.NoRules.Events.Internal
{
    internal static class Map
    {
        public static void Register()
        {
            EventTarget.PickupAdded += CancelAmmoSpawnOnPickupAdded;
        }

        public static void Unregister()
        {
            EventTarget.PickupAdded -= CancelAmmoSpawnOnPickupAdded;
        }

        /// <summary>
        /// Отменяет спавн коробок патронов
        /// </summary>
        /// <param name="ev"></param>
        private static void CancelAmmoSpawnOnPickupAdded(PickupAddedEventArgs ev)
        {
            if (!Plugin.Config.IsDestroyAmmo || !ev.Pickup.Type.IsAmmo())
            {
                return;
            }
            
            ev.Pickup.Destroy();
        }
    }
}