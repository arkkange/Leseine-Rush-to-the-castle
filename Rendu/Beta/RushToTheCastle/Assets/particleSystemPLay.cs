using UnityEngine;
using System.Collections;

public class particleSystemPLay : MonoBehaviour {

	[SerializeField]
	ParticleSystem Dynamite;

	// Use this for initialization
	void Start () {
		Dynamite.Play();
	}

}
