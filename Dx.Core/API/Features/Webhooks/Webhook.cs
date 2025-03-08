using System;
using System.Net.Http;
using System.Text;
using Dx.Core.API.Extensions;
using Dx.Core.API.Features.Webhooks.Enums;
using Exiled.API.Features;
using Newtonsoft.Json;

namespace Dx.Core.API.Features.Webhooks;

public class Webhook
{
    private readonly HttpClient _httpClient = new();
    
    public void Send(string webhookUrl, string message, WebhookEventType eventType, bool pingAll = false)
    {
        if (string.IsNullOrEmpty(webhookUrl) || string.IsNullOrWhiteSpace(webhookUrl))
        {
            return;
        }
        
        var content = new StringContent(JsonConvert.SerializeObject(new
        {
            content = pingAll ? "@everyone" : "",
            embeds = new[]
            {
                new
                {
                    title = eventType.ToString(),
                    description = message,
                    color = eventType.ToColor()
                }
            }
        }), Encoding.UTF8, "application/json");

        try
        {
            _httpClient.PostAsync(webhookUrl, content);
        }
        catch (Exception exception)
        {
            Log.Error($"Не удалось отправить вебхук '{webhookUrl}', с сообщением '{message}'. {exception}");
        }
    }
    
    public void Send(WebhookMessage webhookMessage)
    {
        if (string.IsNullOrEmpty(webhookMessage.Url) || string.IsNullOrWhiteSpace(webhookMessage.Url))
        {
            return;
        }
        
        var content = new StringContent(JsonConvert.SerializeObject(new
        {
            content = webhookMessage.IsPingAll ? "@everyone" : "",
            embeds = new[]
            {
                new
                {
                    title = webhookMessage.Title,
                    description = webhookMessage.Message,
                    color = webhookMessage.Color.HexColorToInteger()
                }
            }
        }), Encoding.UTF8, "application/json");

        try
        {
            _httpClient.PostAsync(webhookMessage.Url, content);
        }
        catch (Exception exception)
        {
            Log.Error($"Не удалось отправить вебхук '{webhookMessage.Url}', с сообщением '{webhookMessage.Message}'. {exception}");
        }
    }
}