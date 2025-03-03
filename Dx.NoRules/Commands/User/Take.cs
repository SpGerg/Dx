using System;
using Dx.Core.API.Features.Commands;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using PlayerRoles;
using UnityEngine;

namespace Dx.NoRules.Commands.User
{
    public class Take : CommandBase
    {
        public override string Command => "take";

        public override string[] Aliases { get; } = Array.Empty<string>();

        public override string Description => "Позволяет подобрать карту в руки";

        public override CommandParameter[] Parameters { get; } = Array.Empty<CommandParameter>();
        
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

            if (!Physics.Raycast(player.Position, player.CameraTransform.forward, out var hit, 3f))
            {
                return new CommandResponse
                {
                    Response = "Вы должны смотреть на карту",
                    Success = false
                };
            }

            var pickup = Pickup.Get(hit.transform.root.gameObject);

            if (!pickup.Type.IsKeycard())
            {
                return new CommandResponse
                {
                    Response = "Предмет на который вы смотрите не карта",
                    Success = false
                };
            }

            player.AddItem(pickup);
            pickup.Destroy();

            return CommandResponse.Successful;
        }
    }
}