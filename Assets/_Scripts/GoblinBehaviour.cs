using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoblinBehaviour : MonoBehaviour
{
    private const string labelText = "{0}: {1}";
    [SerializeField] Animator goblinAnimator;

    [SerializeField] AnimationCurve critical;
    [SerializeField] AnimationCurve hurt;
    [SerializeField] AnimationCurve healthy;

    [SerializeField] TextMeshProUGUI healthyLabel;
    [SerializeField] TextMeshProUGUI hurtLabel;
    [SerializeField] TextMeshProUGUI criticalLabel;
    
    [SerializeField] float currentHealth = 100;
    [SerializeField] Image healthBar;

    private float criticalValue = 0f;
    private float hurtValue = 0f;
    private float healthyValue = 0f;

    float cooldown = 2f;
    
    [SerializeField] GameObject natureSpellParticles;

    private void Update() {
        FuzzyLogic();

        if(Input.GetKeyDown(KeyCode.Space)) {
            currentHealth -= 10;
        }
    }

    public void FuzzyLogic()
    {
        healthyValue = healthy.Evaluate(currentHealth);
        hurtValue = hurt.Evaluate(currentHealth);
        criticalValue = critical.Evaluate(currentHealth);

        cooldown -= Time.deltaTime;

        if(criticalValue > healthyValue && cooldown <= 0) {
            goblinAnimator.SetTrigger("Cast");
            cooldown = 2f;
            StartCoroutine(AwaitThenShowSpellParticles());
        }

        SetLabels();
    }

    void SetLabels() {
        healthyLabel.text = string.Format(labelText, "Healthy", healthyValue);
        hurtLabel.text = string.Format(labelText, "Hurt", hurtValue); 
        criticalLabel.text = string.Format(labelText, "Critical", criticalValue);

        healthBar.fillAmount = currentHealth / 100;
    }

    IEnumerator AwaitThenShowSpellParticles() {
        yield return new WaitForSeconds(0.6f);
        natureSpellParticles.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        currentHealth = 100;

        yield return new WaitForSeconds(1.5f);
        natureSpellParticles.SetActive(false);
    }
}
