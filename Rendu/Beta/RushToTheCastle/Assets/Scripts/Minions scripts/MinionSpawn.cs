using UnityEngine;
using System.Collections;

public class MinionSpawn : MonoBehaviour {


	[SerializeField]
	public Transform MinionPrefab;
	
	[SerializeField]
	public Transform MinionSpawnPoint;

	[SerializeField]
	public int _Time = 30;

	private NetworkView _myNetworkView;

	// Use this for initialization
	void Start () {
		_myNetworkView = this.gameObject.GetComponent<NetworkView>(); 
	}
	
	// Update is called once per frame
	void Update () {
		if(Network.isServer){
			if(!_MinionSpawnTimer){
				SpawnMinion();
				StartMinionSpawnTimer(_Time);
			}
		}

		#region clock maj
		if(_MinionSpawnTimer)
		{
			_countdown -= Time.deltaTime;
			if(_countdown <= 0){
				_MinionSpawnTimer = false;
			}
		}
		#endregion

	}


	//timer
	bool _MinionSpawnTimer = false;
	double _countdown;
	void StartMinionSpawnTimer(double time)
	{
		_MinionSpawnTimer = true;
		_countdown = time;
	}

	
	void SpawnMinion()
	{
		if(Network.isServer){
			if(this.tag == "Lair_Red"){
				MinionPrefab.tag = "Red_Minion";
				Network.Instantiate(MinionPrefab, MinionSpawnPoint.position, MinionSpawnPoint.rotation, 0);
			}
			if(this.tag == "Lair_Blue"){
				MinionPrefab.tag = "Blue_Minion";
				Network.Instantiate(MinionPrefab, MinionSpawnPoint.position, MinionSpawnPoint.rotation, 0);
			}
		}

	}

	/*
	[RPC]
	void Instantiate_Minion(){
		Network.Instantiate(MinionPrefab, MinionSpawnPoint.position, MinionSpawnPoint.rotation, 0);
	}
	*/
}
