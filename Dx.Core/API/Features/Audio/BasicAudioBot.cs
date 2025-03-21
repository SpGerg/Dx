using System.IO;
using Dx.Core.API.Features.Audio.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Components;
using MEC;
using Mirror;
using PlayerRoles;
using SCPSLAudioApi.AudioCore;
using UnityEngine;
using VoiceChat;

namespace Dx.Core.API.Features.Audio;

public class BasicAudioBot : AudioBot
{
    public BasicAudioBot(AudioSettings audioSettings) : base(audioSettings)
    {
    }
    
    public override void Play(Vector3 position, Quaternion? quaternion = null, Vector3? scale = null,
        RoleTypeId roleTypeId = RoleTypeId.Tutorial)
    {
        if (string.IsNullOrEmpty(AudioSettings.Filepath))
        {
            return;
        }
        
        Disconnect();
        
        Npc = Npc.Spawn(AudioSettings.Name, roleTypeId, true, position);
        Npc.RankName = "Аудио бот";
                
        Base = AudioPlayerBase.Get(Npc.ReferenceHub);
        Base.Loop = AudioSettings.IsLoop;
        Base.Volume = AudioSettings.Volume;
        Base.BroadcastChannel = AudioSettings.Channels;

        if (quaternion.HasValue)
        {
            Npc.Rotation = quaternion.Value;
        }

        if (scale.HasValue)
        {
            Npc.Scale = scale.Value;
        }
        
        _audioBots.Add(Npc.Id, this);
        
        Base.Enqueue(AudioSettings.GetFullPath(), -1);
        Base.Play(0);
        
        Timing.CallDelayed(AudioSettings.GetAudioDuration(), Disconnect);
    }

    public override bool Stop()
    {
        if (Npc.Role.Type is RoleTypeId.None)
        {
            return false;
        }
        
        Base.Stoptrack(true);
        return true;
    }
}