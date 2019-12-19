using UnityEngine;
using UnityEngine.UI;

public class FireWeaponHUD : MonoBehaviour {

    public FireWeapon fireweapon;
    public Text ammoText;
    public Image gunIcon;


    private void OnGUI() {
        ammoText.text = fireweapon.gunData.currentAmmo + "/" + fireweapon.gunData.magazineCapacity ;
    }
}
