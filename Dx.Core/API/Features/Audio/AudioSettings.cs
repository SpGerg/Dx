using System.ComponentModel;
using VoiceChat;

namespace Dx.Core.API.Features.Audio;

public class AudioSettings
{
    [Description("Имя бота")]
    public string Name { get; set; } = "Audio bot";
    
    [Description("Путь к файлу")]
    public string Filepath { get; set; }
    
    [Description("Зациклено ли")]
    public bool IsLoop { get; set; }
    
    [Description("Каналы")]
    public VoiceChatChannel Channels { get; set; }

    [Description("Громкость")] 
    public float Volume { get; set; } = 100;
}