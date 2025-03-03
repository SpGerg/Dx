using System;
using CommandSystem;
using Dx.AdminUtilities.Features.Admin;
using Dx.Core.API.Features.Commands;
using Exiled.API.Features.Pools;

namespace Dx.AdminUtilities.Commands.Admin;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class Norma : CommandBase
{
    public override string Command => "norma";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description => "Показывает отыграннаю норму администратора";

    public override CommandParameter[] Parameters { get; } =
    {
        new()
        {
            Name = "steam_id",
            DisplayName = "айди"
        }
    };
    
    public override string[] Permissions { get; } =
    {
        "admin-utils.norma"
    };
    
    public override CommandResponse Execute(CommandContext context)
    {
        if (!context.TryGet("steam_id", out var steamId))
        {
            return new CommandResponse
            {
                Response = Plugin.AdminRepository.Get(new AdminProfile
                {
                    Username = string.Empty,
                    UserId = steamId
                }).ToString(),
                Success = true
            };
        }

        var stringBuilder = StringBuilderPool.Pool.Get();
        stringBuilder.AppendLine();
        
        foreach (var profile in Plugin.AdminRepository.Entities)
        {
            stringBuilder.AppendLine(profile.ToString());
        }
        
        return new CommandResponse
        {
            Response = StringBuilderPool.Pool.ToStringReturn(stringBuilder),
            Success = true
        };
    }
}