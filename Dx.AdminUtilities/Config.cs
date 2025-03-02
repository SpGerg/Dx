using System.Collections.Generic;
using System.ComponentModel;
using Dx.AdminUtilities.Features.Jail.Enums;
using Exiled.API.Interfaces;
using UnityEngine;

namespace Dx.AdminUtilities;

public class Config : IConfig
{
    [Description("Включен или не включен")]
    public bool IsEnabled { get; set; }
    
    [Description("Дебаг включен или не включен")]
    public bool Debug { get; set; }

    [Description("Места тюрем")]
    public Dictionary<JailPlaceType, Vector3> Jails { get; set; } = new()
    {
        { JailPlaceType.FirstTower, new Vector3(39.938f, 1014.112f, -32.715f) },
        { JailPlaceType.SecondTower, new Vector3(-15.477f, 1014.461f, -31.523f) },
        { JailPlaceType.ThirdTower, new Vector3 (130.938f, 993.364f, 20.715f) },
        { JailPlaceType.FourthTower, new Vector3(164.938f, 1019.112f, -10.715f) },
    };

    [Description("Дней перед отправкой нормы")]
    public int DaysBeforeSendNorm { get; set; } = 7;
    
    [Description("Вебхук для отправки нормы")]
    public string NormWebhook { get; set; }

    [Description("Сообщение если игрока заджейлили")]
    public string OnJailMessage { get; set; } = "Не выходите с сервера. Ожидайте администратора.";

    [Description("Сообщение при бане администратора")]
    public string AdminBannedMessage { get; set; } = "%nick% (%steam%)[%ip%] 📛 был заблокирован по подозрению на рейд!";
    
    [Description("Сообщение при бане администратора")]
    public string AdminBannedWebhook { get; set; }

    [Description("Сообщение при репорте отправителю")]
    public string ReportAnswer { get; set; } = "Ваш репорт принят. Понятно?";
    
    [Description("Сообщение при репорте отправителю")]
    public string ReportMessage { get; set; } = "";

    [Description("Время (сек) между наблюдаемыми банами администраторов")]
    public int TimeBetweenBans { get; set; } = 60;
    
    [Description("Кол-во банов перед баном администратора")]
    public int BansCountBeforeAdminBan { get; set; } = 3;
}