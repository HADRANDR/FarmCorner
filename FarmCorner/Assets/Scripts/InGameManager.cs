using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
public class InGameManager : MonoBehaviour
{
    #region Variables
    FarmManager farmManager;
    FarmUpgrade farmUpgrade;
    [SerializeField] private List<FarmManager> prefabList = new();
    
    [Header("Farm Transform Controls")]
    [SerializeField] Vector3 endPointLeft;
    [SerializeField] Vector3 endPointRight;
    [SerializeField] Vector3 centerPoint;
    [SerializeField] float rotationAmount, moveXAmount, moveZAmount, duration, distance; // Rotate Angle // position X point count // position Z point count // animations countdown.
    public static float _staticDuration;
    private Vector2 _startPoint, _endPoint, directionDifX;
    private float _targetRotation, _targetMoveX, _targetMoveZ, _directionDifX;
    private int _count, _tempCount; // List in
    private bool canDrag = true;
    Camera cam;
    public bool CanDrag { get; set; }
    bool canButtonDown;

    #endregion
    #region Functions
    private void Awake() // Create farm plane
    {
        _staticDuration = duration;
        _count = 0;
        prefabList[_count].gameObject.transform.position = centerPoint;
        prefabList[_count].gameObject.SetActive(true);
        cam = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //RaycastHit hit;
            //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, out hit))
            //{
            //    if (hit.collider.gameObject.CompareTag("Button"))
            //    {
            //        Debug.Log("UI");
            //    }
            //}
            //if (EventSystem.current.IsPointerOverGameObject())
            //{
            //    canButtonDown = true;
            //}
            //else canButtonDown = false;
            _startPoint = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, 0, 0));
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            //if (canButtonDown) return;
            Debug.Log("sa");
            _endPoint = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, 0, 0));
            _directionDifX = _startPoint.x - _endPoint.x;
            if (!canDrag) return;
            if (_directionDifX <= -0.25f) // screen swiped to the right?
            {
                StartCoroutine(CooldownAsync(duration));
                PosAndRotUpdate();
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
                AnimalStop();
                Invoke(nameof(ObjectFalse), duration);
                prefabList[_count].gameObject.SetActive(true);
                RotateObjectWithTween(_targetRotation);
                MoveObjectWithTween(_targetMoveX, _targetMoveZ);
            }
            else if (_directionDifX >= 0.25f) // screen swiped to the left?
            {
                StartCoroutine(CooldownAsync(duration));
                PosAndRotUpdate();
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
                AnimalStop();
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

    void AnimalStop()
    {
        prefabList[_tempCount].StopAnimal();
    }
    void ObjectFalse() // Object Set False
    {
        prefabList[_tempCount].CloseFarm();
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
    public IEnumerator CooldownAsync(float time)
    {
        
        //farmUpgrade.CanDrag = false;
        canDrag = false;
        yield return new WaitForSeconds(time);
        canDrag = true;
        //farmUpgrade.CanDrag = true;
    }
    #endregion

}
