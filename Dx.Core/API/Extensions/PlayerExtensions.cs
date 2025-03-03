using System.Collections.Generic;
using System.Linq;
using Dx.Core.API.Features;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using UnityEngine;

namespace Dx.Core.API.Extensions;

public static class PlayerExtensions
{
    public static void SetSnapshot(this Player player, PlayerSnapshot playerSnapshot)
    {
        player.Role.Set(playerSnapshot.Role, SpawnReason.ForceClass, RoleSpawnFlags.None);

        player.Position = playerSnapshot.Position;
        player.AddItem(playerSnapshot.Items);

        foreach(var ammo in playerSnapshot.Ammo)
        {
            player.SetAmmo(ammo.Key, ammo.Value);
        }
    }

    public static bool IsAdministrator(this Player player)
    {
        return Plugin.Instance.Config.AdminGroups.Contains(player.GroupName);
    }
    
    public static IEnumerable<Player> GetAdministrators(this IEnumerable<Player> players)
    {
        return players.Where(player => player.IsAdministrator());
    }

    public static T GetOrAddComponent<T>(this Player player) where T : MonoBehaviour
    {
        return player.GameObject.TryGetComponent<T>(out var component) ? component : player.GameObject.AddComponent<T>();
    }
}