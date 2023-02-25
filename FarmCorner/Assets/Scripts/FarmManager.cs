using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    private enum FarmType { Chicken = 0, Duck = 1, Sheep = 2, Cow = 3 }
    [SerializeField] private FarmType farmType;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(farmType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
