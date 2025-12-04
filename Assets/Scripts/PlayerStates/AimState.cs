using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace Player
{
    public class AimState : IPlayerState
    {
        PlayerController m_manager;
        private Cinemachine3rdPersonFollow _3rdPersonFollow;
        Vector2 animationVector;
        Vector2 movementVector;
        bool isMoving;

        List<Tween> tweens = new();

        public AimState(PlayerController manager)
        {
            m_manager = manager;
            StateStart();
        }

        public void StateStart()
        {
            //Debug.Log("Starting Aim State");
            m_manager.currentState = "Aim";
            m_manager.inputHandler.Aim += OnAim;
            m_manager.inputHandler.UseCurrentItem += OnUseCurrentItem;
            m_manager.inputHandler.UseWeapon += OnUseWeapon;
            m_manager.inputHandler.Guard += OnGuard;

            m_manager.aimProperties.canMove = false;
            //m_manager.SetSwordPosition(true);

            _3rdPersonFollow = CameraManager.instance.VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            //_3rdPersonFollow.ShoulderOffset = m_manager.aimProperties.cameraPos;
            var tween = DOTween.To(() => _3rdPersonFollow.ShoulderOffset, e => _3rdPersonFollow.ShoulderOffset = e, m_manager.aimProperties.cameraPos, 0.1f);

            tweens.Add(tween);
            m_manager.StartCoroutine(Helper.DelayActionByFixedTimeFrames(() => m_manager.aimProperties.canMove = true, m_manager.aimProperties.delayMovementByFrames));
        }

        public void StateEnd()
        {
            //Debug.Log("Ending Aim State");
            m_manager.inputHandler.Aim -= OnAim;
            m_manager.inputHandler.UseCurrentItem -= OnUseCurrentItem;
            m_manager.inputHandler.UseWeapon -= OnUseWeapon;
            m_manager.inputHandler.Guard -= OnGuard;

            //m_manager.Sword.GetComponent<Collider>().enabled = false;
            //m_manager.SetSwordPosition(false);


            foreach (var tween in tweens)
            {
                tween.Kill();
            }
            _3rdPersonFollow.ShoulderOffset = m_manager.sharedProperties.defaultCameraPos;
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
            if (!m_manager.aimProperties.canMove)
            {
                return;
            }

            Vector2 targetVector = m_manager.inputHandler.moveVector;

            isMoving = m_manager.inputHandler.moveVector != Vector2.zero;
            float easeTime = isMoving ?
                Time.fixedDeltaTime * m_manager.aimProperties.accelerationStrength :
                Time.fixedDeltaTime * m_manager.aimProperties.decelerationStrength;

            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            movementVector.x = Mathf.Lerp(movementVector.x, targetVector.x, easeTime);
            movementVector.y = Mathf.Lerp(movementVector.y, targetVector.y, easeTime);

            cameraForward.y = 0;
            cameraRight.y = 0;

            Vector3 forwardRelative = cameraForward * movementVector.y;
            Vector3 rightRelative = cameraRight * movementVector.x;

            Vector3 moveVector = forwardRelative + rightRelative;
            m_manager.controller.Move(moveVector * Time.fixedDeltaTime);

            AnimateMovement();
        }

        void AnimateMovement()
        {
            float easeTime = isMoving ? Time.fixedDeltaTime * m_manager.aimProperties.easeInAnimationStrength : Time.fixedDeltaTime * m_manager.aimProperties.easeOutAnimationStrength;
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
            if(context.performed)
            {
                m_manager.SetState(new FreeMovementState(m_manager));
                m_manager.Animator.SetBool("Aim", false);
            }
        }

        void OnUseCurrentItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                m_manager.Animator.SetTrigger("UseItem");
                return;
                var item = m_manager.inventory.currentItem;
                var data = Item_Data.GetItemData(item.data);

                var thrownItem = Object.Instantiate(item.modelPrefab, m_manager.throwPoint.transform.position, Quaternion.identity);

                if(thrownItem.TryGetComponent(out Rigidbody rb))
                {
                    RaycastHit hit;
                    Transform cameraTransform = CameraManager.instance.VirtualCamera.transform;

                    var throwVector = (cameraTransform.forward * 1000 - m_manager.throwPoint.position).normalized;

                    if (Physics.Raycast(cameraTransform.position, cameraTransform.forward * 1000, out hit, 1000))
                    {
                        throwVector = (hit.collider.transform.position - m_manager.throwPoint.position).normalized;
                    }
                    
                    Debug.DrawRay(m_manager.throwPoint.position, throwVector * 1000, Color.magenta, 5f);

                    rb.AddForce((throwVector + Vector3.up * 0.5f) * m_manager.aimProperties.throwStrength, ForceMode.Impulse);
                }
                //Debug.Log($"THROW ITEM: {data.itemName}");
            }
        }

        void OnUseWeapon(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                m_manager.Animator.SetBool("Attack", true);
            }

            if (context.canceled)
            {
                m_manager.Animator.SetBool("Attack", false);
            }
        }

        void OnGuard(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                m_manager.Animator.SetBool("Guard", true);
            }

            if (context.canceled)
            {
                m_manager.Animator.SetBool("Guard", false);
            }
        }
        #endregion
    }
}