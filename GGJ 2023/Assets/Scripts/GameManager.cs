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

    //private void PIM_onPlayerJoined(PlayerInput obj) {
    //    currentPlayers++;
    //    obj.transform.tag = $"Player{currentPlayers}";
    //}
}
