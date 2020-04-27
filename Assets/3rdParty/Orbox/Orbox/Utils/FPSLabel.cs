#if USE_FAST_FPSLABEL

using System;

using UnityEngine;
using UnityEngine.UI;

namespace Orbox.Utils
{

    public abstract class FPSLabel : MonoBehaviour
    {
        private const int MinTextSize = 10;
        private const int MaxTextSize = 150;

        private const int AverageFrameSize = 100;
        private int AverageFrameCounter = 0;

        private int FPS = 0;
        private int AverageFPS = 0;

        private int DisplayedFPS;
        private int DisplayedAverageFPS;

        private float DeltaTimeSumm = 0f;

        private Text Label;
        private string FPSText = "fps -/- avg -/- ";


        // --- unity ---

        void Awake()
        {

            GameObject go = new GameObject("FPS");
            go.transform.SetParent(this.transform, false);
            Label = go.AddComponent<Text>();

            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            Label.font = ArialFont;
            Label.color = Color.blue;
            Label.text = FPSText;

            var canvas = GetComponentInParent<Canvas>();

            var canvasWidth = canvas.pixelRect.width / canvas.scaleFactor;
            var canvasHeight = canvas.pixelRect.height / canvas.scaleFactor;


            // set text rect
            var rect = new Vector2();
            rect.x = canvasWidth / 4;
            rect.y = canvasHeight / 16;
            Label.rectTransform.sizeDelta = rect;

            //set anchors 
            Label.rectTransform.anchorMin = new Vector2(1, 1);
            Label.rectTransform.anchorMax = new Vector2(1, 1);
            Label.rectTransform.pivot = new Vector2(1, 1);


            //calculate text size to best fit
            var settings = Label.GetGenerationSettings(rect);

            // probably some unity bug here, Use 1.0f sclase to avoid
            // set scale 0.9 to reserve some space for number width changes
            settings.scaleFactor = 0.9f; 
 
            settings.resizeTextForBestFit = true;           
            settings.resizeTextMinSize = MinTextSize;
            settings.resizeTextMaxSize = MaxTextSize;

            var textGenerator = new TextGenerator();
            textGenerator.Populate(Label.text, settings);

            //set bestFit size
            Label.fontSize = textGenerator.fontSizeUsedForBestFit;

        }


        private void Update()
        {

            if (AverageFrameCounter < AverageFrameSize)
            {
                AverageFrameCounter++;
                DeltaTimeSumm += Time.deltaTime;

                if (AverageFrameCounter == AverageFrameSize)
                {
                    AverageFPS = (int)(AverageFrameSize / DeltaTimeSumm);

                    AverageFrameCounter = 0;
                    DeltaTimeSumm = 0;
                }
            }


            FPS = (int)(1 / Time.deltaTime);

            if (FPS != DisplayedFPS || AverageFPS != DisplayedAverageFPS)
            {
                ChangeLabelText();

                DisplayedFPS = FPS;
                DisplayedAverageFPS = AverageFPS;
            }
        }

        // --- private ---

        private void ChangeLabelText()
        {
            unsafe
            {
                fixed (char* chars = FPSText)
                {
                    var fpsChars = NumberText.ThreeDigitIntToChar(FPS);

                    chars[4] = fpsChars.First;
                    chars[5] = fpsChars.Second;
                    chars[6] = fpsChars.Third;


                    var avarageChars = NumberText.ThreeDigitIntToChar(AverageFPS);

                    chars[12] = avarageChars.First;
                    chars[13] = avarageChars.Second;
                    chars[14] = avarageChars.Third;
                }
            }

            Label.text = "";
            Label.cachedTextGenerator.Invalidate();

            Label.text = FPSText;
        }

    }
}
#endif