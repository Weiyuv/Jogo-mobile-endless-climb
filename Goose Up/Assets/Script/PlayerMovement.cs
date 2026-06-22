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
    private Vector2 inputPos;

    private bool isDragging;
    private bool isGrounded;

    private bool doubleJumpAvailable = true;
    private bool platformSpawnAvailable = true;

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

        UpdatePlatformUI();
        UpdateDoubleTapUI();
    }

    void Update()
    {
        HandleInput();
        HandleTapReset();

        animator.SetBool("isGrounded", isGrounded);
    }

    void LateUpdate()
    {
        if (!isDragging || aimLine == null) return;

        Vector2 drag = startPos - inputPos;
        drag = Vector2.ClampMagnitude(drag, maxDragDistance);

        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, (Vector2)transform.position + drag);
    }

    void HandleInput()
    {
        bool inputDown;
        bool inputUp;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            inputPos = Camera.main.ScreenToWorldPoint(touch.position);

            inputDown = touch.phase == TouchPhase.Began;
            inputUp = touch.phase == TouchPhase.Ended;
        }
        else
        {
            inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            inputDown = Input.GetMouseButtonDown(0);
            inputUp = Input.GetMouseButtonUp(0);
        }

        if (inputDown && (isGrounded || canSecondDrag))
        {
            isDragging = true;
            startPos = inputPos;

            if (aimLine != null)
                aimLine.enabled = true;
        }

        if (inputUp && isDragging)
        {
            isDragging = false;

            Vector2 drag = startPos - inputPos;
            drag = Vector2.ClampMagnitude(drag, maxDragDistance);

            Jump(drag);

            if (aimLine != null)
                aimLine.enabled = false;

            canSecondDrag = false;
            rb.gravityScale = 1f;
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

            if (collision.gameObject.CompareTag("Platform"))
            {
                Platform platform = collision.collider.GetComponentInParent<Platform>();

                if (platform != null && platform.alreadyCounted)
                    return;

                if (platform != null)
                    platform.alreadyCounted = true;

                // 🔥 SÓ conta depois que habilidade foi usada
                if (!doubleJumpAvailable)
                {
                    stepsDoubleTap++;

                    if (stepsDoubleTap >= stepsForDoubleTapRecharge)
                    {
                        stepsDoubleTap = 0;
                        doubleJumpAvailable = true;
                        UpdateDoubleTapUI();
                    }
                }

                if (!platformSpawnAvailable)
                {
                    stepsPlatform++;

                    if (stepsPlatform >= stepsForPlatformRecharge)
                    {
                        stepsPlatform = 0;
                        platformSpawnAvailable = true;
                        UpdatePlatformUI();
                    }
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
        if (isGrounded || !doubleJumpAvailable) return;

        doubleJumpAvailable = false;

        canSecondDrag = true;

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        UpdateDoubleTapUI();
    }

    void UsePlatformSpawn()
    {
        if (!platformSpawnAvailable) return;

        Vector3 spawnPos = transform.position + Vector3.down * 1f;
        Instantiate(platformPrefab, spawnPos, Quaternion.identity);

        platformSpawnAvailable = false;

        UpdatePlatformUI();
    }

    void UpdatePlatformUI()
    {
        if (GameManager.instance != null)
            GameManager.instance.SetPlatformReady(platformSpawnAvailable);
    }

    void UpdateDoubleTapUI()
    {
        if (GameManager.instance != null)
            GameManager.instance.SetDoubleTapReady(doubleJumpAvailable);
    }

    public void OnPlatformButtonPressed()
    {
        UsePlatformSpawn();
    }
}