using UnityEngine;

public class SpriteInitializer : MonoBehaviour
{
    #region serialized fields
    [SerializeField] Sprite[] possibleSprites;
    [SerializeField] SpriteRenderer sr;
    #endregion

    #region private fields

    #endregion

    void Awake()
    {
        if (sr)
            sr.sprite = possibleSprites[Random.Range(0, possibleSprites.Length)];
    }
}