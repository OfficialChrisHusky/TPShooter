using UnityEngine;

public class AmmoPickup : MonoBehaviour {
    
    [SerializeField] private uint amount = 18;
    [SerializeField] private Weapon weapon;

    void Pickup() {

        if (!Player.instance.PickupAmmo(weapon.WeaponID, amount)) return;
        Destroy(gameObject);

    }

    void OnTriggerEnter(Collider other) {
        
        if (other.tag != "Player") return;
        Pickup();

    }

}