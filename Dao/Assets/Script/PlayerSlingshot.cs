using UnityEngine;

public class PlayerSlingshot : MonoBehaviour
{
    public float forceMultiplier = 5f;

    private Rigidbody2D rb;
    private Vector2 startTouch;
    private Vector2 endTouch;
    private bool dragging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // começo do toque
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouch = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            dragging = true;
        }

        // soltou o dedo
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && dragging)
        {
            endTouch = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            Vector2 force = startTouch - endTouch;

            rb.AddForce(force * forceMultiplier, ForceMode2D.Impulse);

            dragging = false;
        }
    }
}