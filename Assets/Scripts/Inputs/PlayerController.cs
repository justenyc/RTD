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
        [SerializeField] Animator m_animator;
        [SerializeField] InputHandler m_inputHandler;
        [SerializeField] InteractionHub m_interactionHub;
        [SerializeField] Inventory m_inventory;
        [SerializeField] Transform m_playerTransform;
        [SerializeField] Transform m_cameraFollow;
        [SerializeField] Transform m_cameraTarget;
        [SerializeField] Transform m_throwPoint;
        [SerializeField] RigidbodyThrower m_rigidbodyThrower;
        [SerializeField] Status m_status;
        [SerializeField] AttackDB_SO m_attackDB;
        [SerializeField] FrameData m_frameData;
        [SerializeField] EventBus_Thea m_eventBus;

        [Header("State Properties")]
        [SerializeField] MovementProperties m_freeMovementProperties;
        [SerializeField] AimProperties m_aimProperties;
        [SerializeField] SharedProperties m_sharedProperties;
        [SerializeField] bool m_canMove;
        [SerializeField] bool m_listeningForInputs;
        [SerializeField] bool m_canThrow;

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
        public RigidbodyThrower rigidbodyThrower => m_rigidbodyThrower;
        public Status status => m_status;
        public MovementProperties freeMovementProperties => m_freeMovementProperties;
        public AimProperties aimProperties => m_aimProperties;
        public SharedProperties sharedProperties => m_sharedProperties;
        public FrameData frameData => m_frameData;
        public EventBus_Thea eventBus => m_eventBus;
        public bool canMove => m_canMove;
        public bool listeningForInputs => m_listeningForInputs;
        public bool canThrow => m_canThrow;
        #endregion

        #region State Properties

        [System.Serializable]
        public class MovementProperties
        {
            public float moveSpeed = 1;
            public float sprintSpeed = 1;
            public float cameraRotateSpeed = 1;
            public float cameraRotateSpeedDuringSprint = 0f;
            [Tooltip("How strong the rotation of the camera needs to be before the player stops moving and only rotates. A.K.A \"The Leon Shuffle\"")]
            public float leonShuffleThreshold = 100f;
            public float accelerationStrength = 1;
            public float decelerationStrength = 1;
            public float easeInAnimationStrength = 1;
            public float easeOutAnimationStrength = 1;
            public float maxCameraVert = 75f;
            public float minCameraVert = -75f;
        }
        
        [System.Serializable]
        public class AimProperties
        {
            public float moveSpeed = 1;
            public float rotateSpeed = 1;
            public float accelerationStrength = 1;
            public float decelerationStrength = 1;
            public float easeInAnimationStrength = 1;
            public float easeOutAnimationStrength = 1;
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

        public void SetCanMove(bool b)
        {
            m_canMove = b;
        }

        public void SetListeningForInputs(bool b)
        {
            m_listeningForInputs = b;
        }

        public void SetCanThrow(bool b)
        {
            m_canThrow = b;
        }

        public void SetState(IPlayerState newState)
        {
            m_currentState.StateEnd();
            m_currentState = newState;
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
            hurtbox.PostOnHurt(m_status.GenerateHbArgs(m_attackDB.GetArgsByName(a)));
        }

        void OnCanCancel()
        {
            m_animator.SetBool("CanCancel", true);
        }

        void ResetAnimParamsOnTransition()
        {
            m_animator.SetBool("CanCancel", false);
        }

        public void ThrowCurrentItem()
        {
            var itemToThrow = inventory.GetCurrentItem();
            Vector3 throwDir = (Camera.main.transform.forward.normalized * aimProperties.throwStrength) - (rigidbodyThrower.transform.right + -Camera.main.transform.right);

            itemToThrow.Throw(rigidbodyThrower, throwDir, aimProperties.throwStrength);
            return;
        }
    }
}