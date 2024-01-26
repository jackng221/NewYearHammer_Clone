using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjHammer : MonoBehaviour
{
    GameManager gameManager;

    public List<GameObject> sectionsList;
    public List<LineRenderer> linesList;
    [SerializeField] TrailRenderer trail;

    GameObject hammerSection;

    List<Vector2> savedPos;

    private void Awake()
    {
        sectionsList = new List<GameObject>();
        sectionsList.Add(gameObject);
        GameObject tempObj = gameObject;
        while (tempObj.transform.childCount > 0)
        {
            tempObj = tempObj.transform.GetChild(0).gameObject;
            sectionsList.Add(tempObj);
            linesList.Add(tempObj.GetComponent<LineRenderer>());
            tempObj.AddComponent<ObjHammerMoveableSection>();
        }
        hammerSection = sectionsList[sectionsList.Count - 1];
        hammerSection.AddComponent<ObjHammerBit>();

        savedPos = new List<Vector2>();
    }
    private void Start()
    {
        gameManager = FindObjectsByType<GameManager>(FindObjectsSortMode.None)[0];
        gameManager.GameReset.AddListener(OnSetup);
        gameManager.GameStart.AddListener(OnAction);      
    }
    private void Update()
    {
        for (int i = 1; i < sectionsList.Count; i++)
        {
            linesList[i-1].SetPosition(0, sectionsList[i-1].transform.position);
            linesList[i-1].SetPosition(1, sectionsList[i].transform.position);
        }
        hammerSection.transform.rotation =
            Quaternion.AngleAxis(
                Vector2.SignedAngle(Vector2.right, sectionsList[sectionsList.Count - 2].transform.position - hammerSection.transform.position),
                Vector3.forward);
    }

    public void RecordPos()
    {
        foreach (GameObject section in sectionsList)
        {
            savedPos.Add(section.transform.position);
        }
    }
    public void LoadPos()
    {
        if (savedPos == null) return;

        for (int i = 0; i < savedPos.Count; i++)
        {
            sectionsList[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            sectionsList[i].GetComponent<Rigidbody2D>().angularVelocity = 0;
            sectionsList[i].transform.position = savedPos[i];
        }

        //requires manual syncing via SyncTransforms or autoSyncTransforms = true
        Physics2D.SyncTransforms();

        ClearPos();
    }
    void ClearPos()
    {
        savedPos.Clear();
    }

    //on game manager game phase
    void OnSetup()
    {
        LoadPos();
        trail.enabled = false;
        trail.Clear();
    }
    void OnAction()
    {
        RecordPos();
        trail.enabled = true;
    }
}
