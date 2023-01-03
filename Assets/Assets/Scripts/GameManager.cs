using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public PromptGenerator promptGenerator;

    public List<PrompScript> currentFundedProjects = new List<PrompScript>();
    public List<PrompScript> fundedProjectsRecord = new List<PrompScript>();

    public List<Transform> resultPositions = new List<Transform>();

    public int currentGold;
    public TextMeshProUGUI currentGoldText;

    public GameObject promptPrefab, affiniyResultsPrefab;
    public RectTransform promptPosition, slideStartPos, slideEndPos, slideObject;
    public Slider militarSlider, commerceSlider, religionSlider, infrastructureSlider;

    public bool Funding, interludeFinished;


    //----Mulitpliers----//
    public float peopleMultiplier, rangeMultiplier, ROIMultiplier, fundCostMultiplier;

    public int currentTotalPeople, peopleSeen, roundNumber;

    private int lowRange, highRange;



    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920,1080,true);



        peopleMultiplier = 1;
        rangeMultiplier = 1;
        ROIMultiplier = 1;
        fundCostMultiplier = 1;

        peopleSeen = 0;


        //Instantiate new char
        //Animation
        Funding = true;
        roundNumber = 1;

        slideObject.anchoredPosition = slideStartPos.anchoredPosition;  

        NextPrompt();
    }


    private void Update()
    {
        if (interludeFinished && Input.GetKey(KeyCode.E))
        {
            interludeFinished = false;
            StartCoroutine(SlideOut());
            Funding = true;


            nextRound();
        }
    }
    public void nextRound()
    {
        if (roundNumber >= 10)
        {
            //Game end
        }
        else
        {
            peopleSeen = 0;
            roundNumber++;
            for (int i = 0; i < currentFundedProjects.Count; i++)
            {
                fundedProjectsRecord.Add(currentFundedProjects[i]);
            }
            for (int i = 0; i< resultPositions.Count;i++)
            {
                if(resultPositions[i].transform.childCount > 0) Destroy(resultPositions[i].GetChild(0).gameObject);

            }
            currentFundedProjects.Clear();
            NextPrompt();
       

        }
        
    }

    public void AddToCurrentList(PrompScript prompt)
    {
        currentFundedProjects.Add(prompt);
        recalculateCurrentGold(-int.Parse(prompt.FundCost.text));
        NextPrompt();
    }

    public void recalculateCurrentGold(int value)
    {
        currentGold += value;
        currentGoldText.text = currentGold.ToString();
    }

    public void NextPrompt()
    {
        
        if (peopleSeen < currentTotalPeople)
        {
            GameObject newPrompt = Instantiate(promptPrefab);
            newPrompt.transform.SetParent(promptPosition, false);
            
            GenerateNewPrompt(newPrompt.GetComponent<PrompScript>());
            

            //newPrompt.transform.SetParent(promptPosition);
            peopleSeen++;
        }
        else
        {
            Funding = false;
            Interlude();

        }
        
    }

    public void Interlude()
    {
        StartCoroutine(SlideIn());

        IEnumerator SlideIn(){

            float elapsedTime = 0;

            while (elapsedTime < 0.5f)
            {
                slideObject.anchoredPosition = Vector3.Lerp(slideObject.anchoredPosition, slideEndPos.anchoredPosition, (elapsedTime / 0.5f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            //Funded options appear
            for (int i=0;i<currentFundedProjects.Count;i++)
            {
                GameObject newResult = Instantiate(affiniyResultsPrefab);
                newResult.transform.SetParent(resultPositions[i], false);
                //newResult.transform.GetChild(0).GetComponent<Image>().color = currentFundedProjects[i].AffinityText.text;
                newResult.GetComponent<Image>().color = currentFundedProjects[i].Affinity.color;
                /*switch (newResult.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text)
                {
                    case "Religion":
                        newResult.GetComponent<Image>().color = new Color(1,0,1,1);
                        break;
                    case "Militar":
                        newResult.GetComponent<Image>().color = Color.red;
                        break;
                    case "People":
                        newResult.GetComponent<Image>().color = Color.cyan;
                        break;
                    case "Business":
                        newResult.GetComponent<Image>().color = Color.green;
                        break;
                }*/
                yield return new WaitForSeconds(0.3f);

                //calculate if winner winner chicken dinner
                if (CalculateWinnerInRange(currentFundedProjects[i].percentageRangeA, currentFundedProjects[i].percentageRangeB))
                {
                    newResult.transform.GetChild(0).gameObject.SetActive(true);
                    currentGold += (int)(currentFundedProjects[i].fundCostValue * currentFundedProjects[i].ROIPercentageValue);
                    currentGoldText.text = currentGold.ToString();
                    currentFundedProjects[i].hasWon = true;
                }
                    
                else newResult.transform.GetChild(1).gameObject.SetActive(true);
                yield return new WaitForSeconds(0.15f);
            }


            interludeFinished = true;

        }

        
    }

    IEnumerator SlideOut()
    {

        float elapsedTime = 0;

        while (elapsedTime < 0.5f)
        {
            slideObject.anchoredPosition = Vector3.Lerp(slideObject.anchoredPosition, slideStartPos.anchoredPosition, (elapsedTime / 0.5f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void GenerateNewPrompt(PrompScript prompt)
    {

        prompt.gameObject.GetComponent<PromptGenerator>().GeneratePrompt();

        int _id = prompt.gameObject.GetComponent<PromptGenerator>()._colorId;
        prompt.setAffinityColor(_id);
        prompt.setAffinityShadow(_id);
        //prompt.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = prompt.gameObject.GetComponent<PromptGenerator>()._tag;  //Tag


        prompt.setFundCost(CalculateFundCost().ToString());
        //prompt.transform.GetChild(2).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = CalculateFundCost().ToString();  //Fund cost

        CalculatePercentageRange();
        prompt.setPercentageRange(lowRange, highRange);
        //prompt.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = lowRange + " - " + highRange;  //Percentage Range

        prompt.setROIPercentage(CalculateROI());
        //prompt.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = CalculateROI().ToString() + "%"; //ROI Percentage
    }


    public void CalculatePercentageRange()
    {
        lowRange = Random.Range(25, 100 - 1);

        highRange = Random.Range(lowRange, 100 - 1);

    }

    public float CalculateROI()
    {
        return Random.Range(0f, 10f -1);
    }

    public int CalculateFundCost()
    {
        return Random.Range(300, 900 - 1);
    }

    public bool CalculateWinnerInRange(int a, int b)
    {
        int targetChance = Random.Range(a, b);
        int value = Random.Range(0, 100);

        if (value <= targetChance) return true;

        return false;

    }
}
