using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class FarmManager : MonoBehaviour
{
    public static bool buttonControl;
    private enum FarmType { Chicken = 0, Duck = 1, Sheep = 2, Cow = 3}
    [SerializeField] private FarmType farmType;
    public int FarmTypeInt
    {
        get
        {
            return (int)farmType;
        }
        private set
        {

        }
    }

    //[SerializeField] private List<SOFarmLevels> levels = new();
    public static bool canBuy;
    // public static bool canInteractable = true;
    [SerializeField] private AnimalPool animalPool;
    [SerializeField] private HarvestPool harvestPool;
    [SerializeField] GameObject AnimalSpawnPoint;

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI capacityText;
    [SerializeField] TextMeshProUGUI levelText;

    [Header("Buttons")]
    [SerializeField] Button buyButton;
    [SerializeField] Button MergeButton;
    [SerializeField] Button harvesButton;
    [SerializeField] Button upgradeValueButton;
    [SerializeField] Button upgradeRateButton;
    [SerializeField] Button upgradeFarmCapacityButton;
    [SerializeField] Button upgradeFarmButton;
    
    [Header("Harvest Timer")]
    [SerializeField] int harvestTimer;

    [Header("Values")]
    
    [SerializeField] List<int> farmUpgradeValuePrice; // ÇİFTLİK SEVIYE ATLARKEN ÖDENECEK TUTAR.  price
    [SerializeField] List<int> levelOfFarmCapacity; // ÇİFTLİK HER SEVİYE ATLADIĞINDA NE KADAR KAPASİTEYE SAHİP OLACAK. value 
    [SerializeField] List<int> upgradeFarmCapacityValuePrice; // ÇİFTLİK KAPASİTESİ GÜÇLENDİRME KARTI FİYATI. price 
    [SerializeField] List<int> upgradeFarmCapacityPERCENT; // ÇİFTLİK KAPASİTESİ GÜÇLENDİRME KARTI SONRASI % KAÇ ARTACAK. value

    [SerializeField] int CurrentAnimalCapacity; // BAŞLANGIÇ KAPASİTESİ
    [SerializeField] List<int> levelOfAnimalValuePrice; // HAYVAN HER SEVİYE BAŞINDA NE KADAR OLACAK. 
    [SerializeField] List<int> onLevelAnimalValuePERCENT; // HAYVAN SEVIYE İÇERİSİNDE HER SATIN ALMADA % KAÇ PAHALANACAK. 
    

    [SerializeField] List<int> levelOfProductValuePrice; // ÜRÜN HER SEVİYE BAŞINDA NE KADAR OLACAK.
    [SerializeField] List<int> upgradeProductValuePrice; // ÜRÜN DEĞERİ GÜÇLENDİRME KARTI FİYATI.
    [SerializeField] List<int> upgradeProductValuePERCENT; // ÜRÜN DEĞERİ HER GÜÇLENDİRME SONRASI % KAÇ ARTACAK.
    [SerializeField] List<int> upgradeGrowRateValuePrice; // ÜRÜN ÜRETİM HIZI GÜÇLENDİRME KARTI FİYATI.
    [SerializeField] List<int> upgradeGrowRateValuePERCENT; // ÜRÜN ÜRETİM HIZI HER GÜÇLENDİRME SONRASI % KAÇ ARTACAK.

    
    
    public int HarvestTimer
    {
        get
        {
            return harvestTimer;
        }
        set
        {
            harvestTimer = HarvestTimer;
        }
    }

    private List<AnimalManager> animalsList = new();
    private List<GameObject> harvestList = new();



    //void UpgradeBarn()
    //{
    //     maxAnimal = levels[0].maxAnimal;
    //}
    private void OnEnable()
    {
        GameManager.Instance.OnOpenNewFarm.Invoke((int)farmType);
        buyButton.onClick.AddListener(BuyAnimal);
        MergeButton.onClick.AddListener(MergeAnimal);
        if ((int)farmType == 2)
        {
            harvesButton.onClick.AddListener(CutSkin);
        }
        else
        {
            harvesButton.onClick.AddListener(HarvestObject);
        }
        
    }
    private void OnDisable()
    {
        buyButton.onClick.RemoveListener(BuyAnimal);
        MergeButton.onClick.RemoveListener(MergeAnimal);
        if ((int)farmType == 2)
        {
            harvesButton.onClick.RemoveListener(CutSkin);
        }
        else
        {
            harvesButton.onClick.RemoveListener(HarvestObject);
        }
    }
    public void MergeAnimal()
    {
        canBuy = true;
        for (int i = 0; i < animalsList.Count-1; i++)   
        {
            if (animalsList[i].Level == 1 && animalsList[i + 1].Level == 1)
            {
                animalsList[i].Level++;
                animalsList[i + 1].CloseAnimal();
                animalPool.ReturnAnimalPool(animalsList[i + 1]);
                animalsList.Remove(animalsList[i + 1]);
                i = animalsList.Count;             
            }

            canBuy = false;
        }
    }
    public void HarvestObject()
    {
        for (int i = 0; i < harvestList.Count; i++)
        {
            harvestList[i].SetActive(false);
            harvestPool.ReturnHarvestPool(harvestList[i]);
            if (i==harvestList.Count-1)
            {
                for (int a = 0; a < harvestList.Count;)
                {
                    harvestList.Remove(harvestList[a]);
                }
            }
        }      
    }
    public void BuyAnimal()
    {
        canBuy = true;
        AnimalManager animal = animalPool.GetPooledAnimalObject(); // POOL CEKME NOKTASI
        animal.farmManager = this;
        animal.Level = 1;
        animalsList.Add(animal);
        animal.gameObject.transform.position = AnimalSpawnPoint.transform.position;
        animal.enabled = true;
        animal.gameObject.SetActive(true);
        canBuy = false;
    }
    public void SpawnCount(Vector3 pos)
    {
        GameObject harvest = harvestPool.GetPooledHarvestObject();
        foreach (var animal in animalsList)
        {
            switch (animal.Level)
            {
                case 1:
                    harvest.tag = "level1";
                    break;
                case 2:
                    harvest.tag = "level2";
                    break;
                case 3:
                    harvest.tag = "level3";
                    break;
            }
        }
        harvestList.Add(harvest);
        harvest.transform.position = pos;
        harvest.SetActive(true);
        
    }
    //public void SpawnHarvestObject(Vector3 pos)
    //{
    //    GameObject harvest = harvestPool.GetPooledHarvestObject();
    //    harvestList.Add(harvest);
    //    harvest.transform.position = pos;
    //    harvest.SetActive(true);
    //}
    public void CloseFarm()
    {
        gameObject.SetActive(false);
    }
    public void StopAnimal()
    {
        foreach (var animal in animalsList)
        {
            animal.CloseAnimal();
        }
    }
    public void CutSkin()
    {
        GameManager.Instance.HarvestWools.Invoke();
    } 
    public void LevelUpFarm()
    {

    }
    
}
