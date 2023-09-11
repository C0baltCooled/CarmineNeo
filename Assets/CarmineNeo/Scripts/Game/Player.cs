using UnityEngine;

namespace Harpnet.Carmine {
    [RequireComponent(typeof(CharacterController), typeof(InputManager), typeof(AudioSource))]
    public class Player : MonoBehaviour {
        [Header("References")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private AudioSource audioSource;

        [Header("General")]
        [SerializeField] private float gravity = 50.0f;
        [SerializeField] private LayerMask groundedLayerMask = -1;
        [SerializeField] private float distanceFromController = 0.05f;

        [Header("Movement")]
        [SerializeField] private float maxGroundedSpeed = 10.0f;
        [SerializeField] private float groundedAcceleration = 15.0f;

        [SerializeField] private float maxAirSpeed = 6.0f;
        [SerializeField] private float airAcceleration = 25.0f;

        [SerializeField] private float killHeight = -50.0f;

        [Header("Rotation")]
        [SerializeField] private float cameraRotationSpeed = 200.0f;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 15.5f;

        [Header("Audio")]
        [SerializeField] private AudioClip jumpSfx = null;
        [SerializeField] private AudioClip landSfx = null;
        [SerializeField] private AudioClip playerDamageSfx = null;

        public Vector3 characterVelocity { get; set; }
        public bool isGrounded { get; private set; }
        public bool hasJumpedThisFrame { get; private set; }
        public bool isDead { get; private set; }

        private Health m_Health;
        private InputManager m_InputManager;
        private CharacterController m_CharacterController;
        private PlayerWeaponManager m_WeaponManager;
        private Actor m_Actor;
        private Vector3 m_GroundNormal;
        private Vector3 m_CharacterVelocity;
        private Vector3 m_LatestImpactSpeed;
        private float m_LastTimeJumped = 0.0f;
        private float m_CameraVerticalAngle = 0.0f;
        private float m_CameraHorizontalAngle = 0.0f;
        private bool m_JumpQueue = false;
        private bool wasGrounded = false;

        private const float k_JumpGroundingPreventionTime = 0.2f;
        private const float k_GroundCheckDistanceInAir = 0.07f;

        private void Awake() {
            ActorsManager actorManager = FindObjectOfType<ActorsManager>();
            if(actorManager != null)
                actorManager.SetPlayer(gameObject);
        }

        private void Start() {
            m_CharacterController = GetComponent<CharacterController>();
            m_CharacterController.enableOverlapRecovery = true;

            m_InputManager = GetComponent<InputManager>();
            m_WeaponManager = GetComponent<PlayerWeaponManager>();
            m_Health = GetComponent<Health>();
            m_Actor = GetComponent<Actor>();

            m_Health.OnDie += OnDie;
        }

        private bool m_IsBelowKillHeight => transform.position.y < killHeight;
        private bool m_HasLanded => !isGrounded && wasGrounded;

        private void Update() {
            if(!isDead && m_IsBelowKillHeight)
                m_Health.Kill();

            hasJumpedThisFrame = false;

            wasGrounded = isGrounded;
            GroundCheck();

            if(m_HasLanded) {
                audioSource.PlayOneShot(landSfx);
            }

            UpdateCharacterMovement();
        }

        private void OnDie() {
            isDead = true;
        }

        private void GroundCheck() {
            float checkGroundDistance = isGrounded ? (m_CharacterController.skinWidth + distanceFromController) : k_GroundCheckDistanceInAir;

            isGrounded = false;
            m_GroundNormal = Vector3.up;

            if(Time.time >= m_LastTimeJumped + k_JumpGroundingPreventionTime) {
                if(Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(m_CharacterController.height), m_CharacterController.radius, Vector3.down, out RaycastHit hit, checkGroundDistance, groundedLayerMask, QueryTriggerInteraction.Ignore)) {
                    m_GroundNormal = hit.normal;

                    if(Vector3.Dot(hit.normal, transform.up) > 0.0f && IsNormalUnderSlopeLimit(m_GroundNormal)) {
                        isGrounded = true;

                        if(hit.distance > m_CharacterController.skinWidth)
                            m_CharacterController.Move(Vector3.down * hit.distance);
                    }
                }
            }
        }

        private void UpdateCharacterMovement() {
            transform.Rotate(new Vector3(0.0f, (m_InputManager.GetLookInputHorizontal() * cameraRotationSpeed), 0.0f), Space.Self);

            float targetRotation = m_InputManager.GetMoveInput().x;
            m_CameraHorizontalAngle = Mathf.Lerp(m_CameraHorizontalAngle, targetRotation, groundedAcceleration * Time.deltaTime);

            m_CameraVerticalAngle += m_InputManager.GetLookInputVertical() * cameraRotationSpeed;

            m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89.0f, 89.0f);

            playerCamera.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0.0f, -m_CameraHorizontalAngle);

            Vector3 worldSpaceMoveInput = transform.TransformVector(m_InputManager.GetMoveInput());

            if(isGrounded) {
                Vector3 targetVelocity = worldSpaceMoveInput * maxGroundedSpeed;

                characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, groundedAcceleration * Time.deltaTime);

                if(isGrounded && m_InputManager.GetJumpInputDown() || m_JumpQueue) {
                    characterVelocity = new Vector3(characterVelocity.x, 0.0f, characterVelocity.z);

                    characterVelocity += Vector3.up * jumpForce;

                    audioSource.PlayOneShot(jumpSfx);

                    m_LastTimeJumped = Time.time;
                    hasJumpedThisFrame = true;

                    isGrounded = false;
                    m_GroundNormal = Vector3.up;
                    m_JumpQueue = false;
                }
            } else {
                characterVelocity += worldSpaceMoveInput * airAcceleration * Time.deltaTime;

                float verticalVelocity = characterVelocity.y;
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(characterVelocity, Vector3.up);
                horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxAirSpeed);
                characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

                if(!m_JumpQueue && m_InputManager.GetJumpInputDown()) {
                    m_JumpQueue = true;
                }

                characterVelocity += Vector3.down * gravity * Time.deltaTime;
            }

            Vector3 capsuleBottomBeforeMove = GetCapsuleBottomHemisphere();
            Vector3 capsuleTopBeforeMove = GetCapsuleTopHemisphere(m_CharacterController.height);
            m_CharacterController.Move(characterVelocity * Time.deltaTime);

            m_LatestImpactSpeed = Vector3.zero;
            if(Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, m_CharacterController.radius, characterVelocity.normalized, out RaycastHit hit, characterVelocity.magnitude * Time.deltaTime, -1, QueryTriggerInteraction.Ignore)) {
                m_LatestImpactSpeed = characterVelocity;

                characterVelocity = Vector3.ProjectOnPlane(characterVelocity, hit.normal);
            }
        }

        private bool IsNormalUnderSlopeLimit(Vector3 normal) {
            return Vector3.Angle(transform.up, normal) <= m_CharacterController.slopeLimit;
        }

        private Vector3 GetCapsuleBottomHemisphere() {
            return transform.position + (transform.up * m_CharacterController.radius);
        }

        private Vector3 GetCapsuleTopHemisphere(float atHeight) {
            return transform.position + (transform.up * (atHeight - m_CharacterController.radius));
        }
    }
}