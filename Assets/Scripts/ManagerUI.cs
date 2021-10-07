using UnityEngine;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour
{
    public static ManagerUI MUI;

    public GameObject layoutMenu;
    public Text textError;
    public Slider sliderProgress;

    public GameObject layoutData;
    public Transform parentTeams;
    public GameObject prefabTeam;

    public InputField fieldSheetID;

    private void Awake()
    {
        MUI = this;
    }

    private void Start(){
        EndEditSheetID();
    }

    public void OpenMenu()
    {
        layoutData.SetActive(false);
        FlushMenu();
        layoutMenu.SetActive(true);
    }

    private void FlushMenu()
    {
        ShowError(false);
        UpdateProgress(0);
    }

    public void ButtonPullData()
    {
        FlushMenu();
        ProcessSOTG.PS.ClearData();
        Loader.L.Load();
    }

    public void OpenData()
    {
        layoutMenu.SetActive(false);

        for (int i = 0; i < parentTeams.childCount; i++)
            Destroy(parentTeams.GetChild(i).gameObject);

        for (int i = 0; i < ProcessSOTG.PS.GetNTeams(); i++)
        {
            Text t = Instantiate(prefabTeam, parentTeams).GetComponent<Text>();
            t.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.85f, Screen.height * 0.08f);
            t.text = ProcessSOTG.PS.GetTeam(i);
        }

        layoutData.SetActive(true);
    }

    public void ShowError(bool _value, string _message = "")
    {
        textError.gameObject.SetActive(_value);
        textError.text = _message;
    }

    public void UpdateProgress(float _value)
    {
        sliderProgress.value = _value;
    }

    public void EndEditSheetID()
    {
        if (fieldSheetID.text == string.Empty)
            PlayerPrefs.SetString("sheetID", "1eHAVwkxDYbol5FT3kSrfezi89HXXjSpFvrLE0T-MiQw");
        else
            PlayerPrefs.SetString("sheetID", fieldSheetID.text);
    }
}
