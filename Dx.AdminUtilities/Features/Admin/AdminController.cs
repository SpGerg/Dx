using System;
using Dx.Core.API.Features.Webhooks.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace Dx.AdminUtilities.Features.Admin;

public class AdminController : MonoBehaviour
{
    public Player Player { get; private set; }
    
    public int CountOfRecentBans { get; private set; }
    
    public DateTime LastBanTime { get; private set; } = DateTime.Now;

    public DateTime LastTime { get; private set; } = DateTime.Now;
    
    //Из-за stackoverflow
    public bool IsAlreadyBanned { get; private set; }

    public void Awake()
    {
        Player = Player.Get(gameObject);
    }

    public void Verified()
    {
        LastTime = DateTime.Now;
    }

    public void UpdateTime()
    {
        var profile = Plugin.AdminRepository.Get(new AdminProfile()
        {
            Username = Player.Nickname,
            UserId = Player.UserId
        });

        profile.ModeratedTime += DateTime.Now - LastTime;
        
        Plugin.AdminRepository.Update(profile);
        
        LastTime = DateTime.Now;
    }
    
    public void Ban()
    {
        CountOfRecentBans++;
        
        if ((DateTime.Now - LastBanTime).TotalSeconds > Plugin.Config.TimeBetweenBans)
        {
            CountOfRecentBans = 0;
        }

        if (CountOfRecentBans > Plugin.Config.BansCountBeforeAdminBan)
        {
            Player.Ban(int.MaxValue, "Вы забанены за подозрение в бан рейде! Обратитесть к администратору за аппеляцией.");

            IsAlreadyBanned = true;
            
            var message = Plugin.Config.AdminBannedMessage
                .Replace("%nick%", Player.Nickname)
                .Replace("%steam%", Player.UserId)
                .Replace("%ip%", Player.IPAddress);
            
            Core.Plugin.Webhook.Send(Plugin.Config.AdminBannedWebhook, message, WebhookEventType.Error, true);
            
            return;
        }
        
        LastBanTime = DateTime.Now;
    }
}