using System;
using Newtonsoft.Json;

namespace Dx.AdminUtilities.Features.Admin;

public class AdminProfile
{
    [JsonProperty("user_id")]
    public string UserId { get; set; }
    
    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("moderated_time")]
    public DateTime ModeratedTime { get; set; }

    public override string ToString()
    {
        var timeSpan = ModeratedTime.TimeOfDay;
        
        return $"{Username} ({UserId}): {timeSpan.Days} дней, {timeSpan.Hours} часов, {timeSpan.Minutes} минут";
    }
}