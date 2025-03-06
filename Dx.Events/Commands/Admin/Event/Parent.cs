using System;
using CommandSystem;
using Dx.Core.API.Features.Commands;

namespace Dx.Events.Commands.Admin.Event
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Parent : ParentCommandBase<CommandBase>
    {
        public override string Command => "event";

        public override string[] Aliases { get; } = Array.Empty<string>();

        public override string Description => "Отправляет вебхук";

        public override CommandBase[] Children { get; } =
        {
            new Start(),
            new Stop()
        };
    }
}