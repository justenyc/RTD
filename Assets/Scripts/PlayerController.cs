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
        [SerializeField] Hitbox m_hitbox;

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
        }

        [System.Serializable]
        public class SharedProperties
        {
            public Vector3 defaultCameraPos = new Vector3(1, -0.4f, -0.5f);
        }

        #endregion

        private void Start()
        {
            m_hitbox.OnHit += OnHit;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            m_currentState = new FreeMovementState(this);
        }

        private void FixedUpdate()
        {
            m_controller.Move(-Vector3.up * 9.81f);
            m_currentState.StateFixedUpdate();
        }

        public void SetState(IPlayerState newState)
        {
            m_currentState.StateEnd();
            m_currentState = newState;
        }

        void OnHit(Hurtbox hurtbox)
        {
            var args = new Hitbox.Args(10);
            hurtbox.PostOnHurt(args);
        }
    }
}