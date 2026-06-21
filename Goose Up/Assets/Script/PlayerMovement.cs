using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 15f;
    public float maxDragDistance = 3f;

    public Animator animator;
    public LineRenderer aimLine;

    public GameObject platformPrefab;

    [Header("Recharge Settings")]
    public int stepsForDoubleTapRecharge = 5;
    public int stepsForPlatformRecharge = 10;

    private int stepsDoubleTap = 0;
    private int stepsPlatform = 0;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Vector2 startPos;
    private Vector2 currentPos;
    private bool isDragging;
    private bool isGrounded;

    private bool doubleJumpAvailable = true;
    private bool platformSpawnAvailable = true;

    private bool isSuspended = false;
    private bool canSecondDrag = false;

    private int tapCount = 0;
    public float tapResetTime = 0.3f;
    private float tapTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (aimLine != null)
        {
            aimLine.positionCount = 2;
            aimLine.enabled = false;
        }
    }

    void Update()
    {
        HandleInput();
        HandleTapReset();

        animator.SetBool("isGrounded", isGrounded);
    }

    void HandleInput()
    {
        Vector2 inputPos;
        bool inputDown;
        bool inputHold;
        bool inputUp;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            inputPos = Camera.main.ScreenToWorldPoint(touch.position);

            inputDown = touch.phase == TouchPhase.Began;
            inputHold = touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary;
            inputUp = touch.phase == TouchPhase.Ended;
        }
        else
        {
            inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            inputDown = Input.GetMouseButtonDown(0);
            inputHold = Input.GetMouseButton(0);
            inputUp = Input.GetMouseButtonUp(0);
        }

        if (inputDown && (isGrounded || canSecondDrag))
        {
            isDragging = true;
            startPos = inputPos;

            if (aimLine != null)
                aimLine.enabled = true;
        }

        if (inputHold && isDragging)
        {
            currentPos = inputPos;

            Vector2 drag = startPos - currentPos;
            drag = Vector2.ClampMagnitude(drag, maxDragDistance);

            if (aimLine != null)
            {
                aimLine.SetPosition(0, transform.position);
                aimLine.SetPosition(1, (Vector2)transform.position + drag);
            }
        }

        if (inputUp && isDragging)
        {
            Vector2 drag = startPos - currentPos;
            drag = Vector2.ClampMagnitude(drag, maxDragDistance);

            Jump(drag);

            isDragging = false;
            isSuspended = false;
            canSecondDrag = false;

            rb.gravityScale = 1f;

            if (aimLine != null)
                aimLine.enabled = false;
        }

        if (inputUp)
        {
            RegisterTap();
        }
    }

    void Jump(Vector2 drag)
    {
        Vector2 direction = drag.normalized;
        float strength = Mathf.InverseLerp(0, maxDragDistance, drag.magnitude);

        rb.linearVelocity = direction * strength * jumpForce;

        if (drag.x > 0) sr.flipX = false;
        if (drag.x < 0) sr.flipX = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;

            bool isPlatform = collision.gameObject.CompareTag("Platform");

            // ✔ DOUBLE JUMP só recarrega ao pisar em Platform
            if (isPlatform)
            {
                stepsDoubleTap++;

                if (stepsDoubleTap >= stepsForDoubleTapRecharge)
                {
                    stepsDoubleTap = 0;
                    doubleJumpAvailable = true;
                }

                stepsPlatform++;

                if (stepsPlatform >= stepsForPlatformRecharge)
                {
                    stepsPlatform = 0;
                    platformSpawnAvailable = true;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    void RegisterTap()
    {
        tapCount++;
        tapTimer = tapResetTime;

        if (tapCount == 2)
        {
            if (!isGrounded)
                UseDoubleTap();

            tapCount = 0;
        }
    }

    void HandleTapReset()
    {
        if (tapCount > 0)
        {
            tapTimer -= Time.deltaTime;

            if (tapTimer <= 0f)
                tapCount = 0;
        }
    }

    void UseDoubleTap()
    {
        if (isGrounded) return;
        if (!doubleJumpAvailable) return;

        doubleJumpAvailable = false;

        isSuspended = true;
        canSecondDrag = true;

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
    }

    void UsePlatformSpawn()
    {
        if (!platformSpawnAvailable) return;

        Vector3 spawnPos = transform.position + Vector3.down * 0.6f;
        Instantiate(platformPrefab, spawnPos, Quaternion.identity);

        platformSpawnAvailable = false;
    }

    public void OnPlatformButtonPressed()
    {
        UsePlatformSpawn();
    }
}