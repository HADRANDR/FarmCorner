using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public static bool buttonControl;
    private enum FarmType { Chicken = 0, Duck = 1, Sheep = 2}
    [SerializeField] private FarmType farmType; 
    [SerializeField] private Pool pool;
    [SerializeField] GameObject SpawnPoint;

    private List<AnimalManager> animals = new();
    private bool canCreated = false;
    private int upgradePhase = 0;
    private int _buyCountDown = 0;
    private void OnEnable()
    {
        GameManager.Instance.OnOpenNewFarm.Invoke((int)farmType);
    }
    public void Merge2Animal()
    {
        if (!canCreated) return;

        for (int i = 0; i < animals.Count-1; i++)   
        {
            if (animals[i].gameObject.CompareTag("Merge1") && animals[i+1].gameObject.CompareTag("Merge1"))
            {
                animals[i].gameObject.transform.GetChild(0).gameObject.SetActive(false);
                animals[i].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                animals[i].gameObject.tag = "Merge2";
                animals[i + 1].CloseAnimal();
                pool.ReturnPool(animals[i + 1]);
                animals.Remove(animals[i + 1]);
                i = animals.Count;
                upgradePhase++;
            }            
        }
    }
    public void Merge3Animal()
    {
        if (upgradePhase<2) return;

        for (int i = 0; i < animals.Count-1; i++)   
        {
            if (animals[i].gameObject.CompareTag("Merge2") && animals[i + 1].gameObject.CompareTag("Merge2"))
            {
                animals[i].gameObject.transform.GetChild(1).gameObject.SetActive(false);
                animals[i].gameObject.transform.GetChild(2).gameObject.SetActive(true);
                animals[i].gameObject.tag = "Merge3";
                animals[i + 1].CloseAnimal();
                pool.ReturnPool(animals[i + 1]);
                animals.Remove(animals[i + 1]);
                i = animals.Count;
            }
        }
    }
    public void BuyAnimal()
    {
        if (_buyCountDown > Pool._poolLength-1) return;

        _buyCountDown++;
        AnimalManager animal = pool.GetPooledObject();
        animals.Add(animal);
        animal.gameObject.transform.position = SpawnPoint.transform.position;
        animal.enabled = true;
        animal.gameObject.SetActive(true);       
        if (animals.Count==2)
        {
            canCreated = true;
        }

    }
    public void CloseFarm()
    {
        gameObject.SetActive(false);
    }
    public void StopAnimal()
    {
        foreach (var animal in animals)
        {
            animal.CloseAnimal();
        }
    }
}
