using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class InGameManager : MonoBehaviour
{
    #region Variables
    private Vector3 _firstPointX, _stayPointX; // First touched point. // End touched point.   
    [SerializeField] GameObject PlanePrefab; 
    [SerializeField] private List<GameObject> prefabList = new List<GameObject>();
    [SerializeField] Vector3 endPointLeft, endPointRight, centerPoint; 
    [SerializeField] float rotationAmount, moveXAmount, moveZAmount, duration; // Rotate Angle // position X point count // position Z point count // animations countdown.
    [SerializeField] int FarmCount; // Farm number 
    private float _targetRotation, _targetMoveX, _targetMoveZ, _time;   
    private int _count, _tempCount; // List in
    private bool objectControl = false;
    private enum FarmType { Chicken = 0, Duck = 1, Sheep = 2, Cow = 3 }
    [SerializeField] private FarmType farmType;
    #endregion
    #region Functions

    private void Awake() // Create farm plane
    {
        for (int i = 0; i < FarmCount; i++)
        {         
            GameObject prefab = Instantiate(PlanePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            prefab.transform.eulerAngles = new Vector3(0, 90, 0);
            prefabList.Add(prefab);
            prefabList[i].SetActive(false);
        }
        _count = 0;
        prefabList[_count].transform.position = centerPoint;
        prefabList[_count].SetActive(true);
    }
    private void Update() // Touch enable timing
    {
        if (objectControl == true)
        {
            _time += Time.deltaTime;
        }
        if (_time > duration)
        {
            objectControl = false;
            _time = 0;
        }
    }
    void FixedUpdate()
    {

        if (objectControl == false)
        {
            float _directionDifX; // First - End.
            if (Input.GetMouseButtonDown(0))
            {
                _firstPointX = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, 0, 0));
            }
            if (Input.GetMouseButtonUp(0))
            {
                prefabList[_count].SetActive(true);
                _stayPointX = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, 0, 0));
                _directionDifX = _firstPointX.x - _stayPointX.x;
                _tempCount = _count;
                prefabList[_count].transform.position = centerPoint;
                PosAndRotUpdate();
                if (_directionDifX < 0) // screen swiped to the right?
                {                                     
                    ScrollRightFrontToBack();
                    RotateObjectWithTween(_targetRotation);
                    MoveObjectWithTween(_targetMoveX, _targetMoveZ);
                    _count++;
                    CountUpade();
                    prefabList[_count].transform.position = endPointLeft;                   
                    RotateUpdateWithTween(0);
                    PosAndRotUpdate();
                    ScrollRightBackToFront();                   
                }
                if (_directionDifX > 0) // screen swiped to the left?
                {
                    ScrollLeftFrontToBack();
                    RotateObjectWithTween(_targetRotation);
                    MoveObjectWithTween(_targetMoveX, _targetMoveZ);
                    _count--;
                    CountUpade();
                    prefabList[_count].transform.position = endPointRight;
                    RotateUpdateWithTween(180);
                    PosAndRotUpdate();
                    ScrollLeftBackToFront();                          
                }
                RotateObjectWithTween(_targetRotation);
                MoveObjectWithTween(_targetMoveX, _targetMoveZ);
                prefabList[_count].SetActive(true);
                Invoke(nameof(ObjectFalse), duration);
                objectControl = true;
            }
        }
    }
    void RotateObjectWithTween(float _target) // Object rotation algorithm
    {
        prefabList[_count].transform.DORotate(new Vector3(prefabList[_count].transform.rotation.eulerAngles.x, _target, prefabList[_count].transform.rotation.eulerAngles.z), duration);
    }
    void RotateUpdateWithTween(float rotate) // Starting angle of next object
    {
        prefabList[_count].transform.eulerAngles = new Vector3(0, rotate, 0);
    }
    void MoveObjectWithTween(float _targetX, float _targetZ) // Object position algorithm
    {
        prefabList[_count].transform.DOMoveX(_targetX, duration);
        prefabList[_count].transform.DOMoveZ(_targetZ, duration);
    }
    void PosAndRotUpdate() // Assigns the coordinate points of the object to the variables.
    {
        _targetRotation = prefabList[_count].transform.rotation.eulerAngles.y;
        _targetMoveX = prefabList[_count].transform.position.x;
        _targetMoveZ = prefabList[_count].transform.position.z;
    }
    #region ScrollPlaneAlgorithm
    void ScrollRightFrontToBack()
    {
        _targetRotation += rotationAmount;
        _targetMoveX += moveXAmount;
        _targetMoveZ -= moveZAmount;
    }
    void ScrollRightBackToFront()
    {
        _targetRotation += rotationAmount;
        _targetMoveX += moveXAmount;
        _targetMoveZ += moveZAmount;
    }
    void ScrollLeftFrontToBack()
    {
        _targetRotation -= rotationAmount;
        _targetMoveX -= moveXAmount;
        _targetMoveZ -= moveZAmount;
    }
    void ScrollLeftBackToFront()
    {
        _targetRotation -= rotationAmount;
        _targetMoveX -= moveXAmount;
        _targetMoveZ += moveZAmount;
    }
    #endregion

    void ObjectFalse() // Object Set False
    {
        prefabList[_tempCount].SetActive(false);
    }
    void CountUpade() // List in count Update
    {
        if (_count < 0)
        {
            _count = prefabList.Count - 1;
        }
        if (_count == prefabList.Count)
        {
            _count = 0;
        }
    }
    #endregion

}
