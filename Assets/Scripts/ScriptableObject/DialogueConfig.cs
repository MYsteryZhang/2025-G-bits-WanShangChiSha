using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueConfig", menuName = "Dialogue/New Dialogue")]
public class DialogueConfig : ScriptableObject
{
    [System.Serializable]
    public class DialogueData
    {
        public string dialogueID;       // Ψһ��ʶ���� "Tutorial_1"��
        public string title;            // �Ի������
        [TextArea(3, 10)]
        public string[] content;          // ��������
        public Sprite icon;             // ͼ�꣨��ѡ��
        public bool hasConfirmButton = true; // �Ƿ���Ҫȷ�ϰ�ť
    }

    public List<DialogueData> dialogs = new List<DialogueData>();
   
}