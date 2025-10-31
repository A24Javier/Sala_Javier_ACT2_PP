using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ConfigSystem : MonoBehaviour
{
    [Header("Elementos UI")]
    [SerializeField] private CanvasGroup canvasConfig;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TMP_Dropdown screenResDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    // Keys para los PlayerPrefs
    private const string BGM_KEY = "BGM_Volume";
    private const string SFX_KEY = "SFX_Volume";
    private const string SCREEN_RES_KEY = "ScreenRes_selected";
    private const string FULLSCREEN_KEY = "Fullscreen";

    // Lista de resoluciones disponibles
    private List<Resolution> resolutionsList = new List<Resolution>();

    // Variables
    private bool isFullscreen;
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(BGM_KEY)) { PlayerPrefs.SetFloat(BGM_KEY, 1); }

        if(!PlayerPrefs.HasKey(SFX_KEY)) { PlayerPrefs.SetFloat(SFX_KEY, 1); }

        GetAvalaibleResolutions();
        if (!PlayerPrefs.HasKey(SCREEN_RES_KEY)) { PlayerPrefs.SetInt(SCREEN_RES_KEY, GetActualResolution()); }
        else { UpdateResolution(PlayerPrefs.GetInt(SCREEN_RES_KEY)); }

        if (!PlayerPrefs.HasKey(FULLSCREEN_KEY)) { PlayerPrefs.SetInt(FULLSCREEN_KEY, 1); isFullscreen = true; }
        else { SetFullscreen(PlayerPrefs.GetInt(FULLSCREEN_KEY) == 1 ? true : false); }

    }

    void Start()
    {
        bgmSlider.value = PlayerPrefs.GetFloat(BGM_KEY);
        UpdateBGM();
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_KEY);
        UpdateSFX();

        bgmSlider.onValueChanged.AddListener(delegate { UpdateBGM(); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateSFX(); });

        screenResDropdown.value = PlayerPrefs.GetInt(SCREEN_RES_KEY);
        screenResDropdown.onValueChanged.AddListener(delegate { UpdateResolution(screenResDropdown.value); });

        fullscreenToggle.isOn = PlayerPrefs.GetInt(FULLSCREEN_KEY) == 1 ? true : false;
        Screen.fullScreen = fullscreenToggle.isOn;
        fullscreenToggle.onValueChanged.AddListener(delegate { SetFullscreen(fullscreenToggle.isOn); });
    }

    private void UpdateBGM()
    {
        PlayerPrefs.SetFloat(BGM_KEY, bgmSlider.value);

        float volume = Mathf.Log10(bgmSlider.value) * 20;
        audioMixer.SetFloat("BGM_Volume", volume);

        Debug.Log("Volumen BGM cambiado");
    }

    private void UpdateSFX()
    {
        PlayerPrefs.SetFloat(SFX_KEY, sfxSlider.value);

        float volume = Mathf.Log10(sfxSlider.value) * 20;
        audioMixer.SetFloat("SFX_Volume", volume);

        Debug.Log("Volumen SFX cambiado");
    }

    private void UpdateResolution(int screenId)
    {
        Screen.SetResolution(resolutionsList[screenId].width, resolutionsList[screenId].height, isFullscreen);
        PlayerPrefs.SetInt(SCREEN_RES_KEY, screenId);
    }

    private void SetFullscreen(bool fullscreen)
    {
        isFullscreen = fullscreen;
        if(fullscreen) 
        {
            PlayerPrefs.SetInt(FULLSCREEN_KEY, 1);
        }
        else
        {
            PlayerPrefs.SetInt(FULLSCREEN_KEY, 0);
        }

        UpdateResolution(PlayerPrefs.GetInt(SCREEN_RES_KEY));

    }

    private void GetAvalaibleResolutions()
    {
        resolutionsList.Clear();
        resolutionsList.AddRange(Screen.resolutions);

        List<string> resolutions = new List<string>();
        for(int i = 0; i < resolutionsList.Count; i++)
        {
            resolutions.Add(resolutionsList[i].width + "x" + resolutionsList[i].height);
        }

        screenResDropdown.ClearOptions();

        screenResDropdown.AddOptions(resolutions);
    }

    private int GetActualResolution()
    {
        for(int i = 0; i < resolutionsList.Count; i++)
        {
            if (resolutionsList[i].width == Screen.currentResolution.width && resolutionsList[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }

        return 0;
    }

    public void ResetToDefault()
    {
        // Asignamos valores por defecto a los PlayerPrefs
        PlayerPrefs.SetFloat(BGM_KEY, 1);
        PlayerPrefs.SetFloat(SFX_KEY, 1);
        PlayerPrefs.SetInt(SCREEN_RES_KEY, GetActualResolution());
        PlayerPrefs.SetInt(FULLSCREEN_KEY, 1);

        // Actualizamos el valor de todos los sliders y demás
        bgmSlider.value = PlayerPrefs.GetFloat(BGM_KEY);
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_KEY);
        screenResDropdown.value = PlayerPrefs.GetInt(SCREEN_RES_KEY);
        fullscreenToggle.isOn = PlayerPrefs.GetInt(FULLSCREEN_KEY) == 1 ? true : false;

        // Actualizamos los componentes y la resolución del programa
        UpdateBGM();
        UpdateSFX();
        UpdateResolution(PlayerPrefs.GetInt(SCREEN_RES_KEY));
        SetFullscreen(PlayerPrefs.GetInt(FULLSCREEN_KEY) == 1 ? true : false);
    }

    public void ShowConfig(bool show)
    {
        canvasConfig.alpha = show ? 1 : 0;
        canvasConfig.interactable = show;
        canvasConfig.blocksRaycasts = show;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
