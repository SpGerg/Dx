using Dx.Core.API.Extensions;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using EventTarget = Exiled.Events.Handlers.Player;

namespace Dx.AdminUtilities.Events.Internal;

internal static class Player
{
    public static void Register()
    {
        EventTarget.Banning += ShowEffectOnBanning;
        EventTarget.Kicking += ShowEffectOnKicking;
    }

    public static void Unregister()
    {
        EventTarget.Banning -= ShowEffectOnBanning;
        EventTarget.Kicking -= ShowEffectOnKicking;
    }

    /// <summary>
    /// Показать эффект от молекуляр. разрушителя при бане
    /// </summary>
    /// <param name="ev"></param>
    private static void ShowEffectOnBanning(BanningEventArgs ev)
    {
        ev.Target.Vaporize();
    }
    
    /// <summary>
    /// Показать эффект от молекуляр. разрушителя при бане
    /// </summary>
    /// <param name="ev"></param>
    private static void ShowEffectOnKicking(KickingEventArgs ev)
    {
        ev.Target.Vaporize();
    }
}