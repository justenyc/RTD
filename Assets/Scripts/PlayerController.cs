using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        IPlayerState m_currentState;
        public CharacterController m_controller {get; private set;}
        public InputHandler m_inputHandler { get; private set; }

        [Header("References")]
        public Transform m_playerTransform;
        public Transform m_cameraFollow;
        public Transform m_cameraTarget;

        public MovementProperties m_moveProperties;

        [System.Serializable]
        public class MovementProperties
        {
            public float m_moveSpeed = 1;
            public float m_sprintSpeed = 1;
            public float m_rotateSpeed = 1;
            public float m_maxCameraVert = 75f;
            public float m_minCameraVert = -75f;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            m_controller = GetComponent<CharacterController>();
            m_inputHandler = GetComponent<InputHandler>();

            m_currentState = new FreeMovementState(this);
        }

        private void FixedUpdate()
        {
            m_currentState.StateFixedUpdate();
        }

        public void SetState(IPlayerState newState)
        {
            m_currentState = newState;
        }

        public void Test(InputAction.CallbackContext ctx)
        {
            Debug.Log("Test!");
        }
    }
}