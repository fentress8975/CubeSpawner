using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.Globalization;

public class UIManager : SingletonMono<UIManager>
{
    private const int DEFAULT_VALUE = 0;

    public Action<float> SpeedChange;
    public Action<float> DistanceChange;
    public Action<float> SpawnRateChange;

    private TouchScreenKeyboard keyboard;

    [SerializeField] private TMP_InputField m_SpeedIF;
    [SerializeField] private TMP_InputField m_DistanceIF;
    [SerializeField] private TMP_InputField m_SpawnRateIF;
    [SerializeField] private TMP_Text m_CurrentPoolSizeText;

    public void Initialize(float speed, float distance, float spawnRate, int poolsize)
    {
        m_SpeedIF.text = speed.ToString();
        m_DistanceIF.text = distance.ToString();
        m_SpawnRateIF.text = spawnRate.ToString();
        m_CurrentPoolSizeText.text = "Current pool size = " + poolsize.ToString();
    }

    private void Start()
    {
        SetUpActions();
    }

    private void SetUpActions()
    {
        m_SpeedIF.onEndEdit.AddListener((string value) =>
        {
            SpeedChange?.Invoke(GetFloat(value));
        });
        m_DistanceIF.onEndEdit.AddListener((string value) =>
        {
            DistanceChange?.Invoke(GetFloat(value));
        });
        m_SpawnRateIF.onEndEdit.AddListener((string value) =>
        {
            SpawnRateChange?.Invoke(GetFloat(value));
        });
    }

    public float GetFloat(string value)
    {
        float result;

        if (!float.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
            !float.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
            !float.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result))
        {
            result = DEFAULT_VALUE;
        }

        return result;
    }

    public void ChangePoolSizeText(int count)
    {
        m_CurrentPoolSizeText.SetText("Current Pool Size = " + count.ToString());
    }
}
