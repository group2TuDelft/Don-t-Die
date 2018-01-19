using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General idea of how the script is supposed to work:
/// - The player will have a radius around it wherein NPCs are active.
/// - If NPCs leave this radius they will deactivate.
/// - If NPCs renter this radius they will reactivate.
/// - Waves Will be triggered by Time with some difficulty factor.
/// </summary>

public class SpawningScript2 : MonoBehaviour
{

    public float despawnradius = 100f;
    public float alienspawnradius = 40f;
    public float spawnradius = 4f;     // Afstand
    private float spawncooldowntimer = 0f;   // soort van timer tussen het spawnen van enemies
    public float spawncooldown = 3f; // Tijd die tussen het spannen gaan zitten
    public float groupspawnradius = 3f; //Afstand waar tussen enemies in een group bij elkaar spawnen

    public float mediumtimestart = 300f; //Time wanneer enemies medium diffeculty gaan worden
    public float hardtimestart = 600f;  //Time wanneer enemies hard diffeculty gaan worden
    public float waveinterval = 100f;   //Intervallen waartussen grote waves mogen komen

    public int despawncheckfraction = 2;
    private float gametimer; // De tijd dat er gespeeld wordt


    // Game object list[0] = Zergling, [0][0] is easy zergling
    private List<List<GameObject>> prefablist = new List<List<GameObject>>();
    private List<List<int>> spawningroupamount = new List<List<int>>();
    private List<List<int>> spawngroups = new List<List<int>>();

    private Transform playertr;
    public List<GameObject> Enemies = new List<GameObject>();

    // Enemy Prefabs:

    [SerializeField] GameObject EasyHumanoid;
    [SerializeField] GameObject MediumHumanoid;
    [SerializeField] GameObject HardHumanoid;

    [SerializeField] GameObject EasyZergling;
    [SerializeField] GameObject MediumZergling;
    [SerializeField] GameObject HardZergling;

    [SerializeField] GameObject EasyHivemind;
    [SerializeField] GameObject MediumHivemind;
    [SerializeField] GameObject HardHivemind;



    // Use this for initialization
    void Start()
    {
        prefablist.Add(new List<GameObject> { EasyZergling, MediumZergling, HardZergling });
        prefablist.Add(new List<GameObject> { EasyHumanoid, MediumHumanoid, HardHumanoid });
        prefablist.Add(new List<GameObject> { EasyHivemind, MediumHivemind, HardHivemind });

        spawningroupamount.Add(new List<int> { 2, 3, 4 }); // Deze lijsten kunnen aangepast worden op te balancen
        spawningroupamount.Add(new List<int> { 1, 2, 3 });
        spawningroupamount.Add(new List<int> { 1, 1, 1 });

        spawngroups.Add(new List<int> { 1, 3, 5});
        spawngroups.Add(new List<int> { 1, 2, 4 });
        spawngroups.Add(new List<int> { 1, 2, 3 });

        playertr = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        gametimer += Time.deltaTime;
        CheckSpawn();
        CheckWave();
        CheckActiveEnemies();
    }

    void CheckSpawn()
    {
        if (Time.fixedTime > spawncooldowntimer)
        {

            spawncooldowntimer += spawncooldown; // Als er iemand spawnt, tel the cooldowntijd bij de cooldown op
            //if (Time.fixedTime % 10 == 0) Hoezo?
            Spawn();


        }

    }

    void CheckWave()
    {
        // Nog implenteren    
    }

    void CheckActiveEnemies()
    {
        if (Time.fixedTime % despawncheckfraction == 0)
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i] != null)
                {
                    if ((playertr.position - Enemies[i].transform.position).magnitude > despawnradius && Enemies[i].activeSelf == true)
                    {
                        Enemies[i].SetActive(false);
                    }
                    if ((playertr.position - Enemies[i].transform.position).magnitude < despawnradius && Enemies[i].activeSelf == false)
                    {
                        Enemies[i].SetActive(true);
                    }
                }
            }

        }
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        return pos;
    }

    void Spawn()
    {
        int enemyindex = Random.Range(0, 3); // Pick a random enemy in order Zergling, Humanoid and Hivemind
        int difficultyindex = new int(); //0 easy, 1 medium, 2 hard
        if (gametimer < mediumtimestart)
        {
            difficultyindex = 0;
        }
        else if(gametimer < hardtimestart)
        {
            difficultyindex = 1;
        }
        else
        {
            difficultyindex = 2;
        }
        GameObject tobespawned = prefablist[enemyindex][difficultyindex];
        List<Vector3> spawnlocations = new List<Vector3>();
        spawnlocations = PickLocations(spawngroups[enemyindex][difficultyindex], spawningroupamount[enemyindex][difficultyindex]);
        for (int i = 0; i < spawnlocations.Count; i++)
        {
            Enemies.Add(Instantiate(tobespawned, spawnlocations[i], Quaternion.Euler(0, Random.Range(0, 180),0))); /////// Deze quaterion moet nog goed en 
        }
    }

    private List<Vector3> PickLocations(int groups, int ingroupammount)
    {
        List<Vector3> locations = new List<Vector3>();
        for (int i = 0; i < groups; i++)
        {
            FindFreeGroupSpot(locations, groups, ingroupammount);
        }
        FindFreeGroupMateSpots(locations, ingroupammount);
        return locations;
    }
    void FindFreeGroupSpot(List<Vector3> list, int groups, int ingroupammount) // Plaatst groups random om de player. Moet eigenlijk iets beter voor hard setting want wil dat ze echt aan alle kanten
    {                                                                           // Nu kunnen zo alsnog allemaal aan 1 kant spawnen met rng
        for (int i = 0; i < groups; i++)
        {
            Vector3 center = playertr.position;
            Vector3 suggestedposition = RandomCircle(center, alienspawnradius);
            Vector3 pos2 = suggestedposition;
            pos2.y = pos2.y + spawnradius; //Even vragen waar dit om gaat
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - suggestedposition);
            if (!Physics.CheckSphere(pos2, spawnradius)) // Als er geen object zit waar je wilt spawnen, dan {}
            {
                list.Add(suggestedposition);

            }
            else     // Als er wel een object zit waar je wilt spawnen, roep dan de functie opnieuw aan en krijg een andere random waarde
            {
                i -= 1;
            }

        }
    }
    void FindFreeGroupMateSpots(List<Vector3> list, int ingroupammount) // Kijkt naar elke group gevormd in vorige functie en zet en group mates in de buurt van
    {
        int groups = list.Count;
        for (int i = 0; i < groups; i++)
        {
            for (int j = 0; j < ingroupammount; j++)
            {
                Debug.Log("Error Check 4");
                Vector3 center = list[i];
                Vector3 suggestedposition = RandomCircle(center, groupspawnradius);
                Vector3 pos2 = suggestedposition;
                pos2.y = pos2.y + spawnradius; //Even vragen waar dit om gaat
                Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - suggestedposition);
                if (!Physics.CheckSphere(pos2, spawnradius)) // Als er geen object zit waar je wilt spawnen, dan {}
                {
                    list.Add(suggestedposition);
                }
                else     // Als er wel een object zit waar je wilt spawnen, roep dan de functie opnieuw aan en krijg een andere random waarde
                {
                    j -= 1;
                }

            }

        }

    }


}