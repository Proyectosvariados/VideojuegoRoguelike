using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerWeaponHandler weaponHandler;

    void Start()
    {
        weaponHandler = GetComponent<PlayerWeaponHandler>();
        if (weaponHandler == null)
        {
            Debug.LogWarning("PlayerAttack: No PlayerWeaponHandler found on player.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left click detected.");
            if (weaponHandler != null && weaponHandler.IsWeaponEquipped())
            {
                Weapon currentWeapon = weaponHandler.GetCurrentWeapon();
                if (currentWeapon != null)
                {
                    Debug.Log("PlayerAttack: Calling PerformAttack on weapon: " + currentWeapon.name);
                    currentWeapon.PerformAttack();
                }
                else
                {
                    Debug.Log("PlayerAttack: Weapon is not assigned.");
                }
            }
            else
            {
                Debug.Log("PlayerAttack: No weapon equipped.");
            }
        }
    }
}
