using System.Diagnostics;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour {

    public static Player instance;
    void Awake() { instance = this; }
    
    [Header("Body Parts")]
    [SerializeField] private Transform mainCam;
    [SerializeField] private Transform eyes;

    [Header("Weapons")]
    [SerializeField] private List<Weapon> weapons = new List<Weapon>(2);
    [SerializeField] private int currentWeaponIndex;
    [SerializeField] private TMP_Text weaponText;
    [SerializeField] private Vector3 weaponOffset;

    [Header("ADS")]
    [SerializeField] private bool ads = false;
    [SerializeField] private Vector3 adsCamOffset = new Vector3(0.6f ,0.35f ,-1.15f);

    void Start() {
        
        if (!weapons[currentWeaponIndex]) return;

        weaponText.text = "Weapon: " + weapons[currentWeaponIndex].WeaponName;

    }

    void Update() {
        
        if (!weapons[currentWeaponIndex]) return;

        if (Input.GetKey(KeyCode.Mouse0))
            Shoot();
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
            ToggleADS();
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
            SwitchWeapon(0);
        if(Input.GetKeyDown(KeyCode.Alpha2))
            SwitchWeapon(1);
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue > 0.0f)
            SwitchWeapon(currentWeaponIndex + 1);
        else if(scrollValue < 0.0f)
            SwitchWeapon(currentWeaponIndex - 1);
        
        if(Input.GetKeyDown(KeyCode.G))
            DropWeapon();
        
        
    }

    public bool PickupWeapon(Weapon weapon) {

        int freeIndex = -1;
        if (!weapons[0]) freeIndex = 0;
        else if(!weapons[1]) freeIndex = 1;
        else return false;

        if (weapons[currentWeaponIndex])
            weapons[currentWeaponIndex].gameObject.SetActive(false);

        weapons[freeIndex] = Instantiate(weapon.gameObject, eyes).GetComponent<Weapon>();
        weapons[freeIndex].transform.localPosition = weaponOffset;
        weaponText.text = "Weapon: " + weapons[freeIndex].WeaponName;
        currentWeaponIndex = freeIndex;

        return true;

    }
    public bool PickupAmmo(uint weaponID, uint amount) {

        int index = -1;
        if (weapons[0].WeaponID == weaponID) index = 0;
        else if(weapons[1].WeaponID == weaponID) index = 1;
        else return false;

        weapons[index].AddAmmo(amount);
        return true;

    }

    void Shoot() {

        if (!weapons[currentWeaponIndex].Shoot()) return;

        RaycastHit hit;
        if (!Physics.Raycast(mainCam.position, mainCam.forward, out hit)) return;

        if (!hit.rigidbody) return;
        hit.rigidbody.AddForce(eyes.forward * weapons[currentWeaponIndex].HitForce * 10.0f);
        
        ShootableObject obj = hit.collider.GetComponent<ShootableObject>();
        if (!obj) return;

        obj.Hit(weapons[currentWeaponIndex].Damage);

    }
    void Reload() {

        weapons[currentWeaponIndex].Reload();

    }
    void ToggleADS() {

        ads = !ads;

        if(ads)
            mainCam.localPosition = adsCamOffset;
        else
            PlayerController.instance.ResetCamera();

    }
    void SwitchWeapon(int index) {

        if (index > 1) index = 0;
        else if(index < 0) index = 1;

        if (index == currentWeaponIndex) return;
        if (!weapons[index]) return;

        weapons[currentWeaponIndex].gameObject.SetActive(false);
        currentWeaponIndex = index;
        weapons[currentWeaponIndex].gameObject.SetActive(true);

    }
    void DropWeapon() {
        
        if (!weapons[currentWeaponIndex]) return;

        Destroy(weapons[currentWeaponIndex].gameObject);
        weapons[currentWeaponIndex] = null;

        if(currentWeaponIndex == 0) currentWeaponIndex = 1;
        else currentWeaponIndex = 0;
        weapons[currentWeaponIndex].gameObject.SetActive(true);

    }

    public Transform MainCam { get { return mainCam; } }
    public Transform Eyes { get { return eyes; } }

}