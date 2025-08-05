using UnityEngine;

public class NPCDetection : MonoBehaviour
{
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private NPCStateManager _npc = null;

    private const float c_viewRadius = 3.0f;
    private const float c_viewAngle = 60.0f;
    private Collider[] _colliderBuffer = new Collider[10]; // Buffer reutilizable
    private float _nextCheckTime;
    private const float CHECK_INTERVAL = 0.2f;

    public GameObject NPC { get => _npc != null ? _npc.gameObject : null; }

    private void Update()
    {
        // Chequeo por intervalos en lugar de cada frame
        if (Time.time >= _nextCheckTime)
        {
            FindVisibleTargetsOptimized();
            _nextCheckTime = Time.time + CHECK_INTERVAL;
        }
    }

    private void FindVisibleTargetsOptimized()
    {
        // Usar OverlapSphereNonAlloc para evitar allocations de memoria
        int targetsFound = Physics.OverlapSphereNonAlloc(
            transform.position,
            c_viewRadius,
            _colliderBuffer,
            _targetMask
        );

        for (int i = 0; i < targetsFound; i++)
        {
            Transform target = _colliderBuffer[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Comprobar si está dentro del ángulo de visión
            if (Vector3.Angle(transform.forward, directionToTarget) < c_viewAngle)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // Comprobar si hay obstáculos en medio
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstacleMask))
                {
                    NPCStateManager potentialNPC = target.GetComponent<NPCStateManager>();
                    if (potentialNPC != null)
                    {
                        _npc = potentialNPC;
                        DialogueManager.Instance.CurrentNPC = _npc;

                        // Mostrar canvas solo si se puede interactuar y no hay diálogo activo
                        bool shouldShowCanvas = !DialogueManager.Instance.DialogueIsPlaying && _npc.CanInteract;
                        _npc.CanShowCanvas = shouldShowCanvas;

                        // Solo un NPC a la vez
                        if (shouldShowCanvas)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        // Limpieza cuando el componente se desactiva
        if (_npc != null)
        {
            _npc.CanShowCanvas = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualización del área de detección en el editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, c_viewRadius);

        Vector3 viewAngleA = DirFromAngle(-c_viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(c_viewAngle / 2, false);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * c_viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * c_viewRadius);
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}