using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Dx.Core.API.Features;
using Dx.Core.API.Features.Commands;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.CustomRoles.API;
using UnityEngine;

namespace Dx.NoRules.Commands.User
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Stalk : CommandBase
    {
        private static readonly Dictionary<string, Cooldown> _cooldowns = new();

        public override string Command => "stalk";

        public override string[] Aliases { get; } = Array.Empty<string>();

        public override string Description => "Телепортирует к ближайшему игроку";

        public override CommandParameter[] Parameters { get; } = Array.Empty<CommandParameter>();

        private const int _percent = 100;
        
        public override CommandResponse Execute(CommandContext context)
        {
            if (!Player.TryGet(context.CommandSender, out var player))
            {
                return new CommandResponse
                {
                    Response = "Вы должны быть в игре",
                    Success = false
                };
            }

            if (player.Role is not Scp106Role scp106Role)
            {
                return new CommandResponse
                {
                    Response = "Вы не Scp-106",
                    Success = false
                };
            }

            if (Plugin.Config.StalkVigorCost > (scp106Role.Vigor * _percent))
            {
                return new CommandResponse
                {
                    Response = $"Недостаточно энергии, требуется: {Plugin.Config.StalkVigorCost}",
                    Success = false
                };
            }

            if (!_cooldowns.TryGetValue(player.UserId, out var cooldown))
            {
                cooldown = new Cooldown(Plugin.Config.StalkCooldown);
                
                _cooldowns.Add(player.UserId, cooldown);
            }

            if (!cooldown.IsReady)
            {
                return new CommandResponse
                {
                    Response = $"Способность на перезарядке, осталось: {(int) cooldown.RemainingTime}",
                    Success = false
                };
            }

            var playersInSameZone = Player.List.Where(target => target.Zone == player.Zone && target.IsHuman);
            var ordered = playersInSameZone.OrderBy(target => Vector3.Distance(target.Position, player.Position));
            var target = ordered.FirstOrDefault();

            if (target == default)
            {
                return new CommandResponse
                {
                    Response = $"Не удалось найти игрока поблизости",
                    Success = false
                };
            }

            scp106Role.Vigor -= Plugin.Config.StalkVigorCost / _percent;
            scp106Role.UsePortal(target.Position);
            
            cooldown.ForceUse();
            
            return CommandResponse.Successful;
        }
    }
}