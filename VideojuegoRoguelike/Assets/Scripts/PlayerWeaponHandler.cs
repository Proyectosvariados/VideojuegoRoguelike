using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    [Header("Configuración de recogida")]
    public Transform weaponHolder;   // Objeto hijo del jugador que actuará como contenedor del arma
    public LayerMask weaponLayer;    // Capa asignada a los objetos arma
    public float pickupRadius = 2f;  // Radio para detectar armas (opcional)

    private Weapon currentWeapon;    // Arma actualmente equipada
    private Weapon candidateWeapon;  // Arma candidata (detectada por trigger)

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D detectado en: " + collision.gameObject.name);
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("OnTriggerExit2D detectado en: " + collision.gameObject.name);
        if (candidateWeapon != null && collision.gameObject == candidateWeapon.gameObject)
        {
            Debug.Log("El arma candidata ha salido del rango: " + candidateWeapon.name);
            candidateWeapon = null;
        }
    }

void PickupWeapon(Weapon weapon)
{
    currentWeapon = weapon;
    // Asocia el arma al WeaponHolder sin conservar las transformaciones globales.
    weapon.transform.SetParent(weaponHolder, false);
    // Establece la rotación en -90 grados (sobre el eje Z)
    weapon.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
    // Posiciona el arma con un offset; en este caso se mantiene el valor actual (0.5,0,0)
    weapon.transform.localPosition = new Vector3(0.5f, 0.0f, 0f);
    // Restaura la escala original para evitar deformaciones
    weapon.transform.localScale = weapon.DefaultLocalScale;
    Debug.Log("Posición del arma establecida en (0.5,0,0) y rotada -90°, escala: " + weapon.DefaultLocalScale);

    Rigidbody2D rb = weapon.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.simulated = false;
    }
    weapon.isEquipped = true;
    Debug.Log("Arma " + weapon.name + " recogida y equipada.");
}


    void DropWeapon()
    {
        currentWeapon.transform.SetParent(null);
        Rigidbody2D rb = currentWeapon.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;
        }
        // Restaura los valores originales del arma
        
        currentWeapon.transform.localRotation = currentWeapon.DefaultLocalRotation;
        currentWeapon.transform.localScale = currentWeapon.DefaultLocalScale;
        currentWeapon.isEquipped = false;
        Debug.Log("Arma " + currentWeapon.name + " soltada y valores restaurados.");
        currentWeapon = null;
    }


    // Métodos públicos para que otros scripts (como PlayerAttack) consulten el arma equipada
    public bool IsWeaponEquipped() { return currentWeapon != null; }
    public Weapon GetCurrentWeapon() { return currentWeapon; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
