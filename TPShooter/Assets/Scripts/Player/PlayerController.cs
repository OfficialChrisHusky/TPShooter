using System.Runtime.Serialization;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;
    void Awake() { instance = this; }
    
    public float multiplier = 1.0f;
    [Header("Movement")]
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float sprintMultiplier = 1.2f;
    [SerializeField] private float groundDrag = 2.0f;

    [Header("Looking around")]
    [SerializeField] private float sensitivity = 1.0f;
    [SerializeField] private float minX = -70.0f;
    [SerializeField] private float maxX = 10.0f;
    [SerializeField] private Vector3 camOffset = new Vector3(0.0f, 1.35f, -3.35f);
    [SerializeField] private float camXAxisRotOffset = 9.0f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float airDrag = 5.0f;

    [Header("Ground Detection")]
    [SerializeField] private bool grounded;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;

    private Rigidbody rb;

    private Vector3 direction;

    private Vector2 mouseRotation;

    void Start() {
        
        rb = GetComponent<Rigidbody>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update() {
        
        mouseRotation.y += Input.GetAxis("Mouse X") * sensitivity;
        mouseRotation.x -= Input.GetAxis("Mouse Y") * sensitivity;
        
        mouseRotation.x = Mathf.Clamp(mouseRotation.x, minX, maxX);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical   = Input.GetAxisRaw("Vertical");
        direction = transform.forward * vertical + transform.right * horizontal;

        transform.rotation = Quaternion.Euler(0.0f, mouseRotation.y, 0.0f);
        Player.instance.Eyes.rotation = Quaternion.Euler(mouseRotation.x, mouseRotation.y, 0.0f);

        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (grounded && rb.drag != groundDrag) {

            rb.drag = groundDrag;
            multiplier = 1.0f;

        } else if(!grounded && rb.drag != airDrag) {

            rb.drag = airDrag;
            multiplier = 0.3f;

        }

        if(grounded && Input.GetKeyDown(KeyCode.LeftShift)) multiplier = sprintMultiplier;
        else if(grounded && Input.GetKeyUp(KeyCode.LeftShift)) multiplier = 1.0f;

        if(grounded && Input.GetKeyDown(KeyCode.Space)) {

            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        }

    }
    void FixedUpdate() {
        
        rb.AddForce(direction.normalized * speed * multiplier * Time.deltaTime * 100.0f, ForceMode.Acceleration);

    }

    public void ResetCamera() {

        Player.instance.MainCam.localPosition = camOffset;
        Player.instance.MainCam.localRotation = Quaternion.Euler(camXAxisRotOffset, 0.0f, 0.0f);

    }

}