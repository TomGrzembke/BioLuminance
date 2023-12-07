using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] StateManager stateManager;
    [SerializeField] SpriteRenderer mapSpriteRenderer;
    [SerializeField] SpriteRenderer creatureRenderer;
    
    public Vector2 spriteSizeVec;
    
    private void Awake()
    {
        stateManager = GetComponentInChildren<StateManager>();
        mapSpriteRenderer = GetSpriteRendererInLayer(gameObject, "Map");
        if(creatureRenderer == null)
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
        spriteSizeVec = (creatureRenderer.bounds.extents * 2);
        spriteSizeVec = new(Mathf.Round(spriteSizeVec.x * 1.5f), Mathf.Round(spriteSizeVec.y * 1.5f));
        spriteSizeVec.x = spriteSizeVec.y;
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
        if (stateManager.currentState.Dangerous)
            mapSpriteRenderer.color = new Color(255, 0, 0);
        else
            mapSpriteRenderer.color = new Color(255, 255, 255);
    }
}