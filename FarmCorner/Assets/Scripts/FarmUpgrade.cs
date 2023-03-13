using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class FarmUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject upgradeCanvas;
    [SerializeField] private LayerMask layerMask;
    Camera cam;
    bool canDrag;
    public bool CanDrag
    {
        get
        {
            return canDrag;
        }
        set
        {
            canDrag = CanDrag;
        }
    }
    private bool _isOpen = false;
    private bool onHit = false;
    private bool canvasStart = false;

    private void Awake()
    {
        cam = Camera.main;
        canDrag = true;
    }

    void Update()
    {

        
        if (canDrag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.transform.gameObject != upgradeCanvas)
                    {
                        onHit = true;
                        canvasStart = false;
                    }
                    else
                    {
                        onHit = false;
                        canvasStart = true;
                    }
                }
                else
                {
                    onHit = false;
                    canvasStart = false;
                }

            }

            if (Input.GetMouseButtonUp(0))
            {

                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {

                    if (onHit && hit.transform.gameObject != upgradeCanvas)
                    {
                        if (_isOpen)
                        {
                            _isOpen = false;
                            upgradeCanvas.transform.DOScale(Vector3.zero, 0.5f);
                        }
                        else
                        {
                            _isOpen = true;
                            upgradeCanvas.transform.DOScale(new Vector3(1.5f, 1f, 1f), 0.5f);
                        }
                    }
                }
                else
                {
                    if (!onHit)
                    {
                        if (_isOpen && !canvasStart)
                        {
                            _isOpen = false;
                            upgradeCanvas.transform.DOScale(Vector3.zero, 0.5f);
                        }
                    }
                }
            }
        }
        else
        {
            upgradeCanvas.transform.DOScale(Vector3.zero, 0.5f);
        }
        
    }

}
