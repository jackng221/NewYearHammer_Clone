using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float timeScale = 2f;
    [SerializeField] float healthPointMax = 100;
    [SerializeField] float healthPoint;

    [Tooltip("Testing purpose")]
    [SerializeField] bool undead = false;

    RealTimeTimer timer;
    [SerializeField] float actionTimeLimit = 60;

    [SerializeField] TextMeshProUGUI ActionPhaseCountdownText;
    [SerializeField] Slider healthPointSlider; 
    [SerializeField] Slider healthPointChangeSlider; 

    [SerializeField] ObjBell bell;
    [SerializeField] ParticleSystem winParticle;
    public GameObject selectedSection;

    public UnityEvent GameStart;    //->action phase
    public UnityEvent<string> GameOver;     //->result phase
    public UnityEvent GameReset;    //->setup phase

    public enum Phases
    {
        Action,
        Result,
        Setup
    }
    public Phases GamePhase = Phases.Setup;

    private void Awake()
    {
        timer = gameObject.AddComponent<RealTimeTimer>();
        GameStart.AddListener( ()=> timer.TimerStart(actionTimeLimit) );
        GameOver.AddListener( (string text)=> timer.TimerPause() );
        GameReset.AddListener(timer.TimerStop);

        timer.TimerUpdated.AddListener(ReceivesTimerUpdate);
    }
    void Start()
    {
        bell.BellHit.AddListener( (magnitute) => Damage(magnitute) );
        ResetGame();
    }
    private void Update()
    {
        if (healthPointChangeSlider.value != healthPointSlider.value)
        {
            if (healthPointChangeSlider.value > healthPointSlider.value)
            {
                healthPointChangeSlider.fillRect.GetComponent<Image>().color = Color.red;
            }
            else if (healthPointChangeSlider.value < healthPointSlider.value)
            {
                healthPointChangeSlider.fillRect.GetComponent<Image>().color = Color.green;
            }

            healthPointChangeSlider.value = Mathf.MoveTowards( healthPointChangeSlider.value, healthPointSlider.value, 0.5f*timer.GetDeltaTime() );
        }
    }

    public void StartGame()
    {
        GamePhase = Phases.Action;

        healthPoint = healthPointMax;
        healthPointSlider.value = 1;
        healthPointChangeSlider.value = 1;

        GameStart.Invoke();
        Time.timeScale = timeScale;
    }
    public void EndGame(bool win)
    {
        GamePhase = Phases.Result;

        if (win)
        {
            ActionPhaseCountdownText.text = "WIN";
            winParticle.Play();
        }
        else
        {
            ActionPhaseCountdownText.text = "LOSE";
        }

        GameOver.Invoke( win? "WIN" : "LOSE" );
    }
    public void ResetGame()
    {
        GamePhase = Phases.Setup;

        winParticle.Stop();
        winParticle.Clear();
        healthPoint = 100;

        GameReset.Invoke();
        Time.timeScale = 0;
    }
    public void Damage(int magnitude)
    {
        if (GamePhase != Phases.Action) return;

        healthPoint -= magnitude; Debug.Log("HP:" + healthPoint);
        healthPointSlider.value = healthPoint/healthPointMax;
        if (healthPoint <= 0)
        {
            if (undead)
                healthPoint = healthPointMax;
            else
                EndGame(true);
        }
    }

    void ReceivesTimerUpdate()
    {
        if (GamePhase != Phases.Action) return;

        if (timer.TimerGetTimeCountDown() <= 0)
        {
            EndGame(false);
            return;
        }
        ActionPhaseCountdownText.text = Mathf.Ceil(timer.TimerGetTimeCountDown()).ToString();
    }
}
