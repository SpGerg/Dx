using System;
using Dx.Core.API.Features.Webhooks.Enums;

namespace Dx.Core.API.Extensions;

public static class WebhookExtensions
{
    public static int ToColor(this WebhookEventType webhookEventType)
    {
        return webhookEventType switch
        {
            WebhookEventType.Error => 16711680,
            WebhookEventType.Warning => 16776960,
            WebhookEventType.Info => 16777215,
            _ => throw new ArgumentOutOfRangeException(nameof(WebhookEventType))
        };
    }
}