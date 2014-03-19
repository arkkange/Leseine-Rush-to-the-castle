using UnityEngine;
using System.Collections;

public class CartManager : MonoBehaviour {


	public int _Blue_counter = 0;
	public int _Red_counter = 0;

	[SerializeField]
	public int _Cart_Speed;

	NetworkView _myNetworkView = null;

	
	// Use this for initialization
	void Start () {
		this.name = "cart";
		_myNetworkView = this.gameObject.GetComponent<NetworkView>(); 
		HealthManager.Minion_died += new HealthManager.Minion_died_delegate(MinionHaveJustDied); //delegate
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		if(Network.isServer){
			if(_Blue_counter > _Red_counter){
				this.transform.position += Vector3.forward  * Time.deltaTime;
			}
			else if(_Blue_counter < _Red_counter){
				this.transform.position += Vector3.back  * Time.deltaTime;
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Red_Minion"){
			RedPlusPlus();
		}
		else if(other.tag == "Blue_Minion"){
			BluePlusPlus();
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Red_Minion"){
			RedLessLess();
		}
		else if(other.tag == "Blue_Minion"){
			BlueLessLess();
		}
	}

	void MinionHaveJustDied(string tag){
		if(tag == "Red_Minion"){
			RedLessLess();
		}
		else if(tag == "Blue_Minion"){
			BlueLessLess();
		}
	}

	void BluePlusPlus(){
		_Blue_counter++;
	}

	void RedPlusPlus(){
		_Red_counter++;
	}

	void BlueLessLess(){
		_Blue_counter--;
	}

	void RedLessLess(){
		_Red_counter--;
	}


}

