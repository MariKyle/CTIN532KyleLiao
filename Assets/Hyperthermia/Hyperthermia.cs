using UnityEngine;
using System.Collections;

public class Hyperthermia : MonoBehaviour {
	public float Speed = 15;
	public float Range = 50;
	public Camera Camera_;
	public Texture ShuiText;
	public float OffsetPixel=0.03f;
	
	private RenderTexture outterLineTexture = null;
	
	public Shader HeatIslandShader;
    Material m_HeatIslandMaterial = null;
	protected Material HeatIslandMaterial {
		get {
			if (m_HeatIslandMaterial == null) {
                m_HeatIslandMaterial = new Material(HeatIslandShader);

			}
			return m_HeatIslandMaterial;
		} 
	}
	
	void Start () {
		outterLineTexture =  new RenderTexture( (int)GetComponent<Camera>().pixelWidth,(int)GetComponent<Camera>().pixelHeight, 16 );

		Camera_.targetTexture=outterLineTexture;
		HeatIslandMaterial.SetTexture("_ClipTex",outterLineTexture);
		HeatIslandMaterial.SetTexture("_OffsetTex",ShuiText);
		HeatIslandMaterial.SetFloat("_Speed", Speed);
		HeatIslandMaterial.SetFloat("_Range", Range);
		HeatIslandMaterial.SetFloat("_OffsetPixel", OffsetPixel);
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, HeatIslandMaterial);
	}
	
}
