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

    public List<GameObject> player1Roots;
    public List<GameObject> player2Roots;
    private Material player1RootMaterial;
    private Material player2RootMaterial;
    int rootGroupPlayer1 = 0;
    int rootGroupPlayer2 = 0;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        if (instance == null) {
            instance = this;
        }
        //pIM.onPlayerJoined += PIM_onPlayerJoined;
        foreach(GameObject obj in player1Roots){
    obj.GetComponentInChildren<MeshRenderer>().sharedMaterial.SetFloat("_Clip", 1f);
    }
            foreach(GameObject obj in player2Roots){
    obj.GetComponentInChildren<MeshRenderer>().sharedMaterial.SetFloat("_Clip", 1f);
    }
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
        CanvasManager.instance.image.SetActive(false);
        LoadMinigame();
    }

    public void LoadMinigame() {
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
        p2.GetComponentInChildren<Animator>().ResetTrigger("GrabMower");
        p1.GetComponentInChildren<Animator>().ResetTrigger("GrabTurnip");
        p1.GetComponentInChildren<Animator>().ResetTrigger("GrabObject");
        p1.GetComponentInChildren<Animator>().ResetTrigger("GrabMower");
        switch (sceneNames[randomMiniGame]) {
            case "Regar-Montaje":
                p1.GetComponentInChildren<Animator>().SetBool("SmallObject", true);
                p1.GetComponentInChildren<PlayerController>().cube.SetActive(true);
                p2.GetComponentInChildren<Animator>().SetBool("SmallObject", true);
                p2.GetComponentInChildren<PlayerController>().cube.SetActive(true);
                currentMiniGame = MiniGames.Regar;
                sceneNames.Remove("Regar-Montaje");
                break;
            case "Nabos":
                p1.GetComponentInChildren<Animator>().SetBool("SmallObject", false);
                p1.GetComponentInChildren<PlayerController>().cube.SetActive(false);
                p2.GetComponentInChildren<Animator>().SetBool("SmallObject", false);
                p2.GetComponentInChildren<PlayerController>().cube.SetActive(false);
                currentMiniGame = MiniGames.Nabos;
                sceneNames.Remove("Nabos");
                break;
            case "Huerto":
                p1.GetComponentInChildren<Animator>().SetBool("SmallObject", false);
                p1.GetComponentInChildren<PlayerController>().cube.SetActive(false);
                p2.GetComponentInChildren<Animator>().SetBool("SmallObject", false);
                p2.GetComponentInChildren<PlayerController>().cube.SetActive(false);
                currentMiniGame = MiniGames.Huertos;
                sceneNames.Remove("Huerto");
                break;
            case "Podadoras":
                p1.GetComponentInChildren<Animator>().SetBool("SmallObject", false);
                p1.GetComponentInChildren<PlayerController>().cube.SetActive(false);
                p2.GetComponentInChildren<Animator>().SetBool("SmallObject", false);
                p2.GetComponentInChildren<PlayerController>().cube.SetActive(false);
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

        IEnumerator GrowRootsPlayer1()
    {
        player1RootMaterial = player1Roots[rootGroupPlayer1].GetComponentInChildren<MeshRenderer>().sharedMaterial;
        float tempClip = player1RootMaterial.GetFloat("_Clip");
        while (player1RootMaterial.GetFloat("_Clip") > 0f)
        {
            player1RootMaterial.SetFloat("_Clip", tempClip -= 0.0025f);
            yield return new WaitForEndOfFrame();
        }
        if (rootGroupPlayer1 < 5)
        {
            rootGroupPlayer1++;
        }
    }

    IEnumerator GrowRootsPlayer2()
    {
        player2RootMaterial = player2Roots[rootGroupPlayer2].GetComponentInChildren<MeshRenderer>().sharedMaterial;
        float tempClip = player2RootMaterial.GetFloat("_Clip");
        while (player2RootMaterial.GetFloat("_Clip") > 0f)
        {
            player2RootMaterial.SetFloat("_Clip", tempClip -= 0.0025f);
            yield return new WaitForEndOfFrame();
        }
        if (rootGroupPlayer2 < 5)
        {
            rootGroupPlayer2++;
        }
    }

    public void CheckWater(bool isPlayer1) {
        if (isPlayer1) {
            //particulas crecen raices player1
            StartCoroutine(GrowRootsPlayer1());
            if (++waterP1 >= waterToWin) {
                EndGame();
            }
        } else {
            //particulas crecen raices player2
            StartCoroutine(GrowRootsPlayer2());
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
