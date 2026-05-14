using System.Collections.Generic;
using UnityEngine;

public class ComboBuffer
{
    private List<InputType> inputs = new();

    private float resetTime = 0.7f;
    private float lastTime;

    public void Add(InputType input)
    {
        if (Time.time - lastTime > resetTime)
            inputs.Clear();

        inputs.Add(input);

        // 너무 길어지면 앞에서 제거 (중요)
        if (inputs.Count > 10)
            inputs.RemoveAt(0);

        lastTime = Time.time;
    }

    // 🔥 뒤에서부터 패턴 비교 (핵심 수정)
    public bool Match(params InputType[] pattern)
    {
        if (inputs.Count < pattern.Length)
            return false;

        int start = inputs.Count - pattern.Length;

        for (int i = 0; i < pattern.Length; i++)
        {
            if (inputs[start + i] != pattern[i])
                return false;
        }

        return true;
    }

    public void Clear()
    {
        inputs.Clear();
    }
}