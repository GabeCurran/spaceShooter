using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Ship
{
    private Vector3 targetPosition;
    private enum State { Roam, Chase, Shoot }

    [SerializeField] private float chaseDist;
    [SerializeField] private float roamDist;
    [SerializeField] private float shootDist;
    [SerializeField] private float roamSpeed;
    [SerializeField] private float chaseSpeed;

    private State currentState;
    private GameObject player;
    private OutOfBounds outOfBounds;

    protected override void CustomStart()
    {
        currentState = State.Roam;
        SetRandomTargetPosition();

        player = GameObject.FindWithTag("Player");
        outOfBounds = GetComponent<OutOfBounds>();
    }

    void Update()
    {
        if (currentState == State.Roam)
        {
            // Check if the enemy is too close to the target position
            if (outOfBounds.getDistance(transform.position, targetPosition) < 1f)
            {
                targetPosition = (Vector2)transform.position + new Vector2(Random.Range(-roamDist, roamDist), Random.Range(-roamDist, roamDist));
            }
            // Check if the player is within chase distance
            if (outOfBounds.getDistance(transform.position, PlayerShip.Instance.transform.position) < chaseDist)
            {
                currentState = State.Chase;
            }
        }
        else if (currentState == State.Chase)
        {
            targetPosition = PlayerShip.Instance.transform.position;

            // Check if the enemy is within shooting distance
            if (outOfBounds.getDistance(transform.position, PlayerShip.Instance.transform.position) < shootDist)
            {
                currentState = State.Shoot;
            }
            else if (outOfBounds.getDistance(transform.position, PlayerShip.Instance.transform.position) > chaseDist * 1.2f)
            {
                currentState = State.Roam;
                SetRandomTargetPosition();
            }
        }
        else // Shooting state
        {
            targetPosition = PlayerShip.Instance.transform.position;

            if (outOfBounds.getDistance(transform.position, PlayerShip.Instance.transform.position) > shootDist)
            {
                currentState = State.Chase;
            }
            if (canShoot)
            {
                StartCoroutine(Shoot(moveDirection, shootForce));
            }
        }

        // Update target position based on OutOfBounds logic
        targetPosition = outOfBounds.getCoords(targetPosition);
        moveDirection = -outOfBounds.getDirection(transform.position, targetPosition).normalized;

        // Move the enemy ship
        Move();
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

        // Smoothly turn the enemy towards the target position
        transform.up += ((Vector3)moveDirection - transform.up) / 5;
    }

    private void SetRandomTargetPosition()
    {
        targetPosition = (Vector2)transform.position
            + new Vector2(Random.Range(-roamDist, roamDist), Random.Range(-roamDist, roamDist));
    }
}
