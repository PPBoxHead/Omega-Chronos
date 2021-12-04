using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    // añadir a la cámara principal el elemento Cinemachine Impulse Source, poner el Raw Signal en 6D Shake
    // hay que testear esto

    public static ScreenShake Instance { get; private set; }
    CinemachineImpulseSource impulseCam;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        impulseCam = GameObject.FindGameObjectWithTag("Camera").GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    public void ShakeCamera(float SustainTime, float DecayTime, float intensity) //estos valores se pueden incrementar, referenciando en el void qué propiedades son.
    {
        impulseCam.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = SustainTime;
        impulseCam.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = DecayTime;
        impulseCam.m_ImpulseDefinition.m_AmplitudeGain = intensity;
        impulseCam.GenerateImpulse();
    }
}

// en caso de querer llamar esto desde código: ScreenShake.Instance.ShakeCamera(valores floats)
// para más información, ver el siguiente video a partir de 4:19. https://www.youtube.com/watch?v=n1N3PSOVxM0
