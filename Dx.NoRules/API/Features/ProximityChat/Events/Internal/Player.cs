using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using HintServiceMeow.Core.Utilities;
using PlayerRoles.Spectating;
using UnityEngine;
using VoiceChat;
using VoiceChat.Networking;
using EventTarget = Exiled.Events.Handlers.Player;
using SpectatorRole = Exiled.API.Features.Roles.SpectatorRole;

namespace Dx.NoRules.API.Features.ProximityChat.Events.Internal
{
    internal static class Player
    {
        public static void Register()
        {
            EventTarget.TogglingNoClip += ToggleVoiceOnTogglingNoСlip;
            EventTarget.VoiceChatting += SendProximityMessageOnVoiceChatting;
        }

        public static void Unregister()
        {
            EventTarget.TogglingNoClip -= ToggleVoiceOnTogglingNoСlip;
            EventTarget.VoiceChatting -= SendProximityMessageOnVoiceChatting;
        }

        private static void ToggleVoiceOnTogglingNoСlip(TogglingNoClipEventArgs ev)
        {
            if (ev.Player.IsNoclipPermitted)
            {
                return;
            }

            if (!Plugin.Config.ProximityChatRoles.Contains(ev.Player.Role.Type))
            {
                return;
            }

            var isEnabled = true;
            
            if (Plugin.ProximityChat.Toggled.TryGetValue(ev.Player, out var toggle))
            {
                Plugin.ProximityChat.Toggled[ev.Player] = isEnabled = !toggle;
            }
            else
            {
                Plugin.ProximityChat.Toggled.Add(ev.Player, true);
            }

            var playerDisplay = PlayerDisplay.Get(ev.Player);
            playerDisplay.RemoveHint(ProximityChat.HintsId);
            
            if (isEnabled)
            {
                playerDisplay.AddHint(Plugin.ProximityChat.VoiceEnabled);

                Plugin.Coroutines.CallDelayed(Plugin.Config.VoiceEnabledHint.Duration, () =>
                {
                    playerDisplay.RemoveHint(Plugin.ProximityChat.VoiceEnabled);
                });
            }
            else
            {
                playerDisplay.AddHint(Plugin.ProximityChat.VoiceDisabled);

                Plugin.Coroutines.CallDelayed(Plugin.Config.VoiceDisabledHint.Duration, () =>
                {
                    playerDisplay.RemoveHint(Plugin.ProximityChat.VoiceDisabled);
                });
            }
        }

        private static void SendProximityMessageOnVoiceChatting(VoiceChattingEventArgs ev)
        {
            if (ev.VoiceMessage.Channel != VoiceChatChannel.ScpChat)
                return;
        
            if (!ReferenceHub.TryGetHubNetID(ev.Player.Connection.identity.netId, out var player))
                return;
        
            if (!Plugin.Config.ProximityChatRoles.Contains(player.roleManager.CurrentRole.RoleTypeId) || !Plugin.ProximityChat.Toggled.ContainsKey(ev.Player))
                return;
        
            SendProximityMessage(ev.VoiceMessage);
        }
        
        private static void SendProximityMessage(VoiceMessage msg)
        {
            foreach (var player in Exiled.API.Features.Player.List)
            {
                if (player.Role is SpectatorRole && !msg.Speaker.IsSpectatedBy(player.ReferenceHub))
                    continue;
                
                if (player.Role is not IVoiceRole voiceRole2)
                    continue;
            
                if (Vector3.Distance(msg.Speaker.transform.position, player.Position) >= Plugin.Config.ProximityChatDistance)
                    continue;

                if (voiceRole2.VoiceModule.ValidateReceive(msg.Speaker, VoiceChatChannel.Proximity) is VoiceChatChannel.None)
                    continue;
            
                msg.Channel = VoiceChatChannel.Proximity;
                player.Connection.Send(msg);
            }
        }
    }
}