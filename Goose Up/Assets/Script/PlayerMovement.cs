using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 15f;
    public float maxDragDistance = 3f;

    public Animator animator;
    public LineRenderer aimLine;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Vector2 startPos;
    private Vector2 currentPos;
    private bool isDragging;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (aimLine != null)
        {
            aimLine.positionCount = 2;
            aimLine.enabled = false;

            aimLine.SetPosition(0, transform.position);
            aimLine.SetPosition(1, transform.position);
        }
    }

    void Update()
    {
        Vector2 inputPos = Vector2.zero;
        bool inputDown = false;
        bool inputHold = false;
        bool inputUp = false;

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

        if (inputDown && isGrounded)
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

        if (inputUp && isDragging && isGrounded)
        {
            Vector2 drag = startPos - currentPos;
            drag = Vector2.ClampMagnitude(drag, maxDragDistance);

            Vector2 direction = drag.normalized;
            float strength = Mathf.InverseLerp(0, maxDragDistance, drag.magnitude);

            rb.linearVelocity = direction * strength * jumpForce;

            if (drag.x > 0) sr.flipX = false;
            if (drag.x < 0) sr.flipX = true;

            isDragging = false;

            if (aimLine != null)
            {
                aimLine.enabled = false;
                aimLine.SetPosition(0, transform.position);
                aimLine.SetPosition(1, transform.position);
            }
        }

        animator.SetBool("isGrounded", isGrounded);

        if (!isDragging && aimLine != null && !aimLine.enabled)
        {
            aimLine.SetPosition(0, transform.position);
            aimLine.SetPosition(1, transform.position);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}