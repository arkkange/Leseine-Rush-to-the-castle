using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {

	NetworkView _myNetworkView;

	void Start(){
		_myNetworkView = this.gameObject.GetComponent<NetworkView>();
	}
	
	// Update is called once per frame
	void Update () {
			if( Input.GetKeyDown(KeyCode.P)){
				_myNetworkView.RPC("Pause", RPCMode.All );
			}
	}



	#region

		[RPC]
		void Pause(){
			if(Time.timeScale != 0){
				Time.timeScale = 0;
			}
			else{
				Time.timeScale = 1;
			}
		}

	#endregion

}