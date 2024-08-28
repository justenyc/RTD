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
        [SerializeField] InputHandler m_inputHandler;
        [SerializeField] Inventory m_inventory;
        [SerializeField] Transform m_playerTransform;
        [SerializeField] Transform m_cameraFollow;
        [SerializeField] Transform m_cameraTarget;

        [Header("State Properties")]
        public MovementProperties freeMovementProperties;
        public AimProperties aimProperties;
        public SharedProperties sharedProperties;

        #region Public References

        public CharacterController controller => m_controller;
        public InputHandler inputHandler => m_inputHandler;
        public Inventory inventory => m_inventory;
        public Transform playerTransform => m_playerTransform;
        public Transform cameraFollow => m_cameraFollow;
        public Transform cameraTarget => m_cameraTarget;

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

            m_currentState = new FreeMovementState(this);
        }

        private void FixedUpdate()
        {
            m_currentState.StateFixedUpdate();
        }

        public void SetState(IPlayerState newState)
        {
            m_currentState.StateEnd();
            m_currentState = newState;
        }

        public void Test(InputAction.CallbackContext ctx)
        {
            Debug.Log("Test!");
        }
    }
}