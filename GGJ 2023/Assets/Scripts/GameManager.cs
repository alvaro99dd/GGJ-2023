using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum MiniGames {
    Regar, Nabos, Huertos, Podadoras, Lobby
}

public class GameManager : MonoBehaviour {
    public MiniGames currentMiniGame;
    public static GameManager instance;
    public PlayerInputManager pIM;
    public List<string> sceneNames;

    public int currentPlayers;
    public int globalScoreP1, globalScoreP2;
    public int playersInButton;
    public int rootsP1, rootsP2;
    public int vegetablesP1, vegetablesP2;
    public int turnipsP1, turnipsP2;
    public int waterP1, waterP2, waterToWin;
    public float timer;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        if (instance == null) {
            instance = this;
        }
        //pIM.onPlayerJoined += PIM_onPlayerJoined;
    }

    void OnPlayerJoined(PlayerInput obj) {
        currentPlayers++;
        obj.transform.tag = $"Player{currentPlayers}";
    }

    public void LoadMinigame() {
        int randomMiniGame = Random.Range(0, sceneNames.Count);
        SceneManager.LoadScene(sceneNames[randomMiniGame]);
        switch (sceneNames[randomMiniGame]) {
            case "Regar":
                currentMiniGame = MiniGames.Regar;
                sceneNames.Remove("Regar");
                break;
            case "Nabos":
                currentMiniGame = MiniGames.Nabos;
                sceneNames.Remove("Nabos");
                break;
            case "Huerto":
                currentMiniGame = MiniGames.Huertos;
                sceneNames.Remove("Huertos");
                break;
            case "Podadoras":
                currentMiniGame = MiniGames.Podadoras;
                sceneNames.Remove("Podadoras");
                break;
        }
        StartCoroutine(StartTimer());
    }

    public void AddGlobalScore(bool isPlayer1) {
        if (isPlayer1) {
            if (++globalScoreP1 >= 2) {
                Debug.Log("Player1 Wins!");
                Time.timeScale = 0f;
            }
        } else {
            if (++globalScoreP2 >= 2) {
                Debug.Log("Player2 Wins!");
                Time.timeScale = 0f;
            }
        }
    }

    public IEnumerator StartTimer() {
        while (timer <= 60) {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        timer = 0;
        EndGame();
    }

    public void EndGame() {
        bool isPlayer1 = true;
        bool draw = false;
        switch (currentMiniGame) {
            case MiniGames.Regar:
                if (waterP1 > waterP2) {
                    Debug.Log("P1 Wins");
                } else if (waterP1 < waterP2) {
                    Debug.Log("P2 Wins");
                    isPlayer1 = false;
                } else {
                    Debug.Log("Draw");
                    draw = true;
                }
                break;
            case MiniGames.Nabos:
                if (turnipsP1 > turnipsP2) {
                    Debug.Log("P1 Wins");
                } else if (turnipsP1 < turnipsP2) {
                    Debug.Log("P2 Wins");
                    isPlayer1 = false;
                } else {
                    Debug.Log("Draw");
                    draw = true;
                }
                break;
            case MiniGames.Huertos:
                if (vegetablesP1 > vegetablesP2) {
                    Debug.Log("P1 Wins");
                } else if (vegetablesP1 < vegetablesP2) {
                    Debug.Log("P2 Wins");
                    isPlayer1 = false;
                } else {
                    Debug.Log("Draw");
                    draw = true;
                }
                break;
            case MiniGames.Podadoras:
                if (rootsP1 > rootsP2) {
                    Debug.Log("P1 Wins");
                } else if (rootsP1 < rootsP2) {
                    Debug.Log("P2 Wins");
                    isPlayer1 = false;
                } else {
                    Debug.Log("Draw");
                    draw = true;
                }
                break;
        }

        if (!draw) {
            AddGlobalScore(isPlayer1);
        }

        if (sceneNames.Count > 0) {
            LoadMinigame();
        } else {
            Debug.Log("Game ended in Draw!");
        }
    }

    public void CheckWater(bool isPlayer1) {
        if (isPlayer1) {
            //particulas crecen raices player1
            if (++waterP1 >= waterToWin) {
                Debug.Log("P1 Wins");
            }
        } else {
            //particulas crecen raices player2
            if (++waterP2 >= waterToWin) {
                Debug.Log("P2 Wins");
            }
        }
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

    public void CheckTurnips(bool isPlayer1, int score) {
        if (isPlayer1) {
            turnipsP1 += score;
        } else {
            turnipsP2 += score;
        }
    }

    public void DeleteTurnips(bool isPlayer1, int score) {
        if (isPlayer1) {
            turnipsP1 -= score;
        } else {
            turnipsP2 -= score;
        }
    }
    //private void PIM_onPlayerJoined(PlayerInput obj) {
    //    currentPlayers++;
    //    obj.transform.tag = $"Player{currentPlayers}";
    //}
}
