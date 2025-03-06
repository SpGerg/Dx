using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Interactables.Interobjects.DoorUtils;

namespace Dx.NoRules.API.Extensions
{
    public static class PlayerExtensions
    {
        public static bool HasKeycardPermission(this Player player, KeycardPermissions permissions, bool requiresAllPermissions = false)
        {
            return requiresAllPermissions ?
                player.Items.Any(item => item is Keycard keycard && keycard.Base.Permissions.HasFlag(permissions))
                : player.Items.Any(item => item is Keycard keycard && (keycard.Base.Permissions & permissions) != 0);
        }
    }
}