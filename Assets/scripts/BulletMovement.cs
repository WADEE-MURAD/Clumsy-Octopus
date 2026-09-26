using UnityEngine;
using UnityEngine.InputSystem;


public class BulletMovement : MonoBehaviour
{

    private Camera mainCamera;
    private Vector3 mouseWorldPos;
    private Rigidbody2D rb;
    [SerializeField] private float force;
    private Vector2 direction;

    //private float timer = 0f;
    private float spawnTime;
    [SerializeField] private float friendlyFireTimeLimit = 0.5f;
    //public bool canKillPlayer = false;
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();

        Vector3 mousePos = Mouse.current.position.ReadValue();
        mouseWorldPos = mainCamera.ScreenToWorldPoint(mousePos);

        direction = mouseWorldPos - transform.position;
        Vector2 rotation = transform.position - mouseWorldPos;

        rb.linearVelocity = direction.normalized * force;


        float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);


        spawnTime = Time.time;
    }

    /*void Update()
    {
    private float spawnTime;
        if (timer <= friendlyFireTimeLimit)
        {
            timer += Time.deltaTime;
        }
        else
        {
            canKillPlayer = true;
        }
    }
*/
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Deflecting Surface"))
        {
            var firstContact = collision.contacts[0];
            Vector2 newDirection = Vector2.Reflect(direction.normalized, firstContact.normal);
            rb.linearVelocity = newDirection.normalized * force;
        }
        if (collision.gameObject.CompareTag("Player") && canKillPlayer())
        {
            // Destroy(collision.gameObject);
            Debug.LogWarning("you dead nigga");
        }
    }

    bool canKillPlayer()
    {
        return (Time.time - spawnTime) > friendlyFireTimeLimit;
    }

}
