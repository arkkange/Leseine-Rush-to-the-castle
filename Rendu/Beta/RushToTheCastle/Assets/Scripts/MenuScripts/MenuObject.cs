using UnityEngine;
using System.Collections;

public class MenuObject : MonoBehaviour 
{

	public bool isQuit= false;
	public bool isPlay=false;

	void OnMouseEnter()
	{
		renderer.material.color = Color.red;


	}

	void OnMouseExit()
	{

		renderer.material.color = Color.white;

	}

	void OnMouseDown()
	{
		if (isQuit) 
		{
			Application.Quit ();
		}

		if (isPlay) 
		{
			Application.LoadLevel (1);
		}


	}
}
