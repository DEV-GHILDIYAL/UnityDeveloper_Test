using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int point = 0;
    [SerializeField] TextMeshProUGUI scoreUI;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        Time.timeScale = 1f;
    }

    public void IncreasePoint(int pointValue)
    {
        point += pointValue;
    }

    void Update()
    {
        scoreUI.text = point.ToString();
    }
}
