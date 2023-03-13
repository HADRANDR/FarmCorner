using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AnimalManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnLevelChanged = new();
    private NavMeshAgent _agent;
    public static bool start = true;
    [SerializeField] private List<GameObject> levelVisuals = new();
    bool waitIdle;
    private int _level = 1;
    float random;
    [Header("Animation Times")]
    [SerializeField] float IdleTimeMin;
    [SerializeField] float IdleTimeMax;
    private bool harvestControl;

    public FarmManager farmManager { get; set; }
    public int Level 
    {
        get 
        {
            return _level;
        }
        set 
        {
            _level = value;
            SetVisual();
            OnLevelChanged.Invoke();
        } 
    }

    void Awake()
    {
        random = Random.Range(IdleTimeMin, IdleTimeMax);
        _agent = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        InvokeRepeating(nameof(HarvestSpawner), farmManager.HarvestTimer, farmManager.HarvestTimer); // this spawn 

        if (start==true || FarmManager.canBuy==true)
        {
            StartAnimal();
            start = false;
        }
        else OpenAnimal();
  
    }
    void HarvestSpawner()
    {
        farmManager.SpawnCount(this.gameObject.transform.position);
    }
    //private void OnDisable()
    //{
    //    CancelInvoke(nameof(HarvestSpawner));
    //}
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
            StartCoroutine(IdleTiming());
            
        }
    }
    IEnumerator IdleTiming()
    {
        yield return new WaitForSeconds(random);
        InvokeRepeating(nameof(SetRandomDestination), 0f, 0.01f);

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
        StopAllCoroutines();
        CancelInvoke(nameof(Check));
        CancelInvoke(nameof(SetRandomDestination));
        _agent.isStopped = true;
        _agent.enabled = false;
    }
    public GameObject GetCurrentAnimalObject()
    {
        return levelVisuals[Level - 1];
    }
    private void SetVisual()
    {
        foreach (var visual in levelVisuals)
        {
            visual.SetActive(false);
        }
        levelVisuals[Level - 1].SetActive(true);
    }
    private void SetRandomDestinationInvokeRepating()
    {
        _agent.enabled = true;
        _agent.isStopped = false;
        InvokeRepeating(nameof(SetRandomDestination), 0f, 0.01f);
    }
}
