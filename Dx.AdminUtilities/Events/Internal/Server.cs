using Dx.Core.API.Extensions;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;

namespace Dx.AdminUtilities.Events.Internal;

using EventTarget = Exiled.Events.Handlers.Server;

internal static class Server
{
    public static void Register()
    {
        EventTarget.ReportingCheater += SendBroadcastToAdminsOnReportingCheater;
        EventTarget.LocalReporting += SendBroadcastToAdminsOnLocalReporting;
    }

    public static void Unregister()
    {
        EventTarget.ReportingCheater -= SendBroadcastToAdminsOnReportingCheater;
        EventTarget.LocalReporting -= SendBroadcastToAdminsOnLocalReporting;
    }
    
        
    /// <summary>
    /// Кинуть броадкаст админам при репорте
    /// </summary>
    /// <param name="ev"></param>
    private static void SendBroadcastToAdminsOnReportingCheater(ReportingCheaterEventArgs ev)
    {
        ev.Player.Broadcast(10, Plugin.Config.ReportAnswer);

        var administrators = Exiled.API.Features.Player.List.GetAdministrators();
        
        foreach (var player in administrators)
        {
            var message = Plugin.Config.ReportMessage
                .Replace("%nickname%", ev.Player.Nickname)
                .Replace("%id%", ev.Player.Id.ToString())
                .Replace("%steam_id%", ev.Player.UserId)
                .Replace("%target_nickname%", ev.Target.Nickname)
                .Replace("%target_id%", ev.Target.Id.ToString())
                .Replace("%target_steam_id%", ev.Target.UserId)
                .Replace("%reason%", ev.Reason);
            
            player.Broadcast(10, message);
        }
    }
    
    /// <summary>
    /// Кинуть броадкаст админам при репорте
    /// </summary>
    /// <param name="ev"></param>
    private static void SendBroadcastToAdminsOnLocalReporting(LocalReportingEventArgs ev)
    {
        ev.Player.Broadcast(10, Plugin.Config.ReportAnswer);

        var administrators = Exiled.API.Features.Player.List.GetAdministrators();
        
        foreach (var player in administrators)
        {
            var message = Plugin.Config.ReportMessage
                .Replace("%nickname%", ev.Player.Nickname)
                .Replace("%id%", ev.Player.Id.ToString())
                .Replace("%steam_id%", ev.Player.UserId)
                .Replace("%target_nickname%", ev.Target.Nickname)
                .Replace("%target_id%", ev.Target.Id.ToString())
                .Replace("%target_steam_id%", ev.Target.UserId)
                .Replace("%reason%", ev.Reason);
            
            player.Broadcast(10, message);
        }
    }
}