using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSkinManager : MonoBehaviour
{
    [SerializeField] private AnimalManager sheepManager;

    public bool GrowComplete { get; set; } = false;

    float horizontalF = 0.004f; // 2 second.
    float verticalF;
    Vector3 maxScale = Vector3.one * 1.4f;
    private GameObject _currentChild;
    private void Awake()
    {
        verticalF = horizontalF / 2;
    }

    private void OnEnable()
    {
        SetVisualObject();

        sheepManager.OnLevelChanged.AddListener(() => 
        {
            SetVisualObject();
            _currentChild.transform.localScale = Vector3.one;
            _currentChild.transform.localPosition = new Vector3(_currentChild.transform.localPosition.x, 0f, _currentChild.transform.localPosition.z);
            GrowComplete = false;
        });

        GameManager.Instance.HarvestWools.AddListener(HarvestWool);
    }
    private void OnDisable()
    {
        GameManager.Instance.HarvestWools.RemoveListener(HarvestWool);
    }

    void FixedUpdate()  
    {
        if (GrowComplete) return;
        
        GrowSheepSkin();
    
    }
   
    private void SetVisualObject()
    {
        _currentChild = sheepManager.GetCurrentAnimalObject();
    }

    private void GrowSheepSkin()
    {
        if (_currentChild.transform.localScale.magnitude < maxScale.magnitude && _currentChild.transform.localPosition.y < 0.2f)
        {
            _currentChild.transform.localScale = new Vector3(_currentChild.transform.localScale.x + horizontalF, _currentChild.transform.localScale.y + horizontalF, _currentChild.transform.localScale.z + horizontalF);
            _currentChild.transform.localPosition = new Vector3(_currentChild.transform.localPosition.x, _currentChild.transform.localPosition.y + verticalF, _currentChild.transform.localPosition.z);
            if (_currentChild.transform.localScale.magnitude > maxScale.magnitude)
            {
                GrowComplete = true;
            }
        }
    }
   
    private void HarvestWool()
    {
        _currentChild.transform.localScale = Vector3.one;
        _currentChild.transform.localPosition = new Vector3(_currentChild.transform.localPosition.x, 0f, _currentChild.transform.localPosition.z);
    }
}
