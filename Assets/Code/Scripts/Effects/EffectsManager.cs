using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    //usable timers --- float is the deacay time
    public static UnityEvent<float> WallUsed = new UnityEvent<float>();
    public static UnityEvent<float> InvisibilityUsed = new UnityEvent<float>();
    //Effect timers
    public static UnityEvent<float> InvulnerabilityUsed = new UnityEvent<float>();
    public static UnityEvent<float, bool> SpeedEditUsed = new UnityEvent<float, bool>();

    //Timers
    public static float InvulnerabilityTime { get; private set; } = 0f;
    public static float InvulnerabilityStartTime { get; private set; } = 0f;
    public static float WallTime { get; private set; } = 0f;
    public static float WallStartTime { get; private set; } = 0f;
    public static float InvisibilityTime { get; private set; } = 0f;
    public static float InvisibilityStartTime { get; private set; } = 0f;
    public static float SpeedEditTime { get; private set; } = 0f;
    public static float SpeedEditStartTime { get; private set; } = 0f;

    [Header("PlayerReference")]
    [SerializeField] PlayerManager Player;

    public PlayerManager PlayerManager
    {
        get
        {
            if (Player == null)
            {
                Player = FindFirstObjectByType<PlayerManager>();
            }
            return Player;
        }
        set => Player = value;
    }

    private void Awake()
    {
        //create all unity events
        //usable Events
        if(WallUsed == null)
            WallUsed = new UnityEngine.Events.UnityEvent<float>();

        if(InvisibilityUsed == null)
            InvisibilityUsed = new UnityEngine.Events.UnityEvent<float>();

        if(InvulnerabilityUsed == null)
            InvulnerabilityUsed = new UnityEngine.Events.UnityEvent<float>();

        if (SpeedEditUsed == null)
            SpeedEditUsed = new UnityEvent<float, bool>();

        if (PlayerManager == null)
            PlayerManager = FindFirstObjectByType<PlayerManager>();

        //add all listener
        WallUsed.AddListener(OnWallUsed);
        InvisibilityUsed.AddListener(OnInvisibilityEnabled);
        InvulnerabilityUsed.AddListener(OnInvulnerabilityUsed);
        SpeedEditUsed.AddListener(OnSpeedEditUsed);
    }

    private void FixedUpdate()
    {
        //decrease all timers and set to 0 if < 0
        if(InvulnerabilityTime > 0)
        {
            InvulnerabilityTime-=Time.fixedDeltaTime;
            if(InvulnerabilityTime < 0f)
            {
                PlayerManager.Invulnerability = false;
                InvulnerabilityTime = 0f;
            }
        }

        if (WallTime > 0)
        {
            WallTime-=Time.fixedDeltaTime;
            if(WallTime < 0f)
                WallTime = 0f;
        }

        if(InvisibilityTime > 0)
        {
            InvisibilityTime-=Time.fixedDeltaTime;
            if(InvisibilityTime < 0f)
                InvisibilityTime = 0f;
        }

        if(SpeedEditTime > 0)
        {
            SpeedEditTime-=Time.fixedDeltaTime;
            if(SpeedEditTime < 0f)
            {
                Player.GetComponent<Movement>().SpeedLimiter = Player.GetComponent<Movement>().StartSpeedLimiter;
                SpeedEditTime = 0f;
            }
        }
        
    }

    private void OnInvulnerabilityUsed(float time)
    {
        InvulnerabilityStartTime = time;
        InvulnerabilityTime = time;
    }

    private void OnWallUsed(float time)
    {
        WallStartTime = time;
        WallTime = time;
    }

    private void OnSpeedEditUsed(float time, bool malus)
    {
        SpeedEditStartTime = time;
        SpeedEditTime = time;
    }

    private void OnInvisibilityEnabled(float time)
    {
        InvisibilityStartTime = time;
        InvisibilityTime = time;
    }

    public static void ResetSpeedEdit()
    {
        SpeedEditTime = 0f;
        SpeedEditStartTime = 0f;
    }
}
