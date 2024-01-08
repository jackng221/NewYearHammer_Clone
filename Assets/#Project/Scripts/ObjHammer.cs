using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjHammer : MonoBehaviour
{
    public List<GameObject> sectionsList;
    public List<LineRenderer> linesList;

    GameObject hammerSection;

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
        }
        hammerSection = sectionsList[sectionsList.Count - 1];
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
}
