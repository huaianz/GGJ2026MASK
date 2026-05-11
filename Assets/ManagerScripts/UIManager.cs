using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

using TMPro;

public class UIManager: Singleton<UIManager>
{
    public GameObject NoteButton;//��ǩ��ť
    public GameObject NotePanel;//��ǩҳ��
    public GameObject TipPanel;//��ʾҳ��
    public Text TipText;//��ʾҳ���text���?

    [Header("ҳ������")]
    [Header("ҳ������")]
    public CanvasGroup[] pages; // ʹ��CanvasGroup������ʾ
    public float fadeDuration = 0.3f; // ���뵭��ʱ��

    [Header("��ť")]
    public Button nextButton;   // ��һҳ��ť
    public Button prevButton;   // ��һҳ��ť

    [Header("͸��������")]
    [Range(0, 1)] public float activeAlpha = 1.0f;     // ����ʱ��͸����
    [Range(0, 1)] public float inactiveAlpha = 0.5f;   // ������ʱ��͸����

    private int currentPage = 0; 
    private bool isTransitioning = false;
    private bool isOpen=true;

    [Header("��ʾ�ı�")]
    [SerializeField] private string[] hintTexts;

    [Header("��Ϸ˵������")]
    public GameObject DescriptionPanel;
    public GameObject StartPanel;
    public bool IsDescription=true;

    [Header("������Ϸ����")]
    public GameObject PassPanel;

    [Header("����ҳ������")]
    [SerializeField]public string[] memoryTexts;
    public GameObject memoryPanel;
    public Text memoryText;
    public Rigidbody2D player;

    [SerializeField] private bool autoHide = true;
    [SerializeField] private float autoHideSeconds = 2.0f;

    [Header("Level 2 UI Elements")]
    [SerializeField] private GameObject level2UIRoot;   // Level2UIRoot
    [SerializeField] private GameObject phoneUIRoot;    // PhoneUIRoot
    [SerializeField] private TMP_Text messageHeader;    // MessageHeader 上的 TMP
    [SerializeField] private TMP_Text messageText;      // MessageText 上的 TMP
    
    [Header("Level 2 Dialogue System")]
    [SerializeField] private PhoneDialogueRunner phoneDialogueRunner;    
    
    [Serializable]
    public class ReachDialogueBinding
    {
        public ReachLocationType type;
        public DialogueScript dialogue;
    }

    [SerializeField] private List<ReachDialogueBinding> reachDialogueBindings = new();
    private Dictionary<ReachLocationType, DialogueScript> reachDialogueMap;

    
    void Start()
    {
        // ��ʼ����ʾ��һҳ
        ShowPageImmediate(currentPage);

        // �󶨰�ť�¼�
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);

        // ���°�ť״̬
        UpdateButtonStates();

        // Hide Level 2 UI at start
        level2UIRoot.SetActive(false);

        // Build reach dialogue map
        reachDialogueMap = new Dictionary<ReachLocationType, DialogueScript>();
        foreach (var b in reachDialogueBindings)
        {
            if (b == null) continue;
            if (b.dialogue == null) continue;

            reachDialogueMap[b.type] = b.dialogue;
        }
    }

    private void OnEnable()
    {
        EventHandler.UpdateTipsUI += UpdateTipTexts;

        EventHandler.UpdateMemoryUI += UpdateMemoryTexts;
    }

    private void OnDisable()
    {
        EventHandler.UpdateTipsUI -= UpdateTipTexts;

        EventHandler.UpdateMemoryUI -= UpdateMemoryTexts;
    }

    // ��һҳ��������Ч����
    public void NextPage()
    {
        if (!isTransitioning && currentPage < pages.Length - 1)
        {
            StartCoroutine(TransitionToPage(currentPage + 1));
        }
    }

    // ��һҳ��������Ч����
    public void PrevPage()
    {
        if (!isTransitioning && currentPage > 0)
        {
            StartCoroutine(TransitionToPage(currentPage - 1));
        }
    }

    // ҳ������?��??
    private System.Collections.IEnumerator TransitionToPage(int newPage)
    {
        isTransitioning = true;

        // ������ǰҳ��
        yield return StartCoroutine(FadeCanvasGroup(pages[currentPage], 1, 0, fadeDuration));
        pages[currentPage].gameObject.SetActive(false);

        // ����ҳ��
        currentPage = newPage;

        // ������ҳ��
        pages[currentPage].gameObject.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(pages[currentPage], 0, 1, fadeDuration));

        // ���°�ť״̬
        UpdateButtonStates();
        isTransitioning = false;
    }

    // ������ʾҳ�棨�޹��ɣ�
    private void ShowPageImmediate(int pageIndex)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            bool isActive = (i == pageIndex);
            pages[i].gameObject.SetActive(isActive);
            pages[i].alpha = isActive ? 1 : 0;
            pages[i].interactable = isActive;
            pages[i].blocksRaycasts = isActive;
        }
    }

    // Э�̣����뵭��Ч��
    private System.Collections.IEnumerator FadeCanvasGroup(
        CanvasGroup group,
        float startAlpha,
        float endAlpha,
        float duration
    )
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }

        group.alpha = endAlpha;
    }

    // ���°�ť״̬
    private void UpdateButtonStates()
    {
        // ���ý���״̬
        bool canGoPrev = currentPage > 0;
        bool canGoNext = currentPage < pages.Length - 1;

        prevButton.interactable = canGoPrev;
        nextButton.interactable = canGoNext;

        // �ı䰴ť͸����
        SetButtonAlpha(prevButton, canGoPrev);
        SetButtonAlpha(nextButton, canGoNext);
    }

    // ���õ�����ť��͸����
    private void SetButtonAlpha(Button button, bool isActive)
    {
        // ��ȡ��ť��Image���?
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            Color newColor = buttonImage.color;
            newColor.a = isActive ? activeAlpha : inactiveAlpha;
            buttonImage.color = newColor;
        }

        // ��ѡ��ͬʱ�޸İ�ť�ϵ�����͸����
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            Color textColor = buttonText.color;
            textColor.a = isActive ? activeAlpha : inactiveAlpha;
            buttonText.color = textColor;
        }
    }
    public void ToggleNotes()
    {
        if (isOpen)
        {
            NotePanel.SetActive(true);
            isOpen=false;
        }
        else
        {
            NotePanel.SetActive(false);
            isOpen = true;
        }
    }

    //������ʾ�ı�
    public void UpdateTipTexts(int textIndex)
    {
        TipText.text= hintTexts[textIndex];
        TipPanel.SetActive(true);

        // ʹ��Э��2������?
        StartCoroutine(HideAfterDelay(2f));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        TipPanel.SetActive(false);
    }

    //˵�������ĵ��?
    public void ExplanationButton()
    {
        if (IsDescription)
        {
            DescriptionPanel.SetActive(true);
            IsDescription = false;
        }
        else
        {
            DescriptionPanel.SetActive(false);
            IsDescription = true;
        }
    }

    //�ÿ�ʼҳ��ر�?
    public void StartButton()
    {
        StartPanel.SetActive(false);
    }

    //�ر���Ϸ
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    //������Ϸ����Ĳ���?
    public void Passthelevel()
    {
        PassPanel.SetActive(true);
    }

    //���»����ı�
    public void UpdateMemoryTexts(int textIndex)
    {
        memoryText.text = memoryTexts[textIndex];
        memoryPanel.SetActive(true);

        
        StartCoroutine(pass(6f));
    }

    private IEnumerator pass(float delay)
    {
        yield return new WaitForSeconds(delay);
        memoryPanel.SetActive(false);
    }

    //��ɫֹͣ�ƶ�
    public void playerDontMovent()
    {
        player.bodyType=RigidbodyType2D.Kinematic;
    }


    public void ShowPhoneDialogue(ReachLocationType type)
    {
        level2UIRoot.SetActive(true);
        phoneUIRoot.SetActive(true);

        if (phoneDialogueRunner == null)
        {
            Debug.LogWarning("PhoneDialogueRunner is not assigned on UIManager.");
            return;
        }

        if (reachDialogueMap == null)
        {
            Debug.LogWarning("Reach dialogue map not initialized.");
            return;
        }

        if (!reachDialogueMap.TryGetValue(type, out var script) || script == null)
        {
            Debug.LogWarning($"No DialogueScript bound for ReachLocationType: {type}");
            return;
        }

        phoneDialogueRunner.StartDialogue(script);
    }

    public void PhoneCloseBtn()
    {
        phoneUIRoot.SetActive(false);
        Debug.Log("Close button clicked");
    }

}
