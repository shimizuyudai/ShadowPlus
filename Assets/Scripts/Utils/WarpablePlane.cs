using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.Calib3dModule;

public class WarpablePlane : MonoBehaviour
{
	[SerializeField]
	KeyCode toggleKey;
	[SerializeField]
	KeyCode exportKey;
	[SerializeField]
	string fileName;
	[SerializeField]
	bool isEnable;
    public bool IsEnable
    {
        get
        {
            return isEnable;
        }
    }

	[SerializeField]
	Camera cam;

	[SerializeField]
	MeshFilter meshFilter;

	public Vector2 size;
	public int SegmentW;
	public int SegmentH;
	private CornerPoint[] defaultCornerPoints;
	private CornerPoint[] cornerPoints;
    public CornerPoint[] CornerPoints
    {
        get
        {
            return cornerPoints;
        }
    }
	private Vector3[] defaultVertices;

	public int[] cornerPointIndices;

	public int[] CornerPointIndices {
		get {
			return cornerPointIndices;
		}
	}

	private int selectedIndex;

	[SerializeField]
	float touchDistanceThreshold;
    public float TouchDistanceThreshold
    {
        get
        {
            return touchDistanceThreshold;
        }
    }

	public Mesh Mesh{ get { return meshFilter.mesh; } }

	[SerializeField]
	bool isWarp;

	[SerializeField]
	bool isAutoLoad;

	void Awake ()
	{
		create ();
		if (isAutoLoad) {
			LoadSettings (fileName);
		}
	}

	public void OnPointerUp ()
	{
		isWarp = false;
	}

	public void OnPointerDown (Vector3 pos)
	{
		var orderedCornerPoints = cornerPoints.OrderBy (e => Vector3.Distance (e.position, pos));
		if (Vector3.Distance (orderedCornerPoints.First ().position, pos) < touchDistanceThreshold) {
			selectedIndex = orderedCornerPoints.First ().index;
			isWarp = true;
		}
	}

	void create ()
	{
		var mesh = new Mesh ();
		var startPos = new Vector3 (-size.x / 2.0f, size.y / 2.0f);
        //startPos += this.transform.position;

		var step = new Vector2 (size.x / (float)(SegmentW - 1), size.y / (float)(SegmentH - 1));
		var vertices = new List<Vector3> ();
		var uvs = new List<Vector2> ();
		for (var y = 0; y < SegmentH; y++) {
			for (var x = 0; x < SegmentW; x++) {
				var pos = startPos + new Vector3 (step.x * (float)x, -step.y * (float)y, startPos.z);
				var uv = new Vector2 ((float)x / (float)(SegmentW - 1), 1f - (float)y / (float)(SegmentH - 1));
				vertices.Add (pos);
				uvs.Add (uv);
			}
		}
		defaultVertices = vertices.ToArray ();

		cornerPoints = new CornerPoint[] {
			new CornerPoint (0, 0, vertices [0]),
			new CornerPoint (SegmentW - 1, 1, vertices [SegmentW - 1]),
			new CornerPoint (SegmentW * (SegmentH - 1), 2, vertices [SegmentW * (SegmentH - 1)]),
			new CornerPoint (SegmentW * (SegmentH - 1) + SegmentW - 1, 3,  vertices [SegmentW * (SegmentH - 1) + SegmentW - 1])
		};

		defaultCornerPoints = new CornerPoint[] {
			new CornerPoint (0, 0, vertices [0]),
			new CornerPoint (SegmentW - 1, 1, vertices [SegmentW - 1]),
			new CornerPoint (SegmentW * (SegmentH - 1), 2, vertices [SegmentW * (SegmentH - 1)]),
			new CornerPoint (SegmentW * (SegmentH - 1) + SegmentW - 1, 3,  vertices [SegmentW * (SegmentH - 1) + SegmentW - 1])
		};

		mesh.vertices = vertices.ToArray ();
		mesh.uv = uvs.ToArray ();

		var triangles = new List<int> ();
		for (var y = 0; y < SegmentH; y++) {
			for (var x = 0; x < SegmentW; x++) {
				if (x + 1 < SegmentW && y + 1 < SegmentH) {
					var index = x + (y * SegmentW);
					triangles.Add (index);
					triangles.Add (index + 1);
					triangles.Add (index + SegmentW);
					triangles.Add (index + SegmentW);
					triangles.Add (index + 1);
					triangles.Add (index + SegmentW + 1);
				}
			}
		}
		mesh.triangles = triangles.ToArray ();
		mesh.RecalculateNormals ();
		meshFilter.mesh = mesh;

	}

	public void UpdateMesh ()
	{
		meshFilter.mesh.RecalculateBounds ();
		meshFilter.mesh.RecalculateNormals ();
	}


	public void LoadSettings (string fileName)
	{
		var path = Path.Combine (Application.streamingAssetsPath, fileName);
		if (File.Exists (path)) {
			var json = File.ReadAllText (path);
			var corners = JsonConvert.DeserializeObject<CornerPointInfomation[]> (json);
			foreach (var corner in corners) {
				var selectedCornerPoint = cornerPoints.FirstOrDefault (e => e.number == corner.number);
				if (selectedCornerPoint != null) {
					selectedCornerPoint.position = new Vector2 (corner.position.x, corner.position.y);
				}
			}
			refresh ();
		}
	}

	public void Export (string fileName)
	{
		var cornerPointsInfomation = cornerPoints.Select (e => new CornerPointInfomation (e.number,  new Vec2 (e.position.x, e.position.y))).ToArray ();
		var json = JsonConvert.SerializeObject (cornerPointsInfomation);
		var path = Path.Combine (Application.streamingAssetsPath, fileName);
		File.WriteAllText (path, json);
	}

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (toggleKey)) {
			isEnable = !isEnable;
           
		}

		if (Input.GetKeyDown (exportKey)) {
			Export (fileName);
		}

        if (!isEnable)
			return;
		
		if (Input.GetMouseButtonDown (0)) {
			if (!isWarp) {
				var worldMousePos = cam.ScreenToWorldPoint (Input.mousePosition);
				worldMousePos.z = 0f;
				OnPointerDown (worldMousePos);
			}
		} else if (Input.GetMouseButtonUp (0)) {
			if (isWarp) {
				OnPointerUp ();
			}
		}
		if (isWarp) {
			warp ();
		}

	}

	void warp ()
	{
		//まず頂点の移動
		var worldMousePos = cam.ScreenToWorldPoint (Input.mousePosition);
		worldMousePos.z = 0f;
		var selectedCorner = cornerPoints.FirstOrDefault (e => e.index == selectedIndex);
		if (selectedCorner != null) {
			selectedCorner.position = worldMousePos;
		}
		refresh ();
	}

	void refresh ()
	{
		var defaultPoints = defaultCornerPoints.Select (e => e.point).ToArray ();
		var destPoints = cornerPoints.Select (e => e.point).ToArray ();
		using (var defaultCornerMat = new MatOfPoint2f (defaultPoints))
		using (var destCornerMat = new MatOfPoint2f (destPoints))
		using (var defaultMat = new MatOfPoint2f (defaultVertices.Select (e => new Point (e.x, e.y)).ToArray ()))
		using (var destMat = new MatOfPoint2f (meshFilter.mesh.vertices.Select (e => new Point (e.x, e.y)).ToArray ())) {
			var h = Calib3d.findHomography (defaultCornerMat, destCornerMat);
			Core.perspectiveTransform (defaultMat, destMat, h);
			var vertices = destMat.toList ().Select (e => new Vector3 ((float)e.x, (float)e.y, 0f)).ToList ();//resultPoints.Select (e => new Vector3((float)e.x,(float)e.y,0f)).ToList();
			meshFilter.mesh.SetVertices (vertices);
		}
	}

	public class CornerPoint
	{
		public Vector2 position;
		public int index;
		public int number;

		public Point point {
			get {
				return new Point (){ x = this.position.x, y = this.position.y };
			}
		}

		public CornerPoint (int index, int number, Vector2 position)
		{
			this.index = index;
			this.number = number;
			this.position = position;
		}
	}


	public class CornerPointInfomation
	{
		public int index;
		public int number;
		public Vec2 position;

		public CornerPointInfomation (int number, Vec2 position)
		{
			this.number = number;
			this.position = position;
		}
	}

	public struct Vec2
	{
		public float x;
		public float y;

		public Vec2 (float x, float y)
		{
			this.x = x;
			this.y = y;
		}
	}
}




