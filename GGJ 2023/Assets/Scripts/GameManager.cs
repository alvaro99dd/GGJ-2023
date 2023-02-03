using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MiniGames {
    Regar, Nabos, Huertos, Podadoras, Raíz
}

public class GameManager : MonoBehaviour
{
    public MiniGames currentMiniGame;
    public static GameManager instance;
    public PlayerInputManager pIM;
    public int currentPlayers;
    public int rootsP1, rootsP2;
    public int vegetablesP1, vegetablesP2;
    public int turnipsP1, turnipsP2;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        //pIM.onPlayerJoined += PIM_onPlayerJoined;
    }

    void OnPlayerJoined(PlayerInput obj) {
        currentPlayers++;
        obj.transform.tag = $"Player{currentPlayers}";
    }

    public void CheckRoots(bool isPlayer1) {
        if (isPlayer1) {
            if (--rootsP1 <= 0) {
                Debug.Log("P2 Wins");
            }
        } else {
            if (--rootsP2 <= 0) {
                Debug.Log("P1 Wins");
            }
        }
    }

    public void CheckTurnips() {
        
    }
    //private void PIM_onPlayerJoined(PlayerInput obj) {
    //    currentPlayers++;
    //    obj.transform.tag = $"Player{currentPlayers}";
    //}
}
