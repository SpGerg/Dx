using Exiled.API.Features;
using HarmonyLib;
using PlayerRoles;

namespace Dx.Lobby.Patches
{
    [HarmonyPatch(typeof(CharacterClassManager), "ForceRoundStart")]
    public static class CharacterClassManagerPatch
    {
        // Token: 0x06000026 RID: 38 RVA: 0x0000267C File Offset: 0x0000087C
        [HarmonyPrefix]
        private static bool Prefix(CharacterClassManager __instance)
        {
            foreach (var player in Player.List)
            {
                player.Role.Set(RoleTypeId.None);
            }
            
            return true;
        }
    }
}