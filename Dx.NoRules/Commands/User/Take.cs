using System;
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
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Take : CommandBase
    {
        public override string Command => "take";

        public override string[] Aliases { get; } = Array.Empty<string>();

        public override string Description => "Позволяет подобрать карту в руки";

        public override CommandParameter[] Parameters { get; } = Array.Empty<CommandParameter>();

        private readonly int _pickupId = LayerMask.GetMask("Pickup");
        
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

            var ray = new Ray(player.CameraTransform.position, player.CameraTransform.forward);
            
            if (!Physics.Raycast(ray, out var hit, 7f, _pickupId))
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