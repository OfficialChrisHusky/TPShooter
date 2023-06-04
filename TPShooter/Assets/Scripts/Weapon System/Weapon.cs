using UnityEngine;

public class Weapon : MonoBehaviour {
    
    [SerializeField] private string weaponName;
    [SerializeField] private uint weaponID;
    [SerializeField] private float damage = 1.0f;
    [SerializeField] private float hitForce = 1.0f;
    [SerializeField] private float fireRate = 1.0f;

    [Header("Ammo")]
    [SerializeField] private uint ammo = 18;
    [SerializeField] private uint maxAmmo = 18;
    [SerializeField] private uint ammoInInventory = 72;

    float nextFire = -1.0f;

    public bool Shoot() {

        if (Time.time < nextFire) return false;
        if (ammo < 1) return false;

        nextFire = Time.time + 1.0f / fireRate;
        ammo--;
        return true;

    }
    public void Reload() {

        if (ammoInInventory == 0) return;

        uint ammoNeeded = maxAmmo - ammo;
        if(ammoInInventory < ammoNeeded) {

            ammo += ammoInInventory;
            ammoInInventory = 0;
            return;

        }

        ammo += ammoNeeded;
        ammoInInventory -= ammoNeeded;

    }
    public void AddAmmo(uint amount) {

        ammoInInventory += amount;

    }

    public string WeaponName { get {return weaponName; } }
    public uint WeaponID { get { return weaponID; } }
    public float Damage { get { return damage; } }
    public float HitForce { get { return hitForce; } }

}