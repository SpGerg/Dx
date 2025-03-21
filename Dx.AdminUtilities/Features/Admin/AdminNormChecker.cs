using System;
using System.IO;
using Dx.Core.API.Features.Webhooks.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Newtonsoft.Json;

namespace Dx.AdminUtilities.Features.Admin;

public class AdminNormChecker
{
    public AdminNormChecker(string daysToSendNormPath)
    {
        _daysToSendNormPath = daysToSendNormPath;
    }

    private readonly string _daysToSendNormPath;

    public void CheckAndSendWebhook()
    {
        if (!File.Exists(_daysToSendNormPath))
        {
            File.WriteAllText(_daysToSendNormPath, JsonConvert.SerializeObject(DateTime.Now));
            
            return;
        }

        var dateTime = (DateTime) JsonConvert.DeserializeObject(File.ReadAllText(_daysToSendNormPath));

        var timeSpan = DateTime.Now - dateTime;
        
        if (timeSpan.TotalDays < Plugin.Config.DaysBeforeSendNorm)
        {
            Log.Info($"Дней до отправки нормы: {Plugin.Config.DaysBeforeSendNorm - timeSpan.TotalDays} дней");
            
            return;
        }
        
        File.WriteAllText(_daysToSendNormPath,  JsonConvert.SerializeObject(DateTime.Now));

        var stringBuilder = StringBuilderPool.Pool.Get();
        
        foreach (var profile in Plugin.AdminRepository.Entities)
        {
            stringBuilder.AppendLine(profile.ToString());
        }

        var content = StringBuilderPool.Pool.ToStringReturn(stringBuilder);
        
        Core.Plugin.Webhook.Send(Plugin.Config.NormWebhook, content, WebhookEventType.Info);

        Plugin.AdminRepository.Clear();
    }
}