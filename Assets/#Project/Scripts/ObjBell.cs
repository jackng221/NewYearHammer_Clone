using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjBell : MonoBehaviour
{
    public UnityEvent<int> BellHit;

    Rigidbody2D rb;
    [SerializeField] MMF_Player mmfPlayer;
    GameManager gameManager;

    public float cooldown = 0.4f;
    RealTimeTimer timer;
    Vector2 savedPos;   //bell original position

    Vector2 hitPos;
    public Vector2 HitPos { get { return hitPos; } }
    Vector2 hitDirection;
    public Vector2 HitDirection { get {  return hitDirection; } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = gameObject.AddComponent<RealTimeTimer>();
    }
    private void Start()
    {
        gameManager = FindObjectsByType<GameManager>(FindObjectsSortMode.None)[0];
        gameManager.GameStart.AddListener(OnAction);
        gameManager.GameReset.AddListener(OnSetup);
    }
    private void LateUpdate()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3)hitDirection);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (timer.CooldownIsCompleted() == false) return; //skip if cooldown not reached
        if (collision.GetComponent<ObjHammerBit>() == null) return;
        Debug.Log("trigger");

        hitPos = collision.transform.position;
        //from hit point to bell position
        hitDirection = (transform.position - (Vector3)hitPos).normalized * collision.GetComponentInParent<Rigidbody2D>().velocity.magnitude;

        //feedback before force is applied
        mmfPlayer.GetFeedbackOfType<MMF_CinemachineImpulse>().Velocity = hitDirection;
        //magnitude scaled in 0~16 and rescaled
        Debug.Log(hitDirection.magnitude);
        mmfPlayer.GetFeedbackOfType<MMF_Sound>().MaxVolume = Mathf.Lerp(0.33f, 1, Mathf.InverseLerp(5, 15, hitDirection.magnitude));
        mmfPlayer.GetFeedbackOfType<MMF_Sound>().MinVolume = Mathf.Lerp(0.33f, 1, Mathf.InverseLerp(5, 15, hitDirection.magnitude));
        mmfPlayer.GetFeedbackOfType<MMF_VFX>().scale = new Vector2(1, 1) * Mathf.Lerp(0.5f, 2f, Mathf.InverseLerp(5, 15, hitDirection.magnitude));
        mmfPlayer.GetFeedbackOfType<MMF_FloatingText>().Value = Mathf.Round(hitDirection.magnitude).ToString();

        mmfPlayer.PlayFeedbacks();

        rb.AddForce(hitDirection, ForceMode2D.Impulse);

        BellHit.Invoke(Mathf.RoundToInt(hitDirection.magnitude));
    }

    void RecordPos()
    {
        savedPos = transform.position;
    }
    void LoadPos()
    {
        if (savedPos == null) return;

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        transform.position = savedPos;

        Physics2D.SyncTransforms();
        ClearPos();
    }
    void ClearPos()
    {
        savedPos = Vector2.zero;
    }

    //on game manager game phase
    void OnSetup()
    {
        LoadPos();
        mmfPlayer.StopFeedbacks();
        mmfPlayer.ResetFeedbacks();
    }
    void OnAction()
    {
        RecordPos();
        timer.CooldownInit(cooldown);
    }
}
