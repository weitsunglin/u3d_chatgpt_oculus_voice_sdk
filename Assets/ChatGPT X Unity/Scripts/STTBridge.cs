using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi.Json;
using Meta.WitAi;
using Oculus.Voice;
using TMPro;

public class STTBridge : MonoBehaviour
{
    [Header("Default States"), Multiline]
    [SerializeField] 
    private string freshStateText = "Try pressing the Activate button and saying \"Make the cube red\"";

    [Header("UI")]
    [SerializeField] 
    private TMP_InputField promptInputField;

    [Header("Voice")]
    [SerializeField] 
    private AppVoiceExperience appVoiceExperience;

    public bool IsActive => _active;
    private bool _active = false;

    private void OnEnable()
    {
        appVoiceExperience.events.OnRequestCreated.AddListener(OnRequestStarted);
        appVoiceExperience.events.OnPartialTranscription.AddListener(OnRequestTranscript);
        appVoiceExperience.events.OnFullTranscription.AddListener(OnRequestTranscript);
        appVoiceExperience.events.OnStartListening.AddListener(OnListenStart);
        appVoiceExperience.events.OnStoppedListening.AddListener(OnListenStop);
        appVoiceExperience.events.OnStoppedListeningDueToDeactivation.AddListener(OnListenForcedStop);
        appVoiceExperience.events.OnStoppedListeningDueToInactivity.AddListener(OnListenForcedStop);
        appVoiceExperience.events.OnResponse.AddListener(OnRequestResponse);
        appVoiceExperience.events.OnError.AddListener(OnRequestError);
    }

    private void OnDisable()
    {
        RemoveEventListeners();
    }

    private void RemoveEventListeners()
    {
        appVoiceExperience.events.OnRequestCreated.RemoveListener(OnRequestStarted);
        appVoiceExperience.events.OnPartialTranscription.RemoveListener(OnRequestTranscript);
        appVoiceExperience.events.OnFullTranscription.RemoveListener(OnRequestTranscript);
        appVoiceExperience.events.OnStartListening.RemoveListener(OnListenStart);
        appVoiceExperience.events.OnStoppedListening.RemoveListener(OnListenStop);
        appVoiceExperience.events.OnStoppedListeningDueToDeactivation.RemoveListener(OnListenForcedStop);
        appVoiceExperience.events.OnStoppedListeningDueToInactivity.RemoveListener(OnListenForcedStop);
        appVoiceExperience.events.OnResponse.RemoveListener(OnRequestResponse);
        appVoiceExperience.events.OnError.RemoveListener(OnRequestError);
    }

    private void OnRequestStarted(WitRequest r)
    {
        _active = true;
    }

    private void OnRequestTranscript(string transcript)
    {
        promptInputField.text = transcript;
    }

    private void OnListenStart() {}

    private void OnListenStop() {}

    private void OnListenForcedStop()
    {
        OnRequestComplete();
    }

    private void OnRequestResponse(WitResponseNode response)
    {
        if (!string.IsNullOrEmpty(response["text"]))
        {
            promptInputField.text = response["text"];
        }

        OnRequestComplete();
    }

    private void OnRequestError(string error, string message)
    {
        promptInputField.text = $"<color=\"red\">Error: {error}\n\n{message}</color>";
        OnRequestComplete();
    }

    private void OnRequestComplete()
    {
        _active = false;
    }

    public void ToggleActivation()
    {
        SetActivation(!_active);
    }

    public void SetActivation(bool toActivated)
    {
        if (_active != toActivated)
        {
            _active = toActivated;
            if (_active)
            {
                appVoiceExperience.Activate();
            }
            else
            {
                appVoiceExperience.Deactivate();
            }
        }
    }
}
