using UnityEngine;

public class WeaponPickup : MonoBehaviour {
    
    [SerializeField] private Weapon weapon;

    void Pickup() {

        if (!Player.instance.PickupWeapon(weapon)) return;
        Destroy(gameObject);

    }

    void OnTriggerEnter(Collider other) {
        
        if (other.tag != "Player") return;
        Pickup();

    }

}