using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class InGameManager : MonoBehaviour
{
    #region Variables
    private Vector3 _firstPointX; // First touched point.
    private Vector3 _stayPointX; // End touched point.
    float _directionDifX; // First - End.
    [SerializeField] GameObject PlanePrefab; 
    [SerializeField] private List<GameObject> prefabList = new List<GameObject>();
    [SerializeField] Vector3 _endPointLeft, _endPointRight, _centerPoint; 
    [SerializeField] float _rotationAmount; // Rotate Angle
    [SerializeField] float _moveXAmount; // position X point count
    [SerializeField] float _moveZAmount; // position Z point count
    [SerializeField] float _duration; // animations countdown.
    [SerializeField] int _FarmCount; // Farm number 
    private float _targetRotation; 
    private float _targetMoveX;
    private float _targetMoveZ;
    private bool objectControl = false;
    private float _time;
    private int _count; // List in
    private int _tempCount; // List in before

    #endregion

    #region Functions

    private void Awake() // Create farm plane
    {
        for (int i = 0; i < _FarmCount; i++)
        {
            GameObject prefab = Instantiate(PlanePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            prefab.transform.eulerAngles = new Vector3(0, 90, 0);
            prefabList.Add(prefab);
            prefabList[i].SetActive(false);

        }
        _count = 0;
        prefabList[_count].transform.position = _centerPoint;
        prefabList[_count].SetActive(true);
    }
    private void Update() // Touch enable timing
    {
        if (objectControl == true)
        {
            _time += Time.deltaTime;
        }
        if (_time > _duration)
        {
            objectControl = false;
            _time = 0;
        }
    }
    void FixedUpdate()
    {
        if (objectControl == false)
        {
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

                if (_directionDifX < 0) // screen swiped to the right?
                {
                    prefabList[_count].transform.position = _centerPoint;
                    PosAndRotUpdate();
                    ScrollRightFrontToBack();
                    RotateObjectWithTween(_targetRotation);
                    MoveObjectWithTween(_targetMoveX, _targetMoveZ);

                    _count++;
                    CountUpade();

                    prefabList[_count].transform.position = _endPointLeft;
                    prefabList[_count].SetActive(true);
                    RotateUpdateWithTween(0);
                    PosAndRotUpdate();
                    ScrollRightBackToFront();
                    RotateObjectWithTween(_targetRotation);
                    MoveObjectWithTween(_targetMoveX, _targetMoveZ);

                    Invoke(nameof(ObjectFalse), _duration);
                }
                if (_directionDifX > 0) // screen swiped to the left?
                {
                    prefabList[_count].transform.position = _centerPoint;
                    PosAndRotUpdate();
                    ScrollLeftFrontToBack();
                    RotateObjectWithTween(_targetRotation);
                    MoveObjectWithTween(_targetMoveX, _targetMoveZ);

                    _count--;
                    CountUpade();

                    prefabList[_count].transform.position = _endPointRight;
                    prefabList[_count].SetActive(true);
                    RotateUpdateWithTween(180);
                    PosAndRotUpdate();
                    ScrollLeftBackToFront();
                    RotateObjectWithTween(_targetRotation);
                    MoveObjectWithTween(_targetMoveX, _targetMoveZ);

                    Invoke(nameof(ObjectFalse), _duration);
                }
                objectControl = true;
            }
        }

    } // Farm Plane moving algorithm
    void RotateObjectWithTween(float _target) // Object rotation algorithm
    {
        prefabList[_count].transform.DORotate(new Vector3(prefabList[_count].transform.rotation.eulerAngles.x, _target, prefabList[_count].transform.rotation.eulerAngles.z), _duration);
    }
    void RotateUpdateWithTween(float rotate) // Starting angle of next object
    {
        prefabList[_count].transform.eulerAngles = new Vector3(0, rotate, 0);
    }
    void MoveObjectWithTween(float _targetX, float _targetZ) // Object position algorithm
    {
        prefabList[_count].transform.DOMoveX(_targetX, _duration);
        prefabList[_count].transform.DOMoveZ(_targetZ, _duration);
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
        _targetRotation += _rotationAmount;
        _targetMoveX += _moveXAmount;
        _targetMoveZ -= _moveZAmount;
    }
    void ScrollRightBackToFront()
    {
        _targetRotation += _rotationAmount;
        _targetMoveX += _moveXAmount;
        _targetMoveZ += _moveZAmount;
    }
    void ScrollLeftFrontToBack()
    {
        _targetRotation -= _rotationAmount;
        _targetMoveX -= _moveXAmount;
        _targetMoveZ -= _moveZAmount;
    }
    void ScrollLeftBackToFront()
    {
        _targetRotation -= _rotationAmount;
        _targetMoveX -= _moveXAmount;
        _targetMoveZ += _moveZAmount;
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
