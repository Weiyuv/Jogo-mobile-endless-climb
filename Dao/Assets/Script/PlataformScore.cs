using UnityEngine;

public class PlatformScore : MonoBehaviour
{
    private bool used = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (used) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            ScoreManager.instance.AddScore(1);
            used = true;
        }
    }
}