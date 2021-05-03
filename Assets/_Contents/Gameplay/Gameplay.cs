using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TebakAngka.Gameplay
{
    public class Gameplay : MonoBehaviour
    {
        // 1. generate level
        // 2. tampilin level
        // 3. tunggu input
        // 4. cek benar salah
        // 5. a. selebrasi benar. kembali ke 1.
        // 5. b. pernyataan salah. kembali ke 3.

        private const int MinNumber = 1; // Inclusive
        private const int MaxNumber = 10; // Exclusive?

        private int _level; // progress
        private int _correctAnswer; // angka yang benar
        private int _answerCount; // jumlah pilihan jawaban
        private int _guessAnswerIndex = -1; // belum ada input
        private int _step = 0;

        private List<int> _answers = new List<int>();

        private void GenerateLevel()
        {
            _level++; // progress level;
            _correctAnswer = Random.Range(MinNumber, MaxNumber); // MinNumber = 1, MaxNumber = 10 -> 1~9
            
            _answerCount = Mathf.FloorToInt(Mathf.Log(_level + 1, 2)) + 1;
            
            _answers.Clear();
            for (var i = 1; i <= _answerCount; i++)
            {
                if (i == _answerCount && !_answers.Contains(_correctAnswer))
                {
                    _answers.Add(_correctAnswer);
                    ShuffleAnswers();
                    continue;
                }
                
                var temp = Random.Range(MinNumber, MaxNumber);
                
                // memastikan cuma ada 1
                while (_answers.Contains(temp))
                {
                    temp = Random.Range(MinNumber, MaxNumber);
                }

                _answers.Add(temp);
            }
        }

        private void ShuffleAnswers()  
        {  
            var n = _answerCount;  
            while (n > 1) {  
                n--;
                var k = Random.Range(0, n + 1);  
                var value = _answers[k];  
                _answers[k] = _answers[n];  
                _answers[n] = value;  
            }  
        }

        private void PresentLevel()
        {
            // text based
            var answerString = "";
            for (var i = 0; i < _answerCount; i++)
            {
                var answer = _answers[i];
                answerString += $", {answer}";
            }

            Debug.Log($"Level: {_level}, Jumlah Jawaban: {_answerCount}, Jawaban benar: {_correctAnswer}. Pilihan{answerString}");
        }

        private void WaitForInput()
        {
            _guessAnswerIndex = -1;
            // next di handle di update method;
        }

        private void CheckAnswer()
        {
            Debug.Log(_answers[_guessAnswerIndex] == _correctAnswer
                ? $"Jawabannya benar!! {_correctAnswer}"
                : $"Jawabannya salah.. :( yang benar: {_correctAnswer}");
        }

        private void Update()
        {
            DetectInput();
            if (_step == 0)
            {
                GenerateLevel(); // step 1
                _step = 1;
            }
            else if (_step == 1)
            {
                PresentLevel(); // step 2
                WaitForInput();
                _step = 2;
            }
            else if (_step == 2)
            {
                if (_guessAnswerIndex != -1 && _guessAnswerIndex < _answers.Count)
                {
                    Debug.Log($"Angka yang di tebak: {_answers[_guessAnswerIndex]}");
                    _step = 3;
                }
                else
                {
                    WaitForInput();
                }
            }
            else if (_step == 3)
            {
                CheckAnswer(); // step 4 & 5
                _step = 0;
            }
        }

        private void DetectInput()
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                _guessAnswerIndex = 0;
            }
            else if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                _guessAnswerIndex = 1;
            }
            else if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                _guessAnswerIndex = 2;
            }
            else if (Keyboard.current.digit4Key.wasPressedThisFrame)
            {
                _guessAnswerIndex = 3;
            }
            else if (Keyboard.current.digit5Key.wasPressedThisFrame)
            {
                _guessAnswerIndex = 4;
            }
            else if (Keyboard.current.digit6Key.wasPressedThisFrame)
            {
                _guessAnswerIndex = 5;
            }
            else if (Keyboard.current.digit7Key.wasPressedThisFrame)
            {
                _guessAnswerIndex = 6;
            }
            else if (Keyboard.current.digit8Key.wasPressedThisFrame)
            {
                _guessAnswerIndex = 7;
            }
            else if (Keyboard.current.digit9Key.wasPressedThisFrame)
            {
                _guessAnswerIndex = 8;
            }
        }
    }
}
