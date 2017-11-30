using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public Transform player;
    public Vector3 offset;
    public Vector3 rotoffset;
    //private Vector3 rotchange;


    // Update is called once per frame
    private void Start()
    {
        transform.eulerAngles = rotoffset;
    }
    void Update () {
        
        transform.position = player.position + offset;
     
        
		
	}
}
