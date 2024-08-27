using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class AimState : IPlayerState
    {
        PlayerController m_manager;
        private Cinemachine3rdPersonFollow _3rdPersonFollow;

        public AimState(PlayerController manager)
        {
            m_manager = manager;
            StateStart();
        }

        public void StateStart()
        {
            Debug.Log("Starting Aim State");
            m_manager.currentState = "Aim";
            m_manager.inputHandler.Aim += OnAim;
            _3rdPersonFollow = CameraManager.instance.VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            _3rdPersonFollow.ShoulderOffset = m_manager.aimProperties.cameraPos;
        }

        public void StateEnd()
        {
            Debug.Log("Ending Aim State");
            m_manager.inputHandler.Aim -= OnAim;
            _3rdPersonFollow.ShoulderOffset = m_manager.sharedProperties.cameraPos;
        }

        public void StateUpdate()
        {
            
        }

        public void StateFixedUpdate()
        {
            Move();
            ControlCamera();
        }

        #region State Inputs

        private void Move()
        {
            float speed = m_manager.aimProperties.moveSpeed;

            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;
            Vector3 inputVector = m_manager.inputHandler.moveVector * speed;

            cameraForward.y = 0;
            cameraRight.y = 0;

            Vector3 forwardRelative = cameraForward * inputVector.y;
            Vector3 rightRelative = cameraRight * inputVector.x;

            Vector3 moveVector = forwardRelative + rightRelative;
            m_manager.controller.Move(moveVector * Time.fixedDeltaTime);
        }

        private void ControlCamera()
        {
            Vector2 inputVector = m_manager.inputHandler.lookVector;
            Vector3 currentPlayerRot = m_manager.playerTransform.rotation.eulerAngles;
            Vector3 newPlayerRot = currentPlayerRot + Vector3.up * inputVector.x * m_manager.aimProperties.rotateSpeed * Time.fixedDeltaTime;
            m_manager.playerTransform.rotation = Quaternion.Euler(newPlayerRot);

            Vector3 currentCameraFollowRot = m_manager.cameraFollow.rotation.eulerAngles;
            Vector3 newCameraFollowRot = currentCameraFollowRot + Vector3.right * -inputVector.y * m_manager.aimProperties.rotateSpeed * Time.fixedDeltaTime;
            //Debug.Log(newCameraFollowRot);
            newCameraFollowRot.x = (newCameraFollowRot.x > 180) ? newCameraFollowRot.x - 360 : newCameraFollowRot.x;
            newCameraFollowRot.x = Mathf.Clamp(newCameraFollowRot.x, m_manager.aimProperties.minCameraVert, m_manager.aimProperties.maxCameraVert);
            m_manager.cameraFollow.rotation = Quaternion.Euler(newCameraFollowRot);
        }

        void OnAim(InputAction.CallbackContext context)
        {
            if(context.canceled)
            {
                m_manager.SetState(new FreeMovementState(m_manager));
            }
        }

        #endregion
    }
}