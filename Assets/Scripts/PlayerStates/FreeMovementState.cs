using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Player
{
    public class FreeMovementState : IPlayerState
    {
        PlayerController m_manager;

        public FreeMovementState(PlayerController manager)
        {
            m_manager = manager;
            StateStart();
        }

        public void StateStart()
        {
            
        }

        public void StateUpdate()
        {

        }

        public void StateFixedUpdate()
        {
            Move();
            ControlCamera();
        }

        private void Move()
        {
            float speed = m_manager.m_inputHandler.m_sprint ? m_manager.m_moveProperties.m_sprintSpeed :  m_manager.m_moveProperties.m_moveSpeed;

            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;
            Vector3 inputVector = m_manager.m_inputHandler.m_moveVector * speed;

            cameraForward.y = 0;
            cameraRight.y = 0;

            Vector3 forwardRelative = cameraForward * inputVector.y;
            Vector3 rightRelative = cameraRight * inputVector.x;

            Vector3 moveVector = forwardRelative + rightRelative;
            m_manager.m_controller.Move(moveVector * Time.fixedDeltaTime);
        }

        private void ControlCamera()
        {
            Vector2 inputVector = m_manager.m_inputHandler.m_lookVector;
            Vector3 currentPlayerRot = m_manager.m_playerTransform.rotation.eulerAngles;
            Vector3 newPlayerRot = currentPlayerRot + Vector3.up * inputVector.x * m_manager.m_moveProperties.m_rotateSpeed * Time.fixedDeltaTime;
            m_manager.m_playerTransform.rotation = Quaternion.Euler(newPlayerRot);

            Vector3 currentCameraFollowRot = m_manager.m_cameraFollow.rotation.eulerAngles;
            Vector3 newCameraFollowRot = currentCameraFollowRot + Vector3.right * -inputVector.y * m_manager.m_moveProperties.m_rotateSpeed * Time.fixedDeltaTime;
            //Debug.Log(newCameraFollowRot);
            newCameraFollowRot.x = (newCameraFollowRot.x > 180) ? newCameraFollowRot.x - 360 : newCameraFollowRot.x;
            newCameraFollowRot.x = Mathf.Clamp(newCameraFollowRot.x, m_manager.m_moveProperties.m_minCameraVert, m_manager.m_moveProperties.m_maxCameraVert);
            m_manager.m_cameraFollow.rotation = Quaternion.Euler(newCameraFollowRot);
        }
    }
}