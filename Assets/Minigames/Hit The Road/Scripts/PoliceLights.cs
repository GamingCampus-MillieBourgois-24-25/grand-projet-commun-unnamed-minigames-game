using UnityEngine;

public class PoliceLights : MonoBehaviour
{
    public Light redLight; // R�f�rence au projecteur rouge
    public Light blueLight; // R�f�rence au projecteur bleu
    public float flashSpeed = 0.5f; // Vitesse de clignotement (en secondes)

    private bool isRedActive = true; // Indique si la lumi�re rouge est active

    void Start()
    {
        if (redLight == null || blueLight == null)
        {
            Debug.LogError("Les lumi�res rouge et/ou bleue ne sont pas assign�es !");
            return;
        }

        // Initialiser les lumi�res
        redLight.enabled = true;
        blueLight.enabled = false;

        // Lancer l'effet de clignotement
        InvokeRepeating(nameof(SwitchLights), flashSpeed, flashSpeed);
    }

    private void SwitchLights()
    {
        // Alterner entre la lumi�re rouge et la lumi�re bleue
        isRedActive = !isRedActive;
        redLight.enabled = isRedActive;
        blueLight.enabled = !isRedActive;
    }

    public void StopLights()
    {
        // Arr�ter l'effet de clignotement
        CancelInvoke(nameof(SwitchLights));
        redLight.enabled = false;
        blueLight.enabled = false;
    }

    public void StartLights()
    {
        // Red�marrer l'effet de clignotement
        redLight.enabled = true;
        blueLight.enabled = false;
        isRedActive = true;
        InvokeRepeating(nameof(SwitchLights), flashSpeed, flashSpeed);
    }
}
