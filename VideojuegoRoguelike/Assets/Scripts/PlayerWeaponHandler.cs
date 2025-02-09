using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    [Header("Configuración de recogida")]
    public Transform weaponHolder;   // Objeto hijo del jugador donde se acoplará el arma
    public LayerMask weaponLayer;    // Capa en la que se encuentran los objetos arma

    // (Opcional) Radio para visualizar en el editor el área de pickup
    public float pickupRadius = 2f;

    private Weapon currentWeapon;    // Arma actualmente equipada
    private Weapon candidateWeapon;  // Arma candidata a recoger (detectada por trigger)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentWeapon != null)
            {
                Debug.Log("Soltando arma: " + currentWeapon.name);
                DropWeapon();
            }
            else if (candidateWeapon != null)
            {
                Debug.Log("Recogiendo arma: " + candidateWeapon.name);
                PickupWeapon(candidateWeapon);
                candidateWeapon = null;
            }
            else
            {
                Debug.Log("No hay arma en rango para recoger.");
            }
        }
    }

    // Se ejecuta cuando el jugador entra en contacto con un objeto (su collider debe ser trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D detectado en: " + collision.gameObject.name);

        // Comprueba si el objeto colisionado está en la capa definida en weaponLayer
        if (((1 << collision.gameObject.layer) & weaponLayer) != 0)
        {
            Weapon weapon = collision.GetComponent<Weapon>();
            if (weapon != null)
            {
                Debug.Log("Arma candidata encontrada: " + weapon.name);
                candidateWeapon = weapon;
            }
        }
    }

    // Se ejecuta cuando el jugador sale del área del objeto detectado
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("OnTriggerExit2D detectado en: " + collision.gameObject.name);
        if (candidateWeapon != null && collision.gameObject == candidateWeapon.gameObject)
        {
            Debug.Log("El arma candidata ha salido del rango: " + candidateWeapon.name);
            candidateWeapon = null;
        }
    }

    /// <summary>
    /// Recoge el arma indicada y la acopla al weaponHolder.
    /// </summary>
    /// <param name="weapon">Arma a recoger</param>
    void PickupWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        // Se acopla el arma al objeto weaponHolder
        weapon.transform.SetParent(weaponHolder);
        // Posición local: 0.5 unidades a la derecha (puedes ajustar Y o Z si es necesario)
        weapon.transform.localPosition = new Vector3(0.5f, 0f, 0f);
        // Desactiva la física para que no interfiera mientras está equipada
        Rigidbody2D rb = weapon.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false;
        }
        Debug.Log("Arma recogida: " + weapon.name + " y posicionada a 0.5 a la derecha.");
    }

    /// <summary>
    /// Suelta el arma actualmente equipada.
    /// </summary>
    void DropWeapon()
    {
        // Desacopla el arma del jugador
        currentWeapon.transform.SetParent(null);
        // Reactiva la física para que interactúe con el entorno
        Rigidbody2D rb = currentWeapon.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;
        }
        Debug.Log("Arma soltada: " + currentWeapon.name);
        currentWeapon = null;
    }

    // (Opcional) Visualización del radio de recogida en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
