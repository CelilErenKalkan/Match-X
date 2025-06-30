using UnityEngine;
using static Actions;


public class UIManager : MonoBehaviour
{

    private void Start()
    {

    }

    public void GameStartButton()
    {
        GridSelected?.Invoke();
    }
}