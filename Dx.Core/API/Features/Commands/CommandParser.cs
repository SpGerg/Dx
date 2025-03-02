using System.Collections.Generic;
using System.Linq;

namespace Dx.Core.API.Features.Commands;

public static class CommandParser
{
    public static Dictionary<string, string> ParseArguments(CommandParameter[] parameters, string[] arguments)
    {
        arguments = arguments.Skip(1).ToArray();
        
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