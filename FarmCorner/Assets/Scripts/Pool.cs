using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private enum Pools { ChickenPool = 0, DuckPool = 1, SheepPool = 2 }
    [SerializeField] private Pools pools;

    private Queue<AnimalManager> poolObject = new();

    [SerializeField] private int poolSize = 25;
    [SerializeField] private AnimalManager poolGameObject;

    private void Awake()
    {
        FillPool();
    }

    private void Start()
    {
        switch ((int)pools)
        {
            case 0:
                GameManager.Instance.ReturnChickenPool.AddListener(ReturnPool);
                break;
            case 1:
                GameManager.Instance.ReturnDuckPool.AddListener(ReturnPool);
                break;
            case 2:
                GameManager.Instance.ReturnSheepPool.AddListener(ReturnPool);
                break;


        }

    }

    public AnimalManager GetPooledObject()
    {
        AnimalManager animal = poolObject.Dequeue();
        return animal;
    }

    private void FillPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AnimalManager animal = Instantiate(poolGameObject, Vector3.zero, Quaternion.identity);
            animal.gameObject.SetActive(false);
            animal.transform.parent = gameObject.transform;
            poolObject.Enqueue(animal);
        }
    }

    private void ReturnPool(AnimalManager obj)
    {
        obj.enabled = false;
        obj.gameObject.SetActive(false);
        poolObject.Enqueue(obj);
    }
}
