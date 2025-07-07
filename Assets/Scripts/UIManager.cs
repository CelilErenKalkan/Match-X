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

    private int matchCount;
    private const string CountKey = "Count";

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

        LoadCount();
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
            UpdateMatchText();
            board.GenerateBoard(newCols, newRows);
        }
    }

    private void OnBoardMatch()
    {
        matchCount++;
        SaveCount();
        UpdateMatchText();
    }

    private void UpdateMatchText()
    {
        if (matchCountText != null)
        {
            matchCountText.text = "Count: " + matchCount;
        }
    }
    
    private void SaveCount()
    {
        PlayerPrefs.SetInt(CountKey, matchCount);
        PlayerPrefs.Save();
    }

    private void LoadCount()
    {
        matchCount = PlayerPrefs.GetInt(CountKey, 0); // 0 is the default value if key doesn't exist
    }
}
