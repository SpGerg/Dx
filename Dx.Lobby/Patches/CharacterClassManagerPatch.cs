using Exiled.API.Features;
using HarmonyLib;
using PlayerRoles;

namespace Dx.Lobby.Patches
{
    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.ForceRoundStart))]
    public static class CharacterClassManagerPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(CharacterClassManager __instance)
        {
            foreach (var player in Player.List)
            {
                player.Role.Set(RoleTypeId.Spectator);
            }
            
            return true;
        }
    }
}