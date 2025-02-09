using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [Header("Configuración del Arma")]
    public float damage = 10f;           // Daño que causa el arma
    public float attackDuration = 0.2f;  // Duración del swing de ataque
    public Collider2D pickupCollider;  // Collider usado para recoger el arma
    public Collider2D attackCollider;  // Collider usado durante el ataque   // Collider usado para detectar golpes (debe estar en modo Trigger)

    [HideInInspector]
    public bool isEquipped = false;

    // Valores originales para restaurar el arma (se asignan en Awake)
    public Vector3 DefaultLocalPosition { get; private set; }
    public Quaternion DefaultLocalRotation { get; private set; }
    public Vector3 DefaultLocalScale { get; private set; }

    void Awake()
    {
        DefaultLocalPosition = transform.localPosition;
        DefaultLocalRotation = transform.localRotation;
        DefaultLocalScale = transform.localScale;
        Debug.Log("Weapon Awake - Posición: " + DefaultLocalPosition +
                  ", Rotación: " + DefaultLocalRotation.eulerAngles +
                  ", Escala: " + DefaultLocalScale);
    }

    // Este método se utiliza para iniciar el ataque
    public void PerformAttack()
    {
        if (!isEquipped)
        {
            Debug.Log("PerformAttack abortado: El arma no está equipada.");
            return;
        }
        if (attackCollider != null)
            attackCollider.enabled = true;
    }

    // Se desactiva el collider de ataque, por ejemplo, al finalizar el swing
    public void EndAttack()
    {
        if (attackCollider != null)
            attackCollider.enabled = false;
    }

    // Método que detecta colisiones y aplica daño a los enemigos
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el collider de ataque no está activo, no se aplica daño.
        if (attackCollider != null && !attackCollider.enabled)
            return;

        // Verifica que el objeto tenga la etiqueta "Enemy"
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Enemy " + collision.gameObject.name + " recibió " + damage + " de daño.");
            }
        }
    }
}
