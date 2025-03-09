using System;
using System.Linq;
using CommandSystem;
using Dx.Core.API.Features.Commands;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using PlayerRoles;
using UnityEngine;

namespace Dx.NoRules.Commands.User
{
    //[CommandHandler(typeof(ClientCommandHandler))]
    public class Take : CommandBase
    {
        public override string Command => "take";

        public override string[] Aliases { get; } = Array.Empty<string>();

        public override string Description => "Позволяет подобрать карту в руки";

        public override CommandParameter[] Parameters { get; } = Array.Empty<CommandParameter>();

        private const float _maxDistance = 5;

        public override CommandResponse Execute(CommandContext context)
        {
            if (!Player.TryGet(context.CommandSender, out var player))
            {
                return new CommandResponse
                {
                    Response = "Вы должны быть игроком",
                    Success = false
                };
            }

            if (player.Role.Type is not RoleTypeId.Scp049)
            {
                return new CommandResponse
                {
                    Response = "Вы должны быть Scp-049",
                    Success = false
                };
            }

            var pickup = Pickup.List.Where(pickup => pickup.Type.IsKeycard()).OrderBy(keycard => Vector3.Distance(keycard.Position, player.Position)).FirstOrDefault();
            
            if (pickup == default || Vector3.Distance(pickup.Position, player.Position) > _maxDistance)
            {
                return new CommandResponse
                {
                    Response = "Не найдено ближайщей карты",
                    Success = false
                };
            }

            if (player.CurrentItem is not null)
            {
                player.DropItem(player.CurrentItem);
            }

            player.CurrentItem = Item.Get(pickup.Serial);
            pickup.Destroy();

            return CommandResponse.Successful;
        }
    }
}