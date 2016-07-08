using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{

	public GameObject bulletPrefab;

	public float attackRate = 1.0f;

	public Vector3 bulletSpawnPoint = new Vector3(0, 2.5f, 0);

	public int sampleCount = 10;

	public float bulletInitialSpeedAngle = 20.0f;

	public float bulletInitialSpeedMagnitude = 0.0f;



	protected IList<Unit> unitsClose;

	protected Unit target;

	protected float cooldownRemaining = 0.0f;

	protected IList<Unit> removables;

	void Awake()
	{
		unitsClose = new List<Unit>(20);
		removables = new List<Unit>();
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{ 
		if(cooldownRemaining > 0.0f)
		{
			cooldownRemaining -= Time.deltaTime;
		}
		else
		{
			RemoveInvalidTargets();

			Fire();
		}
	}

	void RemoveInvalidTargets()
	{
		removables.Clear();
		for (int i = 0; i < unitsClose.Count; i++)
		{
			if (unitsClose[i] == null || unitsClose[i].Dead)
			{
				removables.Add(unitsClose[i]);
			}
		}
		for (int i = 0; i < removables.Count; i++)
		{
			unitsClose.Remove(removables[i]);
		}
	}

	Unit FindTarget()
	{
		if(target != null && unitsClose.Contains(target))
		{
			return target;
		}

		float minDistSq = -1.0f;
		Unit minUnit = null;

		int count = unitsClose.Count;
		int loopCount = count < sampleCount ? count : sampleCount;
		for(int i = 0; i < loopCount; i++)
		{
			var index = (count < sampleCount) ? i : Random.Range(0, count);
			var unit = unitsClose[index];

			float distSq = (unit.transform.position - transform.position).sqrMagnitude;
			if (minUnit == null || distSq < minDistSq)
			{
				minUnit = unit;
				minDistSq = distSq;
			}
		}
		return minUnit;
	}

	void Fire()
	{
		target = FindTarget();
		if(target == null)
		{
			return;
		}

		var go = Instantiate(bulletPrefab, transform.position + bulletSpawnPoint, Quaternion.identity) as GameObject;
		Bullet b = go.GetComponent<Bullet>();
		b.init(target, bulletInitialSpeedAngle, bulletInitialSpeedMagnitude);

		cooldownRemaining = 1.0f / attackRate;
	}


	void OnTriggerEnter(Collider other)
	{
		var unit = other.GetComponent<Unit>();
		if (unit == null)
		{
			return;
		}

		unitsClose.Add(unit);
	}

	void OnTriggerExit(Collider other)
	{
		var unit = other.GetComponent<Unit>();
		if(unit == null)
		{
			return;	
		}

		unitsClose.Remove(unit);
	}
}
