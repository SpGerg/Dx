using EventTarget = Exiled.Events.Handlers.Server;

namespace Dx.Core.API.Features.Audio.Events.Internal;

internal static class Server
{
    public static void Register()
    {
        EventTarget.WaitingForPlayers += DisconnectAllDummiesOnWaitingForPlayers;
    }

    public static void Unregister()
    {
        EventTarget.WaitingForPlayers -= DisconnectAllDummiesOnWaitingForPlayers;
    }

    private static void DisconnectAllDummiesOnWaitingForPlayers()
    {
        AudioBot.DisconnectAllDummies();
    }
}