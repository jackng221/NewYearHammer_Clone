using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjHammerMoveableSection : MonoBehaviour
{
    GameManager gameManager;
    Vector3 tempPoint;

    private void Start()
    {
        gameManager = FindObjectsByType<GameManager>(FindObjectsSortMode.None)[0];
    }

    private void OnMouseDrag()
    {
        if (gameManager.selectedSection == null)
        {
            gameManager.selectedSection = gameObject;
        }

        if (gameManager.GamePhase == GameManager.Phases.Setup && gameManager.selectedSection == gameObject)
        {
            tempPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tempPoint.z = 0;
            transform.position = tempPoint;
            //requires manual syncing via SyncTransforms or autoSyncTransforms = true
            Physics2D.SyncTransforms();
        }
    }
    private void OnMouseUp()
    {
        gameManager.selectedSection = null;
    }
}
