using UnityEngine;

public class CameraTargetYOnly : MonoBehaviour
{
    public Transform player;
    private float fixedX;

    void Start()
    {
        fixedX = transform.position.x; // trava X inicial
    }

    void LateUpdate()
    {
        transform.position = new Vector3(
            fixedX,                 // X travado
            player.position.y,      // segue só Y
            transform.position.z
        );
    }
}