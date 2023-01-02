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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void nextRound()
    {

    }

    public void AddToCurrentList(PrompScript promp)
    {

        PrompScript newPromp = new PrompScript();

        promp.Copy(newPromp);
 
        currentFundedProjects.Add(promp);
        
        promp.gameObject.SetActive(false);
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
}
