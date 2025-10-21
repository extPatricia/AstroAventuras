using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionSet", menuName = "Quiz/Question Set")]
public class QuestionData : ScriptableObject
{
    [System.Serializable]
    public class Question
    {
        [TextArea(2, 4)]
        public string questionText;
        public string[] options = new string[4];
        public int correctOptionIndex;
    }

    public List<Question> questions = new List<Question>();
}
