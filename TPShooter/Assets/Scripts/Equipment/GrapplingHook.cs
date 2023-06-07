using UnityEngine;

public class GrapplingHook : Equipment {

    [Header("Grapple Hook")]
    [SerializeField] private uint ammo = 10;
    [SerializeField] private uint maxAmmo = 10;
    [SerializeField] private bool grappling = false;
    [SerializeField] private float maxGrappleDistance = 1000.0f;
    [SerializeField] private float grappleSpeed = 1.0f;
    [SerializeField] private LayerMask grappleMask;

    Vector3 grapplePoint;

    void Update() {
        
        if (grappling && Input.GetKeyDown(KeyCode.X)) grappling = false;
        if (!grappling) return;

        Rigidbody rb = Player.instance.Rb;
        rb.AddForce((grapplePoint - Player.instance.transform.position) * grappleSpeed, ForceMode.Impulse);

    }

    public override void Unequip() {

        grappling = false;
        if (Player.instance.ADS)
            Player.instance.ToggleADS();
        
        base.Unequip();

    }

    public override void Use() {

        if (ammo < 1) return;

        RaycastHit hit;
        if (!Physics.Raycast(Player.instance.Eyes.position, Player.instance.MainCam.forward, out hit, maxGrappleDistance, grappleMask)) return;

        ammo--;
        grappling = true;
        grapplePoint = hit.point;

    }
    public override void SecondaryUse() {

        Player.instance.ToggleADS();

    }

    public override uint Add(uint count) {

        uint maxAdd = maxAmmo - ammo;
        uint ret;
        if (maxAdd > count) ret = count;
        else ret = maxAdd;

        ammo += ret;
        return ret;

    }
    
}