using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTelegraph : MonoBehaviour
{
    public Transform target;
    private Vector3 screenPoint;
    private RectTransform horizontal;
    private RectTransform vertical;
    private RectTransform center;
    private Camera camera;

    

    void Awake()
    {
        horizontal = transform.Find("Horizontal").GetComponent<RectTransform>();
        vertical = transform.Find("Vertical").GetComponent<RectTransform>();
        center = transform.Find("Center").GetComponent<RectTransform>();
        camera = Camera.main;

        transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);

        RectTransform rectTransform = GetComponent<RectTransform>();

        rectTransform.localPosition = new Vector3(0f, 0f, 0f);

        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);
        

        rectTransform.anchorMin = new Vector2(0f, 0f);
        rectTransform.localScale = new Vector3(1f, 1f, 1f);

        float ratio = camera.pixelHeight / 1080f;
        horizontal.localScale = new Vector3(1f, .25f * ratio, 1f);
        vertical.localScale = new Vector3(.25f * ratio, 1f , 1f);
        center.localScale = new Vector3(.5f * ratio, .5f * ratio, 1f);


    }
    void Update()
    {
        float bounds = 32f;
        screenPoint = camera.WorldToScreenPoint(target.transform.position);
        screenPoint.y = Mathf.Clamp(screenPoint.y*(screenPoint.z>0f?1:-1), 0f + bounds, camera.pixelHeight - bounds);
        screenPoint.x = Mathf.Clamp(screenPoint.x * (screenPoint.z > 0f ? 1 : -1), 0f + bounds, camera.pixelWidth - bounds);
        horizontal.anchoredPosition = new Vector3(0f,screenPoint.y,0f);
        vertical.anchoredPosition = new Vector3(screenPoint.x,0f, 0f);
        center.anchoredPosition = new Vector3(screenPoint.x, screenPoint.y, 0f);
    }
    
}
