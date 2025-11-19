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
        Vector2 animationVector;
        Vector2 movementVector;
        bool isMoving = false;

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
            //m_manager.inputHandler.Aim += OnAim;
            m_manager.inputHandler.UseCurrentItem += OnUseCurrentItem;
            m_manager.inputHandler.Interact += OnInteract;
        }

        public void StateEnd()
        {
            //Debug.Log("Ending FreeMovement State");
            m_manager.inputHandler.Sprint -= OnSprint;
            //m_manager.inputHandler.Aim -= OnAim;
            m_manager.inputHandler.UseCurrentItem -= OnUseCurrentItem;
            m_manager.inputHandler.Interact -= OnInteract;
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
            Vector2 targetVector = m_manager.inputHandler.moveVector;
            targetVector.y *= m_sprintInputState && targetVector.y > 0 ? m_manager.freeMovementProperties.sprintSpeed : m_manager.freeMovementProperties.moveSpeed;

            isMoving = m_manager.inputHandler.moveVector != Vector2.zero;
            float easeTime = isMoving ? 
                Time.fixedDeltaTime * m_manager.freeMovementProperties.accelerationStrength :
                Time.fixedDeltaTime * m_manager.freeMovementProperties.decelerationStrength;

            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            movementVector.x = Mathf.Lerp(movementVector.x, targetVector.x, easeTime);
            movementVector.y = Mathf.Lerp(movementVector.y, targetVector.y, easeTime);

            cameraForward.y = 0;
            cameraRight.y = 0;

            Vector3 forwardRelative = cameraForward * movementVector.y;
            Vector3 rightRelative = cameraRight * movementVector.x;

            Vector3 moveVector = forwardRelative + rightRelative;
            moveVector.y = -9.81f;
            m_manager.controller.Move(moveVector * Time.fixedDeltaTime);

            AnimateMovement();
        }

        void AnimateMovement()
        {
            float easeTime = isMoving ? Time.fixedDeltaTime * m_manager.freeMovementProperties.easeInAnimationStrength : Time.fixedDeltaTime * m_manager.freeMovementProperties.easeOutAnimationStrength;

            animationVector.x = Mathf.Lerp(animationVector.x, m_manager.inputHandler.moveVector.x, easeTime);
            animationVector.y = Mathf.Lerp(animationVector.y, m_manager.inputHandler.moveVector.y, easeTime);

            m_manager.Animator.SetBool("IsMoving", m_manager.inputHandler.moveVector != Vector2.zero);
            m_manager.Animator.SetFloat("MovementX", animationVector.x);
            m_manager.Animator.SetFloat("MovementY", animationVector.y);
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
            m_manager.Animator.SetBool("Sprint", m_sprintInputState);
        }

        void OnAim(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                m_manager.Animator.SetFloat("MovementX", 0);
                m_manager.Animator.SetFloat("MovementY", 0);
                m_manager.Animator.SetBool("Sprint", false);
                m_manager.Animator.SetBool("Aim", true);
                m_manager.SetState(new AimState(m_manager));
            }
        }

        void OnUseCurrentItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var item = m_manager.inventory.currentItem;
                var result = Item_Data.GetItemData(item.data).useAction(m_manager.gameObject);

                if (result)
                {
                    m_manager.inventory.AddOrRemoveItemFromInventory(item, -1);
                }
            }
        }

        void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var nearest = m_manager.interactionHub.GetNearestInteractable();
                nearest?.Interact(m_manager);
            }
        }

        #endregion
    }
}