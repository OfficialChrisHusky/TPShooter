using UnityEngine;

public class AmmoPickup : Pickup {
    
    [SerializeField] private uint amount = 18;
    [SerializeField] private Weapon weapon;

    public override void OnPickup() {

        if (!Player.instance.PickupAmmo(weapon.WeaponID, amount)) return;
        Destroy(gameObject);

    }

}