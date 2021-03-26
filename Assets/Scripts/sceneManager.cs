using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour {

	public void loadScene (string name){
		Time.timeScale = 1f;
		SceneManager.LoadScene (name);
	}

	public void exit (){
		Application.Quit ();
	}
}
