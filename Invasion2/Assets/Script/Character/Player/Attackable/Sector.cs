﻿using UnityEngine;

public class Sector : MonoBehaviour,IMainAttackable
{
    public GameObject SpawnPosition;
    public GameObject BulletPrefab;
   
    Vector3 SpaPosition;

    public void Attack(int power)
    {
        if (power == 10)
        {

        }
        else if(power == 20)
        {

        }
        else if (power == 30)
        {

        }
    }

   
    void Start ()
    {
		
	}
	
	
	void Update ()
    {
		
	}
}
