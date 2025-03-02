using System.ComponentModel;
using UnityEngine;

namespace Dx.Core.API.Features;

public class HintSettings
{
    [Description("Включена ли подсказка")]
    public bool Enabled { get; set; } = true;

    [Description("Текст подсказки")]
    public string Text { get; set; }

    [Description("Позиция подсказки (максимум 1200 для обеих осей)")]
    public Vector2 Position { get; set; }

    [Description("Размер текста подсказки")]
    public int Size { get; set; }

    [Description("Длительность отображения подсказки (в секундах)")]
    public float Duration { get; set; } = 1f;

    public HintSettings(Vector2 position, int size, string text, float duration = 1f)
    {
        Position = position;
        Size = size;
        Text = text;
        Duration = duration;
    }

    // Пустой конструктор для YAML-десериализации
    public HintSettings() { }
}