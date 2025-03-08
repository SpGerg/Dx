using Exiled.API.Features.Pools;

namespace Dx.Core.API.Utilities;

public static class SliderBuilder
{
    public static string BuildSlider(float current, float maximum, int width = 30)
    {
        return BuildSlider(current / maximum, width);
    }
    
    public static string BuildSlider(float percent, int width)
    {
        var stringBuilder = StringBuilderPool.Pool.Get();
        var size = percent * width;

        for (var i = 0; i < width; i++)
        {
            stringBuilder.Append(size > i ? '◼' : '◻');
        }

        return StringBuilderPool.Pool.ToStringReturn(stringBuilder);
    }
}