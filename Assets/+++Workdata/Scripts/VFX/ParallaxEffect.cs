using UnityEngine;


public class ParallaxEffect : MonoBehaviour
{
    #region serialized fields
    [SerializeField] float amountOfParallax;
    [SerializeField] Transform camTrans;
    [SerializeField] SpriteRenderer spriteRenderer;

    #endregion

    #region private fields
    float startingPos;
    float lengthOfSprite;

    #endregion

    void Start()
    {
        startingPos = transform.position.x;
        lengthOfSprite = spriteRenderer.bounds.size.x;
    }

    void Update()
    {
        Vector3 pos = camTrans.position;
        float tempDistance = pos.x * (1 - amountOfParallax);
        float distance = pos.x * amountOfParallax;

        Vector3 NewPosition = new (startingPos + distance, transform.position.y, transform.position.z);

        transform.position = NewPosition;

        if (tempDistance > startingPos + (lengthOfSprite / 2))
        {
            startingPos += lengthOfSprite;
        }
        else if (tempDistance < startingPos - (lengthOfSprite / 2))
        {
            startingPos -= lengthOfSprite;
        }
    }
}
