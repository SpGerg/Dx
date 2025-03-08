using System.Collections.Generic;
using Exiled.API.Features;
using Mirror;
using PlayerRoles;
using SCPSLAudioApi;
using SCPSLAudioApi.AudioCore;
using UnityEngine;

namespace Dx.Core.API.Features.Audio;

public abstract class AudioBot
{
    static AudioBot()
    {
        Startup.SetupDependencies();

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
        
        audioPlayerBase.Owner.roleManager.ServerSetRole(RoleTypeId.Overwatch, RoleChangeReason.None);
    }
    
    public static Dictionary<int, AudioBot> AudioBots => _audioBots;

    internal static readonly Dictionary<int, AudioBot> _audioBots = new();

    protected AudioBot(AudioSettings audioSettings)
    {
        AudioSettings = audioSettings;
    }
    
    public AudioPlayerBase Base { get; protected set; }
    
    public Npc Npc { get; protected set; }

    public AudioSettings AudioSettings { get; set; }

    public abstract void Play(Vector3 position, Quaternion? quaternion = null, Vector3? scale = null, RoleTypeId roleTypeId = RoleTypeId.Tutorial);
    
    public abstract bool Stop();

    public void Disconnect()
    {
        if (Npc is null)
        {
            return;
        }
        
        _audioBots.Remove(Npc.Id);
        
        NetworkServer.Destroy(Npc.GameObject);
        Base.Stoptrack(true);

        Npc = null;
        Base = null;
    }
}