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
        public CharacterController controller {get; private set;}
        public InputHandler inputHandler { get; private set; }

        [Header("References")]
        public Transform playerTransform;
        public Transform cameraFollow;
        public Transform cameraTarget;

        [Header("State Properties")]
        public MovementProperties freeMovementProperties;
        public AimProperties aimProperties;
        public SharedProperties sharedProperties;

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
            public Vector3 cameraPos = new Vector3(1, -0.4f, -0.5f);
        }

        #endregion

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            controller = GetComponent<CharacterController>();
            inputHandler = GetComponent<InputHandler>();

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