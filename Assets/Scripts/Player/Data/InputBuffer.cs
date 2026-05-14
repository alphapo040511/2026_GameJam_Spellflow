using System.Collections.Generic;
using UnityEngine;

public class InputBuffer
{
    private class BufferedInput
    {
        public InputType type;
        public float time;
    }

    private List<BufferedInput> buffer = new();

    private float bufferTime = 0.25f;
    private float comboWindow = 0.12f;

    public void AddFromRawInput(bool light, bool heavy)
    {
        InputType type;

        if (light && heavy)
            type = InputType.LightHeavy;
        else if (light)
            type = InputType.Light;
        else if (heavy)
            type = InputType.Heavy;
        else
            return;

        buffer.Add(new BufferedInput
        {
            type = type,
            time = Time.time
        });
    }

    public void Update()
    {
        float now = Time.time;
        buffer.RemoveAll(b => now - b.time > bufferTime);
    }

    // 🔥 가장 최근 입력 가져오기 (핵심)
    public bool TryConsumeLatest(out InputType type)
    {
        if (buffer.Count == 0)
        {
            type = default;
            return false;
        }

        type = buffer[buffer.Count - 1].type;
        buffer.Clear();
        return true;
    }

    public bool IsFinisher()
    {
        float now = Time.time;

        bool hasLight = false;
        bool hasHeavy = false;

        foreach (var b in buffer)
        {
            if (now - b.time > comboWindow) continue;

            if (b.type == InputType.Light) hasLight = true;
            if (b.type == InputType.Heavy) hasHeavy = true;
        }

        return hasLight && hasHeavy;
    }

    public void Clear()
    {
        buffer.Clear();
    }
}

public enum InputType
{
    Light,
    Heavy,
    LightHeavy
}