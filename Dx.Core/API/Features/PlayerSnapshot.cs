using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using PlayerRoles;
using UnityEngine;

namespace Dx.Core.API.Features;

public class PlayerSnapshot
{
    public PlayerSnapshot(Player player)
    {
        Role = player.Role.Type;
        Items = player.Items.Select(item => item.Type).ToArray();
        Position = new Vector3(player.Position.x, player.Position.y + player.Transform.localScale.y / 2f,
            player.Position.z);

        var names = Enum.GetValues(typeof(AmmoType)).Cast<AmmoType>();
        
        foreach (var name in names)
        {
            _ammo.Add(name, player.GetAmmo(name));
        }
    }
    
    public RoleTypeId Role { get; }

    public ItemType[] Items { get; }
    
    public Vector3 Position { get; }

    public IReadOnlyDictionary<AmmoType, ushort> Ammo => _ammo;

    private readonly Dictionary<AmmoType, ushort> _ammo = new();
}