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

        public FreeMovementState(PlayerController manager)
        {
            m_manager = manager;
            StateStart();
        }

        public void StateStart()
        {
            //Logger.LogMessage("Starting FreeMovement State");
            m_manager.currentState = "FreeMovement";

            m_manager.inputHandler.Sprint += OnSprint;
            m_manager.inputHandler.Aim += OnAim;
            m_manager.inputHandler.UseCurrentItem += OnUseCurrentItem;
            m_manager.inputHandler.Interact += OnInteract;
        }

        public void StateEnd()
        {
            //Logger.LogMessage("Ending FreeMovement State");
            m_manager.inputHandler.Sprint -= OnSprint;
            m_manager.inputHandler.Aim -= OnAim;
            m_manager.inputHandler.UseCurrentItem -= OnUseCurrentItem;
            m_manager.inputHandler.Interact -= OnInteract;
        }

        public void StateUpdate()
        {
            Move();
            ControlCamera();
        }

        public void StateFixedUpdate()
        {

        }

        private void Move()
        {
            if (!m_manager.canMove || m_manager.useRootMotion)
            {
                return;
            }

            m_manager.interactionHub.HighlightInteractable(m_manager.interactionHub.GetNearestInteractable());

            Vector2 targetVector = m_manager.inputHandler.moveVector.normalized * m_manager.freeMovementProperties.moveSpeed;

            m_manager.isMoving = m_manager.inputHandler.moveVector != Vector2.zero;
            float easeTime = m_manager.isMoving ? 
                Time.deltaTime * m_manager.freeMovementProperties.accelerationStrength :
                Time.deltaTime * m_manager.freeMovementProperties.decelerationStrength;

            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            m_manager.movementVector.x = Mathf.Lerp(m_manager.movementVector.x, targetVector.x, easeTime);
            m_manager.movementVector.y = Mathf.Lerp(m_manager.movementVector.y, targetVector.y, easeTime);

            cameraForward.y = 0;
            cameraRight.y = 0;

            Vector3 forwardRelative = cameraForward * m_manager.movementVector.y;
            Vector3 rightRelative = cameraRight * m_manager.movementVector.x;

            Vector3 leonVector = new Vector3(m_manager.inputHandler.lookVector.x, 0);
            bool leonShuffle = leonVector.magnitude > m_manager.freeMovementProperties.leonShuffleThreshold;
            Vector3 moveVector = leonShuffle ?
                Vector3.zero :
                forwardRelative + rightRelative;
            m_manager.controller.Move(moveVector * Time.deltaTime);

            AnimateMovement(leonShuffle);
        }

        void AnimateMovement(bool leonShuffle)
        {
            float easeTime = m_manager.isMoving ? Time.deltaTime * m_manager.freeMovementProperties.easeInAnimationStrength : Time.deltaTime * m_manager.freeMovementProperties.easeOutAnimationStrength;
            
            m_manager.animationVector.x = leonShuffle ? 
                Mathf.Lerp(m_manager.animationVector.x, 0, easeTime) : 
                Mathf.Lerp(m_manager.animationVector.x, m_manager.movementVector.x, easeTime);
            
            m_manager.animationVector.y = leonShuffle ? 
                Mathf.Lerp(m_manager.animationVector.y, 0, easeTime) : 
                Mathf.Lerp(m_manager.animationVector.y, m_manager.movementVector.y, easeTime);

            m_manager.Animator.SetBool("IsMoving", m_manager.inputHandler.moveVector != Vector2.zero);
            m_manager.Animator.SetFloat("MovementX", m_manager.animationVector.x);
            m_manager.Animator.SetFloat("MovementY", m_manager.animationVector.y);
        }

        private void ControlCamera()
        {
            Vector2 inputVector = m_manager.inputHandler.lookVector.normalized;
            float rotateSpeed = m_manager.freeMovementProperties.cameraRotateSpeed;

            Vector3 currentPlayerRot = m_manager.playerTransform.rotation.eulerAngles;
            Vector3 newPlayerRot = currentPlayerRot + Vector3.up * inputVector.x * rotateSpeed * Time.deltaTime;
            m_manager.playerTransform.rotation = Quaternion.Euler(newPlayerRot);

            //Logger.LogMessage(newCameraFollowRot);
            Vector3 currentCameraFollowRot = m_manager.cameraFollow.rotation.eulerAngles;
            Vector3 newCameraFollowRot = currentCameraFollowRot + Vector3.right * -inputVector.y * rotateSpeed * Time.deltaTime;
            newCameraFollowRot.x = (newCameraFollowRot.x > 180) ? newCameraFollowRot.x - 360 : newCameraFollowRot.x;
            newCameraFollowRot.x = Mathf.Clamp(newCameraFollowRot.x, m_manager.freeMovementProperties.minCameraVert, m_manager.freeMovementProperties.maxCameraVert);
            m_manager.cameraFollow.rotation = Quaternion.Euler(newCameraFollowRot);
        }

        #region State Input Listeners

        void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed) m_manager.SetState(new SprintState(m_manager));
        }

        void OnAim(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                m_manager.Animator.SetFloat("MovementX", 0);
                m_manager.Animator.SetFloat("MovementY", 0);
                m_manager.SetState(new AimState(m_manager));
            }
        }

        void OnUseCurrentItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var inventory = m_manager.inventory;
                var currentItem = inventory.GetCurrentItem();
                m_manager.Animator.SetTrigger("UseItem");
                m_manager.Animator.SetInteger("UseItemAnimID", 0);
                inventory.UseCurrentItem(m_manager.gameObject);
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