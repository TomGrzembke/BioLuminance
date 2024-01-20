using UnityEngine;

public class InformationfieldPivot : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] RectTransform border;
    [SerializeField] Canvas canvas;
    [SerializeField] RectTransform canvasRect;
    [SerializeField] float height;
    [SerializeField] float width;

    void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        canvasRect = canvas.GetComponent<RectTransform>();
        height = canvasRect.rect.height;
        width = canvasRect.rect.width;
    }

    void Update()
    {
        if (border.position.x > width / 2 || border.position.y > height / 2)
        {
            rectTransform.pivot = new Vector2(1, 1);
        }
        else if (border.position.x < width / 2 || border.position.y < height / 2)
        {
            rectTransform.pivot = new Vector2(0, 0);
        }
    }
}