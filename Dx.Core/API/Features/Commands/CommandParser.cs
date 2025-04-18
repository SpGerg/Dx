using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;

namespace Dx.Core.API.Features.Commands;

public static class CommandParser
{
    public static Dictionary<string, string> ParseArguments(CommandParameter[] parameters, string[] arguments)
    {
        if (parameters is null)
        {
            return new Dictionary<string, string>();
        }
        
        var result = new Dictionary<string, string>();

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            
            if (arguments.Length <= i)
            {
                result[parameter.Name] = string.Empty;
                continue;
            }

            result[parameter.Name] = arguments[i];
        }

        return result;
    }
}