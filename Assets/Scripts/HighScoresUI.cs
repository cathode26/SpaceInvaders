using deVoid.Utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpaceInvaders
{
    public class HighScores : MonoBehaviour
    {
        [SerializeField]
        private ScoreManager scoreManager;
        [SerializeField]
        private Button backButton;
        [SerializeField]
        private GameObject ui;
        [SerializeField]
        private TextMeshProUGUI rankTemplate;  // Reference to the TextMeshPro in the scene
        [SerializeField]
        private TextMeshProUGUI nameTemplate;  // Reference to the TextMeshPro in the scene
        [SerializeField]
        private TextMeshProUGUI scoreTemplate;  // Reference to the TextMeshPro in the scene
        [SerializeField]
        private TMP_InputField inputFieldTemplate;  // Reference to the TMP_InputField in the scene
        private float maxNameWidth; // This is the maximum width you want to allow for names. Adjust as needed.
        private bool addingHighScore = false;
        private List<TextMeshProUGUI> rankTexts = new List<TextMeshProUGUI>();
        private List<TextMeshProUGUI> nameTexts = new List<TextMeshProUGUI>();
        private List<TextMeshProUGUI> scoreTexts = new List<TextMeshProUGUI>();

        private void Awake()
        {
            ui = transform.GetChild(0).gameObject;

            maxNameWidth = inputFieldTemplate.GetComponent<RectTransform>().rect.width - 15;
            Signals.Get<Project.MainMenu.OnHighScoresPressedSignal>().AddListener(OnShowHighScores);
            Signals.Get<Project.SceneManager.GamePausedSignal>().AddListener(OnGamePaused);
            backButton.onClick.AddListener(BackButtonPressed);
            inputFieldTemplate.onValueChanged.AddListener(OnNameChanged);
            inputFieldTemplate.onEndEdit.AddListener(OnNameEntered);
        }
        private void OnDestroy()
        {
            Signals.Get<Project.MainMenu.OnHighScoresPressedSignal>().RemoveListener(OnShowHighScores);
            Signals.Get<Project.SceneManager.GamePausedSignal>().RemoveListener(OnGamePaused);
            backButton.onClick.RemoveListener(BackButtonPressed);
            inputFieldTemplate.onValueChanged.RemoveListener(OnNameChanged);
            inputFieldTemplate.onEndEdit.RemoveListener(OnNameEntered);
        }
        private void Update()
        {
            if (addingHighScore == true && inputFieldTemplate && !inputFieldTemplate.isFocused)
            {
                EventSystem.current.SetSelectedGameObject(inputFieldTemplate.gameObject, null);
                inputFieldTemplate.OnPointerClick(new PointerEventData(EventSystem.current)); // Simulate a click to open the keyboard on mobile devices
            }
        }
        private void BackButtonPressed()
        {
            Signals.Get<Project.HighScores.OnBackPressedSignal>().Dispatch();
            ui.SetActive(false);
        }
        private void OnShowHighScores()
        {
            ui.SetActive(true);
            DisplayHighScores();
            if (scoreManager.NewHighScore())
            {
                if (scoreManager.SavedName == string.Empty)
                    EnterName();
                else
                    DisplayHighScores();
            }
        }
        private void OnGamePaused(bool paused)
        {
            if (paused == false)
                ui.SetActive(false);
        }
        private void DisplayHighScores()
        {
            float yOffset = 0;
            float percentageOffset = 0.1f;

            rankTemplate.gameObject.SetActive(true);
            nameTemplate.gameObject.SetActive(true);
            scoreTemplate.gameObject.SetActive(true);

            int index = 0;
            foreach ((string name, int score) highscore in scoreManager.HighScores)
            {
                TextMeshProUGUI rankText;
                TextMeshProUGUI nameText;
                TextMeshProUGUI scoreText;

                if (index >= rankTexts.Count)  // If we haven't instantiated for this index yet
                {
                    rankText = Instantiate(rankTemplate, rankTemplate.transform.parent);
                    rankTexts.Add(rankText);

                    nameText = Instantiate(nameTemplate, nameTemplate.transform.parent);
                    nameTexts.Add(nameText);

                    scoreText = Instantiate(scoreTemplate, scoreTemplate.transform.parent);
                    scoreTexts.Add(scoreText);

                    rankText.gameObject.SetActive(true);
                    rankText.color = CalculateColor(index);

                    nameText.gameObject.SetActive(true);
                    nameText.color = CalculateColor(index);

                    scoreText.gameObject.SetActive(true);
                    scoreText.color = CalculateColor(index);

                    yOffset -= scoreText.preferredHeight * (1 + percentageOffset);

                    rankText.rectTransform.anchoredPosition = new Vector2(rankText.rectTransform.anchoredPosition.x, rankText.rectTransform.anchoredPosition.y + yOffset);
                    nameText.rectTransform.anchoredPosition = new Vector2(nameText.rectTransform.anchoredPosition.x, nameText.rectTransform.anchoredPosition.y + yOffset);
                    scoreText.rectTransform.anchoredPosition = new Vector2(scoreText.rectTransform.anchoredPosition.x, scoreText.rectTransform.anchoredPosition.y + yOffset);

                }
                else  // Reuse the existing objects
                {
                    rankText = rankTexts[index];
                    nameText = nameTexts[index];
                    scoreText = scoreTexts[index];
                    yOffset -= scoreText.preferredHeight * (1 + percentageOffset);
                }

                rankText.gameObject.SetActive(true);
                nameText.gameObject.SetActive(true);
                scoreText.gameObject.SetActive(true);

                rankText.text = FormatRank(index + 1);
                nameText.text = highscore.name;
                scoreText.text = highscore.score.ToString();
                index++;
            }

            rankTemplate.gameObject.SetActive(false);
            nameTemplate.gameObject.SetActive(false);
            scoreTemplate.gameObject.SetActive(false);
        }

        private string FormatRank(int rank)
        {
            string rankText = "";

            switch (rank)
            {
                case 1:
                    rankText = "1ST";
                    break;
                case 2:
                    rankText = "2ND";
                    break;
                case 3:
                    rankText = "3RD";
                    break;
                default:
                    rankText = rank.ToString() + "TH";
                    break;
            }
            return rankText;
        }

        private Color CalculateColor(int rank)
        {
            Color teal = Color.cyan;
            Color red = Color.magenta;

            if (rank <= 4)
            {
                return Color.Lerp(teal, red, rank / 4f);
            }
            else
            {
                return Color.Lerp(red, teal, (rank - 5) / 4f);
            }
        }
        private void EnterName()
        {
            //disable the escape button so the user is forced to input the name
            Signals.Get<Project.Input.OnEnableEscapeSignal>().Dispatch(false);

            inputFieldTemplate.gameObject.SetActive(true);
            inputFieldTemplate.text = "";

            TextMeshProUGUI nameText = nameTexts[scoreManager.SavedRank - 1];
            nameText.text = "";     //make sure there is no text visible

            // Set input fields position
            inputFieldTemplate.transform.position = new Vector2(inputFieldTemplate.transform.position.x, nameText.transform.position.y);

            inputFieldTemplate.Select();
            inputFieldTemplate.ActivateInputField();
            backButton.interactable = false;
            addingHighScore = true;
        }
        private void OnNameEntered(string playerName)
        {
            if (playerName == string.Empty)
            {
                inputFieldTemplate.Select();
                inputFieldTemplate.ActivateInputField();
                return;
            }

            // Save the player's name and update the high score display
            scoreManager.SetPlayerName(playerName);

            // Destroy the input field and refresh the display
            addingHighScore = false;
            backButton.interactable = true;
            inputFieldTemplate.gameObject.SetActive(false);
            DisplayHighScores();
            //Enable the escape key
            Signals.Get<Project.Input.OnEnableEscapeSignal>().Dispatch(true);
        }
        private void OnNameChanged(string newName)
        {
            inputFieldTemplate.text = newName.ToUpper();

            // Measure the width of the new name
            float nameWidth = inputFieldTemplate.textComponent.GetPreferredValues(newName).x;

            // If the name is too wide, truncate it
            if (nameWidth > maxNameWidth)
            {
                // Remove the last character to ensure the name fits within our width
                inputFieldTemplate.text = newName.Substring(0, newName.Length - 1);
            }
        }
    }
}