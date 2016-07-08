using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{

	public float maxHealth = 4.0f;

	public float Health { get; protected set; }

	protected Transform goal;

	protected NavMeshAgent agent;

	public bool Dead { get; protected set; }


	void Awake()
	{
		Dead = false;
		Health = maxHealth;

		agent = GetComponent<NavMeshAgent>();
		goal = GameObject.FindGameObjectWithTag("Goal").transform;
	}

	// Use this for initialization
	void Start ()
	{
		agent.destination = goal.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void TakeDamage(float damage)
	{
		Health -= damage;
		if (Health <= 0.0f)
		{
			Health = 0.0f;
			Killed();
		}
	}

	void Killed()
	{
		if (Dead)
		{
			return;
		}
		Dead = true;

		agent.Stop();
		agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		Destroy(gameObject);
	}
}
