// Unity
using UnityEngine;
using UnityEngine.UI;

namespace GUPS.EasyParallelScene.Demos.VoxelShooter
{
    /// <summary>
    /// Renderer for the Score.
    /// </summary>
    public class ScoreRenderer : MonoBehaviour
    {
        public Text text;

        private void Awake()
        {
            // Set up the reference.
            this.text = GetComponent<Text>();
        }

        private void Update()
        {
            // Set the displayed text to be the word "Score" followed by the score value.
            this.text.text = "Score: " + ScoreManager.Score;
        }
    }
}