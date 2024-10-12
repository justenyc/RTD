using Cinemachine;
using System.Collections;
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

            m_manager.Sword.SetActive(true);

            _3rdPersonFollow = CameraManager.instance.VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            //_3rdPersonFollow.ShoulderOffset = m_manager.aimProperties.cameraPos;
            var tween = DOTween.To(() => _3rdPersonFollow.ShoulderOffset, e => _3rdPersonFollow.ShoulderOffset = e, m_manager.aimProperties.cameraPos, 0.1f);
            
            tweens.Add(tween);
        }

        public void StateEnd()
        {
            //Debug.Log("Ending Aim State");
            m_manager.inputHandler.Aim -= OnAim;
            m_manager.inputHandler.UseCurrentItem -= OnUseCurrentItem;
            m_manager.inputHandler.UseWeapon -= OnUseWeapon;

            m_manager.Sword.GetComponent<Collider>().enabled = false;
            m_manager.Sword.SetActive(false);

            foreach(var tween in tweens)
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
            if(context.performed)
            {
                m_manager.SetState(new FreeMovementState(m_manager));
            }
        }

        void OnUseCurrentItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
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
                m_manager.Animator.SetTrigger("Attack");
            }
        }
        #endregion
    }
}