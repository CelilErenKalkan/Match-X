using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite selectedSprite;

    private SpriteRenderer spriteRenderer;
    private bool isSelected = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite;
    }

    void OnMouseDown()
    {
        if (isSelected) return;

        spriteRenderer.sprite = selectedSprite;
        isSelected = true;
        Actions.TileSelected?.Invoke(this);
    }

    public bool IsSelected() => isSelected;

    public void ResetTile()
    {
        isSelected = false;
        spriteRenderer.sprite = defaultSprite;
    }
}
