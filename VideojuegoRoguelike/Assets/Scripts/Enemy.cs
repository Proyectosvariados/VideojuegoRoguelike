using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Configuración del Enemigo")]
    public float maxHealth = 100f;    // Salud máxima del enemigo
    private float currentHealth;      // Salud actual

    // Opcional: velocidad de movimiento si deseas agregar comportamiento
    public float moveSpeed = 2f;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log(gameObject.name + " iniciado. Salud: " + currentHealth);
    }

    /// <summary>
    /// Aplica daño al enemigo. Si la salud cae a cero o menos, se destruye.
    /// </summary>
    /// <param name="damage">Cantidad de daño a aplicar</param>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " recibió " + damage + " de daño. Salud restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Ejecuta la lógica de muerte del enemigo.
    /// </summary>
    void Die()
    {
        Debug.Log(gameObject.name + " ha muerto.");
        // Aquí puedes añadir efectos de muerte, animaciones, etc.
        Destroy(gameObject);
    }

    // Opcional: ejemplo de movimiento básico (puedes personalizarlo o quitarlo)
    void Update()
    {
        // Ejemplo: mover al enemigo hacia la izquierda de forma constante
        // transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    // Opcional: si deseas detectar colisiones con ataques (por ejemplo, con el arma)
void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("WeaponAttack"))
    {
        Weapon weaponComponent = collision.GetComponent<Weapon>();
        if (weaponComponent != null)
        {
            // Usa el daño proporcionado por el arma
            float damage = weaponComponent.damage;
            TakeDamage(damage);
            Debug.Log(gameObject.name + " recibió " + damage + " de daño desde el arma.");
        }
    }
}

}
