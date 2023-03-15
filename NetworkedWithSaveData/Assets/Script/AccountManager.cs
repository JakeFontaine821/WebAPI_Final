using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    /*******************************************************************************/
    // SINGLETON STUFF
    public static AccountManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    /*******************************************************************************/
    [Header("Local Player's Account")]
    public DataClass Account;
    [Header("Game States")]
    public GameStates currentState = GameStates.MENU;
    public GameObject MenuFolder;
    public GameObject NetworkFolder;

    public void ChangeGameState(GameStates newState)
    {
        if (newState == currentState) return;

        switch (newState)
        {
            case GameStates.MENU:
                currentState = newState;
                MenuFolder.SetActive(true);
                NetworkFolder.SetActive(false);
                break;
            case GameStates.NETWORKMENU:
                currentState = newState;
                MenuFolder.SetActive(false);
                NetworkFolder.SetActive(true);
                break;
        }
    }
}
