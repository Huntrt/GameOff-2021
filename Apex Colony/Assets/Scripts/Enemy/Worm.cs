using UnityEngine;

public class Worm : MonoBehaviour
{
	public LineRenderer line;
	[Tooltip("The total length of this line")]
	public float length;
	[Tooltip("How far each segment are from each other")]
	public float distance;
	public Vector2[] segments;
	Vector2 head; int vertices;

    void Start()
    {
		//Get how many segments need base on length deivde with distance
		segments = new Vector2[(int)(length / distance)];
		//Get the amount of vertices
		vertices = segments.Length;
		//The first segment are at this object
		segments[0] = transform.position;
		//Go through all of the vertices
        for (int s = 1; s < vertices; s++)
		{
			//This segment are the previous segment increase downward with distance 
			segments[s] = segments[s-1] + (Vector2.left * distance);
		}
		//Update line
		UpdateLine();
    }

    void LateUpdate()
    {
		//Get the head position
        head = transform.position;
		//Update the first segment
		segments[0] = head;
		//The total length of line
		float total = 0;
		//Get the total length of line by increase each distance between vertices
		for (int l = vertices - 1; l >= 1 ; l--) {total += Vector2.Distance(segments[l], segments[l-1]);}
		//Update all the vertices position if the line are longer than needed
		if(total > length) {for (int s = vertices - 1; s >= 1 ; s--) {segments[s] = segments[s-1];}}
		//Update line's vertices
		UpdateLine();
    }

	void UpdateLine()
	{
		//Update the line vertices count if it not the same as segment count
		if(segments.Length != line.positionCount) {line.positionCount = segments.Length;}
		//Update each vertice of line renderer to be it related segment position
		for (int l = 0; l < line.positionCount; l++) {line.SetPosition(l, segments[l]);}
	}
}
