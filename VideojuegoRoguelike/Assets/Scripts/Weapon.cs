using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [Header("Configuración del Arma")]
    public float damage = 10f;           // Daño que causa el arma
    public float attackDuration = 0.2f;  // Tiempo que el colisionador está activo
    public Collider2D attackCollider;    // Colisionador (por ejemplo, BoxCollider2D) asignado en el Inspector

    // Flag para indicar si el arma está equipada
    [HideInInspector]
    public bool isEquipped = false;

    void Start()
    {
        // Desactiva el colisionador al inicio para que no detecte colisiones constantemente
        if (attackCollider != null)
            attackCollider.enabled = false;
        else
            Debug.LogWarning("No se ha asignado el attackCollider en Weapon.");
    }

    // Este método es llamado por PlayerAttack cuando se presiona la tecla espacio
    public void PerformAttack()
    {
        if (attackCollider != null)
        {
            StartCoroutine(AttackCoroutine());
        }
        else
        {
            Debug.LogWarning("attackCollider no asignado en Weapon.");
        }
    }

    IEnumerator AttackCoroutine()
    {
        // Activa el colisionador para detectar colisiones (por ejemplo, con enemigos)
        attackCollider.enabled = true;

        // Espera el tiempo definido para el ataque
        yield return new WaitForSeconds(attackDuration);

        // Desactiva el colisionador
        attackCollider.enabled = false;
    }

    // Método que detecta colisiones y aplica daño solo si el arma está equipada
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el arma no está equipada, no se aplica daño
        if (!isEquipped) return;

        // Se comprueba si el objeto colisionado tiene la etiqueta "Enemy"
        if (collision.CompareTag("Enemy"))
        {
            // Ejemplo: acceder al script del enemigo y aplicar daño
            // Enemy enemy = collision.GetComponent<Enemy>();
            // if (enemy != null) { enemy.TakeDamage(damage); }
            Debug.Log("Enemigo golpeado. Daño: " + damage);
        }
    }
}
