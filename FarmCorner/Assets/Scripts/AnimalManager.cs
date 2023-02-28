using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AnimalManager : MonoBehaviour
{
    
    private NavMeshAgent _agent;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
            InvokeRepeating(nameof(SetRandomDestination), 0f, 0.01f);       
    }

    private void SetRandomDestination()
    {
        _agent.SetDestination(GetRandomPoint());

        if (_agent.hasPath)
        {
            CancelInvoke(nameof(SetRandomDestination));
            InvokeRepeating(nameof(Check), 0f, 0.01f);
        }
    }

    private void Check()
    {
        if (_agent.velocity.magnitude < 0.05f)
        {
            CancelInvoke(nameof(Check));
            InvokeRepeating(nameof(SetRandomDestination), 0f, 0.01f);
        }
    }
    private Vector3 GetRandomPoint()
    {
        var pointX = Random.Range(-1.1f, 1.1f);
        var pointZ = Random.Range(-1.8f, 1.8f);
        return new Vector3(transform.position.x + pointX, 0, transform.position.z +pointZ);

    }
}
