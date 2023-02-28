using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    private enum FarmType { Chicken = 0, Duck = 1, Sheep = 2, Cow = 3 }
    [SerializeField] private FarmType farmType; 
    [SerializeField] private Pool pool;

    private List<AnimalManager> animals = new();



    public void BuyAnimal()
    {
        AnimalManager animal = pool.GetPooledObject();

        animals.Add(animal);

        animal.gameObject.transform.position = transform.position;
        animal.gameObject.SetActive(true);
    }
}
