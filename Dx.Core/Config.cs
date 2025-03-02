using Exiled.API.Interfaces;

namespace Dx.Core;

public class Config : IConfig
{
    public bool IsEnabled { get; set; }
    
    public bool Debug { get; set; }

    public string[] AdminGroups { get; set; } =
    {
        "stajor",
        "admin",
        "stadmin"
    };
}