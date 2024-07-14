using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController m_controller;
    [SerializeField] InputHandler m_inputHandler;

    [Header("References")]
    [SerializeField] Transform m_playerTransform;
    [SerializeField] Transform m_cameraFollow;
    [SerializeField] Transform m_cameraTarget;

    [Header("Properties")]
    [SerializeField] float m_moveSpeed = 1;
    [SerializeField] float m_rotateSpeed = 1;
    [SerializeField] float m_maxCameraVert = 75f;
    [SerializeField] float m_minCameraVert = -75f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        Move();
        ControlCamera();
    }

    private void Move()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        Vector3 inputVector = m_inputHandler.m_moveVector * m_moveSpeed;

        cameraForward.y = 0;
        cameraRight.y = 0;

        Vector3 forwardRelative = cameraForward * inputVector.y;
        Vector3 rightRelative = cameraRight * inputVector.x;

        Vector3 moveVector = forwardRelative + rightRelative;
        m_controller.Move(moveVector * Time.fixedDeltaTime);
    }

    private void ControlCamera()
    {
        Vector2 inputVector = m_inputHandler.m_lookVector;
        Vector3 currentPlayerRot = m_playerTransform.rotation.eulerAngles;
        Vector3 newPlayerRot = currentPlayerRot + Vector3.up * inputVector.x * m_rotateSpeed * Time.fixedDeltaTime;
        m_playerTransform.rotation = Quaternion.Euler(newPlayerRot);

        Vector3 currentCameraFollowRot = m_cameraFollow.rotation.eulerAngles;
        Vector3 newCameraFollowRot = currentCameraFollowRot + Vector3.right * -inputVector.y * m_rotateSpeed * Time.fixedDeltaTime;
        //Debug.Log(newCameraFollowRot);
        newCameraFollowRot.x = (newCameraFollowRot.x > 180) ? newCameraFollowRot.x - 360 : newCameraFollowRot.x;
        newCameraFollowRot.x = Mathf.Clamp(newCameraFollowRot.x, m_minCameraVert, m_maxCameraVert);
        m_cameraFollow.rotation = Quaternion.Euler(newCameraFollowRot);
    }
}
