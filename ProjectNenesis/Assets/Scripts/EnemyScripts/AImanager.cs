using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class AImanager : MonoBehaviour
{
    public GameObject enemy; //Enemy instance
    public GameObject player; //Player instance

    //Neural network variables
    public bool isTraining = false;
    private int[] layers = new int[] {2, 5, 5, 5, 5, 2}; //Two inputs, four hidden layers and two outputs
    private List<NeuralNetwork> nets;
    private int generationNumber = 0;
    public int populationSize = 30;
    public int hits = 0;

    public enemyAI enemyAI;

    public GameObject enemySpawner;
    public Transform playerSpawner;

    //In game info
    [SerializeField] GameObject infoPanel;
    string infoStr;
    public Text infoText;

    bool isPause = true;

    // [SerializeField]
    int index;

    public GameObject enemyDeadTrans;

    void start()
    {
    }

    IEnumerator Timer(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        isTraining = false;
        Debug.Log("Timer");
    }

    // Update is called once per frame
    void Update()
    {
        if (generationNumber == 0)
        {
            InitEnemyNeuralNetwork();

        }
        if (!isTraining)
        {

            nets.Sort();
            for (int i = 0; i < populationSize / 2; i++)
            {
                nets[i] = new NeuralNetwork(nets[i + (populationSize / 2)]);
                nets[i].Mutate();

                nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]);
            }

            for (int i = 0; i < populationSize; i++)
            {
                index = i;
                nets[i].SetFitness(0f);
            }

            generationNumber++;
            isTraining = true;
            createEnemy();
        }
        printInfo(); //Print all the info at the Canvas panel
        //Gizmos.DrawLine(enemySpawner.transform.position, enemy.transform.position);
    }


    public void createEnemy()
    {
        //Saves user data every "reset"
        //string name = GameObject.Find("InfoManager").GetComponent<MenuScript>().experimentsName;
        //string level = GameObject.Find("InfoManager").GetComponent<MenuScript>().gamingLevel;
        //writeData(generationNumber, hits, name, level);

        //Moves enemy and player to start point
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = playerSpawner.position;
        player.GetComponent<CharacterController>().enabled = true;

        Destroy(GameObject.Find("UusPahis(Clone)"));
        enemyAI enem = ((GameObject)Instantiate(enemy, enemySpawner.transform.position, enemySpawner.transform.rotation)).GetComponent<enemyAI>();

        for (int i = 0; i < populationSize; i++)
        {
            enem.init(nets[i], player.transform);
        }
    }

    void InitEnemyNeuralNetwork()
    {

        if (populationSize % 2 != 0)
        {
            populationSize = 20;
        }

        nets = new List<NeuralNetwork>();

        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Mutate();
            nets.Add(net);
        }
    }

    void printInfo()
    {
        infoStr = "Generations: " + generationNumber + "\nDeaths: " + hits
        + '\n' + GameObject.Find("UusPahis(Clone)").GetComponent<enemyAI>().enemyInfo()
        + "\nisTraining: " + isTraining;
        infoText.text = infoStr;

    }

    //[MenuItem("Tools/Write file")]
    static void writeData(int gn, int kills, string name, string level)
    {
        //Make txt file to OutputData folder
        string path = "Assets/OutputData/" + name + "Experiments.txt";

        //This writes text to txt file
        StreamWriter writer = new StreamWriter(path);
        writer.WriteLine("Experiment: " + name + " gaming level: " + level +
        "\nGenerations: " + gn + "\tKills: " + kills);
        writer.Close();
    }
}


//Re-import the file to update the reference in the editor
// AssetDatabase.ImportAsset(path);
// TextAsset tAsset = Resources.Load("test");