using UnityEngine;
using UnityEngine.AI;

public class NavMeshRandomPointFinder
{
    private readonly float _minDistance;
    private readonly int _maxAttempts;

    public NavMeshRandomPointFinder(float minDistance = 0.5f, int maxAttempts = 30)
    {
        _minDistance = minDistance;
        _maxAttempts = maxAttempts;
    }

    public bool TryFindPoint(Vector3 origin, float searchRadius, out Vector3 result)
    {
        result = Vector3.zero;
        
        for (var attempt = 0; attempt < _maxAttempts; attempt++)
        {
            var randomOffset = Random.insideUnitSphere.normalized * searchRadius;
            var candidatePoint = origin + randomOffset;
            
            if (NavMesh.SamplePosition(candidatePoint, out var hit, searchRadius, NavMesh.AllAreas))
            {
                if ((hit.position - origin).magnitude >= _minDistance)
                {
                    result = hit.position;
                    return true;  
                }
            }
        }
        
        return false;  
    }
}