using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFPSControler : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private MouseLook mouseLook;
    private GunAiming gunAiming;
    private FireWeapon fireWeapon;
    

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GameObject.Find("Capsule").GetComponent<MeshRenderer>().enabled = false;

        characterMovement = GetComponent<CharacterMovement>();
        mouseLook = GetComponent<MouseLook>();
        gunAiming = GetComponentInChildren<GunAiming>();
        fireWeapon = GetComponentInChildren<FireWeapon>();

    }
    private void Update()
    {
        movement();
        rotation();
        aiming();
        shooting();
    }
    private void movement()
    {
        float hMovementInput = Input.GetAxisRaw("Horizontal");
        float vMovementInput = Input.GetAxisRaw("Vertical");

        bool jumpInput = Input.GetButtonDown("Jump");
        bool dashInput = Input.GetButton("Dash");

        characterMovement.moveCharacter(hMovementInput, vMovementInput, jumpInput, dashInput);
    }
   private void rotation()
    {
        float hRotationInput = Input.GetAxis("Mouse X");
        float vRotationInput = Input.GetAxis("Mouse Y");

        mouseLook.handleRotation(hRotationInput, vRotationInput); 




    }
    private void aiming()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            gunAiming.OnButtonDown();
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            gunAiming.OnButtonUp();
        }
    }
    private void shooting()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            fireWeapon.OnReloadButtonDown();
        }
        else
        {
            switch (fireWeapon.gunData.firetype)
            {
                case FIRETYPE.REPEATER:
                case FIRETYPE.SEMIAUTOMATIC:
                    fireWeapon.shoot(Input.GetButtonDown("Fire1"));
                    break;
                case FIRETYPE.AUTOMATIC:
                    fireWeapon.shoot(Input.GetButton("Fire1"));
                    break;

            }
        }
    }
}
