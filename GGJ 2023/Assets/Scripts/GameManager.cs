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
    public List<string> sceneNamesBackUp;
    public Material mat1, mat2;
    public bool gameStarted, comeBack;

    public int currentPlayers;
    public int globalScoreP1, globalScoreP2;
    public int playersInPlay, playersInExit;
    public int rootsP1, rootsP2;
    public int vegetablesP1, vegetablesP2;
    public int turnipsP1, turnipsP2;
    public int waterP1, waterP2, waterToWin;
    public float timer;

    AudioSource aS;
    MusicManager mM;
    WaterEvents wE;
    bool finishingGame, waterEvent;
    int randomMiniGame;

    public string player1WinsRound, player2WinsRound, roundDraw;
    public string player1WinsGame, player2WinsGame, gameDraw;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        if (instance == null) {
            instance = this;
        }
        aS = GetComponent<AudioSource>();
        mM = GetComponent<MusicManager>();
        aS.clip = mM.temaC;
        aS.Play();
        //pIM.onPlayerJoined += PIM_onPlayerJoined;
    }

    private void Update() {
        if (currentMiniGame == MiniGames.Regar && !waterEvent) {
            wE = GameObject.Find("MiniGameEvents").GetComponent<WaterEvents>();
            waterEvent = true;
        }
    }

    void OnPlayerJoined(PlayerInput obj) {
        currentPlayers++;
        obj.transform.tag = $"Player{currentPlayers}";
    }

    void SwitchImage() {
        randomMiniGame = Random.Range(0, sceneNames.Count);
        CanvasManager.instance.nextMiniGame.SetActive(true);
        switch (sceneNames[randomMiniGame]) {
            case "Regar-Montaje":
                CanvasManager.instance.image.GetComponent<Image>().sprite = CanvasManager.instance.regar;
                break;
            case "Nabos-Montaje":
                CanvasManager.instance.image.GetComponent<Image>().sprite = CanvasManager.instance.nabos;
                break;
            case "Huerto-Montaje":
                CanvasManager.instance.image.GetComponent<Image>().sprite = CanvasManager.instance.huertos;
                break;
            case "Podadoras-Montaje":
                CanvasManager.instance.image.GetComponent<Image>().sprite = CanvasManager.instance.podadoras;
                break;
        }
        CanvasManager.instance.image.SetActive(true);
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
        CanvasManager.instance.image.SetActive(false);
        LoadMinigame();
    }

    public void LoadMinigame() {
        CanvasManager.instance.globalScore.SetActive(true);
        int changeMusic;
        if (aS.clip == mM.temaA) {
            changeMusic = Random.Range(0, 2);
        } else {
            changeMusic = Random.Range(0, 3);
        }

        switch (changeMusic) {
            case 0:
                if (aS.clip == mM.temaA) {
                    break;
                }
                aS.Stop();
                aS.clip = mM.temaA;
                aS.Play();
                break;
            case 1:
                if (aS.clip == mM.temaB) {
                    break;
                }
                aS.Stop();
                aS.clip = mM.temaB;
                aS.Play();
                break;
        }

        GameObject p1 = GameObject.FindGameObjectWithTag("Player1");
        GameObject p2 = GameObject.FindGameObjectWithTag("Player2");

        switch (currentMiniGame) {
            case MiniGames.Nabos:
                if (p1.GetComponentInChildren<GrabBehaviour>().objectGrabbed) {
                    p1.GetComponentInChildren<GrabBehaviour>().objectGrabbed = false;
                    Destroy(p1.GetComponentInChildren<GrabBehaviour>().transform.GetChild(2).gameObject);
                }
                if (p2.GetComponentInChildren<GrabBehaviour>().objectGrabbed) {
                    p2.GetComponentInChildren<GrabBehaviour>().objectGrabbed = false;
                    Destroy(p2.GetComponentInChildren<GrabBehaviour>().transform.GetChild(2).gameObject);
                }
                break;
            case MiniGames.Huertos:
                if (p1.GetComponentInChildren<GrabBehaviour>().objectGrabbed) {
                    p1.GetComponentInChildren<GrabBehaviour>().objectGrabbed = false;
                    p1.GetComponentInChildren<GrabBehaviour>().vegetablePlaceHolder.SetActive(false);
                }
                if (p2.GetComponentInChildren<GrabBehaviour>().objectGrabbed) {
                    p2.GetComponentInChildren<GrabBehaviour>().objectGrabbed = false;
                    p2.GetComponentInChildren<GrabBehaviour>().vegetablePlaceHolder.SetActive(false);
                }
                break;
            case MiniGames.Podadoras:
                if (p1.GetComponentInChildren<GrabBehaviour>().objectGrabbed) {
                    p1.GetComponentInChildren<GrabBehaviour>().objectGrabbed = false;
                    p1.GetComponentInChildren<GrabBehaviour>().mowerPlaceHolder.SetActive(false);
                }
                if (p2.GetComponentInChildren<GrabBehaviour>().objectGrabbed) {
                    p2.GetComponentInChildren<GrabBehaviour>().objectGrabbed = false;
                    p2.GetComponentInChildren<GrabBehaviour>().mowerPlaceHolder.SetActive(false);
                }
                break;
        }

        SceneManager.LoadScene(sceneNames[randomMiniGame]);

        p2.GetComponentInChildren<Animator>().ResetTrigger("GrabObject");
        p2.GetComponentInChildren<Animator>().ResetTrigger("GrabTurnip");
        p2.GetComponentInChildren<Animator>().ResetTrigger("GrabPodadora");
        p1.GetComponentInChildren<Animator>().ResetTrigger("GrabTurnip");
        p1.GetComponentInChildren<Animator>().ResetTrigger("GrabObject");
        p1.GetComponentInChildren<Animator>().ResetTrigger("GrabPodadora");
        switch (sceneNames[randomMiniGame]) {
            case "Regar-Montaje":
                p1.GetComponentInChildren<Animator>().SetBool("SmallObject", true);
                p1.GetComponentInChildren<Animator>().SetBool("Podadora", false);
                p1.GetComponentInChildren<PlayerController>().cube.SetActive(true);
                p2.GetComponentInChildren<Animator>().SetBool("SmallObject", true);
                p2.GetComponentInChildren<Animator>().SetBool("Podadora", false);
                p2.GetComponentInChildren<PlayerController>().cube.SetActive(true);
                currentMiniGame = MiniGames.Regar;
                sceneNames.Remove("Regar-Montaje");
                break;
            case "Nabos-Montaje":
                p1.GetComponentInChildren<Animator>().SetBool("SmallObject", false);
                p1.GetComponentInChildren<Animator>().SetBool("Podadora", false);
                p1.GetComponentInChildren<PlayerController>().cube.SetActive(false);
                p2.GetComponentInChildren<Animator>().SetBool("SmallObject", false);
                p2.GetComponentInChildren<Animator>().SetBool("Podadora", false);
                p2.GetComponentInChildren<PlayerController>().cube.SetActive(false);
                currentMiniGame = MiniGames.Nabos;
                sceneNames.Remove("Nabos-Montaje");
                break;
            case "Huerto-Montaje":
                p1.GetComponentInChildren<Animator>().SetBool("SmallObject", false);
                p1.GetComponentInChildren<Animator>().SetBool("Podadora", false);
                p1.GetComponentInChildren<PlayerController>().cube.SetActive(false);
                p2.GetComponentInChildren<Animator>().SetBool("SmallObject", false);
                p2.GetComponentInChildren<Animator>().SetBool("Podadora", false);
                p2.GetComponentInChildren<PlayerController>().cube.SetActive(false);
                currentMiniGame = MiniGames.Huertos;
                sceneNames.Remove("Huerto-Montaje");
                break;
            case "Podadoras-Montaje":
                p1.GetComponentInChildren<Animator>().SetBool("SmallObject", false);
                p1.GetComponentInChildren<Animator>().SetBool("Podadora", false);
                p1.GetComponentInChildren<PlayerController>().cube.SetActive(false);
                p2.GetComponentInChildren<Animator>().SetBool("SmallObject", false);
                p2.GetComponentInChildren<Animator>().SetBool("Podadora", false);
                p2.GetComponentInChildren<PlayerController>().cube.SetActive(false);
                currentMiniGame = MiniGames.Podadoras;
                sceneNames.Remove("Podadoras-Montaje");
                break;
        }
        finishingGame = false;
        StartCoroutine(StartTimer());
    }

    public bool AddGlobalScore(bool isPlayer1) {
        bool gameEnded = false;
        if (isPlayer1) {
            if (++globalScoreP1 >= 2) {
                CanvasManager.instance.playerWins.GetComponent<Text>().text = player1WinsGame;
                CanvasManager.instance.playerWins.SetActive(true);
                gameEnded = true;
                StartCoroutine(KillGame());
                Time.timeScale = 0f;
            }
            CanvasManager.instance.p1Global.text = globalScoreP1.ToString();
        } else {
            if (++globalScoreP2 >= 2) {
                CanvasManager.instance.playerWins.GetComponent<Text>().text = player2WinsGame;
                CanvasManager.instance.playerWins.SetActive(true);
                gameEnded = true;
                StartCoroutine(KillGame());
                Time.timeScale = 0f;
            }
            CanvasManager.instance.p2Global.text = globalScoreP2.ToString();
        }
        return gameEnded;
    }

    public IEnumerator StartTimer() {
        timer = 0;
        SoundManager.instance.aS.PlayOneShot(SoundManager.instance.pito);
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

        if (!draw && AddGlobalScore(isPlayer1)) {
            return;
        }

        if (sceneNames.Count > 0) {
            StopCoroutine(StartTimer());
            timer = 60;
            StartCoroutine(GameCountDown());
        } else {
            CanvasManager.instance.playerWins.GetComponent<Text>().text = gameDraw;
            CanvasManager.instance.playerWins.SetActive(true);
            StartCoroutine(KillGame());
            Time.timeScale = 0f;
            return;
        }
    }



    public void CheckWater(bool isPlayer1) {
        if (isPlayer1) {
            //particulas crecen raices player1
            StartCoroutine(wE.GrowRootsPlayer1());
            if (++waterP1 >= waterToWin) {
                EndGame();
            }
        } else {
            //particulas crecen raices player2
            StartCoroutine(wE.GrowRootsPlayer2());
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
            GameObject.FindGameObjectWithTag("Player1").GetComponentInChildren<GrabBehaviour>().playerInterfaceNabos.text = turnipsP1.ToString();
        } else {
            turnipsP2 += score;
            GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<GrabBehaviour>().playerInterfaceNabos.text = turnipsP2.ToString();
        }
    }

    public void DeleteTurnips(bool isPlayer1, int score) {
        if (isPlayer1) {
            turnipsP1 -= score;
            GameObject.FindGameObjectWithTag("Player1").GetComponentInChildren<GrabBehaviour>().playerInterfaceNabos.text = turnipsP1.ToString();
        } else {
            turnipsP2 -= score;
            GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<GrabBehaviour>().playerInterfaceNabos.text = turnipsP2.ToString();
        }
    }

    IEnumerator KillGame() {
        yield return new WaitForSecondsRealtime(5f);
        System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe")); //new program
        Application.Quit(); //kill current process
    }

    public void GoBackToLobby() {
        CanvasManager.instance.playerWins.SetActive(false);
        Time.timeScale = 1f;
        currentMiniGame = MiniGames.Lobby;
        aS.Stop();
        aS.clip = mM.temaC;
        aS.Play();
        gameStarted = false;
        comeBack = true;
        SceneManager.LoadScene("Lobby-Montaje");
    }
    //private void PIM_onPlayerJoined(PlayerInput obj) {
    //    currentPlayers++;
    //    obj.transform.tag = $"Player{currentPlayers}";
    //}
}
