using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public string currentState;
        IPlayerState m_currentState;

        [Header("References")]
        [SerializeField] CharacterController m_controller;
        [SerializeField] AnimationHooks m_animationHooks;
        [SerializeField] Animator m_animator;
        [SerializeField] InputHandler m_inputHandler;
        [SerializeField] InteractionHub m_interactionHub;
        [SerializeField] Inventory m_inventory;
        [SerializeField] Transform m_playerTransform;
        [SerializeField] Transform m_cameraFollow;
        [SerializeField] Transform m_cameraTarget;
        [SerializeField] Transform m_throwPoint;
        [SerializeField] Transform m_swordInHandPoint;
        [SerializeField] Transform m_swordRestPoint;
        [SerializeField] AttackDB_SO m_attackDB;

        [Header("Props")]
        public GameObject Sword;

        [Header("State Properties")]
        [SerializeField] MovementProperties m_freeMovementProperties;
        [SerializeField] AimProperties m_aimProperties;
        [SerializeField] SharedProperties m_sharedProperties;

        #region Public References

        public CharacterController controller => m_controller;
        public Animator Animator => m_animator;
        public InputHandler inputHandler => m_inputHandler;
        public InteractionHub interactionHub => m_interactionHub;
        public Inventory inventory => m_inventory;
        public Transform playerTransform => m_playerTransform;
        public Transform cameraFollow => m_cameraFollow;
        public Transform cameraTarget => m_cameraTarget;
        public Transform throwPoint => m_throwPoint;
        public MovementProperties freeMovementProperties => m_freeMovementProperties;
        public AimProperties aimProperties => m_aimProperties;
        public SharedProperties sharedProperties => m_sharedProperties;

        #endregion

        #region State Properties

        [System.Serializable]
        public class MovementProperties
        {
            public float moveSpeed = 1;
            public float sprintSpeed = 1;
            public float rotateSpeed = 1;
            public float maxCameraVert = 75f;
            public float minCameraVert = -75f;
        }
        
        [System.Serializable]
        public class AimProperties
        {
            public float moveSpeed = 1;
            public float rotateSpeed = 1;
            public Vector3 cameraPos = new Vector3(1, -0.4f, 0.5f);
            public float maxCameraVert = 75f;
            public float minCameraVert = -75f;
            public float throwStrength = 10f;
            public int currentAttackindex = -1;
        }

        [System.Serializable]
        public class SharedProperties
        {
            public Vector3 defaultCameraPos = new Vector3(1, -0.4f, -0.5f);
        }

        #endregion

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //m_animationHooks.CanCancel += OnCanCancel;
            m_currentState = new FreeMovementState(this);
        }

        private void FixedUpdate()
        {
            m_controller.Move(-Vector3.up * 9.81f);
            if(m_animator.IsInTransition(0))
            {
                ResetAnimParamsOnTransition();
            }
            m_currentState.StateFixedUpdate();
        }

        public void SetState(IPlayerState newState)
        {
            m_currentState.StateEnd();
            m_currentState = newState;
        }
        public void SetSwordPosition(bool inHand)
        {
            var point = inHand ? m_swordInHandPoint : m_swordRestPoint;
            Sword.transform.parent = point;
            Sword.transform.localPosition = Vector3.zero;
            Sword.transform.localRotation = Quaternion.identity;
        }

        public void SetCurrentAttackIndex(int index)
        {
            if(index < 0 || index >= m_attackDB.GetDbLength())
            {
                Debug.LogError($"Invalid index: {index} - SCAI110");
                return;
            }

            m_aimProperties.currentAttackindex = index;
        }

        public void OnHit(Hurtbox hurtbox)
        {
            var a = m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            hurtbox.PostOnHurt(m_attackDB.GetArgsByName(a));
        }

        void OnCanCancel()
        {
            m_animator.SetBool("CanCancel", true);
        }

        void ResetAnimParamsOnTransition()
        {
            m_animator.SetBool("CanCancel", false);
        }
    }
}