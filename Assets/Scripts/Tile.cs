using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private float darkenDuration = 0.1f;
    [SerializeField] private float darkenAmount = 0.01f;

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

        StartCoroutine(DarkenEffect());

        spriteRenderer.sprite = selectedSprite;
        isSelected = true;
        Actions.TileSelected?.Invoke(this);
    }

    private IEnumerator DarkenEffect()
    {
        Color originalColor = spriteRenderer.color;
        Color darkenedColor = originalColor * darkenAmount;
        darkenedColor.a = originalColor.a; // Keep the original alpha

        spriteRenderer.color = darkenedColor;
        yield return new WaitForSeconds(darkenDuration);
        spriteRenderer.color = originalColor;
    }

    public bool IsSelected() => isSelected;

    public void ResetTile()
    {
        isSelected = false;
        spriteRenderer.sprite = defaultSprite;
        spriteRenderer.color = Color.white;
    }
}