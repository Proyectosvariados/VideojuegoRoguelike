using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Asigna el transform del jugador en el Inspector
    // Offset para posicionar la cámara. Por ejemplo, en 2D es común usar un offset en Z para ver la escena:
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    void LateUpdate()
    {
        if (player != null)
        {
            // Actualiza la posición de la cámara para que siga al jugador
            transform.position = player.position + offset;
        }
    }
}