using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    public static bool buttonControl;
    private enum FarmType { Chicken = 0, Duck = 1, Sheep = 2, Cow = 4}
    [SerializeField] private FarmType farmType;
    public static bool canBuy;
    public static bool canInteractable = true;
    [SerializeField] private Pool pool;
    [SerializeField] GameObject SpawnPoint;
    public static int _skinMoney;

    [Header("Buttons")]
    [SerializeField] Button buyButton;
    [SerializeField] Button MergePhase;
    [SerializeField] Button CutSheep;

    private List<AnimalManager> animals = new();

   

    private void OnEnable()
    {
        GameManager.Instance.OnOpenNewFarm.Invoke((int)farmType);
        buyButton.onClick.AddListener(BuyAnimal);
        MergePhase.onClick.AddListener(MergeAnimal);
        if ((int)farmType == 2)
        {
            CutSheep.onClick.AddListener(CutSkin);
        }
        else return;
        
    }
    private void OnDisable()
    {
        buyButton.onClick.RemoveListener(BuyAnimal);
        MergePhase.onClick.RemoveListener(MergeAnimal);
        if ((int)farmType == 2)
        {
            CutSheep.onClick.RemoveListener(CutSkin);
        }
        else return;
    }
    public void MergeAnimal()
    {
        canBuy = true;
        for (int i = 0; i < animals.Count-1; i++)   
        {
            animals[i].Level++;
            animals[i + 1].CloseAnimal();
            pool.ReturnPool(animals[i + 1]);
            animals.Remove(animals[i + 1]);
            i = animals.Count;
            canBuy = false;          
        }
    }
    public void BuyAnimal()
    {
        canBuy = true;
        AnimalManager animal = pool.GetPooledObject();
        animal.tag = "Merge1";
        animal.transform.GetChild(0).gameObject.SetActive(true);
        animal.transform.GetChild(1).gameObject.SetActive(false);
        animal.transform.GetChild(2).gameObject.SetActive(false);

        animals.Add(animal);
        animal.gameObject.transform.position = SpawnPoint.transform.position;
        animal.enabled = true;
        animal.gameObject.SetActive(true);       
        canBuy = false;
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
    public void CutSkin()
    {

        GameManager.Instance.HarvestWools.Invoke();

    } 
    public void OnInteractableButtons()
    {
        buyButton.interactable = true;
        MergePhase.interactable = true;
    }
}
