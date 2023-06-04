using UnityEngine;

public class EquipmentPickup : Pickup {
    
    [SerializeField] private Equipment equipment;
    [SerializeField] private uint count = 1;

    public override void OnPickup() {

        count -= Player.instance.PickupEquipment(equipment, count);
        if(count > 0) return;

        Destroy(gameObject);

    }

}