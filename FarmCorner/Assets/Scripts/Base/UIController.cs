using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject WinPanel, LosePanel, InGamePanel;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private List<string> moneyMulti = new();
    [SerializeField] private GameObject coin, money;

    [Header("Upgrade Panels")]    
    [SerializeField] private List<Button> chickenPanel = new();
    [SerializeField] private List<Button> duckPanel = new();
    [SerializeField] private List<Button> sheepPanel = new();
    [SerializeField] private List<Button> cowPanel = new();
    [SerializeField] private GameObject chickenP;
    [SerializeField] private GameObject duckP;
    [SerializeField] private GameObject sheepP;
    [SerializeField] private GameObject cowP;
    //[SerializeField] private GameObject buyButton;
    //[SerializeField] private TextMeshProUGUI buyButtonText;

    private Canvas UICanvas;

    private Button Next, Restart;

    private LevelManager levelManager;

    private Settings settings;
    FarmManager farmManager;

    private void Awake()
    {
        ScriptInitialize();
        ButtonInitialize();
    }

    private void Start()
    {
        GameManager.Instance.OnMoneyChange.Invoke();

    }

    void ScriptInitialize()
    {
        levelManager = FindObjectOfType<LevelManager>();
        settings = FindObjectOfType<Settings>();
        UICanvas = GetComponentInParent<Canvas>();
    }

    void ButtonInitialize()
    {
        Next = WinPanel.GetComponentInChildren<Button>();
        Restart = LosePanel.GetComponentInChildren<Button>();

        Next.onClick.AddListener(() => levelManager.LoadLevel(1));
        Restart.onClick.AddListener(() => levelManager.LoadLevel(0));
    }

    void ShowPanel(GameObject panel, bool canvasMode = false)
    {
        panel.SetActive(true);
        GameObject panelChild = panel.transform.GetChild(0).gameObject;
        panelChild.transform.localScale = Vector3.zero;
        panelChild.SetActive(true);
        panelChild.transform.DOScale(Vector3.one, 0.5f);

        UICanvas.worldCamera = Camera.main;
        UICanvas.renderMode = canvasMode ? RenderMode.ScreenSpaceCamera : RenderMode.ScreenSpaceOverlay;
    }

    void GameReady()
    {
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
        InGamePanel.SetActive(true);
    }

    void SetMoneyText()
    {
        if (coin.activeSelf)
        {
            coin.transform.DORewind();
            coin.transform.DOPunchScale(Vector3.one, 0.5f, 2, 1);
        }


        if (money.activeSelf)
        {
            money.transform.DORewind();
            money.transform.DOPunchScale(Vector3.one, 0.5f, 2, 1);
        }

        int moneyDigit = GameManager.Instance.PlayerMoney.ToString().Length;
        int value = (moneyDigit - 1) / 3;
        if (value < 1)
        {
            moneyText.text = GameManager.Instance.PlayerMoney.ToString();
        }
        else
        {
            float temp = GameManager.Instance.PlayerMoney / Mathf.Pow(1000, value);
            moneyText.text = temp.ToString("F2") + " " + moneyMulti[value];
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.LevelFail.AddListener(() => ShowPanel(LosePanel, true));
        GameManager.Instance.LevelSuccess.AddListener(() => ShowPanel(WinPanel, true));
        GameManager.Instance.GameReady.AddListener(GameReady);
        GameManager.Instance.OnMoneyChange.AddListener(SetMoneyText);
        GameManager.Instance.OnOpenNewFarm.AddListener(OpenUpgradePanel);
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.LevelFail.RemoveListener(() => ShowPanel(LosePanel, true));
            GameManager.Instance.LevelSuccess.RemoveListener(() => ShowPanel(WinPanel, true));
            GameManager.Instance.GameReady.RemoveListener(GameReady);
            GameManager.Instance.OnOpenNewFarm.RemoveListener(OpenUpgradePanel);
        }
    }

    void CloseAllUpgradePanel()
    {
        chickenP.SetActive(false);
        duckP.SetActive(false);
        sheepP.SetActive(false);
        cowP.SetActive(false);
    }
    int _indexTemp = 0;
    bool firstControl = true;
    void OpenUpgradePanel(int index)
    {
        
        CloseAllUpgradePanel();
        switch (index)
        {
            case 0:
                _indexTemp = 0;
                chickenP.SetActive(true);
                OffInteractableButton();
                if (firstControl == true)
                {
                    OnInteractableButton();
                    firstControl = false;
                }
                else
                {
                    Invoke(nameof(OnInteractableButton), InGameManager._staticDuration);
                }               
                break;
            case 1:
                _indexTemp = 1;
                duckP.SetActive(true);
                OffInteractableButton();
                Invoke(nameof(OnInteractableButton), InGameManager._staticDuration);
                break;
            case 2:
                _indexTemp = 2;
                sheepP.SetActive(true);
                OffInteractableButton();
                Invoke(nameof(OnInteractableButton), InGameManager._staticDuration);
                break;
            case 3:
                _indexTemp = 3;
                cowP.SetActive(true);
                OffInteractableButton();
                Invoke(nameof(OnInteractableButton), InGameManager._staticDuration);
                break;
        }
    }
    void OnInteractableButton()
    {
        for (int i = 0; i < chickenPanel.Count; i++)
        {
            if (_indexTemp == 0)
            {
                chickenPanel[i].interactable = true;
            }
            else if (_indexTemp == 1)
            {
                duckPanel[i].interactable = true;
            }
            else if (_indexTemp == 2)
            {
                sheepPanel[i].interactable = true;
            }
            else if (_indexTemp == 3)
            {
                cowPanel[i].interactable = true;
            }
        }
        
    }
    void OffInteractableButton()
    {
        for (int i = 0; i < chickenPanel.Count; i++)
        {
            if (_indexTemp == 0)
            {
                chickenPanel[i].interactable = false;
            }
            else if (_indexTemp == 1)
            {
                duckPanel[i].interactable = false;
            }
            else if (_indexTemp == 2)
            {
                sheepPanel[i].interactable = false;
            }
            else if (_indexTemp == 3)
            {
                cowPanel[i].interactable = false;
            }
        }
    }
}
