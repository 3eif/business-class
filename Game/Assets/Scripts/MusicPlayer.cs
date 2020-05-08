using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
	public static MusicPlayer instance = null;

	//This function makes sure that the music does not destroy itself when the game switches scenes.
	void Awake ()
	{
		if(instance != null){
			Destroy(gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}
}
