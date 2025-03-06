using System;

namespace Dx.Core.API.Extensions;

public static class StringExtensions
{
    public static bool IsEveryone(this string content)
    {
        return content is "*" or "all";
    }

    public static int HexColorToInteger(this string content)
    {
        content = content.TrimStart('#');
        
        return Convert.ToInt32(content, 16);
    }
}