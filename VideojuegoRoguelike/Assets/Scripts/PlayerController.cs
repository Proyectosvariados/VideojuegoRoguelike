using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 3f;       // Velocidad de movimiento del personaje
    private Rigidbody2D rb;            // Referencia al componente Rigidbody2D
    private Vector2 movimiento;        // Vector para almacenar la dirección del movimiento

    void Start()
    {
        // Obtiene la referencia al componente Rigidbody2D del GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Inicializa el vector de movimiento en cero en cada frame
        movimiento = Vector2.zero;

        // Comprueba las teclas de dirección para establecer la dirección del movimiento
        if (Input.GetKey(KeyCode.D))
        {
            movimiento.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movimiento.x = -1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            movimiento.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movimiento.y = -1;
        }
    }

    void FixedUpdate()
    {
        // Actualiza la posición del personaje de forma física
        rb.MovePosition(rb.position + movimiento * velocidad * Time.fixedDeltaTime);
    }
}