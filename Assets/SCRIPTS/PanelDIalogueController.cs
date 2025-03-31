using System.Collections;
using UnityEngine;
using TMPro;

public class PanelDialogueController : MonoBehaviour
{
    public TMP_Text dialogueText;
    public TMP_Text continueText;
    public GameObject currentPanel;
    public GameObject nextPanel;
    public float typewriterSpeed = 0.05f;

    [TextArea(3, 10)]
    public string[] sentences;

    private int currentSentenceIndex = 0;
    private bool isTyping = false;
    private bool readyToContinue = false;

    void Start()
    {
        continueText.gameObject.SetActive(false); // Hide "Click to Continue" initially
        DisableTextGlow();
        StartCoroutine(TypeSentence(sentences[currentSentenceIndex]));
    }

    void Update()
    {
        // User input only works when readyToContinue is true
        if (readyToContinue && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
        {
            NextSentenceOrPanel();
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        continueText.gameObject.SetActive(false); // Hide flashing text while typing

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typewriterSpeed);
        }

        isTyping = false;
        readyToContinue = true;

        // Show the flashing "Click to Continue" text
        StartCoroutine(FlashContinueText());
    }

    IEnumerator FlashContinueText()
    {
        while (readyToContinue)
        {
            continueText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            continueText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    void NextSentenceOrPanel()
    {
        if (currentSentenceIndex < sentences.Length - 1)
        {
            // Move to the next sentence
            currentSentenceIndex++;
            readyToContinue = false;
            StopCoroutine(FlashContinueText());
            continueText.gameObject.SetActive(false);
            StartCoroutine(TypeSentence(sentences[currentSentenceIndex]));
        }
        else
        {
            // All sentences finished -> Hide current panel and show next panel
            currentPanel.SetActive(false);
            if (nextPanel != null)
            {
                nextPanel.SetActive(true);
            }
        }
    }

    void DisableTextGlow()
    {
        Material material = dialogueText.fontSharedMaterial;

        if (material != null)
        {
            // Reset the glow power and color
            material.SetFloat("_GlowPower", 0f);
            material.SetColor("_GlowColor", Color.black);
        }
    }
}
