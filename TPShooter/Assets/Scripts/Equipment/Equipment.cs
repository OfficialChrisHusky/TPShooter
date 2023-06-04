using UnityEngine;

public class Equipment : MonoBehaviour {
    
    [SerializeField] private string equipmentName;
    [SerializeField] private uint amount = 1;
    [SerializeField] private uint maxAmount = 1;

    public void Equip() {

        gameObject.SetActive(true);

    }
    public void Unequip() {

        gameObject.SetActive(false);

    }
    public void Use() {

        amount--;

    }

    public uint Add(uint count) {

        uint maxAdd = maxAmount - amount;
        uint ret;
        if (maxAdd > count) ret = count;
        else ret = maxAdd;

        amount += ret;
        return ret;

    }

    public uint Amount { get { return amount; } }
    public string Name { get { return equipmentName; } }

}