using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstPersonController : Singleton<FirstPersonController> {
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float mouseSensitivity = 2f;

    [Header("Jump and Gravity Settings")]
    [SerializeField] private float fallMultiplier = 15f;    // Increased gravity while falling
    [SerializeField] private float lowJumpMultiplier = 12f; // Additional gravity when releasing jump button
    [SerializeField] private float regularGravityScale = 8f;// Regular gravity scale

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController controller;
    private Camera playerCamera;
    private float xRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;
    public float stamina = 100f, health = 100f;
    public Image staminaBar, healthBar;
    public Transform shootingPos;
    public GameObject shootingPrefab;
    public float shootingCooldown = 0.05f;
        
    public float staminaLostForJump = 30f,
        staminaLostForSprintPerSecond = 5f,
        staminaRecoverInWalking = 2f,
        staminaRecoverInStanding = 10f;

    private void Start() {
        health *= StatsChecker.Instance.finalHealthMultiplier;
        
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(HandleShooting());
    }

    private void Update() {
        HandleMovement();
        HandleMouseLook();
        HandleJump();
        HandleUi();
        HandleStats();
    }

    private void HandleMovement() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 move = transform.right * x + transform.forward * z;
        bool walking = !Mathf.Approximately(move.x + move.z, 0f);
        
        float currentSpeed;
        if (walking && Input.GetKey(KeyCode.LeftShift) && stamina > 1f) {
            currentSpeed = moveSpeed * sprintMultiplier;
            stamina -= staminaLostForSprintPerSecond * Time.deltaTime;
        } else if (walking) {
            currentSpeed = moveSpeed;
            stamina += staminaRecoverInWalking * StatsChecker.Instance.finalStaminaRecovery * Time.deltaTime;
        }
        else {
            currentSpeed = 0;
            stamina += staminaRecoverInStanding * StatsChecker.Instance.finalStaminaRecovery * Time.deltaTime;
        }

        currentSpeed *= StatsChecker.Instance.finalSpeedMultiplier;
            
        controller.Move(move * currentSpeed * Time.deltaTime);
        
        // Apply different gravity scales based on vertical movement
        float gravityMultiplier = regularGravityScale;
        
        if (velocity.y < 0) {
            // Falling
            gravityMultiplier = fallMultiplier;
        }
        else if (velocity.y > 0 && !Input.GetButton("Jump")) {
            // Rising but jump button released (for shorter jumps)
            gravityMultiplier = lowJumpMultiplier;
        }
        
        velocity.y += Physics.gravity.y * Time.deltaTime * gravityMultiplier;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleMouseLook() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleJump() {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && stamina > staminaLostForJump) {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            stamina -= staminaLostForJump;
        }
    }

    private void HandleUi() {
        staminaBar.fillAmount = stamina / 100f;
        healthBar.fillAmount = health / (100f * StatsChecker.Instance.finalHealthMultiplier);
    }

    private void HandleStats() {
        stamina = Mathf.Clamp(stamina, 0, 100);
        health = Mathf.Clamp(health, 0, (100f * StatsChecker.Instance.finalHealthMultiplier));
    }

    private IEnumerator HandleShooting() {
        while (true) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                var newInst = Object.Instantiate(shootingPrefab, shootingPos.position, Quaternion.identity);
                newInst.GetComponent<Rigidbody>().velocity = playerCamera.transform.forward * 100;
                yield return new WaitForSeconds(shootingCooldown * StatsChecker.Instance.finalFireRate);
            }

            yield return null;
            
        }
    }

    public void DealDamage(float damage) {
        health -= damage;

        if (health == 0) {
            SceneManager.LoadScene("MainMenu");
        }
    }
}