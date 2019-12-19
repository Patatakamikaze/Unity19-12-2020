using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FIRETYPE { REPEATER, SEMIAUTOMATIC, AUTOMATIC }
public class FireWeapon : MonoBehaviour
{
    [System.Serializable]

    public struct FireWeaponData
    {
        public FIRETYPE firetype;
        public float power;
        public float recoil;
        public float fireRate;
        public float range;
        public int magazineCapacity;
        [Range(0, 1f)]
        public float muzzleFireFrequency;
        public int currentAmmo { get; set; }
        
        public FireWeaponData(FIRETYPE firetype, float power, float recoil, float fireRate, float range, int magazineCapacity, float muzzleFireFrequency)
        {

            this.firetype = firetype;
            this.power = power;
            this.recoil = recoil;
            this.fireRate = fireRate;
            this.range = range;
            this.magazineCapacity = magazineCapacity;
            this.muzzleFireFrequency = muzzleFireFrequency;
            currentAmmo = magazineCapacity;

        }

    }


    [System.Serializable]
    public struct FireWeaponFXData
    {
        public ParticleSystem weaponFireParticles;
        public Light weaponFireLight;
        public AudioClip reloadSound;
        public AudioClip shootSound;
        public AudioClip emptySound;

        public FireWeaponFXData(ParticleSystem weaponFireParticles, Light weaponFireLight, AudioClip reloadSound, AudioClip shootSound, AudioClip emptySound)
        {
            this.weaponFireParticles = weaponFireParticles;
            this.reloadSound = reloadSound;
            this.weaponFireLight = weaponFireLight;

            this.shootSound = shootSound;
            this.emptySound = emptySound;




        }

    }


    public FireWeaponData gunData = new FireWeaponData(FIRETYPE.AUTOMATIC, 10f, 0.1f, 200f, 700f, 30, 0.7f);
    public FireWeaponFXData gunFX = new FireWeaponFXData();
    public Camera FPSCamera;
    public LayerMask impactMask;
    public GameObject bulletHole;
    public bool isReloading { get; private set; }

    private RaycastHit hit;
    private Ray ray;
    private Recoiler gunRecoiler;
    private Recoiler camRecoiler;
    private AudioSource audioSource;
    private float firingTimer;
    BulletHitController bulletHitController;
    private void Start()
    {
        gunRecoiler = GetComponentInParent<Recoiler>();
        camRecoiler = FPSCamera.GetComponentInParent<Recoiler>();

        gunData.currentAmmo = gunData.magazineCapacity;

        if(audioSource== null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

       bulletHitController = GetComponent<BulletHitController>();
    }

    private void Update()
    {
        if (gunFX.weaponFireLight != null)
        {
            gunFX.weaponFireLight.enabled = gunFX.weaponFireParticles.isPlaying;
        }
    }

    private void playFX()
    {
        if (gunFX.weaponFireParticles != null)
        {
            gunFX.weaponFireParticles.transform.parent.Rotate(0f, 0f, Random.Range(0f,360f));
            gunFX.weaponFireParticles.Play(true);
        }
    }

    public void shoot(bool fireInput)
    {
        if(Time.time>= firingTimer && fireInput && !isReloading)
        {
            firingTimer = Time.time + 60 / gunData.fireRate;

            if (gunData.currentAmmo == 0)
            {
                audioSource.PlayOneShot(gunFX.emptySound);
                firingTimer += gunFX.emptySound.length;
                return;

            }

            gunData.currentAmmo--;

            gunRecoiler.recoil += gunData.recoil;
            camRecoiler.recoil += gunData.recoil;

            audioSource.PlayOneShot(gunFX.shootSound);

            if(Random.Range(0,100)<gunData.muzzleFireFrequency+100)
            { playFX(); }

            Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);

            ray= FPSCamera.ScreenPointToRay(screenCenterPoint);
    
            bulletHitController.handleHit(ray,gunData.range, gunData.power);
        }
         


    }

    private IEnumerator reload()
    {
        isReloading = true;
        audioSource.PlayOneShot(gunFX.reloadSound);
        yield return new WaitForSeconds(gunFX.reloadSound.length);

        gunData.currentAmmo=gunData.magazineCapacity;

        isReloading = false;
    }

    public void OnReloadButtonDown()
    {
        if (isReloading == false && gunData.currentAmmo < gunData.magazineCapacity)
        {
            StartCoroutine(reload());
        }
    }
}

