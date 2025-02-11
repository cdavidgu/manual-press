using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using View.UI;

namespace Assets.UI
{
    public class IntroManager : MonoBehaviour
    {
        [SerializeField]
        GameObject Dots;

        [SerializeField]
        Sprite spriteDotFilled, spriteDotEmpty;

        [SerializeField]
        Button OKButton;

        [SerializeField]
        GameObject introCardPrefab;

        [SerializeField]
        IntroCardData[] introCardData;

        int counter = 0, totalDots;
        float carrouselMovementConst;
        UnityAction endCarrouselAction;
        GameObject cardContainer;


        // Start is called before the first frame update
        void Start()
        {
            carrouselMovementConst = introCardPrefab.GetComponent<RectTransform>().rect.width;
            Debug.Log("carrouselMovementConst: " + carrouselMovementConst);
            cardContainer = GameObject.Find("CardContainer");
            // SelectionModePanel = GameObject.Find("SelectionModePanel");
            totalDots = Dots.transform.childCount;
            InitCarrousel();
            ResetUIComponents();

            //register onclick action for OKButton
            OKButton.onClick.AddListener(() =>
            {
                counter += 1;
                // Debug.Log("counter: " + totalDots);
                if (counter == totalDots)
                {
                    endCarrouselAction.Invoke();
                    // this.gameObject.SetActive(false);
                    return;
                }
                //TODO: update content
                MoveCarrouselLeft(counter);
                updateDots(counter);
            });
        }

        public void EndCarrouselAction(UnityAction callback)
        {
            endCarrouselAction = callback;
        }
        void ResetUIComponents()
        {
            //Initialize every child of Dots
            for (int i = 0; i < totalDots; i++)
            {
                GameObject child = Dots.transform.GetChild(i).gameObject;
                Image dotImg = child.gameObject.GetComponent<Image>();
                if (i == 0)
                {
                    child.transform.localScale = new Vector3(1, 1, 1);
                    dotImg.sprite = spriteDotFilled;
                }
                else
                {
                    child.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    dotImg.sprite = spriteDotEmpty;
                }
            }
        }

        void updateDots(int counter)
        {

            GameObject tempDot;
            Image dotImg;

            tempDot = Dots.transform.GetChild(counter - 1).gameObject;
            dotImg = tempDot.gameObject.GetComponent<Image>();
            dotImg.sprite = spriteDotEmpty;
            tempDot.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

            tempDot = Dots.transform.GetChild(counter).gameObject;
            dotImg = tempDot.gameObject.GetComponent<Image>();
            dotImg.sprite = spriteDotFilled;
            tempDot.transform.localScale = new Vector3(1, 1, 1);
        }

        void MoveCarrouselLeft(int counter)
        {
            //1305.1
            Vector3 actualContainerPos = cardContainer.transform.localPosition;
            float offset = 55f;

            iTween.MoveTo(cardContainer, iTween.Hash("position", new Vector3(actualContainerPos.x - (carrouselMovementConst + offset), actualContainerPos.y, actualContainerPos.z),
           "islocal", true, "time", 0.75f, "easeType", iTween.EaseType.easeOutBack, "delay", 0.23f));
        }
        // 
        void InitCarrousel()
        {
            for (int i = 0; i < introCardData.Length; i++)
            {
                GameObject card = Instantiate(introCardPrefab, cardContainer.transform);
                card.GetComponent<IntroCard>().Init(introCardData[i]);
                //card.Init(introCardData[i]);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
