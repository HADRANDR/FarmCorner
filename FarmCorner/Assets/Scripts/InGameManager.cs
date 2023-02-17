using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InGameManager : MonoBehaviour
{
    [SerializeField] GameObject PlanePrefab;
    [SerializeField] private List<GameObject> prefabList = new List<GameObject>();

    private Vector3 _firstPointX;
    private Vector3 _stayPointX;
    [SerializeField] Vector3 _endPointLeft, _endPointRight, _centerPoint;
    float _directionDifX;

    [SerializeField] float _rotationAmount = 90f;
    [SerializeField] float _moveXAmount = 90f;
    [SerializeField] float _moveZAmount = 30f;
    [SerializeField] float _duration = 3f;

    [SerializeField] int _FarmCount;
    private float _targetRotation;
    private float _targetMoveX;
    private float _targetMoveZ;
    private bool objectControl = false;
    private float _time;
    private int _count = 0;
    private void Awake()
    {
        for (int i = 0; i < _FarmCount; i++)
        {
            GameObject prefab = Instantiate(PlanePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            prefabList.Add(prefab);
            //prefabList[i].SetActive(false);
        }
    }
    void Start()
    {
        //prefabList[0].SetActive(true);
        //prefabList[0].transform.Translate(_endPointLeft);
    }
    private void Update()
    {
        if (objectControl ==true)
        {
            _time += Time.deltaTime;
        }
        if (_time>3)
        {
            objectControl = false;
            _time = 0;
        }
    }
    void FixedUpdate()
    {
        if (objectControl==false)
        {          
            if (Input.GetMouseButtonDown(0))
            {              
                _firstPointX = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, 0, 0));
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                _stayPointX = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, 0, 0));

                _directionDifX = _firstPointX.x - _stayPointX.x;

                if (_directionDifX < 0) // Ekran sağa doğru çekildi mi?
                {
                    PosAndRotUpdate();
                    ScrollRight();
                    RotateObjectWithTween(_targetRotation);
                    MoveObjectWithTween(_targetMoveX, _targetMoveZ);
                    objectControl = true;
                }
                if (_directionDifX > 0) // Ekran sola doğru çekildi mi?
                {
                    PosAndRotUpdate();
                    ScrollLeft();
                    RotateObjectWithTween(_targetRotation);
                    MoveObjectWithTween(_targetMoveX, _targetMoveZ);
                    objectControl = true;
                }
            }
            _count += 1;
        }           
    }
    void RotateObjectWithTween(float _target) // Objein rotasyon hareket algoritması.
    {
        prefabList[0].gameObject.transform.DORotate(new Vector3(prefabList[0].gameObject.transform.rotation.eulerAngles.x, _target, prefabList[0].gameObject.transform.rotation.eulerAngles.z), _duration);
    }
    void MoveObjectWithTween(float _targetX, float _targetZ) // Objenin pozisyon hareket algoritmas.
    {
        prefabList[0].gameObject.transform.DOMoveX(_targetX, _duration);
        prefabList[0].gameObject.transform.DOMoveZ(_targetZ, _duration);
    }
    void PosAndRotUpdate() // Obje koordinat bilgilerini her kaydırma öncesi gerekli değişkende günceller.
    {
        _targetRotation = prefabList[0].gameObject.transform.rotation.eulerAngles.y;
        _targetMoveX = prefabList[0].gameObject.transform.position.x;
        _targetMoveZ = prefabList[0].gameObject.transform.position.z;
    }
    void ScrollRight() // Objenin hareket edeceği yöne dair koordinat güncellemesi.
    {
        _targetRotation += _rotationAmount;
        _targetMoveX += _moveXAmount;
        _targetMoveZ -= _moveZAmount;
    }
    void ScrollLeft() // Objenin hareket edeceği yöne dair koordinat güncellemesi.
    {
        _targetRotation -= _rotationAmount;
        _targetMoveX -= _moveXAmount;
        _targetMoveZ -= _moveZAmount;
    }
    
}
