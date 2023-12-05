using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using System;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    private bool _dialogueIsPlaying;
    public bool DialogueIsPlaying {
        get {
            return _dialogueIsPlaying;
        }
        set {
            _dialogueIsPlaying = value;
        }
    }

    private static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one dialogue manager in the scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        // Get all of the choiches text
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices) {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        // if (!DialogueIsPlaying)
        // {
        //     return;
        // }

        if (DialogueIsPlaying && InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueStory();
        }
    }

    // private void OnDisable() {
    //     dialoguePanel.transform.position = Vector3.zero;
    // }

    public void EnterDialogueMode(TextAsset inkJSON, Transform npcTransform)
    {
        // MovePanelToNPCPosition(npcTransform);

        currentStory = new Story(inkJSON.text);
        DialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    public void ExitDialogueMode()
    {
        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();

            // Display choices if any for this dialogue line
            DisplayChoices();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void MovePanelToNPCPosition(Transform npcTransform) {
        Vector3 npcPosition = Camera.main.WorldToScreenPoint(npcTransform.position);
        dialoguePanel.transform.position = npcPosition;
        // Debug.Log(npcPosition);
    }

    private void DisplayChoices() {
        List<Choice> currentChoices = currentStory.currentChoices;
        if (currentChoices.Count > choices.Length) {
            Debug.LogError("Too many choices for the UI support");
        }

        int index = 0;
        //enable and initialize the choices up to the amount of choices for this line of operation

        foreach (Choice choice in currentChoices) {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++) {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice() {
        // Unity event system need to be cleared, and then select it after at least one frame
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex) {
        currentStory.ChooseChoiceIndex(choiceIndex);
    }
}
