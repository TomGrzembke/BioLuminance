using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DepthLighting : MonoBehaviour
{
    #region serialized fields
    [SerializeField] DepthTracker depthTracker;
    public new ParticleSystem[] particleSystem;


    [Header("Lighting")]
    [SerializeField] Light2D light2D;

    [SerializeField] float lightLevel;
    [SerializeField] float maxLightLevel;
    [SerializeField] float minLightLevel;
    #endregion

    #region private fields
    float SubjectVerticalPos => depthTracker.SubjectVerticalPos;
    float alpha;
    #endregion

    void Update()
    {
        LightInfo();
    }

    void LightInfo()
    {
        if (SubjectVerticalPos > 0) return;

        float height = SubjectVerticalPos / 800;

        lightLevel = maxLightLevel + height;

        alpha = 1 + height * 1.6f;
        alpha = Mathf.Clamp(alpha, minLightLevel, 1);

        foreach (ParticleSystem p in particleSystem)
        {
            var mainModule = p.main;


            mainModule.startColor = mainModule.startColor.color.ChangeChannel(ColorChannels.A, alpha);
        }


        lightLevel = Mathf.Clamp(lightLevel, minLightLevel, maxLightLevel);

        light2D.intensity = lightLevel;
    }
}