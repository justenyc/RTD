using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

/*
 * TODO:
 * - Transition animation fix ^_^b
 * - Reset Camera ^_^b
 * - Toggle instead of Hold for sprint
 */

namespace Player
{
    public class SprintState : IPlayerState
    {
        PlayerController m_manager;

        public SprintState(PlayerController manager)
        {
            m_manager = manager;
            StateStart();
        }

        public void StateStart()
        {
            m_manager.currentState = "Sprint";
            m_manager.animationVector.x = 0;
            m_manager.movementVector.x = 0;

            m_manager.Animator.SetBool("Sprint", true);
            m_manager.Animator.SetFloat("MovementX", m_manager.animationVector.x);

            m_manager.inputHandler.Sprint += OnSprint;
            m_manager.inputHandler.Aim += OnAim;
            m_manager.inputHandler.Interact += OnInteract;

            m_manager.cameraFollow.DOLocalRotate(Vector3.zero, m_manager.sprintProperties.cameraResetSpeed);
            m_manager.isMoving = true;
        }

        public void StateEnd()
        {
            m_manager.Animator.SetBool("Sprint", false);
            Vector3 currentPlayerRot = m_manager.playerTransform.rotation.eulerAngles;
            Vector3 newPlayerRot = currentPlayerRot + Vector3.up * Vector3.SignedAngle(m_manager.playerTransform.forward, Camera.main.transform.forward, Vector3.up);
            m_manager.playerTransform.rotation = Quaternion.Euler(newPlayerRot);
            m_manager.cameraFollow.localRotation = Quaternion.identity;

            m_manager.inputHandler.Sprint -= OnSprint;
            m_manager.inputHandler.Aim -= OnAim;
            m_manager.inputHandler.Interact -= OnInteract;
        }

        public void StateFixedUpdate()
        {
            
        }

        public void StateUpdate()
        {
            Move();
            ControlCamera();
        }

        void Move()
        {
            if (!m_manager.canMove || m_manager.useRootMotion)
            {
                return;
            }

            m_manager.isMoving = m_manager.movementVector != Vector2.zero;

            if (!m_manager.isMoving)
            {
                m_manager.SetState(new FreeMovementState(m_manager));
            }

            m_manager.interactionHub.HighlightInteractable(m_manager.interactionHub.GetNearestInteractable());
            
            Vector2 targetVector = m_manager.inputHandler.moveVector.normalized;
            float rotateSpeed = m_manager.sprintProperties.sprintRotateSpeed;

            m_manager.movementVector.y = Mathf.Abs(Mathf.Ceil(targetVector.y));
            //m_manager.movementVector.y = Mathf.Lerp(m_manager.movementVector.y, targetVector.y, Time.deltaTime * m_manager.freeMovementProperties.accelerationStrength);
            //m_manager.movementVector.y = Mathf.Clamp(m_manager.movementVector.y, 0, targetVector.y);

            Vector3 currentPlayerRot = m_manager.playerTransform.rotation.eulerAngles;
            Vector3 newPlayerRot = currentPlayerRot + Vector3.up * targetVector.x * rotateSpeed * Time.deltaTime;
            m_manager.playerTransform.rotation = Quaternion.Euler(newPlayerRot);

            m_manager.controller.Move(m_manager.transform.forward * m_manager.movementVector.y * m_manager.sprintProperties.sprintSpeed * Time.deltaTime);

            AnimateMovement();
        }

        void ControlCamera()
        {
            Vector2 inputVector = m_manager.inputHandler.lookVector.normalized;
            float rotateSpeed = m_manager.sprintProperties.cameraRotateSpeed;

            Vector3 currentCameraFollowRot = m_manager.cameraFollow.rotation.eulerAngles;
            Vector3 newCameraFollowRot = currentCameraFollowRot + Vector3.up * inputVector.x * rotateSpeed * Time.deltaTime;
            m_manager.cameraFollow.rotation = Quaternion.Euler(newCameraFollowRot);
            return;
        }

        void AnimateMovement()
        {
            float easeTime = m_manager.isMoving ? Time.deltaTime * m_manager.freeMovementProperties.easeInAnimationStrength : Time.deltaTime * m_manager.freeMovementProperties.easeOutAnimationStrength * 2;

            float yFloat = m_manager.movementVector.y * m_manager.sprintProperties.sprintSpeed;
            yFloat = Mathf.Clamp(yFloat, 0, 2);
            m_manager.animationVector.y = Mathf.Lerp(m_manager.animationVector.y, yFloat, easeTime);

            m_manager.Animator.SetBool("IsMoving", m_manager.inputHandler.moveVector != Vector2.zero);
            m_manager.Animator.SetFloat("MovementY", m_manager.animationVector.y);
        }

        void OnSprint(InputAction.CallbackContext context)
        {
            //if (context.canceled) m_manager.SetState(new FreeMovementState(m_manager));
        }

        void OnAim(InputAction.CallbackContext context)
        {
            if (context.performed) m_manager.SetState(new AimState(m_manager));
        }

        void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var nearest = m_manager.interactionHub.GetNearestInteractable();
                nearest?.Interact(m_manager);
            }
        }
    }
}