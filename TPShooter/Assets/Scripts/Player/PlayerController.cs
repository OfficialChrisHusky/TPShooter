using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;
    void Awake() { instance = this; }
    
    [Header("Movement")]
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float sprintMultiplier = 1.2f;

    [Header("Looking around")]
    [SerializeField] private float sensitivity = 1.0f;
    [SerializeField] private float minX = -70.0f;
    [SerializeField] private float maxX = 10.0f;

    [Header("Third Person")]
    [SerializeField] private bool thirdPerson = true;
    [SerializeField] private Vector3 camOffset = new Vector3(0.0f, 1.35f, -3.35f);
    [SerializeField] private float camXAxisRotOffset = 9.0f;
    [SerializeField] private float tpMinX = -70.0f;
    [SerializeField] private float tpMaxX = 10.0f;

    private Rigidbody rb;

    private Vector3 direction;
    private float multiplier = 1.0f;

    private Vector2 mouseRotation;

    private bool thirdPersonLast;

    void Start() {
        
        rb = GetComponent<Rigidbody>();
        thirdPersonLast = thirdPerson;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if(thirdPerson) {

            Player.instance.MainCam.localPosition = camOffset;
            Player.instance.MainCam.localRotation = Quaternion.Euler(camXAxisRotOffset, 0.0f, 0.0f);

        } else {

            Player.instance.MainCam.localPosition = Vector3.zero;
            Player.instance.MainCam.localRotation = Quaternion.identity;

        }

    }

    void Update() {

        if(!thirdPersonLast && thirdPerson) {

            Player.instance.MainCam.localPosition = camOffset;
            Player.instance.MainCam.localRotation = Quaternion.Euler(camXAxisRotOffset, 0.0f, 0.0f);

        } else if(thirdPersonLast && !thirdPerson) {

            Player.instance.MainCam.localPosition = Vector3.zero;
            Player.instance.MainCam.localRotation = Quaternion.identity;

        }
        
        mouseRotation.y += Input.GetAxis("Mouse X") * sensitivity;
        mouseRotation.x -= Input.GetAxis("Mouse Y") * sensitivity;
        
        if (thirdPerson)
            mouseRotation.x = Mathf.Clamp(mouseRotation.x, tpMinX, tpMaxX);
        else
            mouseRotation.x = Mathf.Clamp(mouseRotation.x, minX, maxX);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical   = Input.GetAxisRaw("Vertical");
        direction = transform.forward * vertical + transform.right * horizontal;

        transform.rotation = Quaternion.Euler(0.0f, mouseRotation.y, 0.0f);
        Player.instance.Eyes.rotation = Quaternion.Euler(mouseRotation.x, mouseRotation.y, 0.0f);

        if(Input.GetKeyDown(KeyCode.LeftShift)) multiplier = sprintMultiplier;
        else if(Input.GetKeyUp(KeyCode.LeftShift)) multiplier = 1.0f;

        thirdPersonLast = thirdPerson;

    }
    void FixedUpdate() {
        
        rb.AddForce(direction.normalized * speed * multiplier * Time.deltaTime * 100.0f, ForceMode.Acceleration);

    }

    public void ResetCamera() {

        Player.instance.MainCam.localPosition = camOffset;
        Player.instance.MainCam.localRotation = Quaternion.Euler(camXAxisRotOffset, 0.0f, 0.0f);

    }

}