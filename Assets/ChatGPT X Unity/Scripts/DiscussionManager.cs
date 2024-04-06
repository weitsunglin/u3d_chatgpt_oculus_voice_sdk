using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;
using OpenAI;
using OpenAI.Chat;
using System.Xml;
using System.IO;

public class DiscussionManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private DiscussionBubble bubblePrefab;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Transform bubblesParent;

    [Header("Events")]
    public static Action onMessageReceived;
    public static Action<string> onChatGPTMessageReceived;

    [Header("Authentication")]
    [SerializeField] private string apiKey;
    [SerializeField] private string organizationId;

    [Header("Settings")]
    [SerializeField] private List<ChatPrompt> chatPrompts = new List<ChatPrompt>();

    private OpenAIClient api;
    private string value;

    void Start()
    {
        ReadXML();
        CreateBubble("Hey There! How can I help you?", false);
        Authenticate();
        Initialize();
    }

    private void Authenticate()
    {
        api = new OpenAIClient(new OpenAIAuthentication(apiKey, organizationId));
    }

    private void ReadXML()
    {
        string filePath = Path.Combine(Application.dataPath, "string.xml");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filePath);
        XmlElement root = xmlDoc.DocumentElement;
        string keyToRead = "GameIntroduction";
        XmlNodeList settings = root.SelectNodes($"Setting[@key='{keyToRead}']");
        if (settings.Count > 0)
        {
            value = settings[0].Attributes["value"].Value;
        }
    }

    private void Initialize()
    {
        Debug.Log(value);
        ChatPrompt prompt = new ChatPrompt("system", value);
        chatPrompts.Add(prompt);
    }

    public async void AskButtonCallBack()
    {
        CreateBubble(inputField.text, true);
        ChatPrompt prompt = new ChatPrompt("user", inputField.text);
        chatPrompts.Add(prompt);
        inputField.text = "";

        ChatRequest request = new ChatRequest(
            messages: chatPrompts,
            model: OpenAI.Models.Model.GPT3_5_Turbo,
            temperature: 1);

        try
        {
            var result = await api.ChatEndpoint.GetCompletionAsync(request);
            ChatPrompt chatResult = new ChatPrompt("system", result.FirstChoice.ToString());
            chatPrompts.Add(chatResult);
            onChatGPTMessageReceived?.Invoke(result.FirstChoice.ToString());
            CreateBubble(result.FirstChoice.ToString(), false);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void CreateBubble(string message, bool isUserMessage)
    {
        DiscussionBubble discussionBubble = Instantiate(bubblePrefab, bubblesParent);
        discussionBubble.Configure(message, isUserMessage);
        Debug.Log(message);
        onMessageReceived?.Invoke();
    }
}
