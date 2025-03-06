using System.Collections.Generic;
using Exiled.API.Features;
using Mirror;
using PlayerRoles;
using SCPSLAudioApi.AudioCore;
using UnityEngine;

namespace Dx.Core.API.Features.Audio;

public abstract class AudioBot
{
    static AudioBot()
    {
        AudioPlayerBase.OnFinishedTrack += HideNpcAfterEnding;
    }

    public static void DisconnectAllDummies()
    {
        foreach (var dummy in _audioBots)
        {
            dummy.Value.Disconnect();
        }
        
        _audioBots.Clear();
    }

    private static void HideNpcAfterEnding(AudioPlayerBase audioPlayerBase, string track, bool directPlay, ref int nextQueue)
    {
        if (!_audioBots.ContainsKey(audioPlayerBase.Owner.PlayerId))
        {
            return;
        }
        
        audioPlayerBase.Owner.roleManager.ServerSetRole(RoleTypeId.None, RoleChangeReason.None);
    }
    
    public static Dictionary<int, AudioBot> AudioBots => _audioBots;

    private static readonly Dictionary<int, AudioBot> _audioBots = new();

    protected AudioBot(AudioSettings audioSettings)
    {
        AudioSettings = audioSettings;

        Npc = Npc.Spawn(AudioSettings.Name);
        Npc.Id = (_audioBots.Count + Player.List.Count) * 2;
        
        _audioBots.Add(Npc.Id, this);
        
        Base = AudioPlayerBase.Get(Npc.ReferenceHub);
    }

    public AudioPlayerBase Base { get; }
    
    public Npc Npc { get; }

    public AudioSettings AudioSettings { get; }

    public abstract void Play(Vector3 position, Quaternion? quaternion = null, Vector3? scale = null, RoleTypeId roleTypeId = RoleTypeId.Tutorial);
    
    public abstract bool Stop();

    public void Disconnect()
    {
        NetworkServer.Destroy(Npc.GameObject);
    }
}