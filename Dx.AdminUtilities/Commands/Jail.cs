using System;
using CommandSystem;
using Dx.AdminUtilities.Features.Jail;
using Dx.Core.API.Extensions;
using Dx.Core.API.Features.Commands;
using Exiled.API.Features;

namespace Dx.AdminUtilities.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class Jail : CommandBase
{
    public override string Command => "jail";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description => "Отправляет игрока в башню";

    public override string[] Permissions { get; } =
    {
        "admin-utils.jail"
    };

    public override CommandParameter[] Parameters { get; } =
    {
        new()
        {
            Name = "id",
            DisplayName = "айди",
            IsRequired = true
        }
    };

    public override CommandResponse Execute(CommandContext context)
    {
        if (!context.TryGetPlayersOrPlayer(context.Get("id"), out var players))
        {
            return new CommandResponse
            {
                Response = "Игрок(и) не найден.",
                Success = false
            };
        }

        foreach (var player in players)
        {
            var jailController = player.GetOrAddComponent<JailController>();

            jailController.Toggle();
        }
        
        return CommandResponse.Successful;
    }
}