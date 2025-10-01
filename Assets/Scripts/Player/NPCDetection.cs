using UnityEngine;

/// <summary>
/// Handles NPC detection within a vision cone using physics checks.
/// Uses optimized sphere overlap with obstacle checks to determine if
/// an NPC is visible and interactable.
/// </summary>
public class NPCDetection : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private LayerMask _targetMask;   // Layer mask for potential NPC targets
    [SerializeField] private LayerMask _obstacleMask; // Layer mask for obstacles blocking vision
    [SerializeField] private NPCStateManager _npc = null; // Currently detected NPC

    [Header("View Settings")]
    private const float VIEW_RADIUS = 3.0f;   // Radius of detection sphere
    private const float VIEW_ANGLE = 60.0f;   // Field of view angle (degrees)

    [Header("Optimization")]
    private const float CHECK_INTERVAL = 0.2f;  // How often to perform detection checks (seconds)
    private float _nextCheckTime;               // Timestamp of next allowed check
    private readonly Collider[] _colliderBuffer = new Collider[10]; // Reusable buffer for OverlapSphere

    /// <summary>
    /// Returns the currently detected NPC GameObject (null if none).
    /// </summary>
    public GameObject NPC => _npc != null ? _npc.gameObject : null;

    private void Update()
    {
        // Run detection only at intervals (not every frame)
        if (Time.time >= _nextCheckTime)
        {
            FindVisibleTargets();
            _nextCheckTime = Time.time + CHECK_INTERVAL;
        }
    }

    /// <summary>
    /// Performs NPC detection using a vision cone and obstacle checks.
    /// Uses OverlapSphereNonAlloc to avoid memory allocations.
    /// </summary>
    private void FindVisibleTargets()
    {
        int targetsFound = Physics.OverlapSphereNonAlloc(
            transform.position,
            VIEW_RADIUS,
            _colliderBuffer,
            _targetMask
        );

        for (int i = 0; i < targetsFound; i++)
        {
            Transform target = _colliderBuffer[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Check if the target is inside the vision cone
            if (Vector3.Angle(transform.forward, directionToTarget) < VIEW_ANGLE)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // Check for obstacles between this object and the target
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstacleMask))
                {
                    NPCStateManager potentialNPC = target.GetComponent<NPCStateManager>();
                    if (potentialNPC != null)
                    {
                        _npc = potentialNPC;
                        DialogueManager.Instance.CurrentNPC = _npc;

                        // NPC canvas should only be visible if no dialogue is active and NPC is interactable
                        bool shouldShowCanvas = !DialogueManager.Instance.DialogueIsPlaying && _npc.CanInteract;
                        _npc.CanShowCanvas = shouldShowCanvas;

                        // Stop after finding one valid NPC
                        if (shouldShowCanvas)
                            break;
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        // Ensure NPC canvas is hidden when detector is disabled
        if (_npc != null)
        {
            _npc.CanShowCanvas = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection radius in Scene view for debugging
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, VIEW_RADIUS);

        Vector3 viewAngleA = DirFromAngle(-VIEW_ANGLE / 2, false);
        Vector3 viewAngleB = DirFromAngle(VIEW_ANGLE / 2, false);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * VIEW_RADIUS);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * VIEW_RADIUS);
    }

    /// <summary>
    /// Converts an angle into a directional vector, relative to this transform.
    /// </summary>
    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(
            Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),
            0,
            Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)
        );
    }
}
