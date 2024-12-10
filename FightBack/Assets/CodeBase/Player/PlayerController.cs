using System;
using System.Collections;
using CodeBase.Camera;
using UnityEngine;

namespace CodeBase.Player
{
    public enum PlayerState
    {
        Idle,
        Move,
        Attack,
        Immobilized
    }

    public class PlayerController : MonoBehaviour
    {
        [Header("Player")] 
        [SerializeField] private float moveSpeed = 3f;
        
        [Header("Dash Settings")] 
        [SerializeField] private AnimationCurve dashSpeedCurve;
        [SerializeField] private float dashMultiplier = 2f; // Множитель рывка
        [SerializeField] private float dashDistance = 5f; // Дистанция рывка
        [SerializeField] private float dashDuration = 0.5f; // Длительность рывка

        [Header("Camera")] 
        [SerializeField] private UnityEngine.Camera mainCamera;
        
        public PlayerState currentState = PlayerState.Move;
        public static Action<PlayerState> OnPlayerStateChanged;

        private CharacterController _characterController;
        private bool isDashing = false;
        
        private void Start()
        {
            mainCamera.GetComponent<CameraFollow>().Follow(this.gameObject.transform);
            _characterController = GetComponent<CharacterController>();

        }

        private void Update()
        {
            HandleMovement();
            ApplyDash();
        }

        private void ApplyDash()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing) // Удерживаем клавишу для рывка
            {
                StartCoroutine(Dash());
            }
        }

        private void HandleMovement()
        {
            if (currentState == PlayerState.Move && !isDashing)
            {
                Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

                float moveDistance = moveSpeed * Time.deltaTime;
                float playerRadius = .7f;
                float playerHeight = 2f;
                bool canMove = !Physics.CapsuleCast(transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius, moveDir, moveDistance);

                if (!canMove)
                {
                    Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
                    canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position,
                        transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

                    if (canMove)
                    {
                        moveDir = moveDirX;
                    }
                    else
                    {
                        Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                        canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position,
                            transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                        if (canMove)
                        {
                            moveDir = moveDirZ;
                        }
                    }
                }

                if (canMove)
                {
                    _characterController.Move(moveDir * moveDistance);
                }

                float rotateSpeed = 10f;
                transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
            }
        }

        
        
        private IEnumerator Dash()
        {
            isDashing = true;

            Vector3 dashDirection = transform.forward; // Направление рывка (вперёд)
            float elapsedTime = 0f;

            while (elapsedTime < dashDuration)
            {
                elapsedTime += Time.deltaTime;

                // Используем анимационную кривую для скорости в данный момент времени
                float normalizedTime = elapsedTime / dashDuration;
                float dashSpeed = dashMultiplier * dashSpeedCurve.Evaluate(normalizedTime);

                // Перемещаем персонажа в направлении рывка
                _characterController.Move(dashDirection * dashSpeed * dashDistance * Time.deltaTime);

                yield return null;
            }

            isDashing = false;
        }

        void OnDisable()
        {
            //GameManager.Instance.PlayerMachine.OnStateEnter -= OnStateEnter;
            //ameManager.Instance.PlayerMachine.OnStateExit -= OnStateExit;
        }

        public void SetPlayerState(PlayerState playerState)
        {
            currentState = playerState;
            OnPlayerStateChanged?.Invoke(currentState);
        }
        
    }
}