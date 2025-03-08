using System.ComponentModel;
using Dx.Core.API.Features;
using Dx.Core.API.Features.Webhooks;
using Exiled.API.Interfaces;
using UnityEngine;

namespace Dx.Events
{
    public class Config : IConfig
    {
        [Description("Включён или нет")]
        public bool IsEnabled { get; set; }
        
        [Description("Дебаг или нет")]
        public bool Debug { get; set; }

        [Description("Сообщение начала ивента")]
        public WebhookMessage EventStartedMessage { get; set; } = new()
        {
            Title = "Начался ивент",
            Message = "Начался ивент %name%, с уровнем отыгрыша %rp_level% начался",
            Color = "#ff0d00"
        };
        
        [Description("Сообщение начала ивента")]
        public WebhookMessage EventStoppedMessage { get; set; } = new()
        {
            Title = "Ивент закончился",
            Message = "Ивент %name%, с уровнем отыгрыша %rp_level% закончился",
            Color = "#ff0d00"
        };
        
        [Description("Настройки для HUD-хинта, отображающего информацию об ивенте")]
        public HintSettings EventInfoHint { get; set; } = new HintSettings
        {
            Enabled = true,
            Text = "Название ивента: %name%\nОтыгровка: %rp_level%\nПроводящий: %host%\nИвент идёт: %timer%",
            Position = new Vector2(0, 800),
            Size = 25,
            Duration = 1
        };
    }
}