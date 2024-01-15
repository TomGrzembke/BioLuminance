using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DepthLighting : MonoBehaviour
{
    #region serialized fields
    [SerializeField] DepthTracker depthTracker;
    public new ParticleSystem[] particleSystem;
    [SerializeField] ParticleSystem bubbleSystem;


    [Header("Lighting")]
    [SerializeField] Light2D light2D;

    [SerializeField] float lightLevel;
    [SerializeField] float maxLightLevel;
    [SerializeField] float minLightLevel;
    #endregion

    #region private fields
    float SubjectVerticalPos => depthTracker.SubjectVerticalPos;
    float alpha;
    float savedBubbleAlpha;
    #endregion
    void Awake()
    {
        savedBubbleAlpha = bubbleSystem.emission.rateOverDistance.constant;
    }

    void Update()
    {
        LightInfo();
    }

    void LightInfo()
    {
        if (SubjectVerticalPos > 0) return;

        float height = SubjectVerticalPos / 800;

        alpha = Mathf.Clamp(1 + height * 1.6f, minLightLevel, 1);

        lightLevel = Mathf.Clamp(maxLightLevel + height, minLightLevel, maxLightLevel);

        light2D.intensity = lightLevel;

        if (bubbleSystem)
        {
            var bubbleEmission = bubbleSystem.emission;
            bubbleEmission.rateOverDistance = savedBubbleAlpha * alpha;
        }

        foreach (ParticleSystem p in particleSystem)
        {
            if (p == null)
            {
                Debug.Log("Particle system is null");
                continue;
            }
            var mainModule = p.main;

            mainModule.startColor = mainModule.startColor.color.ChangeChannel(ColorChannels.A, alpha);
        }
    }
}