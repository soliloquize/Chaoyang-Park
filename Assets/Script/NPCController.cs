using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour {

    [SerializeField]
    float viewDistance = 4;
    [SerializeField]
    float viewAngle = 60;
    [SerializeField]
    float riceDelayTime = 2;
    [SerializeField]
    float normalSpeed = 0.5f;
    [SerializeField]
    float aroundPlayerSpeed = 0.3f;
    [SerializeField]
    float finalTargetDistance = 1.5f;
    [SerializeField]
    Transform target;
    [SerializeField]
    Player player;

    public enum BigMomState
    {
        Patrol,
        PlayerSeen,
        //InDistance,
        Spread,
        NPCSeen,
        BeSeen,
        Follow
    }

    public enum BigMomId
    {
        Leader,
        Normal,
        Member
    }
    
    [SerializeField]
    NavMeshAgent selfNMA;
    [SerializeField]
    Transform[] patrolPoints;

    GameObject[] npcs;

    BigMomId id = BigMomId.Normal;
    Player.PlayerState talkingState = Player.PlayerState.Normal;

    /// <summary>
    /// 如果是队员，对应的队长
    /// </summary>
    Transform leader = null;

    [HideInInspector]
    public BigMomState sta = BigMomState.Patrol;
  
    [HideInInspector]
    public bool gotMessage = false;

    GameObject sector;

    int patrolIndex = 0;
	// Use this for initialization
	void Start () {
        //SetState(BigMomState.Patrol);
        sector = GlobalTool.FindChild(transform, "SpriteSensor");
        if (player == null)
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        npcs = GameObject.FindGameObjectsWithTag("NPC");
        selfNMA = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update() {
        if (id == BigMomId.Member && sector.active)
            sector.SetActive(false);
        //Tool.DrawSector(transform, transform.position+Vector3.up, viewAngle, viewDistance);
        
    }

    public void SetStateFollow()
    {
        sta = BigMomState.Follow;
        InvokeRepeating("Follow", 0.2f, 0.1f);
    }

    void Follow()
    {
        selfNMA.destination = leader.position;
        selfNMA.radius = 0.1f;
    }
    

    void BeSeen(Transform leader)
    {
        sta = BigMomState.BeSeen;
        this.leader = leader;
        selfNMA.destination = transform.position;
        //StopCoroutine(ChangePatrolPoint());
        id = BigMomId.Member;
        gotMessage = true;
    }
    IEnumerator SeePlayer()
    {
        talkingState = Player.PlayerState.Talking;
        sta = BigMomState.PlayerSeen;
        StopCoroutine(ChangePatrolPoint());
        gotMessage = true;
        id = BigMomId.Leader;
        player.SetStateTalking();
        player.talkedNPC = transform;
        selfNMA.SetDestination(player.transform.position);
        yield return new WaitUntil(() => selfNMA.remainingDistance <= 0.7f);
        UIDialogueManager.Singleton.onDialogueEnd += OnPlayerTalkEnd;
        UIDialogueManager.Singleton.onDialogueEnd += player.SetStateNormal;
        StartTalking(transform,player.transform,DialogueManager.DiaGroupType.ToPlayer);
    }

    void SetStateSpread()
    {
        talkingState = Player.PlayerState.Normal;
        sta = BigMomState.Spread;
        StopCoroutine(ChangePatrolPoint());
        selfNMA.SetDestination(FindSpreadNPC().position);
    }

    void OnPlayerTalkEnd()
    {
        talkingState = Player.PlayerState.Normal;
        StartCoroutine(DelaySpread());
    }

    IEnumerator DelaySpread()
    {
        sta = BigMomState.Patrol;
        yield return new WaitForSeconds(riceDelayTime);
        SetStateSpread();
    }
    IEnumerator SeeAnotherNPC(Transform another)
    {
        talkingState = Player.PlayerState.Talking;
        StopCoroutine(ChangePatrolPoint());
        sta = BigMomState.NPCSeen;
        selfNMA.SetDestination(another.position);
        yield return new WaitUntil(() => selfNMA.remainingDistance <= 0.7f);
        UIDialogueManager.Singleton.onDialogueEnd += SetStateSpread;
        UIDialogueManager.Singleton.onDialogueEnd += another.GetComponent<NPCController>().SetStateFollow;
        StartTalking(transform,another,DialogueManager.DiaGroupType.ToNPC);
    }

    void FixedUpdate()
    {
        if (Tool.CheckIfSeen(transform, transform.position, viewAngle, viewDistance, GameObject.FindWithTag("Player").transform) )
        {
            selfNMA.speed = aroundPlayerSpeed;
        }
        else
            selfNMA.speed = normalSpeed;

        if (id == BigMomId.Member || talkingState == Player.PlayerState.Talking)
            return;

        if (Tool.CheckIfSeen(transform, transform.position, viewAngle, viewDistance, GameObject.FindWithTag("Player").transform) && !gotMessage && GameObject.FindWithTag("Player").GetComponent<Player>().SelfState== Player.PlayerState.Normal)
        {
            StartCoroutine(SeePlayer());
        }
        foreach (var npc in npcs)
            if (Tool.CheckIfSeen(transform, transform.position, viewAngle, viewDistance, npc.transform) && transform != npc.transform)
            {
                NPCController n = npc.GetComponent<NPCController>();
                if (!n.gotMessage && gotMessage&&n.talkingState!= Player.PlayerState.Talking)
                {
                    //n.BeSeen(transform);
                    //n.leader = transform;
                    //n.selfNMA.SetDestination(transform.position);
                    StartCoroutine(SeeAnotherNPC(n.transform));
                }
            }

        if (sta == BigMomState.Patrol && selfNMA.remainingDistance <= 0.7f && !selfNMA.isStopped)
        {
            StartCoroutine(ChangePatrolPoint());
        }

        if (Vector3.Distance(transform.position, target.position) < finalTargetDistance)
        {
            GameObject.FindWithTag("Level").GetComponent<LevelManager>().LevelFail();
            selfNMA.isStopped = true;
        }
    }

    IEnumerator ChangePatrolPoint()
    {
        if (patrolIndex >= patrolPoints.Length)
            patrolIndex = 0;
        selfNMA.isStopped = true;
        yield return new WaitForSeconds(1);

        selfNMA.isStopped = false;
        if (sta == BigMomState.Patrol)
        {
            if (patrolPoints.Length > 0)
            {
				if(patrolIndex < patrolPoints.Length&&patrolIndex>=0)
                {
                    selfNMA.SetDestination(patrolPoints[patrolIndex].position);
                    ++patrolIndex;
                }
            }
        }
    }
    

    Transform FindSpreadNPC()
    {
        ///无消息NPC
        List<GameObject> npcsNoMessage = new List<GameObject>();
        for (int i = 0; i < npcs.Length; ++i)
        {
            if (!npcs[i].GetComponent<NPCController>().gotMessage)
                npcsNoMessage.Add(npcs[i]);
        }

        /// 只寻找无消息NPC
        float[] lengths = new float[npcsNoMessage.Count];
        for (int i = 0;i< npcsNoMessage.Count;++i)
        {
            if (!npcsNoMessage[i].GetComponent<NPCController>().gotMessage)
            {
                NavMeshPath path1 = new NavMeshPath();
                NavMeshPath path2 = new NavMeshPath();
                selfNMA.CalculatePath(npcsNoMessage[i].transform.position, path1);
                NavMesh.CalculatePath(npcsNoMessage[i].transform.position, target.position, NavMesh.AllAreas, path2);

                if (CalculatePathLength(path1) == 0)
                    lengths[i] = 0;
                else
                    lengths[i] = CalculatePathLength(path1) + CalculatePathLength(path2);
            }
        }

        float minLength = 10000f;
        int minLengthIndex = 0;
        for(int i = 0;i<lengths.Length;++i)
        {
            if (lengths[i] < minLength && lengths[i]!=0)
            {
                minLength = lengths[i];
                minLengthIndex = i;
            }
        }

        NavMeshPath path = new NavMeshPath();
        selfNMA.CalculatePath(target.position, path);
        if (CalculatePathLength(path) < minLength)
            return target;

        return npcsNoMessage[minLengthIndex].transform;
    }

    float CalculatePathLength(NavMeshPath path)
    {
        if (path.corners.Length < 2)
            return 0;

        Vector3 previousCorner = path.corners[0];
        float lengthSoFar = 0.0F;
        int i = 1;
        while (i < path.corners.Length)
        {
            Vector3 currentCorner = path.corners[i];
            lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
            previousCorner = currentCorner;
            i++;
        }
        return lengthSoFar;
    }

    void StartTalking(Transform teller, Transform listener, DialogueManager.DiaGroupType type)
    {
        UIDialogueManager.Singleton.MakeDialogue(teller, listener, type);
        ///完成时继续Spread并且Player变Normal
        //NPCState = NPCController.BigMomState.Spread;
        //player.SelfState = Player.PlayerState.Normal;
    }

    //void BeTalked()
    //{
    //    gotMessage = true;
    //    selfNMA.SetDestination(transform.position);
    //}
}
