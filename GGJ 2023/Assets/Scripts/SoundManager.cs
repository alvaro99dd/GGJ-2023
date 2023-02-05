using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip excavar, pito, podadora, salpicar, ambiente, stun, terremoto;
    public AudioSource aS;

    public static SoundManager instance;
    private void Awake() {
        DontDestroyOnLoad(gameObject);

        if (instance != this) {
            instance = this;
        }
    }
}
