using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    [HideInInspector]
    public bool isThirdLeave = false;
	[HideInInspector]
    public bool stateLocked = false;
    [SerializeField]
    GameObject successPanel;
    [SerializeField]
    GameObject failPanel;

	public AudioClip win, fail;
	private AudioSource source;
	// Use this for initialization
	void Awake () {
        DataManager.Singleton.Initialize();
        DialogueManager.Singleton.Initialize();

		source = GetComponent<AudioSource>();
		source.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LevelFail()
    {
		source.clip = fail;
		source.Play();
        if (!stateLocked)
        {
            stateLocked = true;
            isThirdLeave = true;
            successPanel.SetActive(false);
            failPanel.SetActive(true);
        }
    }

    public void LevelSuccess()
    {
		source.clip = win;
		source.Play();
        if (stateLocked)
        {
            stateLocked = true;
            isThirdLeave = false;
            successPanel.SetActive(true);
            failPanel.SetActive(false);
        }
    }

    public void LoadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex != 3)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            ToFirstScene();
    }

    public void RefreshCurScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToFirstScene()
    {
        SceneManager.LoadScene(0);
    }
    public void CloseGame()
    {
        Application.Quit();
    }
}
