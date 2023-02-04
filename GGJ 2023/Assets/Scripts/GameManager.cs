using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MiniGames {
    Regar, Nabos, Huertos, Podadoras, Lobby
}

public class GameManager : MonoBehaviour {
    public MiniGames currentMiniGame;
    public static GameManager instance;
    public PlayerInputManager pIM;
    public List<string> sceneNames;
    public Material mat1, mat2;

    public int currentPlayers;
    public int globalScoreP1, globalScoreP2;
    public int playersInButton;
    public int rootsP1, rootsP2;
    public int vegetablesP1, vegetablesP2;
    public int turnipsP1, turnipsP2;
    public int waterP1, waterP2, waterToWin;
    public float timer;

    bool finishingGame;
    int randomMiniGame;

    public string player1WinsRound, player2WinsRound, roundDraw;
    public string player1WinsGame, player2WinsGame, gameDraw;

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

    void SwitchImage() {
        randomMiniGame = Random.Range(0, sceneNames.Count);
        CanvasManager.instance.nextMiniGame.SetActive(true);
        CanvasManager.instance.image.SetActive(true);
        switch (sceneNames[randomMiniGame]) {
            case "Regar-Montaje":
                CanvasManager.instance.image.GetComponent<Image>().sprite = CanvasManager.instance.regar;
                break;
            case "Nabos":
                CanvasManager.instance.image.GetComponent<Image>().sprite = CanvasManager.instance.nabos;
                break;
            case "Huerto":
                CanvasManager.instance.image.GetComponent<Image>().sprite = CanvasManager.instance.huertos;
                break;
            case "Podadoras":
                CanvasManager.instance.image.GetComponent<Image>().sprite = CanvasManager.instance.podadoras;
                break;
        }
    }

    public IEnumerator LobbyCountDown() {
        SwitchImage();
        CanvasManager.instance.startingSoon.SetActive(true);
        CanvasManager.instance.countdown.SetActive(true);
        while (CanvasManager.instance.countdownNumber > 0) {
            yield return new WaitForSeconds(1f);
            CanvasManager.instance.countdownNumber--;
            CanvasManager.instance.countdown.GetComponent<Text>().text = CanvasManager.instance.countdownNumber.ToString();
        }
        CanvasManager.instance.countdownNumber = 5;
        CanvasManager.instance.countdown.GetComponent<Text>().text = CanvasManager.instance.countdownNumber.ToString();
        CanvasManager.instance.startingSoon.SetActive(false);
        CanvasManager.instance.countdown.SetActive(false);
        CanvasManager.instance.image.SetActive(false);
        CanvasManager.instance.nextMiniGame.SetActive(false);
        LoadMinigame();
    }

    public IEnumerator GameCountDown() {
        SwitchImage();
        CanvasManager.instance.playerWins.SetActive(true);
        CanvasManager.instance.countdown.SetActive(true);
        while (CanvasManager.instance.countdownNumber > 0) {
            yield return new WaitForSeconds(1f);
            CanvasManager.instance.countdownNumber--;
            CanvasManager.instance.countdown.GetComponent<Text>().text = CanvasManager.instance.countdownNumber.ToString();
        }
        CanvasManager.instance.countdownNumber = 5;
        CanvasManager.instance.countdown.GetComponent<Text>().text = CanvasManager.instance.countdownNumber.ToString();
        CanvasManager.instance.countdown.SetActive(false);
        CanvasManager.instance.playerWins.SetActive(false);
        CanvasManager.instance.nextMiniGame.SetActive(false);
        LoadMinigame();
    }

    public void LoadMinigame() {
        SceneManager.LoadScene(sceneNames[randomMiniGame]);
        switch (sceneNames[randomMiniGame]) {
            case "Regar-Montaje":
                currentMiniGame = MiniGames.Regar;
                sceneNames.Remove("Regar-Montaje");
                break;
            case "Nabos":
                currentMiniGame = MiniGames.Nabos;
                sceneNames.Remove("Nabos");
                break;
            case "Huerto":
                currentMiniGame = MiniGames.Huertos;
                sceneNames.Remove("Huerto");
                break;
            case "Podadoras":
                currentMiniGame = MiniGames.Podadoras;
                sceneNames.Remove("Podadoras");
                break;
        }
        finishingGame = false;
        StartCoroutine(StartTimer());
    }

    public void AddGlobalScore(bool isPlayer1) {
        if (isPlayer1) {
            if (++globalScoreP1 >= 2) {
                CanvasManager.instance.playerWins.GetComponent<Text>().text = player1WinsGame;
                Time.timeScale = 0f;
            }
        } else {
            if (++globalScoreP2 >= 2) {
                CanvasManager.instance.playerWins.GetComponent<Text>().text = player2WinsGame;
                Time.timeScale = 0f;
            }
        }
    }

    public IEnumerator StartTimer() {
        timer = 0;
        CanvasManager.instance.timer.SetActive(true);
        while (timer <= 60) {
            timer += Time.deltaTime;
            CanvasManager.instance.timer.GetComponent<Text>().text = ((int)timer).ToString();
            yield return new WaitForEndOfFrame();
        }
        timer = 0;
        EndGame();
    }

    public void EndGame() {
        if (finishingGame) {
            return;
        }
        finishingGame = true;
        bool isPlayer1 = true;
        bool draw = false;
        switch (currentMiniGame) {
            case MiniGames.Regar:
                if (waterP1 > waterP2) {
                    CanvasManager.instance.playerWins.GetComponent<Text>().text = player1WinsRound;
                    Debug.Log("P1 Wins");
                } else if (waterP1 < waterP2) {
                    CanvasManager.instance.playerWins.GetComponent<Text>().text = player2WinsRound;
                    Debug.Log("P2 Wins");
                    isPlayer1 = false;
                } else {
                    CanvasManager.instance.playerWins.GetComponent<Text>().text = roundDraw;
                    Debug.Log("Draw");
                    draw = true;
                }
                break;
            case MiniGames.Nabos:
                if (turnipsP1 > turnipsP2) {
                    CanvasManager.instance.playerWins.GetComponent<Text>().text = player1WinsRound;
                    Debug.Log("P1 Wins");
                } else if (turnipsP1 < turnipsP2) {
                    CanvasManager.instance.playerWins.GetComponent<Text>().text = player2WinsRound;
                    Debug.Log("P2 Wins");
                    isPlayer1 = false;
                } else {
                    CanvasManager.instance.playerWins.GetComponent<Text>().text = roundDraw;
                    Debug.Log("Draw");
                    draw = true;
                }
                break;
            case MiniGames.Huertos:
                if (vegetablesP1 > vegetablesP2) {
                    CanvasManager.instance.playerWins.GetComponent<Text>().text = player1WinsRound;
                    Debug.Log("P1 Wins");
                } else if (vegetablesP1 < vegetablesP2) {
                    CanvasManager.instance.playerWins.GetComponent<Text>().text = player2WinsRound;
                    Debug.Log("P2 Wins");
                    isPlayer1 = false;
                } else {
                    CanvasManager.instance.playerWins.GetComponent<Text>().text = roundDraw;
                    Debug.Log("Draw");
                    draw = true;
                }
                break;
            case MiniGames.Podadoras:
                if (rootsP1 > rootsP2) {
                    CanvasManager.instance.playerWins.GetComponent<Text>().text = player1WinsRound;
                    Debug.Log("P1 Wins");
                } else if (rootsP1 < rootsP2) {
                    CanvasManager.instance.playerWins.GetComponent<Text>().text = player2WinsRound;
                    Debug.Log("P2 Wins");
                    isPlayer1 = false;
                } else {
                    CanvasManager.instance.playerWins.GetComponent<Text>().text = roundDraw;
                    Debug.Log("Draw");
                    draw = true;
                }
                break;
        }

        if (!draw) {
            AddGlobalScore(isPlayer1);
        }

        if (sceneNames.Count > 0) {
            StopCoroutine(StartTimer());
            timer = 60;
            StartCoroutine(GameCountDown());
        } else {
            CanvasManager.instance.playerWins.GetComponent<Text>().text = gameDraw;
            Time.timeScale = 0f;
        }
    }

    public void CheckWater(bool isPlayer1) {
        if (isPlayer1) {
            //particulas crecen raices player1
            if (++waterP1 >= waterToWin) {
                EndGame();
            }
        } else {
            //particulas crecen raices player2
            if (++waterP2 >= waterToWin) {
                EndGame();
            }
        }
    }

    public void CheckRoots(bool isPlayer1) {
        if (isPlayer1) {
            if (--rootsP1 <= 0) {
                EndGame();
            }
        } else {
            if (--rootsP2 <= 0) {
                EndGame();
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
