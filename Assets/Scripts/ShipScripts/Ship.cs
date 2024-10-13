using UnityEngine;
using System.Collections;

public abstract class Ship : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    public int health;

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float shootForce;
    [SerializeField] protected float reloadTime;
    [SerializeField] protected float friction;

    protected Vector2 moveDirection;
    protected bool canShoot = true;

    public Bullet bullet;
    public string opponentTag;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        CustomStart();
    }

    protected abstract void CustomStart();
    protected abstract void Move();

    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        // Calculate shoot direction and update player rotation
        Vector2 shootDirection = (GetMousePos() - (Vector2)transform.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, -90 + Mathf.Atan2(shootDirection.y, shootDirection.x) * 180 / Mathf.PI);

        // Shoot when left mouse button is pressed
        if (Input.GetMouseButton(0))
        {
            if (canShoot)
            {
                StartCoroutine(Shoot(shootDirection, shootForce));
            }
        }
    }

    protected IEnumerator Shoot(Vector3 shootDirection, float shootForce)
    {
        Bullet newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        newBullet.SetTarget(opponentTag, shootDirection, shootForce);
        canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
    }

    Vector2 GetMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);
        return worldPos2D;
    }

    // Method for taking damage
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
