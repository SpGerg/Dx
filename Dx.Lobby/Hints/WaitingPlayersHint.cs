using System;
using System.Linq;
using Exiled.API.Features;
using HintServiceMeow.Core.Models.Hints;
using PlayerRoles;

namespace Dx.Lobby.Hints
{
    public static class WaitingPlayersHint
    {
        private static string Text => Plugin.Config.WaitingPlayersHint.Text;
        
        public static string OnRender(AbstractHint.TextUpdateArg ev)
        {
            if (!Round.IsLobby)
                return string.Empty;
            
            var count = Player.List.Count(player => player != null && player.IsConnected && player.Role.Type != RoleTypeId.Spectator);

            return Text.Replace("%count%", count.ToString());
        }
    }
}