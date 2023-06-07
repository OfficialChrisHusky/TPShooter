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

    [Header("Wall Running")]
    [SerializeField] private bool onWall;
    [SerializeField] private float wallDistance = 0.6f;
    [SerializeField] private float minJumpHeight = 1.5f;
    [SerializeField] private float wallRunGravity = 1.0f;
    [SerializeField] private float wallRunMultiplier = 1.5f;
    [SerializeField] private float cancelJumpForce;

    [Header("Ground Detection")]
    [SerializeField] private bool grounded;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;

    private Rigidbody rb;
    private Vector3 force;

    private Vector3 direction;

    private Vector2 mouseRotation;

    private RaycastHit wallHit;

    void Start() {
        
        rb = GetComponent<Rigidbody>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update() {

        CheckWall();
        
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
            multiplier = 1.0f;

        }

        if(grounded && Input.GetKeyDown(KeyCode.LeftShift)) multiplier = sprintMultiplier;
        else if(grounded && Input.GetKeyUp(KeyCode.LeftShift)) multiplier = 1.0f;

        if(grounded && Input.GetKeyDown(KeyCode.Space)) {

            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        }

    }
    void FixedUpdate() {

        if(onWall) {

            multiplier = wallRunMultiplier;
            rb.useGravity = false;
            rb.AddForce(Vector3.down * wallRunGravity);

            if(Input.GetKeyDown(KeyCode.Space)) {

                Vector3 cancelDir = transform.up + wallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
                rb.AddForce(cancelDir * cancelJumpForce, ForceMode.Impulse);

            }

        } else {

            rb.useGravity = true;
            if(grounded && Input.GetKey(KeyCode.LeftShift))
                multiplier = sprintMultiplier;
            else if(grounded && !Input.GetKey(KeyCode.LeftShift))
                multiplier = 1.0f;
            else
                multiplier = 1.0f;

        }

        rb.AddForce(direction.normalized * speed * multiplier * Time.deltaTime * 100.0f, ForceMode.Acceleration);

    }

    void CheckWall() {

        if(grounded) {
            
            onWall = false;
            return;
            
        }

        onWall = Physics.Raycast(transform.position, -transform.right, out wallHit, wallDistance);
        
        if (onWall) return;
        onWall = Physics.Raycast(transform.position,  transform.right, out wallHit, wallDistance);

    }

    public void ResetCamera() {

        Player.instance.MainCam.localPosition = camOffset;
        Player.instance.MainCam.localRotation = Quaternion.Euler(camXAxisRotOffset, 0.0f, 0.0f);

    }

}