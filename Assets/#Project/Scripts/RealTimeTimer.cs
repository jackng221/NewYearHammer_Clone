using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RealTimeTimer : MonoBehaviour
{
    [SerializeField] float timer = 0;
    [SerializeField] float targetTime = 0;
    float realTimeByLastUpdate;

    bool runTimer;

    public UnityEvent TimerUpdated = new UnityEvent();  //seems to need manual initialization when added via AddComponent

    private void Awake()
    {
        runTimer = false; //not run by default
    }
    private void Update()
    {
        if (runTimer)
        {
            //Alternative method to calcuate time past, as Time.deltaTime is affected by Time.scale
            //Adds realtime on timer every Update, until timer >= targetTime
            if (timer < targetTime)
            {
                timer += (Time.realtimeSinceStartup - realTimeByLastUpdate);
                TimerUpdated.Invoke();
            }
        }

        //record time passed on each update, which is used to calcualate [time since last update] on next update
        //needs to be recorded regardless of runTimer state
        realTimeByLastUpdate = Time.realtimeSinceStartup;
    }

    public float GetDeltaTime()
    {
        return Time.realtimeSinceStartup - realTimeByLastUpdate;
    }

    //for timers
    public void TimerStart(float targetTime)
    {
        TimerStop();
        this.targetTime = targetTime;
        runTimer = true;
    }
    public void TimerPause()
    {
        runTimer = false;
    }
    public void TimerResume()
    {
        runTimer = true;
    }
    public void TimerRestart()
    {
        timer = 0;
    }
    public void TimerStop()
    {
        runTimer = false;
        timer = 0;
    }
    public float TimerGetTimeCountUp()
    {
        return timer;
    }
    public float TimerGetTimeCountDown()
    {
        return targetTime - timer;
    }

    //for cooldowns
    public void CooldownInit(float cooldown)
    {
        timer = targetTime = cooldown; //cooldowned by default
        runTimer = true;
    }
    public bool CooldownIsCompleted()
    {
        if (timer >= targetTime)
        {
            timer = 0;
            return true;
        }
        return false;
    }
}
