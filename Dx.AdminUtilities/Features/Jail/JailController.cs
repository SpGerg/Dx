using System;
using System.Collections.Generic;
using System.Linq;
using Dx.AdminUtilities.Features.Jail.Enums;
using Dx.Core.API.Extensions;
using Dx.Core.API.Features;
using Exiled.API.Features;
using PlayerRoles;
using UnityEngine;

namespace Dx.AdminUtilities.Features.Jail;

public class JailController : MonoBehaviour
{
    public Player Player { get; private set; }
    
    public PlayerSnapshot Snapshot { get; private set; }
    
    public bool IsJailed { get; private set; }

    private JailPlaceType _jail = JailPlaceType.None;

    private static List<JailPlaceType> _busiedJails = new();
    
    public void Awake()
    {
        Player = Player.Get(gameObject);
    }

    public void Toggle()
    {
        if (IsJailed)
        {
            UnJail();
        }
        else
        {
            Jail();
        }
    }
    
    public void Jail()
    {
        if (IsJailed)
        {
            return;
        }

        Snapshot = new PlayerSnapshot(Player);

        var position = Vector3.zero;

        var freeJails = Plugin.Config.Jails.Where(jail => !_busiedJails.Contains(jail.Key));
        
        foreach (var jail in freeJails)
        {
            _jail = jail.Key;
            position = jail.Value;
            
            break;
        }

        if (_jail == JailPlaceType.None)
        {
            _jail = _busiedJails.RandomItem();
            position = Plugin.Config.Jails[_jail];
        }
        
        _busiedJails.Add(_jail);

        Player.Role.Set(RoleTypeId.Tutorial);
        Player.ClearInventory();

        Player.Position = position;
        
        Player.Broadcast(5, Plugin.Config.OnJailMessage);

        IsJailed = true;
    }

    public void UnJail()
    {
        if (!IsJailed)
        {
            return;
        }
        
        Player.SetSnapshot(Snapshot);

        _busiedJails.Remove(_jail);

        _jail = JailPlaceType.None;
        
        IsJailed = false;
    }
}