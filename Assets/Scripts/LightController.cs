using UnityEngine;

public class LightController : MonoBehaviour
{
    public float smoothSpeed= 1;
    private Light m_light;
    void Start()
    {
        m_light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PortControl.speed!=null)
        {
            int speed = int.Parse(PortControl.speed);
            m_light.intensity = Mathf.Lerp(m_light.intensity, speed, Time.deltaTime* smoothSpeed);
        }
    }
}
