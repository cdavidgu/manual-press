using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class Manual
    {

        List<string> animations;
        List<Sprite> infographies;
        List<bool> isStepAnimation;
        int actualAnimationIndex;
        int actualInfographyIndex;
        int totalSteps;
        int actualStep;
        bool firstAnimation = true;
        bool firstInfography = true;
        public int ActualStep
        {
            get { return actualStep; }
        }
        public int TotalSteps
        {
            get { return totalSteps; }
        }

        public string Name { get; }
        public string ID { get; }

        public Manual(string name, string id, int numSteps, List<bool> isStepAnimationList)
        {
            Name = name;
            ID = id;
            totalSteps = numSteps;
            actualStep = 0;
            animations = new List<string>();
            infographies = new List<Sprite>();
            isStepAnimation = new List<bool>();
            isStepAnimation = isStepAnimationList.GetRange(0, isStepAnimationList.Count);
        }
        public void AddAnimation(string animationName)
        {
            animations.Add(animationName);
        }
        public void AddInfography(Sprite infography)
        {
            infographies.Add(infography);
        }

        public void NextStep()
        {
            if (actualStep < totalSteps - 1)
            {
                if (IsStepAnimation(actualStep) && (actualAnimationIndex < animations.Count - 1))
                    actualAnimationIndex++;
                else if (!IsStepAnimation(actualStep) && (actualInfographyIndex < infographies.Count - 1))
                    actualInfographyIndex++;
                actualStep++;
            }
        }
        public void PrevStep()
        {
            if (actualStep > 0)
            {
                if (IsStepAnimation(actualStep) && actualAnimationIndex > 0)
                    actualAnimationIndex--;
                if (!IsStepAnimation(actualStep) && actualInfographyIndex > 0)
                    actualInfographyIndex--;
                actualStep--;
            }
        }
        public bool IsStepAnimation(int stepIndex)
        {
            return isStepAnimation[stepIndex];
        }

        public string GetActualAnimation()
        {
            string animationName = animations[actualAnimationIndex];
            return animationName;
        }
        public Sprite GetActualInfography()
        {
            Sprite actualInfo = infographies[actualInfographyIndex];
            return actualInfo;
        }
    }
}