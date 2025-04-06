using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] int cubePointValue = 1;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.IncreasePoint(cubePointValue);
                Destroy(gameObject);
            }
        }
    }
}
