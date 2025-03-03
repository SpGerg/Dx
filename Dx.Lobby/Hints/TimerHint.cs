using GameCore;
using HintServiceMeow.Core.Models.Hints;

namespace Dx.Lobby.Hints
{
    public static class TimerHint
    {
        private static string Text => Plugin.Config.TimerHint.Text;
        
        public static string OnRender(AbstractHint.TextUpdateArg ev)
        {
            var timer = RoundStart.singleton.NetworkTimer;

            if (timer < 0)
                return string.Empty;

            var minutes = timer / 60;
            var seconds = timer % 60;

            var formatted = $"{minutes:0}:{seconds:00}";
            
            return Text.Replace("%time%", formatted);
        }
    }
}