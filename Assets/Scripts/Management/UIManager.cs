using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField rowInputField;
    public TMP_InputField columnInputField;
    public Button regenerateButton;
    public TMP_Text matchCountText;

    [Header("Board Reference")]
    public Board board;

    private int matchCount = 0;

    private void Awake()
    {
        if (regenerateButton != null)
        {
            regenerateButton.onClick.AddListener(OnRegenerateButtonClicked);
        }

        if (board != null)
        {
            Actions.Match += OnBoardMatch;
        }

        UpdateMatchText();
    }

    private void OnDestroy()
    {
        if (board != null)
        {
            Actions.Match -= OnBoardMatch;
        }
    }

    private void OnRegenerateButtonClicked()
    {
        if (string.IsNullOrWhiteSpace(rowInputField.text) || string.IsNullOrWhiteSpace(columnInputField.text))
            return;

        if (int.TryParse(rowInputField.text, out int newRows) && int.TryParse(columnInputField.text, out int newCols))
        {
            matchCount = 0;
            UpdateMatchText();
            board.GenerateBoard(newCols, newRows); // Note: You must implement this method in your Board class
        }
    }

    private void OnBoardMatch()
    {
        matchCount++;
        UpdateMatchText();
    }

    private void UpdateMatchText()
    {
        if (matchCountText != null)
        {
            matchCountText.text = "Count: " + matchCount;
        }
    }
}
