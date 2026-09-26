using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    private Camera mainCamera;
    private float timer;
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawner;

    private bool toggle = false; // for testing

    private bool canFire = true;


    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        followMouse();
        shoot();

        //for testing
        if (Keyboard.current.ctrlKey.wasPressedThisFrame)
        {
            toggle = !toggle;
        }

    }


    void followMouse()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mousePos);

        Vector2 direction = mouseWorldPos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }


    void shoot()
    {

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > (1 / fireRate))
            {
                canFire = true;
                timer = 0;
            }
        }


        if (canFire && toggle) //toggle is for testing
        {
            Instantiate(bullet, bulletSpawner.position, Quaternion.identity);
            canFire = false;
        }


    }

}
