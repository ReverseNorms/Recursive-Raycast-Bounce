using UnityEngine;
using System.Collections;

[ExecuteAlways]
public class RecursiveRay : MonoBehaviour
{
	[SerializeField] LayerMask layerMask = 1;
	[SerializeField, Range(0, 1000)] int raysToCast = 360;
	[SerializeField] float maxRayTravel = 20f;
	RaycastHit hit;

	public bool increaserays = false;
	void OnValidate()
	{
		if (increaserays)
		{
			increaserays = false;
			StartCoroutine(RayIncrease());
		}
	}

	IEnumerator RayIncrease()
	{
		while (maxRayTravel < 50f)
		{
			maxRayTravel += Time.deltaTime * 5;
			yield return null;
		}
	}

	void Update()
	{
		Vector3 rot = transform.localEulerAngles;
		float rotIncrement = 1f / raysToCast;

		for (int i = 0; i < raysToCast; i++)
		{
			float angle = rotIncrement * i;
			transform.localEulerAngles = new Vector3(rot.x, angle * 360, rot.z);
			RaycastRecursion(transform.position, transform.forward, maxRayTravel, new Color(1f, 1f, 1f));
		}
		transform.localEulerAngles = rot;
	}

	void RaycastRecursion(Vector3 position, Vector3 dir, float maxDist, Color col)
	{
		while(maxDist > 0)
        {
			Raycast(ref position, ref dir, ref maxDist, col);
			col.g -= 0.25f; col.b -= 0.25f;
		}
	}
	void Raycast(ref Vector3 position, ref Vector3 dir, ref float maxDist, Color col)
	{
		if (Physics.Raycast(position, dir, out hit, maxDist, layerMask))
		{
			float dist = Vector3.Distance(position, hit.point);
			Debug.DrawRay(position, dir.normalized * dist, col);

			maxDist -= dist;

			if (maxDist > 0)
			{
				position = hit.point;
				dir = Vector3.Reflect(dir, hit.normal);
			}
		}
		else
		{ //Disabled this part if you only want to see rays that have hit something
			Debug.DrawRay(position, dir.normalized * maxDist, col);
			maxDist = 0f;
		}
	}
}