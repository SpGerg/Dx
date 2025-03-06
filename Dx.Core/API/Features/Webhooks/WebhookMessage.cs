using System.ComponentModel;

namespace Dx.Core.API.Features.Webhooks;

public class WebhookMessage
{
    public WebhookMessage(WebhookMessage copy)
    {
        Title = copy.Title;
        Message = copy.Message;
        IsPingAll = copy.IsPingAll;
        Color = copy.Color;
        Url = copy.Url;
    }
    
    public WebhookMessage() { }
    
    [Description("Заголовок")]
    public string Title { get; set; }
    
    [Description("Сообщение")]
    public string Message { get; set; }
    
    [Description("Пинговать ли всех")]
    public bool IsPingAll { get; set; }
    
    [Description("Цвет")]
    public string Color { get; set; }
    
    [Description("Вебхук")]
    public string Url { get; set; }
}