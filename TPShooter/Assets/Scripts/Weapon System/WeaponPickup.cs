using UnityEngine;

public class WeaponPickup : Pickup {
    
    [SerializeField] private Weapon weapon;

    public override void OnPickup() {

        if (!Player.instance.PickupWeapon(weapon)) return;
        Destroy(gameObject);

    }

}