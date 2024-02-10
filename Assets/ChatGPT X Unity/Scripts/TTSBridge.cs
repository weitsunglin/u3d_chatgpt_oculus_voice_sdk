using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi.TTS.Utilities;

public class TTSBridge : MonoBehaviour
{
    // 文字 >> 語音


    [ Header ( "Elements" ) ]
    [ SerializeField ] private TTSSpeaker speaker; 


    void Start()
    {
        DiscussionBubble.onVoiceButtonClicked += Speak;
    }

    private void onDestroy()
    {
        DiscussionBubble.onVoiceButtonClicked -= Speak;
    }

    private void VoiceButtonClickedCallback( string message )
    {
        if( speaker.IsSpeaking )
        {
            Debug.Log( "Stopping the Speaker" );
            speaker.Stop();
        }
        else
        {
            Debug.Log( "Started Speaking" );
            Speak( message );
        }
    }

    private void Speak( string message )
    {
        string[] messages =  message.Split( '.' );
        speaker.StartCoroutine( speaker.SpeakQueuedAsync( messages ) );
    }

}