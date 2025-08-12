using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Shambler : AI_Entity
{
    [Header("Aggression Properties")]
    //[SerializeField] float m_moveSpeed = 2;
    //[SerializeField] float m_turnSpeed = 1;
    
    [SerializeField]
    [Tooltip("How close the enemy needs to be before deciding to attack")]
    float m_attackRange = 10;

    [SerializeField]
    [Tooltip("How close the enemy needs to be to perform a melee attack")]
    float m_meleeRange = 1;

    [SerializeField] 
    [Tooltip("How long the enemy continues to search for the player after losing sight of them")] 
    float m_aggroTime = 5;
    
    [SerializeField]
    [Tooltip("How often the enemy decides to attack")]
    float m_attackFrequency = 5;
    
    [SerializeField]
    [Tooltip("The range of randomness multiplied to the attack frequency. Lower numbers increase frequency, higher numbers decrease frequency")]
    Vector2 m_attackFrequencyRandomness = Vector2.one;

    [Header("Defensive Properties")]
    [SerializeField] float m_knockdownValue;
    [SerializeField] float m_knockdownValueRecovery = 1;
    [SerializeField] float m_hitstunResist = 2;
    [SerializeField] float m_hitstaggerResist = 4;
    [SerializeField] float m_knockdownThreshold = 6;
    
    [SerializeField] LayerMask m_layerMask;

    [Header("References")]
    [SerializeField] Transform m_mainTransform;
    [SerializeField] Animator m_animator;
    [SerializeField] AnimationHooks m_animationHooks;
    [SerializeField] List<Transform> m_patrolPoints;
    [SerializeField] Detector m_detector;
    [SerializeField] Health m_health;

    #region Animation

    public void SetAnimatorBool(string name, bool b)
    {
        m_animator.SetBool(name, b);
    }

    public void SetAnimatorFloat(string name, float f)
    {
        m_animator.SetFloat(name, f);
    }

    public void SetAnimatorTrigger(string name)
    {
        m_animator.SetTrigger(name);
    }

    public void SetAnimatorInt(string name, int i)
    {
        m_animator.SetInteger(name, i);
    }

    void ResetAnimatorParams()
    {
        SetAnimatorBool("InAttackRange", false);
    }

    public void OnAttackFinish()
    {
        Debug.Log("OnAttackFinish() called");
        m_blackboard.SetValue(ATTACKING, false);
        m_blackboard.SetValue(TRACK_ROTATION, true);
        ResetAnimatorParams();
    }

    #endregion

    #region Blackboard Entry Names

    public const string DETECTED_SOMETHING = "DetectedSomething";
    public const string DETECTED_POSITION = "DetectedPosition";
    public const string AGGRO = "Aggro";
    public const string ATTACKING = "Attacking";
    public const string TRACK_ROTATION = "TrackRotation";

    #endregion

    protected override void Start()
    {
        m_blackboard.AddEntry(DETECTED_SOMETHING, false);
        m_blackboard.AddEntry(DETECTED_POSITION, this.transform.position);
        m_blackboard.AddEntry(AGGRO, false);
        m_blackboard.AddEntry(ATTACKING, false);
        m_blackboard.AddEntry(TRACK_ROTATION, true);

        m_detector.OnTriggerStayPost += Search;
        m_detector.OnTriggerEnterPost += OnDetect;
        m_detector.OnTriggerExitPost += SearchLost;

        m_animationHooks.AttackFinishedPost += OnAttackFinish;

        InitStandardAI();
    }

    void InitStandardAI()
    {
        var patrolPoints = base.ParsePatrolPoints(m_patrolPoints);

        var shamblerAiSelector = new PrioritySelector("ShamblerAiSelector");

        var seq = new Sequence("Search and Stalk", 100);
        seq.AddChild(new Leaf("Condition", new IStrategy.Condition(() =>
        {
            var detection = m_blackboard.GetEntryByKey<bool>(DETECTED_SOMETHING) as BlackboardEntry<bool>;
            return detection.value;
        })));
        seq.AddChild(new Leaf("Stalk", new IStrategy.Stalk(m_agent, m_blackboard, m_attackRange)));

        var randomSelector = new RandomSelector("AttackRNG");
        var tackleSequence = new Sequence("TackleSequence");
        tackleSequence.AddChild(new Leaf("Tackle", new IStrategy.ActionStrategy(() =>
        {
            Debug.Log("RNG picked tackle!");
            m_blackboard.SetValue(ATTACKING, true);
            SetAnimatorBool("InAttackRange", true);
            SetAnimatorFloat("AttackIndex", 0);
            m_blackboard.SetValue(TRACK_ROTATION, false);
            return;
        })));

        tackleSequence.AddChild(new Leaf("Delay Until Attack Finish", new IStrategy.DelayUntil(() =>
        {
            var entry = m_blackboard.GetEntryByKey<bool>(ATTACKING) as BlackboardEntry<bool>;
            return !entry.value;
        })));
        randomSelector.AddChild(tackleSequence);

        var swipeSequence = new Sequence("SwipeSequence");
        var repeatForSeconds = new RepeatForSeconds("RepeatSwipeSequence", m_attackFrequency);
        repeatForSeconds.AddChild(new Leaf("CloseRangeStalk", new IStrategy.Stalk(m_agent, m_blackboard, m_meleeRange)));
        swipeSequence.AddChild(repeatForSeconds);
        swipeSequence.AddChild(new Leaf("MeleeDistanceCheck", new IStrategy.Condition(() =>
        {
            //Debug.Log(Vector3.Distance(m_agent.destination, m_mainTransform.position));
            var detection = m_blackboard.GetEntryByKey<bool>(DETECTED_SOMETHING) as BlackboardEntry<bool>;
            return Vector3.Distance(m_agent.destination, m_mainTransform.position) < m_meleeRange && detection.value;
        })));
        swipeSequence.AddChild(new Leaf("Swipe", new IStrategy.ActionStrategy(() =>
        {
            Debug.Log("RNG picked swipe!");
            m_blackboard.SetValue(ATTACKING, true);
            SetAnimatorBool("InAttackRange", true);
            SetAnimatorFloat("AttackIndex", 1);
            m_blackboard.SetValue(TRACK_ROTATION, false);
            return;
        })));
        randomSelector.AddChild(swipeSequence);
        seq.AddChild(randomSelector);

        shamblerAiSelector.AddChild(seq);
        shamblerAiSelector.AddChild(new Leaf("MoveBetweenPoints", new IStrategy.MoveBetweenPoints(base.m_agent, patrolPoints)));// m_moveSpeed)));
        base.m_behaviourTree.AddChild(shamblerAiSelector);
    }

    protected override void Update()
    {
        UpdateKnockdownValue();
        TrackRotation();
        SetAnimatorFloat("Movement", ScaleMoveSpeedToDistance());
        base.Update();
    }

    float ScaleMoveSpeedToDistance()
    {
        float scale = 0;
        if (m_agent.remainingDistance > 10)
        {
            scale = 1;
        }
        else if (m_agent.remainingDistance > 1f)
        {
            scale = 0.5f;
        }
        else
        {
            scale = 0;
        }
        return scale;
    }

    void UpdateKnockdownValue()
    {
        m_knockdownValue = Mathf.Clamp(m_knockdownValue - Time.deltaTime * m_knockdownValueRecovery, 0, 999);
        if (m_knockdownValue > m_knockdownThreshold)
        {
            m_knockdownValue = 0;
        }
    }

    void ApplyGravity()
    {
        m_mainTransform.position += Physics.gravity * Time.deltaTime;
    }

    void TrackRotation()
    {
        var track = m_blackboard.GetEntryByKey<bool>(TRACK_ROTATION) as BlackboardEntry<bool>;
        //Debug.Log(track.value);
        if (track.value)
        {
            var lookVector = m_agent.steeringTarget - m_mainTransform.position;
            if (lookVector.magnitude > 0)
            {
                m_mainTransform.rotation = Quaternion.LookRotation(lookVector.normalized);
            }
        }
    }

    protected virtual void OnDetect(Collider other)
    {
        m_blackboard.AddEntry<bool>(DETECTED_SOMETHING, false);
    }

    protected virtual void Search(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, (other.transform.position + Vector3.up) - this.transform.position, out hit, 100, m_layerMask))
        {
            Debug.DrawRay(this.transform.position, (other.transform.position + Vector3.up) - this.transform.position, Color.magenta);

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                m_blackboard.SetValue(DETECTED_SOMETHING, true);
                m_blackboard.SetValue(AGGRO, true);
                m_blackboard.SetValue(DETECTED_POSITION, other.transform.position);
                return;
            }

            LostTarget();
            //Debug.Log(other.transform.position);
        }
    }

    protected virtual void SearchLost(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        SetAnimatorBool("InAttackRange", false);
        LostTarget();
    }

    void LostTarget()
    {
        this.StopAllCoroutines();
        m_blackboard.SetValue(AGGRO, false);
        StartCoroutine(
            Helper.DelayActionByTime(
                () => m_blackboard.SetValue(DETECTED_SOMETHING, false), 
                m_aggroTime)
            );
    }

    void OnHurt(Hitbox.Args args)
    {
        //m_health.ChangeCurrentHealth(-args.power);
        m_knockdownValue += 2;
        m_animator.SetTrigger("GotHit");
        if (m_knockdownValue >= m_knockdownThreshold)
        {
            m_animator.SetTrigger("Knockdown");
            ResetAnimatorParams();
        }
        else if (m_knockdownValue >= m_hitstaggerResist)
        {
            m_animator.SetTrigger("HitStagger");
            ResetAnimatorParams();
        }
        else if (m_knockdownValue >= m_hitstunResist)
        {
            m_animator.SetTrigger("HitStun");
            ResetAnimatorParams();
        }
    }
}