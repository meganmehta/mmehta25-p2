using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerSound1 : MonoBehaviour
{
   public AudioSource playSound;
   void OnTriggerEnter(Collider other){
        playSound.Play();
   }

}
