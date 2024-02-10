using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DiscussionBubble : MonoBehaviour
{

    [ Header( "Elements" ) ]
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Image bubbleImage;
    [SerializeField] private Sprite userBubbleSprite;

    [SerializeField] private GameObject voiceButton;


    [ Header( "Setting" ) ]
    [SerializeField] private Color userBubbleColor;

    
    [ Header( "Events" ) ]
    //委託类型，不返回值的方法，接受一個string。
    public static Action< string > onVoiceButtonClicked;

    //建構訊息泡泡
    public  void Configure( string message , bool isUserMessage )
    {
        if ( isUserMessage )
        {
            bubbleImage.sprite = userBubbleSprite;
            bubbleImage.color = userBubbleColor;
            messageText.color = Color.white;
            voiceButton.SetActive( false );
        }
        messageText.text = message;
        // 调用messageText.ForceMeshUpdate()方法会重新计算文本的顶点位置、UV坐标和三角形索引，以确保这些网格数据与最新的文本内容保持同步
        messageText.ForceMeshUpdate();
    }

    public void VoiceButtonCallback()
    {
        onVoiceButtonClicked?.Invoke( messageText.text );
    }

    public void CopyToClipBoardCallBack()
    {
        GUIUtility.systemCopyBuffer = messageText.text;
    }
}
