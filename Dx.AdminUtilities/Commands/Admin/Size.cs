using System;
using CommandSystem;
using Dx.Core.API.Features.Commands;

namespace Dx.AdminUtilities.Commands.Admin;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class Size : CommandBase
{
    public override string Command => "size";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description => "Изменяет размер игрока";

    public override CommandParameter[] Parameters { get; } =
    {
        new()
        {
            Name = "id",
            DisplayName = "айди",
            IsRequired = true
        },
        new()
        {
            Name = "x",
            DisplayName = "x",
            IsRequired = true
        },
        new()
        {
            Name = "y",
            DisplayName = "y",
            IsRequired = true
        },
        new()
        {
            Name = "z",
            DisplayName = "z",
            IsRequired = true
        },
    };
    
    public override string[] Permissions { get; } =
    {
        "admin-utils.size"
    };

    public override CommandResponse Execute(CommandContext context)
    {
        if (!context.TryGetPlayersOrPlayer(context.Get("id"), out var players))
        {
            return new CommandResponse
            {
                Response = "Игрок не найден.",
                Success = false
            };
        }

        if (!context.TryParseVector3(1, out var size))
        {
            return new CommandResponse
            {
                Response = "Неверно указаны координаты.",
                Success = false
            };
        }

        foreach (var player in players)
        {
            player.Scale = size;
        }

        return CommandResponse.Successful;
    }
}