using System;
using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    [Header("Battery Settings")]
    [SerializeField] private float maxBattery = 100f;
    [SerializeField] private float drainRate = 4f;

    [Header("Outside Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float exteriorStartZ = 4.25f;

    public float CurrentBattery { get; private set; }
    public float MaxBattery => maxBattery;
    public bool IsOutside { get; private set; }

    public event Action<float, float> BatteryChanged;

    private void Awake()
    {
        CurrentBattery = maxBattery;
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
                player = playerObject.transform;
        }

        BatteryChanged?.Invoke(CurrentBattery, MaxBattery);
    }

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.CurrentState != GameState.Playing)
            return;

        if (player == null)
            return;

        IsOutside = player.position.z >= exteriorStartZ;

        if (!IsOutside)
            return;

        CurrentBattery -= drainRate * Time.deltaTime;
        CurrentBattery = Mathf.Clamp(CurrentBattery, 0f, MaxBattery);

        BatteryChanged?.Invoke(CurrentBattery, MaxBattery);

        if (CurrentBattery <= 0f)
        {
            GameManager.Instance.FailGame();
        }
    }

    public void ResetBattery()
    {
        CurrentBattery = MaxBattery;
        BatteryChanged?.Invoke(CurrentBattery, MaxBattery);
    }
}