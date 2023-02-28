using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
public class InGameManager : MonoBehaviour
{
    #region Variables
    //int _textCount = 0;
    FarmManager farmManager;
    private Vector3 _firstPointX, _stayPointX; // First touched point. // End touched point.
    [SerializeField] GameObject[] ButtonObjects;
    [SerializeField] private List<FarmManager> prefabList = new();
    [SerializeField] Vector3 endPointLeft, endPointRight, centerPoint; 
    [SerializeField] float rotationAmount, moveXAmount, moveZAmount, duration; // Rotate Angle // position X point count // position Z point count // animations countdown.
    private float _targetRotation, _targetMoveX, _targetMoveZ, _time;   
    private int _count, _tempCount; // List in
    private bool objectControl = false;
    
    #endregion
    #region Functions
    private void Awake() // Create farm plane
    {    
        _count = 0;
        prefabList[_count].gameObject.transform.position = centerPoint;
        prefabList[_count].gameObject.SetActive(true);
    }
    //private void OnEnable()
    //{
    //    GameManager.Instance.OnOpenNewFarm.AddListener(UpdateBuyText);
    //}
    //void UpdateBuyText(int index)
    //{
    //    switch (index)
    //    {
    //        case 0:
    //            _textCount = 0;
    //            break;
    //        case 1:
    //            _textCount = 1;
    //            break;
    //        case 2:
    //            _textCount = 2;
    //            break;
    //    }
    //}
    void Update()
    {
        if (objectControl == true) // Touch Enabled timing
        {
            _time += Time.deltaTime;
        }
        if (_time > duration)
        {
            objectControl = false;
            _time = 0;
        }

        if (objectControl == false)
        {
            Debug.Log(_count);
            
             // First - End.
            if (Input.GetMouseButtonDown(0) /*&& EventSystem.current.currentSelectedGameObject != ButtonObjects[_textCount]*/)
            {
                _firstPointX = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, 0, 0));               
            }
            if (Input.GetMouseButtonUp(0) /*&& EventSystem.current.currentSelectedGameObject != ButtonObjects[_textCount]*/)
            {
                float _directionDifX;
                _stayPointX = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, 0, 0));
                _directionDifX = _firstPointX.x - _stayPointX.x;
                Debug.Log(_directionDifX);               
                prefabList[_count].transform.position = centerPoint;
                PosAndRotUpdate();
                if (_directionDifX <= -0.25f) // screen swiped to the right?
                {
                    objectControl = true;
                    ScrollRightFrontToBack();
                    RotateObjectWithTween(_targetRotation);
                    MoveObjectWithTween(_targetMoveX, _targetMoveZ);
                    _tempCount = _count;
                    _count++;
                    CountUpade();
                    prefabList[_count].transform.position = endPointLeft;                   
                    RotateUpdateWithTween(-90);
                    PosAndRotUpdate();
                    ScrollRightBackToFront();
                }
                if (_directionDifX >= 0.25f) // screen swiped to the left?
                {
                    objectControl = true;
                    ScrollLeftFrontToBack();
                    RotateObjectWithTween(_targetRotation);
                    MoveObjectWithTween(_targetMoveX, _targetMoveZ);
                    _tempCount = _count;
                    _count--;
                    CountUpade();
                    prefabList[_count].transform.position = endPointRight;
                    RotateUpdateWithTween(90);
                    PosAndRotUpdate();
                    ScrollLeftBackToFront();                  
                }
                _directionDifX = 0;
                Invoke(nameof(ObjectFalse), duration);             
                prefabList[_count].gameObject.SetActive(true);               
                RotateObjectWithTween(_targetRotation);
                MoveObjectWithTween(_targetMoveX, _targetMoveZ);
                
                
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
        prefabList[_tempCount].gameObject.SetActive(false);
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
