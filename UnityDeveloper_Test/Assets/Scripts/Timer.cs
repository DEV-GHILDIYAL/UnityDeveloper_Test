using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    public float totalTime = 120f; // 2 minutes
    private float currentTime;

    public int targetCubesToCollect = 10;
    public TextMeshProUGUI timerText;
    public GameObject winUI;
    public GameObject loseUI;
    public TextMeshProUGUI reason;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        currentTime = totalTime;
        winUI.SetActive(false);
        loseUI.SetActive(false);
    }

    void Update()
    {
        if (GameManager.instance == null) return;

        if (GameManager.instance.point >= targetCubesToCollect)
        {
            WinGame();
            return;
        }

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            LoseGame();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void WinGame()
    {
        winUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void LoseGame()
    {
        loseUI.SetActive(true);
        reason.text = "Lol you are unable to collect all cubes NOOB";
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OutOfBound(){
        loseUI.SetActive(true);
        reason.text = "Out Of Region";
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
