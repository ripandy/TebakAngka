using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kassets.Utilities;
using MessagePipe;
using UnityEngine;

namespace TebakAngka.Gameplay
{
    public class GenerateLevelState : IGameState
    {
        private readonly GameModel _gameModel;
        private readonly IAsyncPublisher<GameStateEnum, IList<int>> _answersPublisher;

        private const GameStateEnum OwnState = GameStateEnum.GenerateLevel;

        public GenerateLevelState(
            GameModel gameModel,
            IAsyncPublisher<GameStateEnum, IList<int>> answersPublisher)
        {
            _gameModel = gameModel;
            _answersPublisher = answersPublisher;
        }

        public async UniTask OnStateBegan(CancellationToken cancellationToken)
        {
            GenerateLevel();
            await _answersPublisher.PublishAsync(OwnState, _gameModel.answers, cancellationToken);
        }

        public GameStateEnum OnStateEnded()
        {
            return GameStateEnum.UserInput;
        }

        private void GenerateLevel()
        {
            _gameModel.level++; // progress level;
            _gameModel.correctAnswer = Random.Range(GameModel.MinNumber, GameModel.MaxNumber); // MinNumber = 1, MaxNumber = 10 -> 1~9
            
            var answerCount = Mathf.FloorToInt(Mathf.Log(_gameModel.level + 1, 2)) + 1;
            var answers = _gameModel.answers;

            answers.Clear();
            for (var i = 1; i <= answerCount; i++)
            {
                if (i == answerCount && !answers.Contains(_gameModel.correctAnswer))
                {
                    answers.Add(_gameModel.correctAnswer);
                    _gameModel.ShuffleAnswers();
                    continue;
                }
                
                var temp = Random.Range(GameModel.MinNumber, GameModel.MaxNumber);
                
                // only 1 answer option
                while (answers.Contains(temp))
                {
                    temp = Random.Range(GameModel.MinNumber, GameModel.MaxNumber);
                }

                answers.Add(temp);
            }
            
            PresentLevel();
        }
        
        [Conditional("UNITY_EDITOR")]
        private void PresentLevel()
        {
            // text based
            var answers = _gameModel.answers;
            var answerString = "";
            for (var i = 0; i < answers.Count; i++)
            {
                var answer = answers[i];
                answerString += $", {answer}";
            }

            this.Cyan($"[{GetType().Name}] Level: {_gameModel.level}, Jumlah Jawaban: {answers.Count}, Jawaban benar: {_gameModel.correctAnswer}. Pilihan{answerString}");
        }
    }
}