using System.IO;

namespace Dx.Core.API.Features.Audio.Extensions;

public static class AudioSettingsExtensions
{
    public static string GetFullPath(this AudioSettings audioSettings)
    {
        return Path.Combine(Plugin.AudiosFilepath, audioSettings.Filepath);
    }

    /// <summary>
    /// Получает длительность аудиофайла в секундах.
    /// </summary>
    /// <param name="audioSettings">Настройки аудио.</param>
    /// <returns>Длительность аудиофайла в секундах.</returns>
    public static float GetAudioDuration(this AudioSettings audioSettings)
    {
        using var file = TagLib.File.Create(GetFullPath(audioSettings));
        return (float)file.Properties.Duration.TotalSeconds;
    }
}