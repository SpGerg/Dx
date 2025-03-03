using System.ComponentModel;
using Exiled.API.Interfaces;

namespace Dx.Core;

public class Config : IConfig
{
    [Description("Включен или нет")]
    public bool IsEnabled { get; set; }
    
    [Description("Дебаг или нет")]
    public bool Debug { get; set; }
    
    [Description("Группы админов")]
    public string ServerName { get; set; } = "dx_1";

    [Description("Группы админов")]
    public string[] AdminGroups { get; set; } =
    {
        "stajor",
        "admin",
        "stadmin"
    };
}