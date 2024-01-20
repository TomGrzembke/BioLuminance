using MyBox;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] StateManager stateManager;
    [SerializeField] SpriteRenderer mapSpriteRenderer;
    [SerializeField] SpriteRenderer creatureRenderer;

    [Separator]

    [SerializeField] Color32 neutralColor = new Color(255, 255, 255, 255);
    [SerializeField] Color32 agressiveColor = new Color(255, 0, 0, 255);

    public Vector2 spriteSizeVec;

    void Awake()
    {
        stateManager = GetComponentInChildren<StateManager>();
        if (!mapSpriteRenderer)
            mapSpriteRenderer = GetSpriteRendererInLayer(gameObject, "Map");
        if (!creatureRenderer)
            creatureRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        CalculateSpriteSize();
    }

    private void FixedUpdate()
    {
        HandleMapIndicators();
    }

    void CalculateSpriteSize()
    {
        if (!creatureRenderer) return;
        spriteSizeVec = (creatureRenderer.bounds.extents);
        spriteSizeVec = new(Mathf.Round(spriteSizeVec.x * 1.5f), Mathf.Round(spriteSizeVec.y * 1.5f));
        spriteSizeVec.x = spriteSizeVec.y;
        if (mapSpriteRenderer == null) return;
        mapSpriteRenderer.transform.localScale = spriteSizeVec;
    }

    SpriteRenderer GetSpriteRendererInLayer(GameObject parent, string layerName)
    {
        SpriteRenderer spriteRendererInLayer = null;

        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in allChildren)
        {
            SpriteRenderer childSpriteRenderer = child.GetComponent<SpriteRenderer>();
            if (childSpriteRenderer != null && child.gameObject.layer == LayerMask.NameToLayer(layerName))
            {
                spriteRendererInLayer = childSpriteRenderer;
                break;
            }
        }
        return spriteRendererInLayer;
    }

    public void HandleMapIndicators()
    {
        if (stateManager == null) return;
        if (mapSpriteRenderer == null) return;
        
        if (stateManager.currentState.Dangerous)
            mapSpriteRenderer.color = agressiveColor;
        else
            mapSpriteRenderer.color = neutralColor;
    }
}