// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "AAAA/Hyperthermia" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_OffsetTex ("Shui (RGB)", 2D) = "white" {}
	_ClipTex ("Clip (RGB)", 2D) = "white" {}
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }
				
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform sampler2D _OffsetTex;
uniform sampler2D _ClipTex;
uniform float _Speed;
uniform float _Range;
uniform float _OffsetPixel;

struct v2f {
	float4 pos : POSITION;
	float2 uv : TEXCOORD0;
};

v2f vert( appdata_img v )
{
	v2f o;
	o.pos = UnityObjectToClipPos (v.vertex);
	o.uv = v.texcoord;
	return o;
}

float4 frag (v2f i) : COLOR{
	float2 offset = i.uv;
    float4 clikColor=tex2D(_ClipTex,i.uv+ float2(_OffsetPixel, _OffsetPixel))/8;
    clikColor += tex2D( _ClipTex, i.uv+ float2(_OffsetPixel, -_OffsetPixel))/8;
	clikColor += tex2D( _ClipTex, i.uv+ float2(-_OffsetPixel, _OffsetPixel))/8;
	clikColor += tex2D( _ClipTex, i.uv+ float2(-_OffsetPixel, -_OffsetPixel))/8;
	clikColor += tex2D( _ClipTex, i.uv+ float2(_OffsetPixel, 0))/8;
	clikColor += tex2D( _ClipTex, i.uv+ float2(-_OffsetPixel, 0))/8;
	clikColor += tex2D( _ClipTex, i.uv+ float2(0, _OffsetPixel*2))/8;
	clikColor += tex2D( _ClipTex, i.uv+ float2(0, -_OffsetPixel))/8;
	
    float2 offsetUV=float2(i.uv.x,i.uv.y-_Time*_Speed);
	float4 shuiColor = tex2D(_OffsetTex, offsetUV);
	offsetUV = offset + shuiColor.xy/_Range;
    
    float ff=clikColor.a*1.2f;
	float2 uv=lerp (i.uv, offsetUV, ff);
	
	return tex2D(_MainTex, uv);
}
ENDCG

	}
}

Fallback off

}
