using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemyAI1 : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] private float _followRad = 25.0f; // Follow radius
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // Restrict transform orientation change
        agent.updateUpAxis = false;  // Prevent automatic up-axis realignment

        // Automatically find the player if a target is not set in the inspector
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }

        if (target == null)
        {
            Debug.LogError("Target not assigned and Player tag not found!");
        }
    }

    private void Update()
    {
        if (target == null) return;

        // Calculate the current distance to the target
        float curDist = Vector3.Distance(transform.position, target.position);

        // Check if the target is within the follow radius
        if (curDist <= _followRad)
        {
            // Set the agent's destination to the target's position
            agent.SetDestination(target.position);
        }
        else
        {
            // Stop the agent from moving when the target is out of range
            agent.ResetPath();
        }
    }
}
