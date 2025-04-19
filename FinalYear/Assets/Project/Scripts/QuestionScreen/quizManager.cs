using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class quizManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text title;
    public TMP_Text question;
    public TMP_Text answer1;
    public TMP_Text answer2;
    public TMP_Text answer3;
    public TMP_Text answer4;
    public Button correct;
    public Button tryAgain;
    public GameObject answerStore;
    public GameObject questionScreen;
    public Button quitBtn;

    [Header("Audio")]
    public AudioSource click;
    public AudioSource right;
    public AudioSource wrong;
    public AudioSource celebration;

    [Header("Scripts")]
    public questionDatabase qD;
    public gameManager gameManager;

    //private 
    private int questionCount;
    private int maxQuestions = 5;
    private List<string> answers = new List<string>();
    private List<string> randomAnswers = new List<string>();
    private string subject;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // this function keeps track of which question the user is on whilst displaying it correctly. 
    // it also clears the asked questions once five questions are complete to reset the app
    public void questionTracker()
    {
        if (questionCount < maxQuestions)
        {
            Debug.Log("Question count is below max questions");
            bool gotQuestion = false;
            if (subject == "Maths")
            {
                qD.getMathsQuestion();
                gotQuestion = qD.currentQ != null;
            }else if (subject == "English")
            {
                qD.getEnglishQuestion();
                gotQuestion = qD.currentQ != null;
            }
            else if (subject == "Biology")
            {
                qD.getBiologyQuestion();
                gotQuestion = qD.currentQ != null;
            }
            if (gotQuestion)
            {
                displayQuestion();
                questionCount++;
                title.text = $"Question {questionCount}";
            }
            /*switch (questionCount)
            {
                case 1:
                    title.text = "Question 1";
                    break;
                case 2:
                    title.text = "Question 2";
                    break;
                case 3:
                    title.text = "Question 3";
                    break;
                case 4:
                    title.text = "Question 4";
                    break;
                case 5:
                    title.text = "Question 5";
                    break;
            }*/
        }
        else
        {
            qD.resetAskedQuestions();
            questionCount = 0;
            Debug.Log("Resetting quiz");
            answerStore.SetActive(false);
            quitBtn.gameObject.SetActive(true);
            celebration.Play();
        }
        
    }
    // this function is for displaying the correct answers and question while taking the answers and randomising them
    // so that the answer isnt always in the same position
    public void displayQuestion()
    {
        answers.Clear();
        randomAnswers.Clear();
        question.text = qD.currentQ;
        answers.Add(qD.correctAnswer);
        answers.Add(qD.answer2);
        answers.Add(qD.answer3);
        answers.Add(qD.answer4);

        List<string> tempAnswers = new List<string>(answers);
        while (tempAnswers.Count > 0)
        {
            int randomIndex = Random.Range(0, tempAnswers.Count);
            randomAnswers.Add(tempAnswers[randomIndex]);
            tempAnswers.RemoveAt(randomIndex);
        }

        answer1.text = randomAnswers[0];
        answer2.text = randomAnswers[1];
        answer3.text = randomAnswers[2];
        answer4.text = randomAnswers[3];

    }
    // this function takes the input of the selected answer from the user and checks to see if is correct
    public void answerTracker(int answerSelected)
    {
        switch (answerSelected)
        {
            case 0:
                if (answer1.text == qD.correctAnswer)
                {
                    Debug.Log("Correct");
                    right.Play();
                    correct.gameObject.SetActive(true);
                    answerStore.SetActive(false);
                }else
                {
                    Debug.Log("Incorrect");
                    wrong.Play();
                    tryAgain.gameObject.SetActive(true);
                    answerStore.SetActive(false);
                }
                break;
            case 1:
                if (answer2.text == qD.correctAnswer)
                {
                    Debug.Log("Correct");
                    right.Play();
                    correct.gameObject.SetActive(true);
                    answerStore.SetActive(false);
                }
                else
                {
                    Debug.Log("Incorrect");
                    wrong.Play();
                    tryAgain.gameObject.SetActive(true);
                    answerStore.SetActive(false);
                }
                break;
            case 2:
                if (answer3.text == qD.correctAnswer)
                {
                    Debug.Log("Correct");
                    right.Play();
                    correct.gameObject.SetActive(true);
                    answerStore.SetActive(false);
                }
                else
                {
                    Debug.Log("Incorrect");
                    wrong.Play();
                    tryAgain.gameObject.SetActive(true);
                    answerStore.SetActive(false);
                }
                break;
            case 3:
                if (answer4.text == qD.correctAnswer)
                {
                    Debug.Log("Correct");
                    right.Play();
                    correct.gameObject.SetActive(true);
                    answerStore.SetActive(false);
                }
                else
                {
                    Debug.Log("Incorrect");
                    wrong.Play();
                    tryAgain.gameObject.SetActive(true);
                    answerStore.SetActive(false);
                }
                break;
        }
    }
    // called when first button is pressed reading left to right
    public void answer1Btn()
    {
        click.Play();
        answerTracker(0);
    }
    // called when second button is pressed
    public void answer2Btn()
    {
        click.Play();
        answerTracker(1);
    }
    // called when third button is pressed
    public void answer3Btn()
    {
        click.Play();
        answerTracker(2);
    }
    // called when fourth button is pressed
    public void answer4Btn()
    {
        click.Play();
        answerTracker(3);
    }
    // called when maths is selected setting subject to maths to retrieve question of same type later
    public void mathsBtn()
    {
        click.Play();
        qD.resetAskedQuestions();
        questionScreen.SetActive(true);
        quitBtn.gameObject.SetActive(false);
        answerStore.SetActive(true);
        subject = "Maths";
        questionTracker();
    }
    // called when english is selected
    public void englishBtn()
    {
        click.Play();
        qD.resetAskedQuestions();
        questionScreen.SetActive(true);
        quitBtn.gameObject.SetActive(false);
        answerStore.SetActive(true);
        subject = "English";
        questionTracker();
    }
    // called when biology is selected
    public void biologyBtn()
    {
        click.Play();
        qD.resetAskedQuestions();
        questionScreen.SetActive(true);
        quitBtn.gameObject.SetActive(false);
        answerStore.SetActive(true);
        subject = "Biology";
        questionTracker();
    }
    // if user gets asnwer correct this button appears and allows them to continue
    public void correctBtn()
    {
        click.Play();
        correct.gameObject.SetActive(false);
        answerStore.SetActive(true);
        questionTracker();
    }
    // if user gets asnswer incorrect this button appears and allows them to try again
    public void tryAgainBtn()
    {
        click.Play();
        tryAgain.gameObject.SetActive(false);
        answerStore.SetActive(true);
    }
    // quits question activity and applies logic such as star increase and timer start
    public void quit()
    {
        click.Play();
        questionScreen.SetActive(false);
        gameManager.currentStarStore += 5;
        gameManager.setStar();
    }
}
