#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             DragUI
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.gameObject.GetComponent<RectTransform>(), eventData.position, Camera.main, out position))
        {
			
			
            return;
        }
        transform.localPosition = position;
    }
}
