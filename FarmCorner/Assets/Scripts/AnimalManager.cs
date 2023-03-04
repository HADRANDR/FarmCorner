using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AnimalManager : MonoBehaviour
{
    
    private NavMeshAgent _agent;
    public static bool start = true;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        if (start==true || FarmManager.canBuy==true)
        {
            StartAnimal();
            start = false;
        }
        else OpenAnimal();
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
    public void StartAnimal()
    {
        _agent.enabled = true;
        _agent.isStopped = false;
        InvokeRepeating(nameof(SetRandomDestination), 0f, 0.01f);
    }
    private void OpenAnimal()
    {             
        Invoke(nameof(SetRandomDestinationInvokeRepating), InGameManager._staticDuration);
    }
    public void CloseAnimal()
    {
        CancelInvoke(nameof(Check));
        CancelInvoke(nameof(SetRandomDestination));
        _agent.isStopped = true;
        _agent.enabled = false;
    }
    private void SetRandomDestinationInvokeRepating()
    {
        _agent.enabled = true;
        _agent.isStopped = false;
        InvokeRepeating(nameof(SetRandomDestination), 0f, 0.01f);
    }
}
