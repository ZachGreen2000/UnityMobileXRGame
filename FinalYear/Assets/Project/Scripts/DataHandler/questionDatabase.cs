using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.Linq;

public class questionDatabase : MonoBehaviour
{
    private SQLiteConnection db;
    public string currentQ;
    public string correctAnswer;
    public string answer2;
    public string answer3;
    public string answer4;
    private List<int> askedQuestionIds = new List<int>(); // list of asked questions for comparison
    // Start is called before the first frame update
    void Start()
    {
        // the following code creates a connection to the SQLite database that stores the questions
        string dbPath = System.IO.Path.Combine(Application.streamingAssetsPath, "questions.db");
        var db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // when the following function is called it will get a random english question from the database and store it for use
    public void getEnglishQuestion()
    {
        Questions randomEnglishQuestion = getRandomQuestion("English");
        currentQ = randomEnglishQuestion.question;
        correctAnswer = randomEnglishQuestion.correct_answer;
        answer2 = randomEnglishQuestion.answer2;
        answer3 = randomEnglishQuestion.answer3;
        answer4 = randomEnglishQuestion.answer4;
    }
    // when the following function is called it will get a random maths question from the database and store it for use
    public void getMathsQuestion()
    {
        Questions randomMathsQuestion = getRandomQuestion("Maths");
        currentQ = randomMathsQuestion.question;
        correctAnswer = randomMathsQuestion.correct_answer;
        answer2 = randomMathsQuestion.answer2;
        answer3 = randomMathsQuestion.answer3;
        answer4 = randomMathsQuestion.answer4;
    }
    // when the following function is called it will get a random biology question from the database and store it for use
    public void getBiologyQuestion()
    {
        Questions randomBiologyQuestion = getRandomQuestion("Biology");
        currentQ = randomBiologyQuestion.question;
        correctAnswer = randomBiologyQuestion.correct_answer;
        answer2 = randomBiologyQuestion.answer2;
        answer3 = randomBiologyQuestion.answer3;
        answer4 = randomBiologyQuestion.answer4;
    }
    // this function works by making a query to the database in reference to the subject passed into it
    // if the database subject collumn has a matching subject title and it hasnt been asked then information is taken and stored in a list
    // Finally this is iterated through using the random function to get a random question to be returned
    public Questions getRandomQuestion(string subject)
    {
        var questions = db.Table<Questions>().Where(q => q.subject == subject && !askedQuestionIds.Contains(q.id)).ToList();
        if (questions.Count == 0 )
        {
            Debug.Log("No question found for subject: " + subject);
            return null;
        }
        int randomIndex = Random.Range(0, questions.Count);
        var selectedQuestion = questions[randomIndex];
        askedQuestionIds.Add(selectedQuestion.id);
        return selectedQuestion;
    }

    public void resetAskedQuestions()
    {
        askedQuestionIds.Clear();
    }
    // databse table class set up to mirror the database
    public class Questions
    {
        public int id { get; set; }
        public string subject { get; set; }
        public string question { get; set; }
        public string correct_answer { get; set; }
        public string answer2 { get; set; }
        public string answer3 { get; set; }
        public string answer4 { get; set; }
    }
}
