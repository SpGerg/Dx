using HintServiceMeow.Core.Models.Hints;
using HintServiceMeow.Core.Utilities;

namespace Dx.Core.API.Extensions
{
    public static class PlayerDisplayExtensions
    {
        /// <summary>
        /// Добавить хинт если его нет
        /// </summary>
        /// <param name="playerDisplay"></param>
        /// <param name="hint"></param>
        public static void AddHintIfNotExists(this PlayerDisplay playerDisplay, Hint hint)
        {
            if (playerDisplay.GetHint(hint.Id) is not null)
            {
                return;
            }
            
            playerDisplay.AddHint(hint);
        }
        
        /// <summary>
        /// Добавить хинт если его нет
        /// </summary>
        /// <param name="playerDisplay"></param>
        /// <param name="hint"></param>
        public static void AddHintOrUpdate(this PlayerDisplay playerDisplay, Hint hint)
        {
            if (playerDisplay.GetHint(hint.Id) is not null)
            {
                return;
            }
            
            playerDisplay.RemoveHint(hint.Id);
            playerDisplay.AddHint(hint);
        }
    }
}