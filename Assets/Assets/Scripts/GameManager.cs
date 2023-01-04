using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMOD;
using FMODUnity;

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
    public NPCGenerator nPCGenerator;
    public EndController endController;

    public List<PrompScript> currentFundedProjects = new List<PrompScript>();
    public List<PrompScript> fundedProjectsRecord = new List<PrompScript>();

    public List<Transform> resultPositions = new List<Transform>();

    public int currentGold, roundStartingGold;
    public TextMeshProUGUI currentGoldText;

    public GameObject promptPrefab, affiniyResultsPrefab;
    public RectTransform promptPosition, slideStartPos, slideEndPos, slideObject;
    public Slider militarSlider, commerceSlider, religionSlider, peopleSlider;
    public int militarTimesUpgraded, commerceTimesUpgraded, religionTimesUpgraded, peopleTimesUpdated;

    public bool Funding, interludeFinished;

    public Image head1, head2, head3;

    public GameObject continueButton;


    //----Mulitpliers----//
    public float peopleMultiplier, rangeLowMultiplier, ROIMultiplier, fundCostMultiplier;

    public int currentTotalPeople, peopleSeen, roundNumber, totalRounds;
    public TextMeshProUGUI peopleText, roundText;

    private int lowRange, highRange;

    private int bingoMilitar, bingoReligion, bingoCommerce;
    private bool hasBingoMilitarAppeared, hasBingoReligionAppeared, hasBingoCommerceAppeared;
    public float bingoChance;

    //----FMOD----//
    public string[] randomMusicPaths;
    public string winGamePath, loseGamePath, interludePath;
    public FMOD.Studio.EventInstance eventInstance1, eventInstance2;
    public bool is1Playing;


    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920,1080,true);


        roundStartingGold = currentGold;

        peopleMultiplier = 1;
        rangeLowMultiplier = 1;
        ROIMultiplier = 1;
        fundCostMultiplier = 1;

        militarTimesUpgraded = 0;
        commerceTimesUpgraded = 0;
        religionTimesUpgraded = 0;
        peopleTimesUpdated = 0;

        currentTotalPeople = 8;
        peopleSeen = 0;

        currentGoldText.text = currentGold.ToString();
        //Instantiate new char
        //Animation
        Funding = true;
        roundNumber = 1;

        slideObject.anchoredPosition = slideStartPos.anchoredPosition;

        SetCurrentRound();
        SetPeopleInRound();

        StartMusic(randomMusicPaths[Random.Range(0, randomMusicPaths.Length)]);
        //csm.SetAmount(currentGold);
        NextPrompt();
    }


    private void Update()
    {
        if (interludeFinished && Input.GetKey(KeyCode.E))
        {
            
        }

        /*if (Input.GetKey(KeyCode.P))
        {
            endController.GameEnd(true);//WIN
            StartMusic(winGamePath);
        }*/

        //if (Input.GetKeyDown(KeyCode.A)) StartMusic(music1Path);
        //else if (Input.GetKey(KeyCode.U)) StopMusic();
    }
    public void nextRound()
    {
        if (roundNumber >= totalRounds)
        {
            //Game end
            if (currentGold >= 1000000)
            {
                endController.GameEnd(true);//WIN
                StartMusic(winGamePath);
            }
            else if (currentGold <= 0) 
            {
                endController.GameEnd(false);//LOSE
                StartMusic(loseGamePath);
            } 
        }
        else
        {

            StartMusic(randomMusicPaths[Random.Range(0,randomMusicPaths.Length)]);


            roundStartingGold = currentGold;

            peopleSeen = 0;
            hasBingoCommerceAppeared = false;
            hasBingoMilitarAppeared = false;
            hasBingoReligionAppeared = false;
            roundNumber++;
            SetCurrentRound();
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

    public void NPCDespawn(PrompScript prompt, bool denied)
    {

        StopMusic();

        if (!denied)
        {
            AddToCurrentList(prompt);
        }

        ChangeFace(0);

        StartCoroutine(NPCAnim());

        IEnumerator NPCAnim()
        {
            nPCGenerator.PlayAnimation(false);
            yield return new WaitForSeconds(2.3f);
            
            NextPrompt();
        }
    }


    public void AddToCurrentList(PrompScript prompt)
    {
        currentFundedProjects.Add(prompt);
        recalculateCurrentGold(-int.Parse(prompt.FundCost.text));
    }

    
    [SerializeField] CoinStashManager csm;

    public void recalculateCurrentGold(int value)
    {
        currentGold += value;
        if (currentGold >= 1000000)
        {
            endController.GameEnd(true);//WIN
            StartMusic(winGamePath);
        }

        else if (currentGold <= 0) 
        {
            endController.GameEnd(false);//LOSE
            StartMusic(loseGamePath);
        } 
        currentGoldText.text = currentGold.ToString();
        //csm.SetAmount(currentGold);
    }

    public void NextPrompt()
    {
        
        if (peopleSeen < currentTotalPeople)
        {
            nPCGenerator.GenerateNPC();

            StartCoroutine(NPCAnim());

            IEnumerator NPCAnim()
            {
                nPCGenerator.PlayAnimation(true);
                yield return new WaitForSeconds(2f);

                GameObject newPrompt = Instantiate(promptPrefab);
                newPrompt.transform.SetParent(promptPosition, false);

                GenerateNewPrompt(newPrompt.GetComponent<PrompScript>());


                //newPrompt.transform.SetParent(promptPosition);
                peopleSeen++;
                SetPeopleInRound();
            } 
        }
        else
        {
            Funding = false;
            Interlude();
        }
        
    }

    public void Interlude()
    {

        StartMusic(interludePath);

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
                switch (currentFundedProjects[i]._colorId)
                {
                    case 0:
                        newResult.transform.GetChild(2).gameObject.SetActive(true);
                        break;
                    case 1:
                        newResult.transform.GetChild(3).gameObject.SetActive(true);
                        break;
                    case 2:
                        newResult.transform.GetChild(4).gameObject.SetActive(true);
                        break;
                    case 3:
                        newResult.transform.GetChild(5).gameObject.SetActive(true);
                        break;
                }
                yield return new WaitForSeconds(0.3f);

                //calculate if winner winner chicken dinner
                if (CalculateWinnerInRange(currentFundedProjects[i].percentageRangeA, currentFundedProjects[i].percentageRangeB))
                {
                    newResult.transform.GetChild(0).gameObject.SetActive(true);
                    currentGold += (int)(currentFundedProjects[i].fundCostValue * currentFundedProjects[i].ROIPercentageValue);
                    currentGoldText.text = currentGold.ToString();
                    currentFundedProjects[i].hasWon = true;

                    //Assign value to slider
                    switch(currentFundedProjects[i]._colorId){
                        case 0:
                            militarTimesUpgraded += 1;
                            break;
                        case 1:
                            commerceTimesUpgraded += 1;
                            break;
                        case 2:
                            religionTimesUpgraded += 1;
                            break;
                        case 3:
                            peopleTimesUpdated += 1;
                            break;
                    }
                    UpdateSliders();
                    if (peopleTimesUpdated >= 3) currentTotalPeople = 10;
                    if (peopleTimesUpdated >= 3)
                    {
                        militarTimesUpgraded += 1;
                        commerceTimesUpgraded += 1;
                        religionTimesUpgraded += 1;
                    }
                }
                    
                else newResult.transform.GetChild(1).gameObject.SetActive(true);
                yield return new WaitForSeconds(0.15f);
            }


            interludeFinished = true;
            continueButton.SetActive(true);

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
        prompt.setColor(_id);
        prompt.setAffinityColor(_id);
        prompt.setAffinityShadow(_id);
  
        prompt.setFundCost(CalculateFundCost().ToString()); //Fund cost

        CalculatePercentageRange(prompt);
        switch (prompt._colorId)
        {
            case 0:
                if (militarTimesUpgraded >= 1)
                {
                    prompt.setPercentageRange(lowRange, highRange, true);
                }
                else prompt.setPercentageRange(lowRange, highRange, false);
                break;
            case 1:
                if (commerceTimesUpgraded >= 1)
                {
                    prompt.setPercentageRange(lowRange, highRange, true);
                }
                else prompt.setPercentageRange(lowRange, highRange, false);
                break;
            case 2:
                if (religionTimesUpgraded >= 1)
                {
                    prompt.setPercentageRange(lowRange, highRange, true);
                }
                else prompt.setPercentageRange(lowRange, highRange, false);
                break;
            case 3:
                if (peopleTimesUpdated >= 1)
                {
                    prompt.setPercentageRange(lowRange, highRange, true);
                }
                else  prompt.setPercentageRange(lowRange, highRange, false);
                break;
        }
       
        //IF NO FIRST PERK ??? INSTEAD OF VALUE UP  //Percentage Range

        prompt.setROIPercentage(CalculateROI()); //ROI Percentage
    }


    public void CalculatePercentageRange(PrompScript prompt)
    {

        int startingLowRangeValue = 25;

        if (religionTimesUpgraded >= 3) startingLowRangeValue = 35;

        //Bingo
        float bingoCheck = bingoChance;
        float checking = Random.Range(0f, 10f);
        if (checking <= bingoCheck)
        {
            if (!hasBingoReligionAppeared && religionTimesUpgraded >= 5) startingLowRangeValue = 90;
        }

        float amountToMultiply = startingLowRangeValue * 0.05f;
        
        switch (prompt._colorId)
        {
            case 0:
                if (militarTimesUpgraded >= 1)
                {
                    amountToMultiply *= militarTimesUpgraded;
                    lowRange = Random.Range((int)amountToMultiply, 50 - 1);
                }else lowRange = Random.Range(startingLowRangeValue, 50 - 1);
                break;
            case 1:
                if (commerceTimesUpgraded >= 1)
                {
                    amountToMultiply *= commerceTimesUpgraded;
                    lowRange = Random.Range((int)amountToMultiply, 50 - 1);
                }else lowRange = Random.Range(startingLowRangeValue, 50 - 1);
                break;
            case 2:
                if (religionTimesUpgraded >= 1)
                {
                    amountToMultiply *= religionTimesUpgraded;
                    lowRange = Random.Range((int)amountToMultiply, 50 - 1);
                }else lowRange = Random.Range(startingLowRangeValue, 50 - 1);
                break;
            case 3:
                if (peopleTimesUpdated >= 1)
                {               
                    amountToMultiply *= peopleTimesUpdated;
                    lowRange = Random.Range((int)amountToMultiply, 50 - 1);
                }
                else lowRange = Random.Range(startingLowRangeValue, 50 - 1);
                break;
        }

        highRange = Random.Range(lowRange, 75);

    }

    public float CalculateROI()
    {
        //Bingo
        float bingoCheck = bingoChance;
        float checking = Random.Range(0f,10f);
        if (checking <= bingoCheck)
        {
            if (!hasBingoMilitarAppeared && militarTimesUpgraded >= 5) return Random.Range(5f, 8f);
        }

        

        if (militarTimesUpgraded >=3) return Random.Range(1.75f, 3f);

        return Random.Range(1.25f, 2f);
    }

    public int CalculateFundCost()
    {
        int lowFundCost = 20;
        int highFundCost = 45;
        if(commerceTimesUpgraded >= 3)
        {
            lowFundCost = 20;
            highFundCost = 65;
        }

        float lowFundCostCurrent = currentGold * (lowFundCost / 100f);
        float highFundCostCurrent = currentGold * (highFundCost / 100f);

        //Bingo
        float bingoCheck = bingoChance;
        float checking = Random.Range(0f, 10f);
        if (checking <= bingoCheck)
        {
            if (!hasBingoCommerceAppeared && commerceTimesUpgraded >= 5) return (int)(roundStartingGold * (90f / 100f));
        }
         
        return (int)Random.Range(lowFundCostCurrent, highFundCostCurrent);


    }

    public bool CalculateWinnerInRange(int a, int b)
    {
        int targetChance = Random.Range(a, b);
        int value = Random.Range(0, 100);

        if (value <= targetChance) return true;

        return false;

    }

    public void UpdateSliders()
    {
        militarSlider.value = militarTimesUpgraded * 1;
        commerceSlider.value = commerceTimesUpgraded * 1;
        religionSlider.value = religionTimesUpgraded * 1;
        peopleSlider.value = peopleTimesUpdated * 1;
    }

    public void ChangeFace(int a)
    {
        if (a == 0)
        {
            head1.gameObject.SetActive(true);
            head2.gameObject.SetActive(false);
            head3.gameObject.SetActive(false);
        }
        else if (a == 1)
        {
            head1.gameObject.SetActive(false);
            head2.gameObject.SetActive(true);
            head3.gameObject.SetActive(false);
        }
        else
        {
            head1.gameObject.SetActive(false);
            head2.gameObject.SetActive(false);
            head3.gameObject.SetActive(true);
        }
    }

    public void SetPeopleInRound()
    {
        peopleText.text = "Folk this round: " + peopleSeen + " / " + currentTotalPeople;
    }
    public void SetCurrentRound()
    {
        roundText.text = "Current Round: " + roundNumber + " / " + totalRounds;
    }

    public void NextRoundButton()
    {
        continueButton.SetActive(false);
        interludeFinished = false;
        StartCoroutine(SlideOut());
        Funding = true;


        nextRound();
    }


    public void StartMusic(string path)
    {

        UnityEngine.Debug.Log("sueno");
        if (is1Playing)
        {
            eventInstance1.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventInstance1.release();

            eventInstance2 = FMODUnity.RuntimeManager.CreateInstance(path);

            eventInstance2.start();
            eventInstance2.setVolume(0.5f);

            is1Playing = false;
        }
        else
        {
            eventInstance2.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventInstance2.release();

            eventInstance1 = FMODUnity.RuntimeManager.CreateInstance(path);

            eventInstance1.start();
            eventInstance1.setVolume(0.5f);

            is1Playing = true;
        }

    }

    public void StopMusic()
    {
       // instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
       // instance.release();
    }
}
