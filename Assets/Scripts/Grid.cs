using System;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static event Action<Grid> GridSelected; // static event to broadcast selection

    [SerializeField] private Sprite firstSprite;
    [SerializeField] private Sprite secondSprite;

    private SpriteRenderer spriteRenderer;
    private bool hasChanged = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("Grid script requires a SpriteRenderer component.");
            enabled = false;
            return;
        }

        spriteRenderer.sprite = firstSprite;
    }

    void OnMouseDown()
    {
        if (!hasChanged)
        {
            spriteRenderer.sprite = secondSprite;
            hasChanged = true;
            GridSelected?.Invoke(this); // Invoke event with this Grid
        }
    }

    public bool IsSelected() => hasChanged;

    public void ResetGrid()
    {
        hasChanged = false;
        spriteRenderer.sprite = firstSprite;
    }
}
