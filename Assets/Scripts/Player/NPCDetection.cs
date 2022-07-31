using UnityEngine;

public class NPCDetection : MonoBehaviour
{
    FieldOfView _fov;

    [SerializeField] LayerMask _targetMask;
    [SerializeField] LayerMask _obstacleMask;

    [SerializeField] NPCStateManager _npc = null;

    const float c_viewRadius = 3.0f;
    const float c_viewAngle = 60.0f;
    
    public GameObject NPC { get => _npc.gameObject; }

    private void Start()
    {
        _fov = GetComponent<FieldOfView>();

        _fov.viewRadius = c_viewRadius;
        _fov.viewAngle = c_viewAngle;
    }
    void Update()
    {
        FindVisibleTargets();
    }
    public void FindVisibleTargets()
    {
        _npc = null;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, c_viewRadius, _targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < c_viewAngle)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstacleMask))
                {
                    Debug.DrawRay(transform.position, directionToTarget * distanceToTarget, Color.yellow);
                    if (distanceToTarget < c_viewRadius)
                    {
                        _npc = target.gameObject.GetComponent<NPCStateManager>();
                        DialogueManager.Instance.CurrentNPC = _npc;
                        if(!DialogueManager.Instance.DialogueIsPlaying && _npc.CanInteract) {
                            _npc.CanShowCanvas = true;
                        }
                        else
                        {
                            _npc.CanShowCanvas = false;
                        }
                    }
                }
            }
        }
    }
}
