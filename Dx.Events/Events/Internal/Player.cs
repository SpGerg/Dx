using Exiled.Events.EventArgs.Player;
using HintServiceMeow.Core.Utilities;
using EventTarget = Exiled.Events.Handlers.Player;

namespace Dx.Events.Events.Internal
{
    internal static class Player
    {
        public static void Register()
        {
            EventTarget.Verified += AddInfoHintOnVerified;
        }

        public static void Unregister()
        {
            EventTarget.Verified -= AddInfoHintOnVerified;
        }

        private static void AddInfoHintOnVerified(VerifiedEventArgs ev)
        {
            var playerDisplay = PlayerDisplay.Get(ev.Player);
            playerDisplay.AddHint(Plugin.EventInfoHint);
        }
    }
}