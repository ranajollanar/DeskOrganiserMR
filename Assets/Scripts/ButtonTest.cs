using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour
{
   [SerializeField] private GameObject testObject;


   public void ToggleObject()
   {
      testObject.SetActive(!testObject.activeSelf);
   }
}
