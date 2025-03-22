using EventTarget = Exiled.Events.Handlers.Server;

namespace Dx.ProximityChat.Events.Internal
{
    internal static class Server
    {
        public static void Register()
        {
            EventTarget.WaitingForPlayers += ClearCoroutinesOnWaitingForPlayers;
        }

        public static void Unregister()
        {
            EventTarget.WaitingForPlayers -= ClearCoroutinesOnWaitingForPlayers;
        }

        private static void ClearCoroutinesOnWaitingForPlayers()
        {
            Plugin.Coroutines.Clear();
        }
    }
}