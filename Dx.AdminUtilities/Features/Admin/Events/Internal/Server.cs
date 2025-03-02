using Dx.Core.API.Extensions;
using Exiled.Events.EventArgs.Server;
using EventTarget = Exiled.Events.Handlers.Server;

namespace Dx.AdminUtilities.Features.Admin.Events.Internal;

internal static class Server
{
    public static void Register()
    {
        EventTarget.RoundEnded += SaveRepositoryOnRoundEnded;
    }
    
    public static void Unregister()
    {
        EventTarget.RoundEnded -= SaveRepositoryOnRoundEnded;
    }
    
    internal static void SaveRepositoryOnRoundEnded(RoundEndedEventArgs ev)
    {
        foreach (var player in Exiled.API.Features.Player.List)
        {
            if (!player.IsAdministrator())
            {
                continue;
            }
            
            var controller = player.GetOrAddComponent<AdminController>();
            controller.UpdateTime();
        }
        
        Plugin.AdminRepository.Save();
    }
}