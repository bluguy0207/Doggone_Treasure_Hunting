using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("Dialogue Settings")]
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Image portraitImage;

    [Header("Optional Crow Quest Dialogue")]
    public bool isCrowQuestNPC;

    [TextArea(2, 4)]
    public string[] noCoinsDialogue;

    [TextArea(2, 4)]
    public string[] someCoinsDialogue;

    [TextArea(2, 4)]
    public string[] rewardDialogue;

    [TextArea(2, 4)]
    public string[] alreadyRewardedDialogue;

    [Header("Key Reward")]
    public string keyItemID = "CrowKey";
    public string keyItemName = "Crow's Key";
    public Sprite keyItemIcon;

    private int dialogueindex;
    private bool isTyping;
    private bool isDialogueActive;
    private bool rewardPending;

    private string[] activeDialogueLines;
    private AudioSource voiceAudioSource;

    private void Awake()
    {
        voiceAudioSource = GetComponent<AudioSource>();

        if (voiceAudioSource == null)
            voiceAudioSource = gameObject.AddComponent<AudioSource>();

        voiceAudioSource.playOnAwake = false;
        voiceAudioSource.loop = false;
    }

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null ||
            (PauseController.IsGamePaused && !isDialogueActive))
            return;

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        activeDialogueLines = dialogueData.dialogueLines;
        rewardPending = false;


        if (isCrowQuestNPC)
        {
            QuestController quest = QuestController.Instance;

            if (quest == null)
            {
                Debug.LogError("Could not find QuestController!");
                return;
            }

            CoinsCollected counter = quest.coinCounter;

            if (counter == null)
            {
                Debug.LogError(
                    "QuestController's Coin Counter is not assigned!"
                );
                return;
            }

            Debug.Log("Crow's counter object: " + counter.gameObject.name
                + " | Coins: " + counter.coins);

            if (quest.HasCrowKeyReward())
            {
                activeDialogueLines = alreadyRewardedDialogue;
            }
            else if (counter.coins >= 3)
            {
                activeDialogueLines = rewardDialogue;
                rewardPending = true;
            }
            // else if (counter.coins > 0)
            // {
            //     activeDialogueLines = someCoinsDialogue;
            // }
            else
            {
                activeDialogueLines = noCoinsDialogue;
            }
        }


        if (activeDialogueLines == null ||
            activeDialogueLines.Length == 0)
        {
            Debug.LogWarning("This NPC has no dialogue lines assigned.");
            return;
        }

        isDialogueActive = true;
        dialogueindex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);
        PauseController.SetPause(true);

        StartCoroutine(TypeLine());
    }

    private void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(activeDialogueLines[dialogueindex]);
            isTyping = false;
        }
        else if (++dialogueindex < activeDialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        string line = activeDialogueLines[dialogueindex];

        foreach (char letter in line)
        {
            dialogueText.text += letter;

            if (!char.IsWhiteSpace(letter) &&
                dialogueData.voiceSound != null)
            {
                voiceAudioSource.Stop();
                voiceAudioSource.pitch = dialogueData.voicePitch;
                voiceAudioSource.PlayOneShot(dialogueData.voiceSound);
            }

            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines != null &&
            dialogueData.autoProgressLines.Length > dialogueindex &&
            dialogueData.autoProgressLines[dialogueindex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();

        isTyping = false;
        isDialogueActive = false;

        if (rewardPending)
        {
            QuestController quest = QuestController.Instance;

            if (quest != null && !quest.GiveCrowKey())
            {
                Debug.LogWarning(
                    "The key could not be added. Check inventory space and key settings."
                );
            }

            rewardPending = false;
        }

        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        PauseController.SetPause(false);
    }
}