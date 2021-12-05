using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }
    CinemachineImpulseSource impulseCam;
    private void Awake()
    {
        Instance = this;
        impulseCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineImpulseSource>();
    }

    public void ShakeCamera(float SustainTime, float DecayTime, float intensity) //estos valores se pueden incrementar, referenciando en el void qué propiedades son.
    {
        impulseCam.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = SustainTime;
        impulseCam.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = DecayTime;
        impulseCam.m_ImpulseDefinition.m_AmplitudeGain = intensity;
        impulseCam.GenerateImpulse();
    }
}
