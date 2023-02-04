using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public GameObject playText;
    public GameObject startingSoon;
    public GameObject countdown;
    public GameObject timer;
    public GameObject playerWins;
    public GameObject nextMiniGame;
    public GameObject image;

    [Header("Minigames Images")]
    public Sprite nabos;
    public Sprite regar;
    public Sprite huertos;
    public Sprite podadoras;

    public int countdownNumber;

    public static CanvasManager instance;

    private void Awake() {
        if (instance != this) {
            instance = this;
        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
}
