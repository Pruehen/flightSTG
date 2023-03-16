using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingWdw : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider bgmSlider;//배경음량
    public Slider soundSlider;//효과음량
    public Slider controlSencSlider;//조종감도
    public Toggle autoAimToggle;//자동조준
    public Toggle joyStickUse;//조이스틱사용여부
    public GameObject joyStick;

    public void BgmSet()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(bgmSlider.value) * 20);
        GameManager.instance.BgmSet(bgmSlider.value);
    }
    public void SoundSet()
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(soundSlider.value) * 20);
        GameManager.instance.SoundSet(soundSlider.value);
    }
    public void ControlSencSet()
    {
        GameManager.instance.ControlSencitivitySet(controlSencSlider.value);        
    }
    public void AutoAimSet()
    {
        GameManager.instance.AutoAimSet(autoAimToggle.isOn);
    }
    public void JoyStickUseSet()
    {
        GameManager.instance.JoyStickUseSet(joyStickUse.isOn);
        if (joyStick != null)
        {
            joyStick.SetActive(joyStickUse.isOn);
            if (joyStickUse.isOn)
            {
                PlayerControll.instance.ControlSystemSet(PlayerControll.ControlSystem.joyPad);
            }
            else
            {
                PlayerControll.instance.ControlSystemSet(PlayerControll.ControlSystem.gyro);
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(GameManager.instance.bgmVolume) * 20);
        audioMixer.SetFloat("SFX", Mathf.Log10(GameManager.instance.soundVolume) * 20);
        bgmSlider.value = GameManager.instance.bgmVolume;
        soundSlider.value = GameManager.instance.soundVolume;
        controlSencSlider.value = GameManager.instance.controlSencitivity;
        autoAimToggle.isOn = GameManager.instance.autoAim;
        joyStickUse.isOn = GameManager.instance.joyStickUse;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
