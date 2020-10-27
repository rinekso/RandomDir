using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRandom : MonoBehaviour
{
    float randomDeg;
    public GyroController gyro;
    public GameObject right;
    public GameObject left;
    // Start is called before the first frame update
    void Start()
    {
        RandDir();
        // InvokeRepeating("RandDir",0,10);
    }
    void RandDir(){
        randomDeg = Random.Range(0,360);
        StartCoroutine(move(randomDeg, 1));
    }
    IEnumerator move(float target,float duration){
        Vector3 startRotate = transform.eulerAngles;
        float t = 0;
        while(t<1){
            t+= Time.deltaTime/duration;
            transform.eulerAngles = Vector3.Lerp(startRotate,new Vector3(0,target,0),t);
            yield return null;
        }
        transform.eulerAngles = new Vector3(0,target,0);
    }
    int selisih(int alpha, int beta){
        int phi = (beta - alpha) % 360;       // This is either the distance or 360 - distance
        int distance = Mathf.Abs(phi) > 180 ? 360 - phi : phi;
        return distance;
    }
    int treshold = 20;
    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButtonDown(0)){
            RandDir();
        }
        float cam=gyro.rotAzm;
        float arrow = randomDeg;
        int sel = selisih((int)cam,(int)arrow);
        print(sel);
        if(sel > treshold){
            Right();
        }else if(sel < (0-treshold)){
            Left();
        }else{
            Netral();
        }
    }
    void Right(){
        right.SetActive(true);
        left.SetActive(false);
    }
    void Left(){
        left.SetActive(true);
        right.SetActive(false);
    }
    void Netral(){
        left.SetActive(false);
        right.SetActive(false);
    }
}
