using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

	public float baseDamage = 1.0f;

	public float Speed = 20.0f;

	public float Acceleration = 100.0f;

	// for estimating future position ahead by Tau time
	public float Tau = 0.5f;

	public float ReachingDistanceSquared = 1.0f;



	protected Unit target;

	protected Vector3 velocity;

	protected Vector3 lastTargetPosition;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{ 
		Vector3 targetPos = target ? target.transform.position : lastTargetPosition;
		lastTargetPosition = targetPos;

		Vector3 toTarget = targetPos - transform.position;

		if (toTarget.sqrMagnitude < ReachingDistanceSquared)
		{
			ReachedTarget();
			return;
		}

		if (target && !target.Dead)
		{
			var targetAgent = target.GetComponent<NavMeshAgent>();
			toTarget += targetAgent.desiredVelocity * Tau;
		}

		Vector3 dir = toTarget.normalized;
		Vector3 force = dir * Speed - velocity;
		force = Vector3.ClampMagnitude(force, Acceleration);

		velocity += force * Time.deltaTime;
		transform.position += velocity * Time.deltaTime;
	}

	public void init(Unit target, float angle, float magn)
	{
		this.target = target;
		lastTargetPosition = target.transform.position;

		setInitialVelocity(angle, magn);
	}

	void setInitialVelocity(float angle, float magn)
	{
		Vector3 toTarget = (target.transform.position - transform.position);
		toTarget.y = 0;
		toTarget.Normalize();

		Vector3 rotAxis = new Vector3(-toTarget.z, 0, toTarget.x);
		toTarget = Quaternion.AngleAxis(angle, rotAxis) * toTarget;
		velocity = toTarget * magn;
	}

	void ReachedTarget()
	{
		if (target && !target.Dead)
		{
			target.TakeDamage(baseDamage);
		}

		Destroy(gameObject);
	}
}
