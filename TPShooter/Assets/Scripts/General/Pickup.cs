using UnityEngine;

public abstract class Pickup : MonoBehaviour {
    
    public abstract void OnPickup();

    void OnTriggerEnter(Collider other) {
        
        if (other.tag != "Player") return;
        OnPickup();

    }

}