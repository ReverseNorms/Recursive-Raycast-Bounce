//RecursiveRaycast test by Invertex
using UnityEngine;
using System.Collections;
public class RecursiveRay : MonoBehaviour {
	[SerializeField]LayerMask layerMask = 1;
	[SerializeField]float maxRayTravel = 20f;
	RaycastHit hit;
	public bool increaserays = false;
	void OnValidate(){
		if(increaserays)
		{
			increaserays = false;
			StartCoroutine(RayIncrease());
		}

	}

	IEnumerator RayIncrease(){
		while(maxRayTravel < 50f){
			maxRayTravel += Time.deltaTime * 5;
			yield return null;
		}
	}

	void Update(){
		Vector3 rot = transform.localEulerAngles;
		for(int i = 0; i < 360; i++){
			transform.localEulerAngles = new Vector3(rot.x, (float)i, rot.z);
			RaycastRecursion(transform.position, transform.forward, maxRayTravel, new Color(1f,1f,1f));
			Debug.Log(transform.forward);
		}
		transform.localEulerAngles = rot;
	}

	void RaycastRecursion(Vector3 position, Vector3 dir, float maxDist, Color col){
		if(Physics.Raycast(position, dir, out hit, maxDist, layerMask)) 
		{
			float dist = Vector3.Distance(position, hit.point);
			Debug.DrawRay(position, dir.normalized * dist, col);
			maxDist -= dist;
			if(maxDist > 0) {
				col.g -= 0.25f; col.b -= 0.25f;
				RaycastRecursion(hit.point, Vector3.Reflect(dir, hit.normal), maxDist, col);
			}
		}
		else{ //Disabled this part if you only want to see rays that have hit something
			Debug.DrawRay(position, dir.normalized * maxDist, col);
		}
	}
}