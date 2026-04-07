using UnityEngine;

public class PlatformScore : MonoBehaviour
{
    private bool used = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (used) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            
            float playerBottom = collision.transform.position.y - collision.collider.bounds.extents.y;
            float platformTop = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;

            
            if (playerBottom > platformTop - 0.1f) 
            {
                ScoreManager.instance.AddScore(1);
                used = true;
            }
        }
    }
}