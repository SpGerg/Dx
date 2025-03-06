using System;
using CommandSystem;
using Dx.Core.API.Features.Commands;

namespace Dx.Events.Commands.Admin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class ConfigsRestart : CommandBase
    {
        public override string Command => "pluginsrestart";

        public override string[] Aliases { get; } =
        {
            "pr"
        };

        public override string Description => "Перезапускает конфиги серверов";

        public override CommandParameter[] Parameters { get; } = Array.Empty<CommandParameter>();
        
        public override CommandResponse Execute(CommandContext context)
        {
            Exiled.Loader.Loader.ReloadPlugins();
            
            return CommandResponse.Successful;
        }
    }
}