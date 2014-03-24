using UnityEngine;
using System.Collections;

public class AlegianceManager : MonoBehaviour {


	//datas : red, blue , neutral
	[SerializeField]
	private string team = "neutral";
	public string Team {
		get {
			return team;
		}
		set {
			team = value;
		}
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		
	}

	void SwitchAllegience(){
		if (Team.Equals("blue") ){
			Team = "red";
		}
		else if(Team.Equals("red")){
			Team = "blue";
		}
	}

	void ChangeAllegience(string color){
		Team = color;
	}


}
