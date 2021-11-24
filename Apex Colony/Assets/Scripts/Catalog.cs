using UnityEngine.Events;
using UnityEngine;
using System;

//State of entity combating
[Serializable] public enum combating {none, chase, fight}
//List of int use as min max storage
[Serializable] public class IntMinMax {public int min; public int max; public int raw;}
//List of int use as min max storage
[Serializable] public class FloatMinMax {public float min; public float max; public float raw;}
//Drop data containt the object drop and it ratio to drop
[Serializable] public class DropData {public GameObject obj; public float ratio;}
[Serializable] public class spriteState {public Color normal, highlight, press, select, disable;}