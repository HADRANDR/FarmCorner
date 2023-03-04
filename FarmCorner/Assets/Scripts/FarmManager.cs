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
    [SerializeField] Button MergePhase1;
    [SerializeField] Button MergePhase2;
    [SerializeField] Button CutSheep;

    [Header("Object Values")]
    [SerializeField] int AnimalLevel1;
    [SerializeField] int AnimalLevel2;
    [SerializeField] int AnimalLevel3;
    [SerializeField] int AnimalLevel1HarvestValue1;
    [SerializeField] int AnimalLevel1HarvestValue2;
    [SerializeField] int AnimalLevel1HarvestValue3;
    [SerializeField] int AnimalLevel2HarvestValue1;
    [SerializeField] int AnimalLevel2HarvestValue2;
    [SerializeField] int AnimalLevel2HarvestValue3;
    [SerializeField] int AnimalLevel3HarvestValue1;
    [SerializeField] int AnimalLevel3HarvestValue2;
    [SerializeField] int AnimalLevel3HarvestValue3;
    public static int _money;

    private List<AnimalManager> animals = new();
    private void OnEnable()
    {
        GameManager.Instance.OnOpenNewFarm.Invoke((int)farmType);
        buyButton.onClick.AddListener(BuyAnimal);
        MergePhase1.onClick.AddListener(Merge2Animal);
        MergePhase2.onClick.AddListener(Merge3Animal);
        if ((int)farmType == 2)
        {
            CutSheep.onClick.AddListener(CutSkin);
        }
        else return;
        
    }
    private void OnDisable()
    {
        buyButton.onClick.RemoveListener(BuyAnimal);
        MergePhase1.onClick.RemoveListener(Merge2Animal);
        MergePhase2.onClick.RemoveListener(Merge3Animal);
        if ((int)farmType == 2)
        {
            CutSheep.onClick.RemoveListener(CutSkin);
        }
        else return;
    }
    public void Merge2Animal()
    {
        canBuy = true;
        for (int i = 0; i < animals.Count-1; i++)   
        {
            if (animals[i].gameObject.CompareTag("Merge1") && animals[i+1].gameObject.CompareTag("Merge1"))
            {
                
                UpdateObject(animals[i].gameObject, "Merge2");
                animals[i + 1].CloseAnimal();
                pool.ReturnPool(animals[i + 1]);
                animals.Remove(animals[i + 1]);
                i = animals.Count;
                canBuy = false;
            }            
        }
    }
    public void Merge3Animal()
    {
        canBuy = true;
        //if (upgradePhase<2) return;

        for (int i = 0; i < animals.Count-1; i++)   
        {
            if (animals[i].gameObject.CompareTag("Merge2") && animals[i + 1].gameObject.CompareTag("Merge2"))
            {
                
                UpdateObject(animals[i].gameObject, "Merge3");
                animals[i + 1].CloseAnimal();
                pool.ReturnPool(animals[i + 1]);
                animals.Remove(animals[i + 1]);
                i = animals.Count;
                
            }
        }
        canBuy = false;
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
    public void UpdateObject(GameObject animal, string tag)
    {
        animal.tag = tag;
        if (animal.CompareTag("Merge2"))
        {           
            animal.transform.GetChild(0).gameObject.SetActive(false);
            animal.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (animal.CompareTag("Merge3"))   
        {
            animal.transform.GetChild(1).gameObject.SetActive(false);
            animal.transform.GetChild(2).gameObject.SetActive(true);
        }
    }
    public void CutSkin()
    {
        foreach (var animal in animals)
        {

            if (animal.CompareTag("Merge1"))
            {
                if (animal.transform.GetChild(0).gameObject.CompareTag("Skin1"))
                {
                    _money += AnimalLevel1HarvestValue1;
                }
                else if (animal.transform.GetChild(0).gameObject.CompareTag("Skin2"))
                {
                    _money += AnimalLevel1HarvestValue2;
                }
                else if (animal.transform.GetChild(0).gameObject.CompareTag("Skin3"))
                {
                    _money += AnimalLevel1HarvestValue3;
                }
                animal.transform.GetChild(0).gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                animal.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0f, gameObject.transform.localPosition.z);
                
            }
            if (animal.CompareTag("Merge2"))
            {
                if (animal.transform.GetChild(1).gameObject.CompareTag("Skin1"))
                {
                    _money += AnimalLevel2HarvestValue1;
                }
                else if (animal.transform.GetChild(1).gameObject.CompareTag("Skin2"))
                {
                    _money += AnimalLevel2HarvestValue2;
                }
                else if (animal.transform.GetChild(1).gameObject.CompareTag("Skin3"))
                {
                    _money += AnimalLevel2HarvestValue3;
                }
                animal.transform.GetChild(1).gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                animal.transform.GetChild(1).gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0f, gameObject.transform.localPosition.z);

            }
            if (animal.CompareTag("Merge3"))
            {
                if (animal.transform.GetChild(2).gameObject.CompareTag("Skin1"))
                {
                    _money += AnimalLevel3HarvestValue1;
                }
                else if (animal.transform.GetChild(2).gameObject.CompareTag("Skin2"))
                {
                    _money += AnimalLevel3HarvestValue2;
                }
                else if (animal.transform.GetChild(2).gameObject.CompareTag("Skin3"))
                {
                    _money += AnimalLevel3HarvestValue3;
                }
                animal.transform.GetChild(2).gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                animal.transform.GetChild(2).gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0f, gameObject.transform.localPosition.z);

            }

        }
        Debug.Log(_money);
    } 
    public void OnInteractableButtons()
    {
        buyButton.interactable = true;
        MergePhase1.interactable = true;
        MergePhase2.interactable = true;
    }
}
