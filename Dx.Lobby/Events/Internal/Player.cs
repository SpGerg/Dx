using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using EventTarget = Exiled.Events.Handlers.Player;

namespace Dx.Lobby.Events.Internal
{
    internal static class Player
    {
        public static void Register()
        {
            EventTarget.Verified += TeleportToSchematicOnVerified;
            EventTarget.PickingUpItem += CancelPickingUpInLobbyOnPickingUpItem;
        }

        public static void Unregister()
        {
            EventTarget.Verified -= TeleportToSchematicOnVerified;
            EventTarget.PickingUpItem -= CancelPickingUpInLobbyOnPickingUpItem;
        }

        private static void CancelPickingUpInLobbyOnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (Round.InProgress)
            {
                return;
            }

            ev.IsAllowed = Plugin.Instance.Config.IsCanPickingUp;
        }

        private static void TeleportToSchematicOnVerified(VerifiedEventArgs ev)
        {
            if (Round.InProgress)
            {
                return;
            }

            var player = ev.Player;

            player.Role.Set(Plugin.Instance.Config.LobbyRole);
            player.IsGodModeEnabled = true;
            
            player.Teleport(Plugin.SelectedSchematic.SpawnPosition);
            
        }
    }
}