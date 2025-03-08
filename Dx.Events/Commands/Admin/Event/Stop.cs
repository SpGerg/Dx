using System;
using Dx.Core.API.Features.Commands;
using Dx.Core.API.Features.Webhooks;
using Exiled.API.Features;
using HintServiceMeow.Core.Utilities;

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
                .Replace("%rp_level%", Plugin.EventRpLevel)
                .Replace("%host%", Plugin.EventHost)
                .Replace("%start_time%", Plugin.EventStartTime.ToString())
                .Replace("%passed_time%", (DateTime.Now - Plugin.EventStartTime).ToString());
            
            Core.Plugin.Webhook.Send(webhookMessage);
            
            foreach (var player in Player.List)
            {
                var playerDisplay = PlayerDisplay.Get(player);
                playerDisplay.RemoveHint(Plugin.EventInfoHint);
            }

            Plugin.EventName = null;
            Plugin.EventRpLevel = null;
            Plugin.EventHost = null;
            Plugin.EventStartTime = null;
            
            return CommandResponse.Successful;
        }
    }
}