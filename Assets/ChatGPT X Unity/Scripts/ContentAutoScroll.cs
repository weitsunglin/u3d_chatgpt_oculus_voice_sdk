using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentAutoScroll : MonoBehaviour
{

    private RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        //訂閱onMessageReceived的事件，觸發調用DelayScrollDown
        DiscussionManager.onMessageReceived += DelayScrollDown;
    }

    private void onDestroy()
    {
        //取消訂閱onMessageReceived，調用DelayScrollDown函数。
        DiscussionManager.onMessageReceived -= DelayScrollDown;
    }

    private void DelayScrollDown()
    {
        Invoke("ScrollDown", 3f);
    }

    private void ScrollDown()
    {
        //这段代码的作用是确保UI元素在垂直方向上始终位于一个指定范围内，不会超出上限，并且不会低于下限
        Vector2 anchoredPosition = rt.anchoredPosition;
        anchoredPosition.y = Mathf.Max( 0, rt.sizeDelta.y );
        rt.anchoredPosition = anchoredPosition;
    }
}