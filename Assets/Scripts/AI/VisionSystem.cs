using UnityEngine;
using Zenject;

public class VisionSystem : MonoBehaviour, IEnableable
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float visionRadius = 10f;
    [SerializeField] private LayerMask detectionLayerMask = -1;
    
    private IDamageable _ownHp;
    private readonly Collider[] _overlapResults = new Collider[64];
    private Transform _transform;
    
    public bool Enabled { get; set; } = true;

    [Inject]
    private void Construct(IDamageable ownHp)
    {
        _ownHp = ownHp;
        _transform = transform;
    }

    public IDamageable GetClosestLiveCharacter()
    {
        if (!Enabled)
        {
            return null;
        }

        var count = Physics.OverlapSphereNonAlloc(
            _transform.position, 
            visionRadius, 
            _overlapResults, 
            detectionLayerMask
        );

        if (count == 0)
        {
            return null;
        }

        IDamageable closest = null;
        var closestDistanceSqr = float.MaxValue;
        var myPosition = _transform.position;

        for (var i = 0; i < count; i++)
        {
            var col = _overlapResults[i];

            if (col == null || col.gameObject == gameObject)
            {
                continue;
            }

            var damageable = GetDamageableFromCollider(col);

            if (damageable == null || damageable.IsDead || damageable == _ownHp)
                continue;
            
            var direction = col.transform.position - myPosition;
            var distanceSqr = direction.sqrMagnitude;
            
            if (distanceSqr >= closestDistanceSqr)
            {
                continue;
            }
            closestDistanceSqr = distanceSqr;
            closest = damageable;
        }
        
        return closest;
    }

    private static IDamageable GetDamageableFromCollider(Collider col)
    {
        if (col.TryGetComponent<IDamageable>(out var damageable))
            return damageable;

        return col.GetComponentInParent<IDamageable>();
    }

    public void Enable()
    {
        Enabled = true;
        if (meshRenderer != null)
            meshRenderer.enabled = true;
    }

    public void Disable()
    {
        Enabled = false;
        if (meshRenderer != null)
            meshRenderer.enabled = false;
    }
}