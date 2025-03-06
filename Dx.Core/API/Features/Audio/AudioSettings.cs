using System.ComponentModel;

namespace Dx.Core.API.Features.Audio;

public class AudioSettings
{
    [Description("Имя бота")]
    public string Name { get; set; }
    
    [Description("Путь к файлу")]
    public string Filepath { get; set; }
    
    [Description("Зациклено ли")]
    public bool IsLoop { get; set; }
    
    [Description("Говорить в интерком")]
    public bool IsIntercom { get; set; }
    
    [Description("Громкость")]
    public float Volume { get; set; }
}