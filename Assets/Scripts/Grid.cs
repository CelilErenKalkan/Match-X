using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite selectedSprite;

    private SpriteRenderer spriteRenderer;
    private bool hasChanged = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("Grid script requires a SpriteRenderer component on the same GameObject.");
            enabled = false;
            return;
        }

        spriteRenderer.sprite = emptySprite; // start with first sprite
    }

    void OnMouseDown()
    {
        if (!hasChanged)
        {
            spriteRenderer.sprite = selectedSprite;
            hasChanged = true; // lock after change
        }
    }
}
