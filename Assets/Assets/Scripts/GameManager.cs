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



    public List<PrompScript> currentFundedProjects = new List<PrompScript>();
    public List<PrompScript> fundedProjectsRecord = new List<PrompScript>();

    public int currentGold;
    public TextMeshProUGUI currentGoldText;

    public GameObject promptPrefab;
    public RectTransform promptPosition, slideStartPos, slideEndPos, slideObject;

    //----Mulitpliers----//
    public float peopleMultiplier, rangeMultiplier, ROIMultiplier, fundCostMultiplier;

    public int currentTotalPeople, peopleSeen;



    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1366,768,true);



        peopleMultiplier = 1;
        rangeMultiplier = 1;
        ROIMultiplier = 1;
        fundCostMultiplier = 1;

        peopleSeen = 0;


        

        NextPrompt();
    }

    public void nextRound()
    {
        peopleSeen = 0;
    }

    public void AddToCurrentList(PrompScript prompt)
    {
 
        currentFundedProjects.Add(prompt);
        recalculateCurrentGold(-int.Parse(prompt.FundCost.text));




        NextPrompt();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            GameObject newPrompt = Instantiate(promptPrefab, promptPosition.anchoredPosition, Quaternion.identity);
            newPrompt.transform.SetParent(promptPosition, false);

            //newPrompt.transform.SetParent(promptPosition);
            peopleSeen++;
        }
        else
        {
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
                slideObject.anchoredPosition = Vector3.Lerp(slideStartPos.anchoredPosition, slideEndPos.anchoredPosition, (elapsedTime / 0.5f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        IEnumerator SlideOut(){

            float elapsedTime = 0;

            while (elapsedTime < 0.5f)
            {
                slideObject.anchoredPosition = Vector3.Lerp(slideEndPos.anchoredPosition, slideStartPos.anchoredPosition, (elapsedTime / 0.5f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
