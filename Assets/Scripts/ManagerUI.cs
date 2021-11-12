using UnityEngine;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour
{
    public static ManagerUI MUI;

    public GameObject layoutMenu;
    public Text textError;
    public Slider sliderProgress;

    public GameObject layoutData;
    public Transform parentItems;
    public GameObject prefabItem;
    public Text textTitleData;
    private int currentTeam;

    public InputField fieldSheetID;

    private void Awake()
    {
        MUI = this;
    }

    private void Start()
    {
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

    public void PullDataMale()
    {
        PlayerPrefs.SetString("sheetID", "10xZ0xJFHJXmz8beuqVujDykHOSFW8kdBsGs4eiUCdlI");
        PullData();
    }

    public void PullDataFemale()
    {
        PlayerPrefs.SetString("sheetID", "1YuiW7Me0TFb03mFA_vgByDawX14HWLUlF-umhg3oJDA");
        PullData();
    }

    public void PullData()
    {
        FlushMenu();
        ProcessSOTG.PS.ClearData();
        Loader.L.Load();
    }

    public void OpenData()
    {
        layoutMenu.SetActive(false);
        currentTeam = 0;
        ChangeTeam(false);
        layoutData.SetActive(true);
    }

    public void ChangeTeam(bool _next)
    {
        currentTeam += _next ? 1 : -1;
        if (currentTeam < -1)
            currentTeam = ProcessSOTG.PS.GetNTeams() - 1;
        else if (currentTeam >= ProcessSOTG.PS.GetNTeams())
            currentTeam = -1;

        textTitleData.text = currentTeam <= -1 ? "Ranking" : ProcessSOTG.PS.GetTeam(currentTeam).name;

        for (int i = 0; i < parentItems.childCount; i++)
            Destroy(parentItems.GetChild(i).gameObject);

        if (currentTeam == -1)
            for (int i = 0; i < ProcessSOTG.PS.GetNTeams(); i++)
            {
                Text t = Instantiate(prefabItem, parentItems).GetComponent<Text>();
                t.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.85f, Screen.height * 0.08f);
                t.text = ProcessSOTG.PS.GetTeam(i).name + " " + ProcessSOTG.PS.GetTeam(i).Avg().ToString("0.00");
            }
        else
        {
            for (int i = 0; i < ProcessSOTG.PS.GetTeam(currentTeam).scores.Count; i++)
            {
                Text t = Instantiate(prefabItem, parentItems).GetComponent<Text>();
                t.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.85f, Screen.height * 0.08f);
                t.text = ProcessSOTG.PS.GetTeam(currentTeam).scores[i].time;
                Text t2 = Instantiate(prefabItem, parentItems).GetComponent<Text>();
                t2.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.85f, Screen.height * 0.08f);
                t2.text = ProcessSOTG.PS.GetTeam(currentTeam).scores[i].scoringTeam;
                Text t3 = Instantiate(prefabItem, parentItems).GetComponent<Text>();
                t3.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.85f, Screen.height * 0.08f);
                string textScores = string.Empty;
                for (int j = 0; j < ProcessSOTG.PS.GetTeam(currentTeam).scores[i].partials.Length; j++)
                    textScores += ProcessSOTG.PS.GetTeam(currentTeam).scores[i].partials[j] + " ";
                t3.text = textScores + "= " + ProcessSOTG.PS.GetTeam(currentTeam).scores[i].Sum();
                if (ProcessSOTG.PS.GetTeam(currentTeam).scores[i].comment != string.Empty)
                {
                    Text t4 = Instantiate(prefabItem, parentItems).GetComponent<Text>();
                    t4.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.85f, Screen.height * 0.08f);
                    t4.text = ProcessSOTG.PS.GetTeam(currentTeam).scores[i].comment;
                }
                Text t5 = Instantiate(prefabItem, parentItems).GetComponent<Text>();
                t5.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.85f, Screen.height * 0.08f);
                t5.text = string.Empty;
            }
        }
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
            PlayerPrefs.SetString("sheetID", "11nMXfd7NTtwlMbjaj-on9i2bxUg2u4nC4--6xKeTB9Y");
        else
            PlayerPrefs.SetString("sheetID", fieldSheetID.text);
    }
}
