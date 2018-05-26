using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Collections;



public class CPScript : MonoBehaviour {

	public string viewArrayName = "FullTreeViews";
	public string flowFArrayName = "FullTreeFlowFwd";
	public string flowRArrayName = "FullTreeFlowRev";


	public ComputeShader shader;
//	public RenderTexture rtex;

	private RenderTexture rtex;

	private Texture2DArray viewsArray;
	private	Texture2DArray flowFArray;
	private	Texture2DArray flowRArray;

	int kernelHandle;

	int nViews = 0;

	// Use this for initialization
	void Start () {


		viewsArray = Resources.Load<Texture2DArray> (viewArrayName);
		shader.SetTexture(kernelHandle,"_Views2darray",viewsArray);
		viewsArray.Apply();
	//	Shader.SetGlobalTexture ("_Views2darray",viewsArray);
		nViews = viewsArray.depth;


		flowFArray = Resources.Load<Texture2DArray> (flowFArrayName);
		shader.SetTexture(kernelHandle,"_FlowF2darray",flowFArray);
		flowFArray.Apply();
		// Shader.SetGlobalTexture ("_FlowF2darray",flowFArray);
		flowRArray = Resources.Load<Texture2DArray> (flowRArrayName);
		shader.SetTexture(kernelHandle,"_FlowR2darray",flowRArray);
		flowRArray.Apply();

		//Shader.SetGlobalTexture ("_FlowR2darray",flowRArray);

		// nViews = ((Texture2DArray)globt).depth;
	
		rtex = new RenderTexture(viewsArray.width,viewsArray.height,24);


		rtex.enableRandomWrite = true;
		rtex.Create();
		kernelHandle =  shader.FindKernel("CSMain");
		GetComponent<Renderer>().material.SetTexture("_MainTex",rtex);

		shader.SetTexture(kernelHandle,"Result",rtex);

	//	Texture2DArray viewsArray;
	//	viewsArray = Resources.Load<Texture2DArray> ("TreeViews");
	//	shader.SetTexture (kernelHandle,"_views2darray",viewsArray);
	//	shader.SetTexture (kernelHandle,"_2Dt",fred);
	}
	
	// Update is called once per frame
	int slice = 0;
	float view = 0f;
	float dirf = 0.01f;
	int dir = 1;
	void Update () {
	//	if (slice>=(nViews-1)) dir = -1;
	//	if (slice<0) dir = 1;
	//	slice +=dir;

		if (view>=0.0f + nViews - 1.0f) dirf = -0.01f;
		if (view<0) dirf = 0.01f;
		view += dirf;
		shader.SetFloat("_view",view);
		// Debug.Log(view);
	//	shader.SetInt("_slice",slice);
	//	viewsArray.Apply();
	//	flowFArray.Apply();
		shader.Dispatch(kernelHandle,rtex.width/8,rtex.height/8,1);
	}
}
