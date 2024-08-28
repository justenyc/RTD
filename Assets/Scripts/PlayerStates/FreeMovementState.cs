using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

namespace Player
{
    public class FreeMovementState : IPlayerState
    {
        PlayerController m_manager;

        #region State Input Values (if needed)

        bool m_sprintInputState;

        #endregion 

        public FreeMovementState(PlayerController manager)
        {
            m_manager = manager;
            StateStart();
        }

        public void StateStart()
        {
            //Debug.Log("Starting FreeMovement State");
            m_manager.currentState = "FreeMovement";
            m_manager.inputHandler.Sprint += OnSprint;
            m_manager.inputHandler.Aim += OnAim;
            m_manager.inputHandler.UseCurrentItem += OnUseCurrentItem;
        }

        public void StateEnd()
        {
            //Debug.Log("Ending FreeMovement State");
            m_manager.inputHandler.Sprint -= OnSprint;
            m_manager.inputHandler.Aim -= OnAim;
            m_manager.inputHandler.UseCurrentItem -= OnUseCurrentItem;
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
            float speed = m_sprintInputState ? m_manager.freeMovementProperties.sprintSpeed :  m_manager.freeMovementProperties.moveSpeed;

            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;
            Vector3 inputVector = m_manager.inputHandler.moveVector * speed;

            cameraForward.y = 0;
            cameraRight.y = 0;

            Vector3 forwardRelative = cameraForward * inputVector.y;
            Vector3 rightRelative = cameraRight * inputVector.x;

            Vector3 moveVector = forwardRelative + rightRelative;
            moveVector.y = -9.81f;
            m_manager.controller.Move(moveVector * Time.fixedDeltaTime);
        }

        private void ControlCamera()
        {
            Vector2 inputVector = m_manager.inputHandler.lookVector;
            Vector3 currentPlayerRot = m_manager.playerTransform.rotation.eulerAngles;
            Vector3 newPlayerRot = currentPlayerRot + Vector3.up * inputVector.x * m_manager.freeMovementProperties.rotateSpeed * Time.fixedDeltaTime;
            m_manager.playerTransform.rotation = Quaternion.Euler(newPlayerRot);

            Vector3 currentCameraFollowRot = m_manager.cameraFollow.rotation.eulerAngles;
            Vector3 newCameraFollowRot = currentCameraFollowRot + Vector3.right * -inputVector.y * m_manager.freeMovementProperties.rotateSpeed * Time.fixedDeltaTime;
            //Debug.Log(newCameraFollowRot);
            newCameraFollowRot.x = (newCameraFollowRot.x > 180) ? newCameraFollowRot.x - 360 : newCameraFollowRot.x;
            newCameraFollowRot.x = Mathf.Clamp(newCameraFollowRot.x, m_manager.freeMovementProperties.minCameraVert, m_manager.freeMovementProperties.maxCameraVert);
            m_manager.cameraFollow.rotation = Quaternion.Euler(newCameraFollowRot);
        }

        #region State Input Listeners

        void OnSprint(InputAction.CallbackContext context)
        {
            m_sprintInputState = context.performed;
        }

        void OnAim(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                m_manager.SetState(new AimState(m_manager));
            }
        }

        void OnUseCurrentItem(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                var item = m_manager.inventory.currentItem;
                var result = Item_Data.GetItemData(item.data).useAction(m_manager.gameObject);

                if(result)
                {
                    m_manager.inventory.AddOrRemoveItemFromInventory(item, -1);
                }
            }
        }

        #endregion
    }
}