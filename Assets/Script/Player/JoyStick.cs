using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;
    // 추가
    [SerializeField, Range(10f, 150f)]
    private float leverRange;

    private Vector2 inputVector;    // 추가
    private bool isInput;    // 추가

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (!GameManager.instance.joyStickUse)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var inputDir = eventData.position - rectTransform.anchoredPosition;
        //추가
        var clampedDir = inputDir.magnitude < leverRange ?
            inputDir : inputDir.normalized * leverRange;

        // lever.anchoredPosition = inputDir;
        lever.anchoredPosition = clampedDir;    // 변경

        ControlJoystickLever(eventData);  // 추가
        isInput = true;    // 추가
    }

    // 오브젝트를 클릭해서 드래그 하는 도중에 들어오는 이벤트
    // 하지만 클릭을 유지한 상태로 마우스를 멈추면 이벤트가 들어오지 않음    
    public void OnDrag(PointerEventData eventData)
    {
        var inputDir = eventData.position - rectTransform.anchoredPosition;
        //Debug.Log(inputDir);
        // 추가
        var clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;

        // lever.anchoredPosition = inputDir;
        lever.anchoredPosition = clampedDir;    // 변경

        ControlJoystickLever(eventData);    // 추가
        isInput = false;    // 추가
    }

    public void ControlJoystickLever(PointerEventData eventData)
    {
        var inputDir = eventData.position - rectTransform.anchoredPosition;
        var clampedDir = inputDir.magnitude < leverRange ? inputDir
            : inputDir.normalized * leverRange;
        lever.anchoredPosition = clampedDir;
        inputVector = clampedDir / leverRange;

        Debug.Log(inputVector);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }

    public Vector2 InputVec()
    {
        return new Vector2(inputVector.x + 1, inputVector.y + 1) * 0.5f;
    }
}
