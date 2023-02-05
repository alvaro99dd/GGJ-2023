using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGestor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene() {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Lobby-Montaje");
    }
}
