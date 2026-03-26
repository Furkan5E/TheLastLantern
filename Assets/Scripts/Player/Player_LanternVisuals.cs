using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player_LanternVisuals : MonoBehaviour
{
    [Serializable]
    public struct LightSettings
    {
        public Light2D light;
        public float minIntensity;
        public float maxIntensity;
        public float minOuterRadius;
        public float maxOuterRadius;
    }

    [SerializeField] private Entity_Health healthScript;
    [SerializeField] private LightSettings[] lightsSettings;

    private void OnEnable()
    {
        if (healthScript != null)
            healthScript.OnHealthChanged += UpdateLights;
    }

    private void OnDisable()
    {
        if (healthScript != null)
            healthScript.OnHealthChanged -= UpdateLights;
    }

    private void UpdateLights(float currentHp, float maxHp)
    {
        float healthPercentage = currentHp/maxHp;

        foreach (LightSettings setting in lightsSettings)
        {
            if (setting.light != null)
            {
                setting.light.intensity = Mathf.Lerp(setting.minIntensity, setting.maxIntensity, healthPercentage);
                setting.light.pointLightOuterRadius = Mathf.Lerp(setting.minOuterRadius, setting.maxOuterRadius, healthPercentage);
            }
        }
    }
}