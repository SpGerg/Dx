namespace Dx.Core.API.Extensions;

public static class StringExtensions
{
    public static bool IsEveryone(this string content)
    {
        return content is "*" or "all";
    }
}