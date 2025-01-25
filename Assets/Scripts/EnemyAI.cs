using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;

	NavMeshAgent agent;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false; //Restrict transform orientation change
		agent.updateUpAxis = false; //Prevent automatic up axis realignment
	}

	private void Update()
	{
		//This is where we change the target.
		//There are other controls available.
		agent.SetDestination(target.position);
	}
}
