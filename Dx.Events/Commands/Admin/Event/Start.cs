using System;
using Dx.Core.API.Features.Commands;
using Dx.Core.API.Features.Webhooks;
using Exiled.API.Features;
using HintServiceMeow.Core.Utilities;

namespace Dx.Events.Commands.Admin.Event
{
    public class Start : CommandBase
    {
        public override string Command => "start";

        public override string[] Aliases { get; } = Array.Empty<string>();

        public override string Description => "Отправляет вебхук о начале ивента";

        public override CommandParameter[] Parameters { get; } =
        {
            new()
            {
                Name = "name",
                DisplayName = "имя",
                IsRequired = true,
            },
            new()
            {
                Name = "rp_level",
                DisplayName = "уровень рп",
                IsRequired = true,
            },
            new()
            {
                Name = "host",
                DisplayName = "проводящий",
                IsRequired = true,
            }
        };
        
        public override CommandResponse Execute(CommandContext context)
        {
            if (!string.IsNullOrEmpty(Plugin.EventName))
            {
                return new CommandResponse
                {
                    Response = $"Уже идёт другой ивент, под именем {Plugin.EventName}",
                    Success = false
                };
            }
            
            Plugin.EventName = context.Get("name");
            Plugin.EventRpLevel = context.Get("rp_level");
            Plugin.EventHost = context.Get("host");
            Plugin.EventStartTime = DateTime.Now;

            var webhookMessage = new WebhookMessage(Plugin.Config.EventStartedMessage);
            webhookMessage.Message = webhookMessage.Message
                .Replace("%name%", Plugin.EventName)
                .Replace("%rp_level%", Plugin.EventRpLevel)
                .Replace("%host%", Plugin.EventHost)
                .Replace("%start_time%", Plugin.EventStartTime.ToString());

            foreach (var player in Player.List)
            {
                var playerDisplay = PlayerDisplay.Get(player);
                playerDisplay.AddHint(Plugin.EventInfoHint);
            }
            
            Core.Plugin.Webhook.Send(webhookMessage);
            
            return CommandResponse.Successful;
        }
    }
}