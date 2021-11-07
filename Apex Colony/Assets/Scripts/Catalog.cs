using UnityEngine;
using System;

//State of entity combating
[Serializable] public enum combating {none, chase, fight}
//List of int use as min max storage
[Serializable] public class IntMinMax {public int min; public int max; public int raw;}