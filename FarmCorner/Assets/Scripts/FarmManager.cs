using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public static bool buttonControl;
    private enum FarmType { Chicken = 0, Duck = 1, Sheep = 2}
    [SerializeField] private FarmType farmType; 
    [SerializeField] private Pool pool;

    private List<AnimalManager> animals = new();

    private void OnEnable()
    {
        GameManager.Instance.OnOpenNewFarm.Invoke((int)farmType);
    }

    public void BuyAnimal()
    {
        AnimalManager animal = pool.GetPooledObject();

        animals.Add(animal);

        animal.gameObject.transform.position = transform.position;
        animal.enabled = true;
        animal.gameObject.SetActive(true);
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
