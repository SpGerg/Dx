using System;
using Dx.Core.API.Features.Commands;
using Dx.Core.API.Features.Webhooks;

namespace Dx.Events.Commands.Admin.Event
{
    public class Stop : CommandBase
    {
        public override string Command => "stop";

        public override string[] Aliases { get; } = Array.Empty<string>();

        public override string Description => "Отправляет вебхук от том, что ивент закончился";

        public override CommandParameter[] Parameters { get; } = Array.Empty<CommandParameter>();
        
        public override CommandResponse Execute(CommandContext context)
        {
            if (string.IsNullOrEmpty(Plugin.EventName))
            {
                return new CommandResponse
                {
                    Response = "Нету текущего ивента",
                    Success = false
                };
            }
            
            var webhookMessage = new WebhookMessage(Plugin.Config.EventStoppedMessage);
            webhookMessage.Message = webhookMessage.Message
                .Replace("%name%", Plugin.EventName)
                .Replace("%rp_level%", Plugin.EventRpLevel);
            
            Core.Plugin.Webhook.Send(webhookMessage);

            Plugin.EventName = null;
            Plugin.EventRpLevel = null;
            
            return CommandResponse.Successful;
        }
    }
}