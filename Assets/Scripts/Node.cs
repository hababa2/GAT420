using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
        public static T[] GetNodes<T>() where T : Component
        {
                return FindObjectsOfType<T>();
        }
}