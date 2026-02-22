using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Transform camTransform;

    public float walkspeed = 5f, runspeed = 11f, jumppower = 13f, fallspeed = 22f;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    private bool grounded = true;
    private bool wantsToJump = false;
    private Animator anim; // Variabel untuk menyimpan komponen Animator
    public float animationSmoothTime = 0.1f; // Untuk memperhalus transisi animasi

    [Header("Sound Effects")]
    public AudioSource audioSource; // Drag komponen AudioSource ke sini di Inspector
    public AudioClip[] stepSounds; // Masukkan beberapa file suara langkah kaki di sini

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        camTransform = Camera.main.transform;

        // --- TAMBAHAN ---
        // Dapatkan komponen Animator
        // Gunakan GetComponentInChildren jika Animator ada di model (child object)
        // Gunakan GetComponent jika Animator ada di object yang sama dengan script ini
        anim = GetComponentInChildren<Animator>(); 
        if (anim == null)
        {
            Debug.LogError("Komponen Animator tidak ditemukan!");
        }
        // --- AKHIR TAMBAHAN ---

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            wantsToJump = true;
        }
    }

    void FixedUpdate()
    {
        Movement();
        Jump();

        if (!grounded)
        {
            rb.AddForce(Vector3.down * fallspeed * rb.mass, ForceMode.Force);
        }

        // --- TAMBAHAN ---
        // Selalu update animator apakah kita di darat atau tidak
        anim.SetBool("isGrounded", grounded);
        // --- AKHIR TAMBAHAN ---
    }

    private void Movement()
    {
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        moveDirection = (camForward * verticalInput) + (camRight * horizontalInput);
        
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runspeed : walkspeed;

        rb.velocity = new Vector3(moveDirection.normalized.x * currentSpeed,
                                  rb.velocity.y, 
                                  moveDirection.normalized.z * currentSpeed);

        // --- TAMBAHAN: UPDATE ANIMATOR SPEED ---
        
        // 1. Tentukan target speed untuk animator
        // 0 = Idle, 1 = Walk, 2 = Run
        float targetAnimationSpeed = 0f;
        if (moveDirection.magnitude > 0.1f) // Jika ada input gerakan
        {
            targetAnimationSpeed = isRunning ? 2f : 1f;
        }

        // 2. Dapatkan speed animator saat ini
        float currentAnimationSpeed = anim.GetFloat("Speed");

        // 3. Set float "Speed" di animator secara mulus (smooth)
        anim.SetFloat("Speed", Mathf.Lerp(currentAnimationSpeed, targetAnimationSpeed, Time.fixedDeltaTime / animationSmoothTime));
        
        // --- AKHIR TAMBAHAN ---

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 15f));
        }
    }

    private void Jump()
    {
        if (wantsToJump)
        {
            rb.AddForce(transform.up * jumppower, ForceMode.Impulse);
            wantsToJump = false;
            grounded = false;

            // --- TAMBAHAN ---
            // Panggil trigger "Jump" di animator
            anim.SetTrigger("Jump");
            // --- AKHIR TAMBAHAN ---
        }
    }

    public void SetGrounded(bool state)
    {
        grounded = state;
    }

    public void Footstep()
    {
        // Cek apakah ada suara yang dimasukkan dan player sedang di tanah
        // Kita cek 'grounded' agar tidak bunyi saat melayang/lompat
        if (stepSounds.Length > 0 && grounded)
        {
            // Pilih satu suara secara acak (agar tidak monoton)
            int index = Random.Range(0, stepSounds.Length);

            // Ubah pitch sedikit agar terdengar lebih natural (variasi nada)
            audioSource.pitch = Random.Range(0.9f, 1.1f);

            // Mainkan suaranya
            audioSource.PlayOneShot(stepSounds[index]);
        }
    }
}