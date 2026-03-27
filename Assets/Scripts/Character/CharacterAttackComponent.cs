using UnityEngine;
using Zenject;

public class CharacterAttackComponent : MonoBehaviour
{
    [SerializeField] private DamageSettings damageSettings;
    [SerializeField] private AttackHitBoxSettings hitBoxSettings;
    
    private IDamageable _ownDamageable;
    private Transform _ownTransform;
    
    private readonly Collider[] _hitColliders = new Collider[16]; 

    [Inject]
    private void Construct(IDamageable ownDamageable)
    {
        _ownDamageable = ownDamageable;
        _ownTransform = transform;
    }

    public void Execute()
    {
        DealDamage();
    }

    private void DealDamage()
    {
        var hitBoxPosition = transform.position + transform.forward * hitBoxSettings.Distance;
        
        var hitCount = Physics.OverlapBoxNonAlloc(
            hitBoxPosition,
            hitBoxSettings.HalfExtents,
            _hitColliders,
            transform.rotation,
            hitBoxSettings.LayerMask,
            QueryTriggerInteraction.Ignore
        );

        for (var i = 0; i < hitCount; i++)
        {
            var col = _hitColliders[i];
            
            if (col.gameObject == gameObject)
                continue;

            if (!col.TryGetComponent<IDamageable>(out var damageable))
            {
               return;
            }
            
            if (damageable == _ownDamageable)
                continue;

            damageable.TakeDamage(damageSettings.Damage, _ownTransform);
            Debug.Log("Damage Detected");
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (!enabled)
            return;

        var hitBoxPosition = transform.position + transform.forward * hitBoxSettings.Distance;
        
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(hitBoxPosition, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, hitBoxSettings.HalfExtents * 2);
    }
}

[System.Serializable]
public struct DamageSettings
{
    [field: SerializeField] public float Damage { get; private set; }
}

[System.Serializable]
public struct AttackHitBoxSettings
{
    [field: SerializeField] public float Distance { get; private set; }

    [field: SerializeField] public Vector3 HalfExtents { get; private set; }

    [field: SerializeField] public LayerMask LayerMask { get; private set; }
}