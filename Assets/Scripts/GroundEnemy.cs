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
    [SerializeField]
    private Transform player;
    [SerializeField]
    private bool hasTarget;
    private float damageTimer;

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
        bool wall = WallInFront();

        if (hasTarget)
        {
            moveDirection.x = player.position.x - transform.position.x;
            //normalizing the distance
            moveDirection.x = (moveDirection.x / math.abs(moveDirection.x));

            //if the ground is walkable and there's no wall, move towards the player
            //otherwise do nothing.
            if (walkable && !wall)
            {
                transform.position += moveDirection * Time.deltaTime;
            }


        }
        else
        {

            //no target, so patrol
            if (!walkable || wall)
            {
                //can't walk forward, so turn around
                moveDirection.x *= -1;
                rayOffset.x *= -1;
            }
            transform.position += moveDirection * Time.deltaTime;

        }

        rayStart = transform.position + rayOffset;
        //draw ray that hits ground
        Debug.DrawLine(rayStart, rayStart + (Vector3.down * rayDistance), Color.red);

        //draw ray that hits wall
        Debug.DrawLine(transform.position, transform.position + (Vector3.right * moveDirection.x * .51f), Color.red);
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

    private bool WallInFront()
    {

        rayStart = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.right * moveDirection.x, .51f, LayerMask.GetMask("Ground"));

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
        localPos.z = 0;
        float distSqr = math.lengthsq(localPos);

        //if out of view range, nothing to do.
        if (distSqr > viewDistance * viewDistance)
        {
            //no target
            rayDistance = 1;
            hasTarget = false;
            return;
        }

        //otherwise check if the player is in front of the enemy
        if (math.abs(((localPos.x / math.abs(localPos.x)) - (moveDirection.x / math.abs(moveDirection.x)))) < .001f)
        {
            hasTarget = true;
            //makes it so that enemy can follow player down to lower levels.
            rayDistance = 2f;
        }
        

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.ToLower() == "stomp")
        {
            gameObject.SetActive(false);
        }
        else if (collision.collider.tag == "Player")
        {
            collision.collider.gameObject.GetComponent<PlayerController>().Hurt(10);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.ToLower() == "stomp")
        {
            gameObject.SetActive(false);
        }

    }


}
