using System.Collections.Generic;
using Exiled.API.Features;
using HintServiceMeow.Core.Models.HintContent;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;

namespace Dx.NoRules.API.Features.ProximityChat
{
    public class ProximityChat
    {
        public Dictionary<Player, bool> Toggled { get; } = new();

        public const string HintsId = "proximity-voice-toggle";

        public Hint VoiceEnabled { get; } = new()
        {
            Id = HintsId,
            Content = new StringContent(Plugin.Config.VoiceEnabledHint.Text),
            XCoordinate = Plugin.Config.VoiceEnabledHint.Position.x,
            YCoordinate = Plugin.Config.VoiceEnabledHint.Position.y,
            FontSize = Plugin.Config.VoiceEnabledHint.Size
        };
        
        public Hint VoiceDisabled { get; } = new()
        {
            Id = HintsId,
            Content = new StringContent(Plugin.Config.VoiceDisabledHint.Text),
            XCoordinate = Plugin.Config.VoiceDisabledHint.Position.x,
            YCoordinate = Plugin.Config.VoiceDisabledHint.Position.y,
            FontSize = Plugin.Config.VoiceDisabledHint.Size
        };
    }
}