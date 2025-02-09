using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Configuración de Ataque")]
    public float attackCooldown = 0.5f;  // Tiempo entre ataques
    private float lastAttackTime = 0f;

    [Header("Referencia al Arma")]
    public Weapon weapon;  // Asigna en el Inspector el script del arma

    void Update()
    {
        // Comprueba si se presiona la tecla espacio y si se cumple el cooldown
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;

        if (weapon != null)
        {
            weapon.PerformAttack();
        }
        else
        {
            Debug.LogWarning("No se ha asignado el arma en PlayerAttack.");
        }
    }
}
