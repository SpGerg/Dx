using System.ComponentModel;
using Exiled.API.Features;

namespace Dx.Core.API.Features;

public class CassieMessage
{
    [Description("Сообщение")]
    public string Message { get; set; }
    
    [Description("Перевод")]
    public string Translation { get; set; }
    
    [Description("С задержкой ли")]
    public bool IsHeld { get; set; }
    
    [Description("Шумный ли голос")]
    public bool IsNoisy { get; set; }
    
    [Description("С субтитрами ли")]
    public bool IsSubtitles { get; set; }

    public void Speak()
    {
        if (string.IsNullOrEmpty(Message))
        {
            return;
        }
        
        Cassie.MessageTranslated(Message, Translation, IsHeld, IsNoisy, IsSubtitles);
    }
}