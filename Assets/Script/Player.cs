using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Player : MonoBehaviour {

    public enum PlayerState
    {
        Normal,
        Talking,
        End
    }
    [SerializeField]
    float finalTargetDistance = 1.5f;
    [SerializeField]
    Camera[] cameras;
    [HideInInspector]
    public Transform talkedNPC = null;

    PlayerState selfstate = PlayerState.Normal;
    public PlayerState SelfState
    { get { return selfstate; } }
       

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach (var camera in cameras)
            camera.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);

        if (Vector3.Distance(transform.position,GameObject.FindWithTag("Target").transform.position)< finalTargetDistance)
        {
            if (selfstate != PlayerState.End)
            {
                if (GameObject.FindWithTag("Level").GetComponent<LevelManager>().isThirdLeave)
                {
                    selfstate = PlayerState.End;
                    GetComponent<ThirdPersonUserControl>().m_movable = false;
                    UIDialogueManager.Singleton.MakeDialogue(transform, GameObject.FindWithTag("Target").transform,  DialogueManager.DiaGroupType.Fail);
                    UIDialogueManager.Singleton.onDialogueEnd += GameObject.FindWithTag("Level").GetComponent<LevelManager>().LevelFail;
                }
                else
                {
                    selfstate = PlayerState.End;
                    GetComponent<ThirdPersonUserControl>().m_movable = false;
                    UIDialogueManager.Singleton.MakeDialogue(transform, GameObject.FindWithTag("Target").transform,  DialogueManager.DiaGroupType.Success);
					GameObject.FindWithTag("Level").GetComponent<LevelManager>().stateLocked = true;
                    UIDialogueManager.Singleton.onDialogueEnd += GameObject.FindWithTag("Level").GetComponent<LevelManager>().LevelSuccess;
                }
            }
        }
	}

    public void SetStateTalking()
    {
        selfstate = PlayerState.Talking;
        GetComponent<ThirdPersonUserControl>().m_movable = false;
    }

    public void SetStateNormal()
    {
        selfstate = PlayerState.Normal;
        GetComponent<ThirdPersonUserControl>().m_movable = true;
    }
}
