using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.UI;

namespace GUPS.EasyParallelScene.Demo
{
    public class ScoreRenderer : MonoBehaviour
    {
        Text text;

        void Awake()
        {
            // Set up the reference.
            text = GetComponent<Text>();
        }

        void Update()
        {
            // Set the displayed text to be the word "Score" followed by the score value.
            text.text = "Score: " + ScoreManager.Score;
        }
    }
}