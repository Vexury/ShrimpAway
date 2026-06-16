using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject[] hearts;

    private void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
            if (hearts[i] != null)
                hearts[i].SetActive(i < playerHealth.CurrentHP);
    }
}
