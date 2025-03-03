using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using HintServiceMeow.Core.Utilities;
using UnityEngine;
using EventTarget = Exiled.Events.Handlers.Player;

namespace Dx.Lobby.Events.Internal
{
    internal static class Player
    {
        public static void Register()
        {
            EventTarget.Verified += TeleportToLobbyOnVerified;
            EventTarget.PickingUpItem += CancelPickingUpInLobbyOnPickingUpItem;
        }

        public static void Unregister()
        {
            EventTarget.Verified -= TeleportToLobbyOnVerified;
            EventTarget.PickingUpItem -= CancelPickingUpInLobbyOnPickingUpItem;
        }

        private static void CancelPickingUpInLobbyOnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if (Round.InProgress)
            {
                return;
            }

            ev.IsAllowed = Plugin.Config.IsCanPickingUp;
        }

        private static void TeleportToLobbyOnVerified(VerifiedEventArgs ev)
        {
            if (Round.InProgress)
            {
                return;
            }

            var player = ev.Player;

            player.Role.Set(Plugin.Config.LobbyRole);
            player.IsGodModeEnabled = true;

            Vector3 position;
            
            if (Plugin.IsUsingSchematic)
            {
                position = Plugin.SelectedSchematic.SpawnPosition;
            }
            else
            {
                var room = Room.Get(Plugin.Config.SpawnRoomType);

                foreach (var door in room.Doors)
                {
                    door.Lock(DoorLockType.AdminCommand);
                }

                position = room.WorldPosition(Plugin.Config.RoomOffset);
            }

            player.Teleport(position);

            var playerDisplay = PlayerDisplay.Get(player);
            
            foreach (var hint in Plugin.Hints)
            {
                playerDisplay.AddHint(hint);
            }
        }
    }
}