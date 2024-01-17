using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SkillNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] GameObject[] nextSkills;
    [SerializeField] GameObject[] unacquiredSkillParticles;
    [SerializeField] InformationSO informationFieldSO;
    [SerializeField] TextMeshProUGUI skillNameText;
    [SerializeField] TextMeshProUGUI skillDescriptionText;
    [Space(5)]
    [SerializeField] GameObject AcquiredSkill;
    [SerializeField] GameObject UnacquiredSkill;
    [SerializeField] bool dontChangeNameOnValidate;

    SkillManager skillManager;
    Image img;
    bool check = false;

    void Awake()
    {
        img = GetComponent<Image>();
        skillManager = GetComponentInParent<SkillManager>();
        skillManager.SetImageInformationField(true);

        skillNameText = GameObject.Find("SkillName").GetComponent<TextMeshProUGUI>();
        skillDescriptionText = GameObject.Find("SkillStats").GetComponent<TextMeshProUGUI>();

        if (informationFieldSO == null)
        {
            Debug.LogError("Skill Tree: Skill Node InformationField is empty");
        }
    }

    void OnValidate()
    {
        if (!dontChangeNameOnValidate)
            gameObject.name = informationFieldSO.skillName;
    }

    void Start()
    {
        skillManager.SetImageInformationField(false);
    }

    public void PointerClick()
    {
        if (check == true) return;
        if (informationFieldSO.cost > PointSystem.Instance.Points) return;
        PointSystem.Instance.CalculatePoints(informationFieldSO.cost);

        check = true;

        skillManager.SkillUpdate(informationFieldSO);

        img.color = new Color32(255, 255, 255, 255);
        UnacquiredSkill.SetActive(false);
        AcquiredSkill.SetActive(true);

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

        if (unacquiredSkillParticles != null)
        {
            foreach (var particle in unacquiredSkillParticles)
            {
                particle.SetActive(true);
            }
        }
    }

    public void PointerEnter()
    {
        skillManager.SetImageInformationField(true);

        skillNameText.text = informationFieldSO.skillName;
        skillDescriptionText.text = informationFieldSO.skillDescription.Replace(",", "\n");
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