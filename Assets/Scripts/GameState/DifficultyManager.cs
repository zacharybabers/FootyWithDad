using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    public class DifficultyManager : MonoBehaviour
    {
        [Space(10)]
        [Header("Gravity Parameters")]
        [Tooltip("Minimum gravity at the start of the game (e.g., -9.8). This is the easiest state.")]
        [SerializeField] private float gMin = -9.8f;
        [Tooltip("Maximum gravity when difficulty is at its highest (e.g., -20).")]
        [SerializeField] private float gMax = -9.8f;
        [Tooltip("Controls how quickly gravity ramps up around the midpoint. Suggested: ~0.05–0.2.")]
        [SerializeField] private float gSteepness = .1f;

        [Space(10)]
        [Header("Kick Power Parameters")]
        [Tooltip("Minimum kick power at the start of the game.")]
        [SerializeField] private float kMin = 14f;
        [Tooltip("Maximum kick power when difficulty is at its highest.")]
        [SerializeField] private float kMax = 14f;
        [Tooltip("Controls how quickly kick power ramps up around the midpoint. Suggested: ~0.05–0.2.")]
        [SerializeField] private float kSteepness = .1f;

        [Space(10)]
        [Header("Timing")]
        [Tooltip("Midpoint of the difficulty ramp. Around this time or ball count, difficulty increases most rapidly.")]
        [SerializeField] private float tMid = 40f;
    
        private BallObject ballObject;
        private ScoreKeeper scoreKeeper;
    
        private void Awake()
        {
            ballObject = FindObjectOfType<BallObject>();
            scoreKeeper = FindObjectOfType<ScoreKeeper>();
        }

        private void Update()
        {
            var score = scoreKeeper.GetScore();
            ballObject.yAcceleration = GetGravity(score);
            ballObject.kickPower = GetKickPower(score);
        }

        private float GetGravity(int t)
        {
            return EvalCurve(t, gMin, gMax, gSteepness, tMid);
        }

        private float GetKickPower(int t)
        {
            return EvalCurve(t, kMin, kMax, kSteepness, tMid);
        }


        private float EvalCurve(int t, float min, float max, float k, float mid)
        {
            return min + (max - min) / (1 + Mathf.Exp(-k * (t - mid)));
        }
    }

}
