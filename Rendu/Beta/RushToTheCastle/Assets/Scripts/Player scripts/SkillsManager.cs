using UnityEngine;
using System.Collections;

public class SkillsManager : MonoBehaviour {

	#region list of skills
	[SerializeField]
	SkillShot 		_A_skill = null;

	[SerializeField]
	SkillShot 		_Z_skill = null;

	[SerializeField]
	SkillShield		_E_skill = null;

	[SerializeField]
	SkillHeal		_R_skill = null;
	#endregion

	[SerializeField]
	Transform A_skill_transform_position;

	[SerializeField]
	Transform Z_skill_transform_position;

	[SerializeField]
	ParticleSystem A_skill_particle;

	[SerializeField]
	ParticleSystem Z_skill_particle;

	[SerializeField]
	Transform mouse;

	[SerializeField]
	private NetworkView _myNetworkView;


	//ScriptableObject of the target
	public HealthManager script;

	#region initialization
	void Start () {
		_A_skill = new SkillShot("Coup puissant", 1, 100, 10, 1); 
		_Z_skill = new SkillShot("Coup SURpuissant", 5, 500, 20, 3);
		_E_skill = new SkillShield("Bouclier magnetique", 10, 1000, 5, true);
		_R_skill = new SkillHeal("Soin rapide", 3 , 100, 1);

		NetworkInputManager.Skill_Shot += new NetworkInputManager.skill_Shot_Resolution_delegate(Skill_Shot_resolution); //delegate

		mouse = GameObject.Find("mouse position").transform;
	}
	#endregion
	
	// Update is called once per frame
	void Update () {
	
		#region Timers updates
			#region attack timer clock maj
				if(A_AttackSpeedTiming)
				{
					A_countdown -= Time.deltaTime;
					if(A_countdown <= 0){
						A_AttackSpeedTiming = false;
					}
				}
				
				if(Z_AttackSpeedTiming)
				{
					Z_countdown -= Time.deltaTime;
					if(Z_countdown <= 0){
						Z_AttackSpeedTiming = false;
					}
				}
		#endregion

		#endregion
	
	}

	#region all Timers

		#region A_timer
		bool A_AttackSpeedTiming = false;
		double A_countdown;
		void A_StartSkillTimer(double time){
			A_AttackSpeedTiming = true;
			A_countdown = time;
		}
		#endregion

		#region Z_timer
		bool Z_AttackSpeedTiming = false;
		double Z_countdown;
		void Z_StartSkillTimer(double time){
			Z_AttackSpeedTiming = true;
			Z_countdown = time;
		}
		#endregion
	
	#endregion

	#region skills delegates recived

	void Skill_Shot_resolution(string player, Vector3 MousePosition, string compétence){
		if( this.tag == player){
			if(compétence == "A"){
				if(!A_AttackSpeedTiming){

					mouse.position = MousePosition;
					A_skill_transform_position.LookAt(MousePosition);
					_myNetworkView.RPC("setASkillRotation", RPCMode.Others , A_skill_transform_position.rotation);

					//thespheere cast
					RaycastHit[] SphereCastArea = Physics.SphereCastAll( this.transform.position, _A_skill._radius , (MousePosition - this.transform.position).normalized , _A_skill._Range);

					foreach(RaycastHit cast in SphereCastArea) {				//for each minions/players in the table

						Transform local = cast.transform;
						while(local.parent){ 
							local = local.parent; 
						}
						
						if(local.transform.tag == "Red_Minion" || local.transform.tag == "Blue_Minion"){
							
							script = local.transform.gameObject.GetComponent<HealthManager>();	//we get the script of the target to domage him
							script.LooseHealth( _A_skill._Damage );								//we damage him
						}
					}
					//RPC lancer l'annimation de l'attaque sur client et server
					_myNetworkView.RPC("PLayAskill", RPCMode.All );
					A_StartSkillTimer(_A_skill._cooldown);	//reset attack speed timer
				}
			}
			else if (compétence == "Z"){
				if(!Z_AttackSpeedTiming){
					
					mouse.position = MousePosition;
					Z_skill_transform_position.LookAt(MousePosition);
					_myNetworkView.RPC("setZSkillRotation", RPCMode.Others , Z_skill_transform_position.rotation);
					
					//thespheere cast
					RaycastHit[] SphereCastArea = Physics.SphereCastAll( this.transform.position, _Z_skill._radius , (MousePosition - this.transform.position).normalized , _Z_skill._Range);
					
					foreach(RaycastHit cast in SphereCastArea) {				//for each minions/players in the table
						
						Transform local = cast.transform;
						while(local.parent){ 
							local = local.parent; 
						}
						
						if(local.transform.tag == "Red_Minion" || local.transform.tag == "Blue_Minion" 
						   || local.transform.tag == "Player_red_1" || local.transform.tag == "Player_red_2" 
						   || local.transform.tag == "Player_red_3" || local.transform.tag == "Player_red_4" || local.transform.tag == "Player_red_5"
						   || local.transform.tag == "Player_blue_1" || local.transform.tag == "Player_blue_2" || local.transform.tag == "Player_blue_3"
						   || local.transform.tag == "Player_blue_4" || local.transform.tag == "Player_blue_5"){
							script = local.transform.gameObject.GetComponent<HealthManager>();	//we get the script of the target to domage him
							script.LooseHealth( _Z_skill._Damage );								//we damage him
						}
													//reset attack speed timer
						
					}
					//RPC lancer l'annimation de l'attaque sur client et server
					_myNetworkView.RPC("PLayZskill", RPCMode.All );
					Z_StartSkillTimer(_Z_skill._cooldown);	
				}
			}
			else if(compétence == "E"){
				
			}
			else if(compétence == "R"){
				
			}
		}
	}

	#endregion

	#region RPC
	[RPC]
	void PLayAskill(){
		A_skill_particle.Play();
	}

	[RPC]
	void PLayZskill(){
		Z_skill_particle.Play();
	}

	[RPC]
	void setASkillRotation(Quaternion newrotation){
		A_skill_transform_position.rotation = newrotation;
	}

	[RPC]
	void setZSkillRotation(Quaternion newrotation){
		Z_skill_transform_position.rotation = newrotation;
	}

	#endregion



	#region classes

	//mother class
	class Skill{
		public string 	_name;
		public double 	_cooldown;

		public Skill(string name, double cooldown){
			this._name 		= name;
			this._cooldown 	= cooldown;
		}
	}

	//daughters classes

	class SkillShot : Skill{
		public double	_Damage;
		public float 	_Range;
		public float 	_radius;

		public SkillShot(string name, int cooldown, double Damage,  float Range, float radius): base(name, cooldown){
			this._Damage 	= Damage;
			this._Range 	= Range;
			this._radius	= radius;
		}
	}

	class SkillShield : Skill{
		public double	_Value;
		public int 		_duration;
		public bool 	_IsDestroyable;
		
		public SkillShield(string name, int cooldown, double value,  int Range, bool IsDestroyable): base(name, cooldown){
			this._Value	  		= value;
			this._duration		= Range;
			this._IsDestroyable = IsDestroyable;
		}
	}

	class SkillHeal : Skill{
		public double 	_Value;
		public int 		_TimesCasted;
		
			public SkillHeal(string name, int cooldown, double value, int TimesCasted): base(name, cooldown){
			this._Value	  		= value;
			this._TimesCasted   = TimesCasted;
		}
	}

	#endregion


}
