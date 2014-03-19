using UnityEngine;
using System.Collections;

public class TestNavMesh : MonoBehaviour {

	public NetworkPlayer myPlayerController;
	
	Transform myTransform;
	public Transform MyTransform {
		get {
			return myTransform;
		}
		set {
			myTransform = value;
		}
	}
	
	//declaration de l'objet navmeshagent qui permet de créer le pathfinding
	private NavMeshAgent agent;
	
	// Use this for initialization
	void Start () {
		myTransform = this.transform;
		agent = GetComponent<NavMeshAgent>();
		agent.destination = myTransform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(1)){

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//RaycastHit hit;
			float hit = 0.0f;
			Plane playerPlane = new Plane(Vector3.up, myTransform.position); 	//changed
			if( playerPlane.Raycast(ray , out hit) ){
				agent.destination = ray.GetPoint(hit);
			}

		}
		else if (Input.GetMouseButton(1)){
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//RaycastHit hit;
			float hit = 0.0f;
			Plane playerPlane = new Plane(Vector3.up, myTransform.position);	//changed
			if( playerPlane.Raycast(ray , out hit) ){
				agent.destination = ray.GetPoint(hit);
			}

		}
	}
	

}
