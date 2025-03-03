using Dx.Core.API.Extensions;
using Exiled.Events.EventArgs.Player;
using EventTarget = Exiled.Events.Handlers.Player;

namespace Dx.AdminUtilities.Features.Admin.Events.Internal;

internal static class Player
{
    public static void Register()
    {
        EventTarget.Banned += CheckBanRaidOnBanning;
        EventTarget.Verified += AddComponentOnVerified;
        EventTarget.Left += AddModeratedTimeOnLeft;
    }
    
    public static void Unregister()
    {
        EventTarget.Banned -= CheckBanRaidOnBanning;
        EventTarget.Verified -= AddComponentOnVerified;
        EventTarget.Left -= AddModeratedTimeOnLeft;
    }

    private static void CheckBanRaidOnBanning(BannedEventArgs ev)
    {
        var adminController = ev.Player.GetOrAddComponent<AdminController>();

        if (adminController.IsAlreadyBanned)
        {
            return;
        }
        
        adminController.Ban();
    }

    private static void AddComponentOnVerified(VerifiedEventArgs ev)
    {
        if (!ev.Player.IsAdministrator())
        {
            return;
        }

        ev.Player.GetOrAddComponent<AdminController>().Verified();
    }

    private static void AddModeratedTimeOnLeft(LeftEventArgs ev)
    {
        if (!ev.Player.IsAdministrator())
        {
            return;
        }

        ev.Player.GetOrAddComponent<AdminController>().UpdateTime();
    }
}