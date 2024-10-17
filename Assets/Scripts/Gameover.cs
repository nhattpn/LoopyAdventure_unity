using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Gameover : MonoBehaviour
{
   public void PlayAgain (){
       SceneManager.LoadSceneAsync(1);
   }
   public void GotoMenu (){
       SceneManager.LoadSceneAsync(0);
   }
}