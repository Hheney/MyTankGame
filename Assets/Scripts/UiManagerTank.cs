using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiManagerTank : MonoBehaviour
{
    public TMP_Text textUserInfo;
    public Slider sliderHealth;

    public int health = 0;
    public int maxHealth = 100;

    private static UiManagerTank _instance = null;
    public static UiManagerTank Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("UiManagerTank is null.");
            }
            return _instance;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.startNode();
        GameManager.Instance.updateCountId();
        health = maxHealth;
        updateTextUserInfo();
        updateSliderHealth();
        Invoke("checkCountId", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Debug.Log("UiManagerTank has another instance.");
            Destroy(gameObject);
        }
    }

    public void updateTextUserInfo()
    {
        textUserInfo.text = $"{GameManager.Instance.CountId}:{GameManager.Instance.UserId}={health}";
    }

    public void updateSliderHealth()
    {
        sliderHealth.value = health;
    }

    void checkCountId()
    {
        GameManager.Instance.updateCountId();
        updateTextUserInfo();
    }
}
