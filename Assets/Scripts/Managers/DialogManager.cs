using UnityEngine;
using System.Collections;
using UnityEditor.Search;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    [SerializeField] private DialogueConfig dialogueConfig;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Animator animator;
    [SerializeField] private float textSpeed = .1f;

    private Dictionary<string, DialogueConfig.DialogueData> dialogueDictionary;
    private Queue<string> sentences = new Queue<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        //ʹ��Dictionary����Ի����ݣ�����SoundManagerд��
        dialogueDictionary = new Dictionary<string, DialogueConfig.DialogueData>();

        foreach (var dialog in dialogueConfig.dialogs)
        {
            dialogueDictionary[dialog.dialogueID] = dialog;
        }

    }

    //�����ű��ɵ��øú�������������Ի���ID����ʾ�Ի�
    public void StartDialogue(string _dialogueID, float _waitSecondsBetweenSentences)
    {
        if (!dialogueDictionary.TryGetValue(_dialogueID, out DialogueConfig.DialogueData dialogue))
        {
            Debug.LogWarning($"Dialogue Data {_dialogueID} not found!");
            return;
        }

        sentences.Clear();
        //Ϊ�Ի���ı��⸳ֵ
        Instance.titleText.text = dialogue.title;
        animator.SetBool("isOpen", true);
        //ʹ�ö���(FIFO)����Ի�
        foreach (string sentence in dialogue.content)
        {
            sentences.Enqueue(sentence);
        }
        StartCoroutine(DisplayNextSentence(_waitSecondsBetweenSentences));

    }

    public IEnumerator DisplayNextSentence(float _waitSecondsBetweenSentences)
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            yield break;
        }
        //������������0ʱ��������ʾ�������ĶԻ�
        while(sentences.Count > 0)
        {
            yield return new WaitForSeconds(1f);
            string sentence = sentences.Dequeue();

            //�����ʾsentence�е��ַ�
            Instance.contentText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                Instance.contentText.text += letter;
                yield return new WaitForSeconds(textSpeed);
            }
            yield return new WaitForSeconds(_waitSecondsBetweenSentences);
        }
        //������Ϊ��ʱ���رնԻ���
        EndDialogue();
       
    }

    private void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }

    private void Start()
    {

    }


}
