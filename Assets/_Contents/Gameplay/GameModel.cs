using System.Collections.Generic;
using UnityEngine;

namespace TebakAngka.Gameplay
{
    public class GameModel
    {
        public const int MinNumber = 1; // Inclusive
        public const int MaxNumber = 10; // Exclusive?
        
        public int level;
        public int correctAnswer;
        public int userAnswer;
        public readonly IList<int> answers = new List<int>();

        public int LevelMaxNumber => Mathf.Min(MaxNumber, Mathf.FloorToInt(level * 0.25f) + 4);
        public bool IsAnswerCorrect => userAnswer == correctAnswer;
        
        public void ShuffleAnswers()
        {  
            var n = answers.Count;  
            while (n > 1) {  
                n--;
                var k = Random.Range(0, n + 1);  
                var value = answers[k];  
                answers[k] = answers[n];  
                answers[n] = value;  
            }
        }
    }
}