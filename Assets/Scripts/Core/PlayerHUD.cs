using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text stateLabel;
    [SerializeField] private TMP_Text timerLabel;
    [SerializeField] private TMP_Text coinLabel;
    [SerializeField] private TMP_Text winStatsLabel;
    [SerializeField] private PlayerController controller;

    private bool won;

    private void OnEnable()
    {
        GameTimer.OnWin        += OnWin;
        Collectible.OnCollected += OnCollected;
    }

    private void OnDisable()
    {
        GameTimer.OnWin        -= OnWin;
        Collectible.OnCollected -= OnCollected;
    }

    private void Start()
    {
        coinLabel.text = "0";
        winStatsLabel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (GameTimer.Instance != null)
            timerLabel.text = GameTimer.Instance.FormattedTime();

        if (won || controller == null) return;

        string movement;
        if (controller.IsCrouching)
            movement = "Crouching";
        else if (controller.IsSprinting && controller.NormalizedSpeed > 0.05f)
            movement = "Sprinting";
        else if (controller.NormalizedSpeed > 0.05f)
            movement = "Walking";
        else
            movement = "Idle";

        stateLabel.text = !controller.IsGrounded ? movement + " + Jumping" : movement;
    }

    private void OnWin()
    {
        won = true;
        stateLabel.text = "You Win!";

        string time        = GameTimer.Instance != null ? GameTimer.Instance.FormattedTime() : "--";
        string collectibles = $"Coins: {Collectible.GetCount(CollectibleType.Coin)}  Sandwiches: {Collectible.GetCount(CollectibleType.Sandwich)}";
        string detections  = EnemyNavMeshChaser.DetectionCount.ToString();
        winStatsLabel.gameObject.SetActive(true);
        winStatsLabel.text = $"Time: {time}\nCollectibles: {collectibles}\nDetected: {detections}x";
    }

    private void OnCollected(CollectibleType type, int count)
    {
        if (type == CollectibleType.Coin) coinLabel.text = count.ToString();
    }
}
