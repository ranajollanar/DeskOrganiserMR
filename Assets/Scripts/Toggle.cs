using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour
{
   public void ToggleObject(GameObject testObject)
   {
      testObject.SetActive(!testObject.activeSelf);
   }
}
