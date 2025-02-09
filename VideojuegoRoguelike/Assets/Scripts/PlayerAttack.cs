using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    private PlayerWeaponHandler weaponHandler;

    // Duración total del swing y ángulo total del swing (por ejemplo, 25°)
    public float swingDuration = 0.2f;
    public float swingAngle = 25f;

    // Offset base (en estado reposo) que se desea que tenga el arma
    // En este ejemplo, (0, 0.5, 0)
    public Vector3 baseOffset = new Vector3(0f, 0.5f, 0f);

    void Start()
    {
        weaponHandler = GetComponent<PlayerWeaponHandler>();
        if (weaponHandler == null)
        {
            Debug.LogWarning("PlayerAttack: No se encontró PlayerWeaponHandler en el jugador.");
        }
        else
        {
            Debug.Log("PlayerAttack iniciado.");
        }
    }

    void Update()
    {
        // Ajusta la orientación horizontal del jugador según la posición del mouse
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        if (mouseWorldPos.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            Debug.Log("Orientación: izquierda.");
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            Debug.Log("Orientación: derecha.");
        }

        // Detecta el clic izquierdo para iniciar el ataque
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clic izquierdo detectado.");
            if (weaponHandler != null && weaponHandler.IsWeaponEquipped())
            {
                Weapon currentWeapon = weaponHandler.GetCurrentWeapon();
                if (currentWeapon != null)
                {
                    Debug.Log("Iniciando swing de ataque con el arma: " + currentWeapon.name);
                    StartCoroutine(PerformSwingAttack(currentWeapon));
                }
                else
                {
                    Debug.Log("PlayerAttack: El arma es null.");
                }
            }
            else
            {
                Debug.Log("PlayerAttack: No hay arma equipada.");
            }
        }
    }
IEnumerator PerformSwingAttack(Weapon weapon)
{
    // Obtén el WeaponHolder (padre del arma)
    Transform holder = weapon.transform.parent;
    // Obtén la posición del ratón en mundo y conviértela al espacio local del holder
    Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mouseWorldPos.z = 0f;
    Vector3 localMousePos = holder.InverseTransformPoint(mouseWorldPos);
    
    // Calcula el ángulo "raw" en el espacio local del WeaponHolder
    float rawAngle = Mathf.Atan2(localMousePos.y, localMousePos.x) * Mathf.Rad2Deg;
    // Ajusta el ángulo restándole 90° para tener en cuenta que el arma, al equiparse, ya se rota -90°.
    float targetAngle = rawAngle - 90f;
    Debug.Log("PerformSwingAttack: rawAngle = " + rawAngle + ", targetAngle = " + targetAngle + " (en espacio local del WeaponHolder)");

    // Define el arco de swing: swingAngle total centrado en targetAngle.
    float halfSwing = swingAngle / 2f;
    float startAngle = targetAngle - halfSwing;
    float endAngle = targetAngle + halfSwing;

    // Primera mitad del swing: desde startAngle hasta endAngle.
    float elapsed = 0f;
    float halfDuration = swingDuration / 2f;
    while (elapsed < halfDuration)
    {
        elapsed += Time.deltaTime;
        float t = elapsed / halfDuration;
        float currentAngle = Mathf.Lerp(startAngle, endAngle, t);
        // Rota el vector baseOffset por currentAngle para obtener la posición actual.
        Vector3 newLocalPos = Quaternion.Euler(0, 0, currentAngle) * baseOffset;
        weapon.transform.localPosition = newLocalPos;
        weapon.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
        Debug.Log("Swing - Primera mitad: t = " + t + ", currentAngle = " + currentAngle + ", newLocalPos = " + newLocalPos);
        yield return null;
    }
    // Segunda mitad del swing: regresa desde endAngle hasta targetAngle.
    elapsed = 0f;
    while (elapsed < halfDuration)
    {
        elapsed += Time.deltaTime;
        float t = elapsed / halfDuration;
        float currentAngle = Mathf.Lerp(endAngle, targetAngle, t);
        Vector3 newLocalPos = Quaternion.Euler(0, 0, currentAngle) * baseOffset;
        weapon.transform.localPosition = newLocalPos;
        weapon.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
        Debug.Log("Swing - Segunda mitad: t = " + t + ", currentAngle = " + currentAngle + ", newLocalPos = " + newLocalPos);
        yield return null;
    }
    // Al finalizar, fija el arma en targetAngle.
    Vector3 finalLocalPos = Quaternion.Euler(0, 0, targetAngle) * baseOffset;
    weapon.transform.localPosition = finalLocalPos;
    weapon.transform.localRotation = Quaternion.Euler(0, 0, targetAngle);
    Debug.Log("SwingAttack completado. Ángulo final: " + targetAngle + ", posición final: " + finalLocalPos);
}

}
