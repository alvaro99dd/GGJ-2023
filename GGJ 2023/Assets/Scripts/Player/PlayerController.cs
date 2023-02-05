using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [SerializeField] float speed;
    [SerializeField] float turnSmoothVelocity, turnSmoothTime;

    Vector3 direction;
    Rigidbody rb;
    Coroutine grabbing, earthquakeCoroutine;
    public Animator anim;
    public GrabBehaviour gB;
    public bool dropZone, grabZone, stun;
    public EarthQuake earthquake;
    public BoxCollider bC;
    public SkinnedMeshRenderer skm;
    public GameObject cube;
    public ParticleSystem stunPSystem;
    public ParticleSystem waterThrowPSystem;
    public ParticleSystem waterDropPSystem;

    private void Start() {
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody>();
        gB = GetComponentInChildren<GrabBehaviour>();
        skm = GetComponentInChildren<SkinnedMeshRenderer>();
        anim = GetComponentInChildren<Animator>();
        if (GameManager.instance.currentPlayers == 1) {
            skm.material = GameManager.instance.mat1;
        } else {
            skm.material = GameManager.instance.mat2;
        }
    }

    private void FixedUpdate() {
        Movement();
        if (earthquake != null) {
            CheckEarthquake();
        }
        //CheckPrompt();
        GetEarthQuake();
    }

    void GetEarthQuake() {
        if (GameManager.instance.currentMiniGame == MiniGames.Regar && earthquake == null) {
            earthquake = GameObject.Find("MiniGameEvents").GetComponent<EarthQuake>();
        }
    }

    void Movement() {
        if (stun) {
            return;
        }
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        rb.velocity = direction * speed * Time.fixedDeltaTime;
        anim.SetFloat("Speed", rb.velocity.magnitude);
        if (!rb.velocity.Equals(Vector3.zero)) {
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx) => direction = ctx.ReadValue<Vector3>();

    public void OnGrab(InputAction.CallbackContext ctx) {
        if (grabZone && !gB.objectGrabbed) {
            grabbing = StartCoroutine(EnableGrab());
        } else if (dropZone && gB.objectGrabbed && grabbing == null) {
            gB.DropObject();
        }
    }

    IEnumerator EnableGrab() {
        bC.enabled = true;
        yield return new WaitForSeconds(0.2f);
        bC.enabled = false;
        grabbing = null;
    }

    void CheckEarthquake() {
        if (!rb.velocity.Equals(Vector3.zero) && gB.objectGrabbed && earthquake.earthquake && earthquakeCoroutine == null) {
            gB.ObjectFalledOff();
            earthquakeCoroutine = StartCoroutine(StunBehaviour());
        }
    }

    IEnumerator StunBehaviour() {
        stun = true;
        anim.SetBool("Stun", true);
        //particulas stun aparecen
        stunPSystem.Play();
        SoundManager.instance.aS.PlayOneShot(SoundManager.instance.stun);
        yield return new WaitForSeconds(earthquake.stunDuration);
        stun = false;
        anim.SetBool("Stun", false);
        //particulas stun fuera
        earthquakeCoroutine = null;
    }

    public IEnumerator StunPlayer(float stunDuration) {
        stun = true;
        anim.SetBool("Stun", true);
        //particulas stun aparecen
        stunPSystem.Play();
        SoundManager.instance.aS.PlayOneShot(SoundManager.instance.stun);
        yield return new WaitForSeconds(stunDuration);
        //particulas stun fuera
        stun = false;
        anim.SetBool("Stun", false);
    }

    void CheckPrompt() {
        if (dropZone && gB.objectGrabbed && !transform.Find("Canvas").gameObject.activeInHierarchy) {
            transform.Find("Canvas").gameObject.SetActive(true);
        } else if (grabZone && !gB.objectGrabbed && !transform.Find("Canvas").gameObject.activeInHierarchy) {
            transform.Find("Canvas").gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("DropZone")) {
            gB.huertoName = other.name;
            if (GameManager.instance.currentMiniGame == MiniGames.Huertos) {
                if (gB.huertoName != gB.huerto.parent.name) {
                    dropZone = false;
                } else {
                    dropZone = true;
                }
            } else {
                dropZone = true;
            }
        } else if (other.CompareTag("Water") || other.CompareTag("Vegetable") || other.CompareTag("Mower") || other.CompareTag("Turnip")) {
            grabZone = true;
        }

        if (other.CompareTag("Play")) {
            if (++GameManager.instance.playersInPlay == 2 && !GameManager.instance.gameStarted) {
                CanvasManager.instance.playText.SetActive(false);
                GameManager.instance.gameStarted = true;
                GameManager.instance.StartCoroutine(GameManager.instance.LobbyCountDown());
            } else {
                CanvasManager.instance.playText.SetActive(true);
            }
        }

        if (other.CompareTag("Controls")) {
            CanvasManager.instance.info.SetActive(true);
        }

        if (other.CompareTag("Exit")) {
            if (++GameManager.instance.playersInExit == 2) {
                Application.Quit();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("DropZone")) {
            dropZone = false;
        } else {
            grabZone = false;
        }

        if (other.CompareTag("Play")) {
            GameManager.instance.playersInPlay--;
            CanvasManager.instance.playText.SetActive(false);
        }

        if (other.CompareTag("Exit")) {
            GameManager.instance.playersInExit--;
        }

        if (other.CompareTag("Controls")) {
            CanvasManager.instance.info.SetActive(false);
        }
    }
}
