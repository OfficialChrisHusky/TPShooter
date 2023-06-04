using UnityEngine;

public class ShootableObject : MonoBehaviour {
    
    [SerializeField] private float health;

    public void Hit(float damage) {

        health -= damage;
        if (health <= 0.0f)
            Break();

    }
    public void Break() {

        Destroy(gameObject);

    }

}