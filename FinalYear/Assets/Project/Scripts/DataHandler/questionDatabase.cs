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
        string persistentPath = System.IO.Path.Combine(Application.persistentDataPath, "questions.db");

#if UNITY_ANDROID && !UNITY_EDITOR // this is the sqlite logic for android as the path system works differently (not streaming assets)
        if (!System.IO.File.Exists(persistentPath))
        {
            string streamingPath = System.IO.Path.Combine(Application.streamingAssetsPath, "questions.db");
            StartCoroutine(CopyDbAndroid(streamingPath, persistentPath)); // needs coroutine as file needs copied to persistent path
        }
        else 
        {
            db = new SQLiteConnection(persistentPath, SQLiteOpenFlags.ReadOnly);
        }
#else
        // the following code creates a connection to the SQLite database that stores the questions
        string dbPath = System.IO.Path.Combine(Application.streamingAssetsPath, "questions.db");
        db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly);
#endif
        // store questions needed
        var allQuestions = db.Table<Questions>().ToList();
        /*foreach (var q in allQuestions)
        {
            Debug.Log($"Subject: {q.subject} | Question: {q.question}");
        }*/
    }
    // this enumerator using unitys web requests to copy the data from streaming assets to a persistent data path
    // this is because android devices dont have access to streaming data
    IEnumerator CopyDbAndroid(string source, string target)
    {
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(source))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.Log("Failed to load database from StreamingAssets" + www.error);
            }
            else
            {
                System.IO.File.WriteAllBytes(target, www.downloadHandler.data);
                Debug.Log("Database has been copies to " + target);
                db = new SQLiteConnection(target, SQLiteOpenFlags.ReadOnly);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    // when the following function is called it will get a random english question from the database and store it for use
    public void getEnglishQuestion()
    {
        Questions randomEnglishQuestion = getRandomQuestion("English");
        if (randomEnglishQuestion == null)
        {
            Debug.Log("No english question found");
            return;
        }
        currentQ = randomEnglishQuestion.question;
        correctAnswer = randomEnglishQuestion.correct;
        answer2 = randomEnglishQuestion.incorrect_1;
        answer3 = randomEnglishQuestion.incorrect_2;
        answer4 = randomEnglishQuestion.incorrect_3;
    }
    // when the following function is called it will get a random maths question from the database and store it for use
    public void getMathsQuestion()
    {
        Questions randomMathsQuestion = getRandomQuestion("Maths");
        if (randomMathsQuestion == null)
        {
            Debug.Log("No maths question found");
            return;
        }
        currentQ = randomMathsQuestion.question;
        correctAnswer = randomMathsQuestion.correct;
        answer2 = randomMathsQuestion.incorrect_1;
        answer3 = randomMathsQuestion.incorrect_2;
        answer4 = randomMathsQuestion.incorrect_3;
    }
    // when the following function is called it will get a random biology question from the database and store it for use
    public void getBiologyQuestion()
    {
        Questions randomBiologyQuestion = getRandomQuestion("Biology");
        if (randomBiologyQuestion == null)
        {
            Debug.Log("No biology question found");
            return;
        }
        currentQ = randomBiologyQuestion.question;
        correctAnswer = randomBiologyQuestion.correct;
        answer2 = randomBiologyQuestion.incorrect_1;
        answer3 = randomBiologyQuestion.incorrect_2;
        answer4 = randomBiologyQuestion.incorrect_3;
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
        if (selectedQuestion == null)
        {
            Debug.Log("Selected question is null");
            return null;
        }
        Debug.Log("Selected question is: " + selectedQuestion.question);
        askedQuestionIds.Add(selectedQuestion.id);
        return selectedQuestion;
    }
    // clears questions that have been asked 
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
        public string correct { get; set; }
        public string incorrect_1 { get; set; }
        public string incorrect_2 { get; set; }
        public string incorrect_3 { get; set; }
    }
}
