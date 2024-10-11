using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

//написать функции проверки правильности установки буквы, проверка win/lose

public class LevelGenerate : MonoBehaviour
{
    string[] wordLvl = new string[]
    {
        "SAVANNA",
        "ELEPHANT",
        "GIRAFFE",
        "ZEBRA",
        "CHEETAH",
        "HYENA",
        "BUFFALO",
        "IMPALA",
        "GAZELLE",
        "LEOPARD",
        "MEERKAT",
        "WARTHOG",
        "VULTURE",
        "PREDATOR",
        "PRIDE",
        "SUNSET",
        "MUDFLAT",
        "PATHWAY",
        "HORIZON",
        "WILDLIFE",
        "SAFARI"
    };

    [SerializeField] private TextMeshProUGUI[] lvlTxt; //название уровня

    public GameObject letterBtn; //шаблон кнопки
    public GameObject letters; //панель для букв, родительский объект для кнопок-букв
    public GameObject word; //панель с горизонтальной группой, где должно быть отгадываемое слово
    public GameObject lettersInWord; //массив "картинок" для каждой кнопки-буквы
    public int lastLvl = 0;

    public float minX, maxX, minY, maxY;
    int activeLetter;
    Vector2 tempPosBtn;

    string wordLastLvl;

    private List<Button> buttons = new List<Button>();
    private List<Image> wordImg = new List<Image>();

    [SerializeField] private AudioSource aEffsects;

    public bool isReady = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void lastLvlLoad()
    {
        if (PlayerPrefs.HasKey("lastLvl"))
            lastLvl = PlayerPrefs.GetInt("lastLvl");
        else
            lastLvl = 0;
    }

    private void GenerateImg()
    {
        for (int i = 0; i < wordLastLvl.Length; i++)
        {
            GameObject imageObj = Instantiate(lettersInWord, word.transform);
            Image img = imageObj.GetComponent<Image>();

            wordImg.Add(img);
        }
    }

    private void GenerateButtons()
    {
        for (int i = 0; i < wordLastLvl.Length; i++)
        {
            int buttonIndex = i;
            GameObject buttonObject = Instantiate(letterBtn, letters.transform);
            Button button = buttonObject.GetComponent<Button>();

            float x, y;
            do
            {
                x = Random.Range(minX, maxX);
                y = Random.Range(minY, maxY);
            } while (IsPositionOccupied(x, y, buttonObject));
            buttonObject.transform.localPosition = new Vector2(x, y);

            button.onClick.AddListener(() => clickLetter(buttonIndex));

            button.GetComponentInChildren<TextMeshProUGUI>().text = wordLastLvl[i].ToString();

            buttons.Add(button);
        }
    }

    private bool IsPositionOccupied(float x, float y, GameObject buttonObject)
    {
        // Check if the position is occupied by another button
        foreach (Button button in buttons)
        {
            if (Vector2.Distance(button.transform.localPosition, new Vector2(x, y)) < buttonObject.GetComponent<RectTransform>().sizeDelta.x / 2 + 90f &&
                Vector2.Distance(button.transform.localPosition, new Vector2(x, y)) < buttonObject.GetComponent<RectTransform>().sizeDelta.y / 2 + 90f)
            {
                return true;
            }
        }
        return false;
    }

    public void LoadLevel()
    {
        isReady = false;
        clearLevel(letters);
        buttons.Clear();
        clearLevel(word);
        wordImg.Clear();
        lastLvlLoad();
        lvlTxt[0].text = (lastLvl + 1).ToString();
        lvlTxt[1].text = (lastLvl + 1).ToString();
        wordLastLvl = wordLvl[lastLvl];
        activeLetter = 0;
        GenerateButtons();
        GenerateImg();
    }

    void clearLevel(GameObject lettersPanel)
    {
        foreach (Transform child in lettersPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void clickLetter(int index)
    {
        if (index >= 0 && index < buttons.Count)
        {
            tempPosBtn = buttons[index].transform.position;
            buttons[index].transform.position = wordImg[activeLetter].transform.position;
            positionCheck(index);
        }
        aEffsects.Play();
    }

    void positionCheck(int btnIndex)
    {
        if(buttons[btnIndex].GetComponentInChildren<TextMeshProUGUI>().text == wordLastLvl[activeLetter].ToString())
        {
            wordImg[activeLetter].color = Color.green;
            activeLetter++; 
            buttons[btnIndex].interactable = false;
        }
        else
        {
            wordImg[activeLetter].color = Color.red;
            foreach (var button in buttons)
                button.interactable = false;
            StartCoroutine(returnBackBtn(btnIndex));
        }

        if (activeLetter == wordLastLvl.Length)
            isReady = true;
        else
            isReady = false;
    }

    IEnumerator returnBackBtn(int btnIndex)
    {
        yield return new WaitForSeconds(3);
        buttons[btnIndex].transform.position = tempPosBtn;
        wordImg[activeLetter].color = Color.white;
        foreach (var button in buttons)
            button.interactable = true;
    }
}
