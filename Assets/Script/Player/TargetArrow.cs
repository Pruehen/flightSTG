using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetArrow : MonoBehaviour
{
    public GameObject targetObject;

    [Header("Arrow Transforms")]
    [SerializeField]
    Transform cameraAttachedTransform;
    [SerializeField]
    Transform arrowTransform;

    [Header("Arrow Properties")]
    [SerializeField]
    int lineWidth = 3;
    [SerializeField]
    Material lineMaterial;
    [SerializeField]
    Transform[] vertexTransforms;

    [Header("Text UI")]
    [SerializeField]
    Transform textTransform;
    [SerializeField]
    RectTransform textUITransform;


    bool drawLines = true;
    private Camera cam;
    RectTransform canvasRect;

    // Recursive search
    Canvas GetCanvas(Transform parentTransform)
    {
        if (parentTransform.GetComponent<Canvas>() != null)
        {
            return parentTransform.GetComponent<Canvas>();
        }
        else
        {
            return GetCanvas(parentTransform.parent);
        }
    }

    public void SetTarget(GameObject target)
    {
        targetObject = target;
    }

    public void SetArrowVisible(bool visible)
    {
        drawLines = visible;
    }


    // Draw Arrow
    void OnPostRender()
    {
        if (!drawLines || vertexTransforms == null || vertexTransforms.Length < 2)
            return;

        float nearClip = cam.nearClipPlane + 0.00001f;
        int end = vertexTransforms.Length - 1;
        float thisWidth = 1f / Screen.width * lineWidth * 0.5f;

        lineMaterial.SetPass(0);

        if (lineWidth == 1)
        {
            GL.Begin(GL.LINES);
            for (int i = 0; i < end; ++i)
            {
                Vector2 linePoint = cam.WorldToViewportPoint(vertexTransforms[i].position);
                Vector2 nextlinePoint = cam.WorldToViewportPoint(vertexTransforms[i + 1].position);

                GL.Vertex(cam.ViewportToWorldPoint(new Vector3(linePoint.x, linePoint.y, nearClip)));
                GL.Vertex(cam.ViewportToWorldPoint(new Vector3(nextlinePoint.x, nextlinePoint.y, nearClip)));
            }
        }
        else
        {
            GL.Begin(GL.QUADS);
            for (int i = 0; i < end; ++i)
            {
                Vector2 linePoint = cam.WorldToViewportPoint(vertexTransforms[i].position);
                Vector2 nextlinePoint = cam.WorldToViewportPoint(vertexTransforms[i + 1].position);

                Vector3 perpendicular = (new Vector3(nextlinePoint.y, linePoint.x, nearClip) -
                                     new Vector3(linePoint.y, nextlinePoint.x, nearClip)).normalized * thisWidth;
                Vector3 v1 = new Vector3(linePoint.x, linePoint.y, nearClip);
                Vector3 v2 = new Vector3(nextlinePoint.x, nextlinePoint.y, nearClip);
                GL.Vertex(cam.ViewportToWorldPoint(v1 - perpendicular));
                GL.Vertex(cam.ViewportToWorldPoint(v1 + perpendicular));
                GL.Vertex(cam.ViewportToWorldPoint(v2 + perpendicular));
                GL.Vertex(cam.ViewportToWorldPoint(v2 - perpendicular));
            }
        }
        GL.End();
    }


    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        Canvas canvas = GetCanvas(textUITransform.parent);
        if (canvas != null)
        {
            canvasRect = canvas.GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject == null)
            return;

        cameraAttachedTransform.LookAt(targetObject.transform);
        arrowTransform.eulerAngles = cameraAttachedTransform.localEulerAngles;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, textTransform.position);
        textUITransform.anchoredPosition = screenPoint - canvasRect.sizeDelta * 0.5f;
    }
}
