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
            this.Cyan("OnStateBegan");
            GenerateLevel();
            await _answersPublisher.PublishAsync(OwnState, _gameModel.answers, cancellationToken);
        }

        public GameStateEnum OnStateEnded()
        {
            this.Cyan("OnStateEnded");
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
            
            LogGeneratedLevel();
        }
        
        [Conditional("UNITY_EDITOR")]
        private void LogGeneratedLevel()
        {
            // text based
            var answers = _gameModel.answers;
            var answerString = "";
            foreach (var answer in answers)
            {
                answerString += $", {answer}";
            }

            this.Cyan($"[{GetType().Name}] Level: {_gameModel.level}, Jumlah Jawaban: {answers.Count}, Jawaban benar: {_gameModel.correctAnswer}. Pilihan{answerString}");
        }
    }
}