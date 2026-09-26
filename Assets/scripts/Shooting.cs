using System;
using TarodevController;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    private Camera mainCamera;
    private float timer;

    [SerializeField] private float fireRate;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawner;



    [SerializeField] private float heat;
    [SerializeField] private float heatIncreaseRate;
    [SerializeField] private float coolDownRate;
    [SerializeField] private float maxHeat;


    [SerializeField] private SpriteRenderer gunAppearence;
    private Color normalColor = Color.white;
    private Color hotColor = Color.red;



    private PlayerController player;


    private bool toggle = false; // for testing
    private bool canFire = true;



    void Start()
    {
        mainCamera = Camera.main;
        player = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
        Shoot();
        ManageHeat();
        UpdateGunColor();
        //for testing
        if (Keyboard.current.ctrlKey.wasPressedThisFrame)
        {
            toggle = !toggle;
        }

    }


    void FollowMouse()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mousePos);

        Vector2 direction = mouseWorldPos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }


    void Shoot()
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
        if (!player.inWater && toggle)
        {
            // Increase heat every time a bullet is fired
            heat += heatIncreaseRate * Time.deltaTime;
        }


    }


    void ManageHeat()
    {
        if (player.inWater)
        {
            heat -= coolDownRate * Time.deltaTime;
        }

        heat = Mathf.Clamp(heat, 0, maxHeat);

        if (heat >= maxHeat)
        {
            // Player dies here
            Debug.Log("TOO HOT!");
        }
    }


    void UpdateGunColor()
    {
        // gun gets increasingly red the hotter it gets
        float heatPercent = heat / maxHeat;
        gunAppearence.color = Color.Lerp(normalColor, hotColor, heatPercent);

    }

}
