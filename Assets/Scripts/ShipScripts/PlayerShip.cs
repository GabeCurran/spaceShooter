using UnityEngine;

public class PlayerShip : Ship
{
    private float defaultHealth;
    public static PlayerShip Instance;

    // Add the animator variable
    public Animator animator;
    public GameManager gameManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    protected override void CustomStart()
    {
        defaultHealth = health;
    }

    protected override void Move()
    {
        if (moveDirection.magnitude > 0)
        {
            rigidBody.velocity = moveDirection * moveSpeed;
        }
        else
        {
            rigidBody.velocity -= rigidBody.velocity * friction;
        }
    }

    void Update()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        Vector2 shootDirection = (GetMousePos() - new Vector2(transform.position.x, transform.position.y)).normalized;
        transform.eulerAngles = new Vector3(0, 0, -90 + Mathf.Atan2(shootDirection.y, shootDirection.x) * 180 / Mathf.PI);

        // Shooting logic with animator control
        if (Input.GetMouseButton(0))
        {
            if (canShoot)
            {
                StartCoroutine(Shoot(shootDirection, shootForce));
            }
            animator.SetBool("isShooting", true);
        }
        else
        {
            animator.SetBool("isShooting", false);
        }
    }

    Vector2 GetMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        return new Vector2(worldPos.x, worldPos.y);
    }

    public override void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject);
            gameManager.GameOver(false);
        }
    }
}
