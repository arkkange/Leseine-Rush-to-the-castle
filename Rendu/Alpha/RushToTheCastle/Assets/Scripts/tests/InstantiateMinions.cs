using UnityEngine;
using System.Collections;

public class InstantiateMinions : MonoBehaviour {

	[SerializeField]
	public Rigidbody MinionPrefab;

	[SerializeField]
	public Transform MinionSpawnPoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Network.isServer){
			if(Input.GetKeyDown( KeyCode.M )){		//pour l'instant M mais plus tard un event autre => apparition du minion
				Network.Instantiate(MinionPrefab, MinionSpawnPoint.position, MinionSpawnPoint.rotation, 0);
			}
		}
	}
}