using Dx.Core.API.Utilities;
using HintServiceMeow.Core.Models.Hints;

namespace Dx.NoRules.API.Features.CustomItems.SniperRifle.Hints
{
    public static class SniperRifleHint
    {
        public static string OnRender(AbstractHint.TextUpdateArg ev)
        {
            var itemBase = ev.Player.inventory.CurInstance;

            if (!SniperRifleItem.Cooldowns.TryGetValue(itemBase, out var cooldown))
            {
                return string.Empty;
            }

            return SliderBuilder.BuildSlider((int) cooldown.RemainingTime, cooldown.CooldownTime, 10);
        }
    }
}