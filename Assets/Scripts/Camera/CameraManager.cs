using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    private CinemachineCamera cinemachineCamera;
    private CinemachinePositionComposer composer;
    private Player player;
    private Rigidbody2D rb;

    [Header("Fall Detection")]
    [SerializeField] private float fallGravityScale = 5f;
    [SerializeField] private float fallTransitionSpeed = 3f;
    [SerializeField] private float recoverTransitionSpeed = 2f;

    [Header("Base Settings")]
    [SerializeField] private float normalYDamping = 1f;
    [SerializeField] private float normalScreenY = 0.15f;

    [Header("Fall Settings")]
    [SerializeField] private float fallYDamping = 0.1f;
    [SerializeField] private float fallScreenY = -0.3f;


    private void Start()
    {
        rb = playerTransform.GetComponent<Rigidbody2D>();
        player = playerTransform.GetComponent<Player>();
        cinemachineCamera = GetComponent<CinemachineCamera>();
        composer = cinemachineCamera.GetComponent<CinemachinePositionComposer>();
    }

    private void Update()
    {
        if (rb == null || player == null || composer == null)
            return;

        bool shouldFall = rb.gravityScale > fallGravityScale && !player.wallDetected;
        float transitionSpeed = shouldFall ? fallTransitionSpeed : recoverTransitionSpeed;

        float targetDamping = shouldFall ? fallYDamping : normalYDamping;
        float targetScreenY = shouldFall ? fallScreenY : normalScreenY;

        // Smoothly lerp both values
        composer.Damping.y = Mathf.Lerp(composer.Damping.y, targetDamping, Time.deltaTime * transitionSpeed);
        var composition = composer.Composition;
        composition.ScreenPosition.y = Mathf.Lerp(composition.ScreenPosition.y, targetScreenY, Time.deltaTime * transitionSpeed);
        composer.Composition = composition;
    }
}