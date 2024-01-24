using UnityEngine;

public class JellyInitializer : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Sprite[] possibleBackSprites;
    [SerializeField] Sprite[] possibleBellySprites;
    [SerializeField] Sprite[] possibleStingSprites;
    [SerializeField] Gradient[] possibleGradients;
    [SerializeField] SpriteRenderer backsr;
    [SerializeField] SpriteRenderer bellysr;
    [SerializeField] SpriteRenderer stingsr;
    [SerializeField] LineRenderer lineRenderer;
    #endregion

    #region private fields

    #endregion

    void Awake()
    {
        int randomNumber = Random.Range(0, possibleBackSprites.Length);
        bellysr.sprite = possibleBellySprites[randomNumber];
        stingsr.sprite = possibleStingSprites[randomNumber];
        backsr.sprite = possibleBackSprites[randomNumber];
        lineRenderer.colorGradient = possibleGradients[randomNumber];
    }

}