using MyBox;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardWindow : MonoBehaviour
{
    public static RewardWindow Instance;

    #region serialized fields
    [SerializeField] GameObject rewardWindow;
    [SerializeField] GameObject essentialUI;
    [SerializeField] Image rewardImage;
    [SerializeField] TextMeshProUGUI titelText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] float fadeTime = 2;
    [SerializeField] float currencyFadeTime = 2;
    [SerializeField] float currencyActiveAfterCalc = 2;
    [SerializeField] TextMeshProUGUI rewardText;
    [SerializeField] GameObject currencObject;
    [SerializeField] CanvasGroup rewardTextCanvasGroup;

    #endregion

    #region private fields
    CanvasGroup rewardWindowCanvasGroup;
    CanvasGroup essentialUICanvasGroup;
    Coroutine currentRewarWindowCoroutine;
    Coroutine showCurrencyCoroutine;
    #endregion

    void Awake()
    {
        Instance = this;
        rewardWindowCanvasGroup = rewardWindow.GetComponent<CanvasGroup>();
        essentialUICanvasGroup = essentialUI.GetComponent<CanvasGroup>();
    }

    [ButtonMethod]
    public void OpenRewardWindow()
    {
        if (currentRewarWindowCoroutine != null)
            StopCoroutine(currentRewarWindowCoroutine);

        currentRewarWindowCoroutine = StartCoroutine(ShowCoroutine());
    }

    public void GiveReward(GameObject reward)
    {
        if (!reward) return;
        Ability ability = reward.GetComponent<Ability>();
        rewardImage.sprite = ability.AbilitySO.abilitySprite;
        titelText.text = ability.AbilitySO.abilityTitel;
        descriptionText.text = ability.AbilitySO.abilityDescription;
        SoundManager.Instance.PlaySound(SoundType.SkillAcquired);

        PauseManager.Instance.PauseLogic(true);

        OpenRewardWindow();
    }

    [ButtonMethod]
    public void Close()
    {
        PauseManager.Instance.PauseLogic(false);

        if (currentRewarWindowCoroutine != null)
            StopCoroutine(currentRewarWindowCoroutine);

        currentRewarWindowCoroutine = StartCoroutine(HideCoroutine());
    }
    public void ShowCurrency(float current, float additional)
    {
        if (showCurrencyCoroutine != null)
            StopCoroutine(showCurrencyCoroutine);

        showCurrencyCoroutine = StartCoroutine(ShowCurrencyCoroutine(current, additional));
    }

    IEnumerator ShowCurrencyCoroutine(float current, float additional)
    {
        currencObject.SetActive(true);
        rewardTextCanvasGroup.alpha = 0;
        float time = 0;
        float beforeCalc = current - additional;
        bool valueNegative = additional < 0;
        string plusOrMinus = valueNegative ? " " : " + ";
        rewardText.text = beforeCalc + plusOrMinus + additional;

        while (time < fadeTime)
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            rewardTextCanvasGroup.alpha = Mathf.Clamp01(time / fadeTime);
        }
        rewardTextCanvasGroup.alpha = 1;

        time = 0;
        bool soundPlayed = false;
        float soundLength = SoundManager.Instance.GetSoundLength(SoundType.PointCounter);
        currencyFadeTime = soundLength != 0 ? soundLength : currencyFadeTime;

        while (time < currencyFadeTime)
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            float percentageProgressed = time / currencyFadeTime;
            rewardText.text = (beforeCalc + additional * percentageProgressed).RoundToInt() +
                plusOrMinus + (additional - (additional * percentageProgressed).RoundToInt());

            if (!soundPlayed)
            {
                SoundManager.Instance.PlaySound(SoundType.PointCounter);
                soundPlayed = true;
            }
        }
        rewardText.text = (current.RoundToInt()).ToString();

        yield return new WaitForSeconds(currencyActiveAfterCalc);

        time = 0;
        while (time < fadeTime)
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            rewardTextCanvasGroup.alpha = 1 - Mathf.Clamp01(time / fadeTime);
        }
        rewardTextCanvasGroup.alpha = 0;
    }

    IEnumerator ShowCoroutine()
    {
        essentialUI.SetActive(true);
        rewardWindow.SetActive(true);
        essentialUICanvasGroup.alpha = 1;
        rewardWindowCanvasGroup.alpha = 0;
        float time = 0;

        while (time < fadeTime)
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            essentialUICanvasGroup.alpha = 1 - Mathf.Clamp01(time / fadeTime);
            rewardWindowCanvasGroup.alpha = Mathf.Clamp01(time / fadeTime);
        }
        rewardWindowCanvasGroup.alpha = 1;
        essentialUICanvasGroup.alpha = 0;
    }

    IEnumerator HideCoroutine()
    {
        essentialUICanvasGroup.alpha = 0;
        essentialUI.SetActive(true);
        rewardWindowCanvasGroup.alpha = 0;
        rewardWindow.SetActive(false);
        float time = 0;

        while (time < fadeTime)
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            essentialUICanvasGroup.alpha = Mathf.Clamp01(time / fadeTime);
        }
        essentialUICanvasGroup.alpha = 1;
    }
}