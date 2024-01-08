using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SkillNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] GameObject[] nextSkills;
    [SerializeField] InformationSO informationField;
    [SerializeField] TextMeshProUGUI skillNameText;
    [SerializeField] TextMeshProUGUI skillDescriptionText;

    SkillManager skillManager;
    Button btn;
    Image img;

    void Awake()
    {
        btn = GetComponent<Button>();
        img = GetComponent<Image>();
        skillManager = GetComponentInParent<SkillManager>();
        skillManager.SetImageInformationField(true);

        skillNameText = GameObject.Find("SkillName").GetComponent<TextMeshProUGUI>();
        skillDescriptionText = GameObject.Find("SkillStats").GetComponent<TextMeshProUGUI>();

        btn.onClick.AddListener(Selected);

        if (informationField == null)
        {
            Debug.LogError("Skill Tree: Skill Node InformationField is empty");
        }
    }

    void Start()
    {
        skillManager.SetImageInformationField(false);
    }

    public void Selected()
    {
        img.color = new Color32(255, 255, 255, 255);

        foreach (var nextSkill in nextSkills)
        {
            Image nextSkillImage = nextSkill.GetComponent<Image>();
            if (nextSkillImage != null)
            {
                nextSkillImage.color = new Color32(255, 255, 255, 40);
            }

            SkillNode nextSkillNode = nextSkill.GetComponent<SkillNode>();
            if (nextSkillNode != null)
            {
                nextSkillNode.enabled = true;
            }

            Button nextSkillButton = nextSkillNode.GetComponent<Button>();
            if (nextSkillButton != null)
            {
                nextSkillButton.enabled = true;
            }
        }
    }

    public void PointerClick()
    {
        skillManager.SkillUpdate(informationField);
    }

    public void PointerEnter()
    {
        skillManager.SetImageInformationField(true);

        skillNameText.text = informationField.skillName;
        skillDescriptionText.text = informationField.skillDescription.Replace(",", "\n");
    }

    public void PointerExit()
    {
        skillManager.SetImageInformationField(false);
        skillNameText.text = null;
        skillDescriptionText.text = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PointerClick();
    }


}