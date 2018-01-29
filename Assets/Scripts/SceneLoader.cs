using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public float time;
	public string name;


	void Start(){
		Invoke("LoadSceneName",time);
	}

	void LoadSceneName () {
		SceneManager.LoadScene(name);		
	}
}
