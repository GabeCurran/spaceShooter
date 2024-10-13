using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    private string target;
    protected Rigidbody2D rb;
    protected float speed;

    void Start()
    {
        // Initialize anything here if needed
    }

    public void SetTarget(string name, Vector3 direction, float force)
    {
        target = name;
        rb = GetComponent<Rigidbody2D>();
        speed = force;
        transform.up = direction;
        StartCoroutine(StartCountdown());
    }

    public IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

    // Trigger detection for damaging the target
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == target)
        {
            other.GetComponent<Ship>().TakeDamage(1);
            Destroy(this.gameObject);
        }
    }
}
