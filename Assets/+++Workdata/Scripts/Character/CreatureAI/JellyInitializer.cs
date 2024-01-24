using UnityEngine;

public class JellyInitializer : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Sprite[] possibleSprites;
    [SerializeField] Gradient[] possibleGradients;
    [SerializeField] LineRenderer[] tentecaleRenderer;
    [SerializeField] SpriteRenderer headsr;
    #endregion

    #region private fields

    #endregion

    void Awake()
    {
        int randomNumber = Random.Range(0, possibleSprites.Length);
        headsr.sprite = possibleSprites[randomNumber];

        for (int i = 0; i < tentecaleRenderer.Length; i++)
        {
            tentecaleRenderer[i].colorGradient = possibleGradients[randomNumber];
        }
    }
}