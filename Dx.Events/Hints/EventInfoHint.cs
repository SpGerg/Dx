using Exiled.API.Features;
using HintServiceMeow.Core.Models.Hints;
using System;

namespace Dx.Events.Hints
{
    public static class EventInfoHint
    {
        public static string OnRender(AbstractHint.TextUpdateArg ev)
        {
            // Если ивент не запущен, ничего не показываем
            if (string.IsNullOrEmpty(Plugin.EventName))
                return string.Empty;

            if (!Plugin.Config.EventInfoHint.Enabled)
                return string.Empty;

            var player = Player.Get(ev.Player);
            if (player == null)
                return string.Empty;

            // Получаем шаблон из конфига
            var template = Plugin.Config.EventInfoHint.Text;

            // Подставляем
            var name = Plugin.EventName;
            var rpLevel = Plugin.EventRpLevel ?? "???";
            var host = Plugin.EventHost ?? "???";

            var timerString = "00:00";
            if (Plugin.EventStartTime.HasValue)
            {
                var diff = DateTime.Now - Plugin.EventStartTime.Value;
                var minutes = (int)diff.TotalMinutes;
                var seconds = diff.Seconds;
                timerString = $"{minutes:00}:{seconds:00}";
            }

            var final = template
                .Replace("{name}", name)
                .Replace("{rp_level}", rpLevel)
                .Replace("{host}", host)
                .Replace("{timer}", timerString);

            return final;
        }
    }
}