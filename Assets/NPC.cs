
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

    [Header("Optional Crow Quest")]
    public bool isCrowQuestNPC;

    [TextArea(2, 4)]
    public string[] noCoinsDialogue;
    [TextArea(2, 4)]
    public string[] someCoinsDialogue;
    [TextArea(2, 4)]
    public string[] rewardDialogue;
    [TextArea(2, 4)]
    public string[] alreadyRewardedDialogue;

    [Header("Optional Cat Clover Quest")]
    public bool isCatQuestNPC;

    [TextArea(2, 4)]
    public string[] catNoCloverDialogue;
    [TextArea(2, 4)]
    public string[] catTradeDialogue;
    [TextArea(2, 4)]
    public string[] catCompletedDialogue;

    public string cloverItemID = "clover";

    [Header("Optional Bus Ticket Event")]
    public bool isBusTicketNPC;

    [TextArea(2, 4)]
    public string[] busTicketMissingDialogue;

    [TextArea(2, 4)]
    public string[] busTicketReturnDialogue;

    [TextArea(2, 4)]
    public string[] busTicketCompletedDialogue;

    public string busTicketItemID = "BusTicket";

    private bool busTicketEventCompleted;
    private bool busTicketRewardPending;

    [Header("Key Reward (Legacy Settings)")]
    public string keyItemID = "CrowKey";
    public string keyItemName = "Crow's Key";
    public Sprite keyItemIcon;

    private int dialogueindex;
    private bool isTyping;
    private bool isDialogueActive;
    private bool rewardPending;
    private bool catRewardPending;

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
        catRewardPending = false;
        busTicketRewardPending = false;

        if (isCrowQuestNPC)
        {
            QuestController quest = QuestController.Instance;

            if (quest == null || quest.coinCounter == null)
            {
                Debug.LogError(
                    "Crow quest: QuestController or Coin Counter is missing!"
                );
                return;
            }

            if (quest.HasCrowKeyReward())
                activeDialogueLines = alreadyRewardedDialogue;
            else if (quest.coinCounter.coins >= 3)
            {
                activeDialogueLines = rewardDialogue;
                rewardPending = true;
            }
            else if (quest.coinCounter.coins > 0 &&
                     someCoinsDialogue != null &&
                     someCoinsDialogue.Length > 0)
                activeDialogueLines = someCoinsDialogue;
            else
                activeDialogueLines = noCoinsDialogue;
        }

        if (isCatQuestNPC)
        {
            QuestController quest = QuestController.Instance;

            if (quest == null || quest.inventory == null)
            {
                Debug.LogError(
                    "Cat quest: QuestController or Inventory is missing!"
                );
                return;
            }

            if (quest.catCloverQuestCompleted)
                activeDialogueLines = catCompletedDialogue;
            else if (quest.inventory.HasItem(cloverItemID))
            {
                activeDialogueLines = catTradeDialogue;
                catRewardPending = true;
            }
            else
                activeDialogueLines = catNoCloverDialogue;
        }

        if (isBusTicketNPC)
        {
            QuestController quest = QuestController.Instance;

            if (busTicketEventCompleted)
            {
                activeDialogueLines = busTicketCompletedDialogue;
            }
            else if (quest == null || quest.inventory == null)
            {
                Debug.LogError(
                    "Bus ticket event: QuestController or Inventory is missing!"
                );
                return;
            }
            else if (quest.inventory.HasItem(busTicketItemID))
            {
                activeDialogueLines = busTicketReturnDialogue;
                busTicketRewardPending = true;
            }
            else
            {
                activeDialogueLines = busTicketMissingDialogue;
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
            yield return new WaitForSeconds(
                dialogueData.autoProgressDelay
            );
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();

        isTyping = false;
        isDialogueActive = false;

        QuestController quest = QuestController.Instance;

        if (busTicketRewardPending)
        {
            if (quest != null &&
                quest.inventory != null &&
                quest.coinCounter != null &&
                quest.inventory.RemoveItem(busTicketItemID))
            {
                KujoAudio.Instance.Bark();
                quest.coinCounter.AddCoin();
                busTicketEventCompleted = true;

                Debug.Log(
                    "Bus ticket returned! Kujo received one coin."
                );
            }
            else
            {
                Debug.LogWarning(
                    "Bus ticket trade failed. Check the inventory, " +
                    "coin counter, and ticket ID."
                );
            }

            busTicketRewardPending = false;
        }

        if (catRewardPending)
        {
            if (quest != null &&
                quest.inventory != null &&
                quest.coinCounter != null)
            {
                if (!quest.catCloverQuestCompleted &&
                    quest.inventory.RemoveItem(cloverItemID))
                {
                    quest.coinCounter.AddCoin();
                    quest.catCloverQuestCompleted = true;
                    KujoAudio.Instance.Bark();
                }
                else
                {
                    Debug.LogWarning("Cat Clover trade failed.");
                }
            }

            catRewardPending = false;
        }

        if (rewardPending)
        {
            if (quest != null && !quest.GiveCrowKey())
            {
                Debug.LogWarning(
                    "The Golden Key could not be added."
                );
            }

            rewardPending = false;
        }

        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        PauseController.SetPause(false);
    }

    public void StartBusTicketEvent()
    {
        if (!isBusTicketNPC || isDialogueActive ||
            busTicketEventCompleted)
            return;

        Interact();
    }
}
