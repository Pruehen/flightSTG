using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingWdw : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider bgmSlider;//�������
    public Slider soundSlider;//ȿ������
    public Slider controlSencSlider;//��������
    public Toggle autoAimToggle;//�ڵ�����

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

    // Start is called before the first frame update
    void Start()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(GameManager.instance.bgmVolume) * 20);
        audioMixer.SetFloat("SFX", Mathf.Log10(GameManager.instance.soundVolume) * 20);
        bgmSlider.value = GameManager.instance.bgmVolume;
        soundSlider.value = GameManager.instance.soundVolume;
        controlSencSlider.value = GameManager.instance.controlSencitivity;
        autoAimToggle.isOn = GameManager.instance.autoAim;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
