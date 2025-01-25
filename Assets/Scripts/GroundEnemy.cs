using Unity.Mathematics;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{

    public Vector2 raycastOffset;
    public float rayDistance;
    public float speed;
    public float viewDistance;

    private Vector3 moveDirection;
    private Vector3 rayStart;
    private Vector3 rayOffset;
    private Transform player;
    private bool hasTarget;

    private void Start()
    {
        rayOffset = new Vector3(raycastOffset.x, raycastOffset.y, 0);
        moveDirection = new Vector3(1, 0, 0);
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Update()
    {

        /*
         * 
         * check if player is within certain distance
         *      - if within certain distance, and enemy is moving towards the player
         *          make it a target
         *      - else patrol and do idle movement
         * 
         */


        CheckTargetValidity();

        //now if enemy has a target, path towards if there's walkable ground
        //otherwise do a patrol path
        bool walkable = WalkableGround();

        if (hasTarget)
        {
            moveDirection.x = player.position.x - transform.position.x;
            //normalizing the distance
            moveDirection.x = (moveDirection.x / math.abs(moveDirection.x));

            //if the ground is walkable, move towards the player
            //otherwise do nothing.
            if (walkable)
            {
                transform.position += moveDirection * Time.deltaTime;
            }


        }
        else
        {

            //no target, so patrol
            if (!walkable)
            {
                //can't walk forward, so turn around
                moveDirection.x *= -1;
                rayOffset.x *= -1;
            }
            transform.position += moveDirection * Time.deltaTime;

        }

        Debug.DrawLine(rayStart, rayStart + (Vector3.down * rayDistance), Color.green);

    }

    private bool WalkableGround()
    {

        rayStart = transform.position + rayOffset;
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, rayDistance, LayerMask.GetMask("Ground"));

        if (hit.collider == null)
        {
            return false;
        }

        return true;
    }

    private void CheckTargetValidity()
    {

        Vector3 localPos = player.position - transform.position;
        localPos.y = 0;
        float distSqr = math.lengthsq(localPos);

        //if out of view range, nothing to do.
        if (distSqr > viewDistance * viewDistance)
        {
            //no target
            hasTarget = false;
            return;
        }

        //otherwise check if the player is in front of the enemy
        if (math.abs(((localPos.x / math.abs(localPos.x)) - (moveDirection.x / math.abs(moveDirection.x)))) < .001f)
        {
            hasTarget = true;
        }
        

    }



}
