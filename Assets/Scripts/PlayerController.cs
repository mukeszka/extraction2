using UnityEngine;
using FishNet.Object;

public class PlayerController : NetworkBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public float rotationSpeed = 15f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundDistance = 0.2f;

    private Rigidbody rb;
    private Animator anim;
    private bool isGrounded;
    private Vector3 moveInput;
    private bool isRunning;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        // A kamera csak a saját karakterünket kövesse
        if (IsOwner)
        {
            CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
            if (cam != null) cam.target = transform;
        }
        else
        {
            // Ha nem a mi karakterünk, a Rigidbody-t ne engedjük fizikailag
            // "harcolni" a hálózati pozíció-szinkronnal
            rb.isKinematic = true;
        }
    }

    private void Update()
    {
        // Csak a saját karakterünkön fusson az input-kezelés
        if (!IsOwner) return;

        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundDistance, groundLayer);

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(moveX, 0f, moveZ).normalized;

        isRunning = Input.GetKey(KeyCode.LeftShift);

        if (anim != null)
        {
            float animSpeed = moveInput.magnitude * (isRunning ? 2f : 1f);
            anim.SetFloat("Speed", animSpeed);
            anim.SetBool("IsGrounded", isGrounded);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (anim != null) anim.SetTrigger("Jump");
        }

        if (anim != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) anim.SetTrigger("Dance1");
            if (Input.GetKeyDown(KeyCode.Alpha2)) anim.SetTrigger("Dance2");
            if (Input.GetKeyDown(KeyCode.Alpha3)) anim.SetTrigger("Dance3");
            if (Input.GetKeyDown(KeyCode.Alpha4)) anim.SetTrigger("Dance4");
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        if (moveInput.magnitude >= 0.1f)
        {
            float currentSpeed = isRunning ? runSpeed : moveSpeed;
            Vector3 targetPosition = rb.position + moveInput * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);

            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }
}