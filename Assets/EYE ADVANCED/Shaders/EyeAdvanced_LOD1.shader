Shader "EyeAdvanced/EyeAdvanced_LOD1" {

Properties {

	_pupilSize("Pupil Dilation", Range(0.0,1.0)) = 0.7
	_irisSize("Eye Iris Size", Range(1.5,5.0)) = 2.15
	_parallax("Parallax Effect", Range(0.0,0.05)) = 0.025
	_scleraSize("Eye Sclera Size", Range(0.85,2.2)) = 1.0

	_scleraColor ("Sclera Color", Color) = (1,1,1,1)
	_irisColor ("Iris Color", Color) = (1,1,1,1)
	_illumColor ("Iris Illumination", Color) = (1,1,1,1)
   	_subDermColor ("SubDermal Color", Color) = (0.5, 0.5, 0.5, 1)
	_brightShift("Brightness Shift", float) = 1.0
	_specsize("Specular", Range(0.0,1.0)) = 0.4
	_smoothness("Smoothness", Range(0.0,1.0)) = 0.9

	_IrisColorTex ("Iris Color", 2D) = "white" {}
	_IrisTex ("Iris Texture", 2D) = "white" {}
	_CorneaBump ("Cornea Normal Map", 2D) = "bump" {}
	_EyeBump ("Eye Normal Map", 2D) = "bump" {}
	_MainTex ("Sclera Texture", 2D) = "white" {}


}

SubShader { 
	Tags {"RenderType"="Opaque" "Queue"= "Geometry"}
	Cull Back

	
CGPROGRAM
#pragma target 3.0
#include "UnityPBSLighting.cginc"
#pragma surface surf StandardSpecular
#pragma glsl

sampler2D _IrisColorTex;
sampler2D _IrisTex;
sampler2D _MainTex;
float4 _albedoColor;
float4 reflectionMatte;
float4 irradianceTex;
float4 _subDermColor;
float3 albedoColor;
float _roughness;
float _reflective;
float _metalMap;
float _ambientMap;
float _skinMap;
float _irisSize;
float _scleraSize;
float _pupilSize;
sampler2D _CorneaBump;
sampler2D _EyeBump;
float4 _scleraColor;
float4 _irisColor;
float4 _irisColorB;
float4 _pupilColor;
float4 _illumColor;
float _parallax;
float _brightShift;
float irismasktex;
float _smoothness;
float _specsize;


struct Input {
	float2 uv_MainTex;
	float3 viewDir;
	float3 worldRefl;
	INTERNAL_DATA
};




void surf (Input IN, inout SurfaceOutputStandardSpecular o) {

	//CALCULATE NORMAL MAPS
	half3 cBump = UnpackNormal(tex2D(_CorneaBump, float2((IN.uv_MainTex.x*_irisSize)-((_irisSize-1.0)/2.0),(IN.uv_MainTex.y*_irisSize)-((_irisSize-1.0)/2.0))));

	//CALCULATE ALBEDO MAP (SCLERA)
	half4 scleratex = tex2D(_MainTex, float2((IN.uv_MainTex.x*_scleraSize)-((_scleraSize-1.0)/2.0),(IN.uv_MainTex.y*_scleraSize)-((_scleraSize-1.0)/2.0)));
	scleratex.rgb = lerp(scleratex.rgb,scleratex.rgb*_scleraColor.rgb,_scleraColor.a);
	half3 eBump = UnpackNormal(tex2D(_EyeBump, float2((IN.uv_MainTex.x*_scleraSize)-((_scleraSize-1.0)/2.0),(IN.uv_MainTex.y*_scleraSize)-((_scleraSize-1.0)/2.0))));
	
	//CALCULATE ALBEDO MAP (IRIS)
	irismasktex = tex2D(_MainTex, float2((IN.uv_MainTex.x*_irisSize)-((_irisSize-1.0)/2.0),(IN.uv_MainTex.y*_irisSize)-((_irisSize-1.0)/2.0))).a;

	//FINAL NORMAL COMBINATION
	o.Normal = lerp(eBump,cBump,irismasktex);


	// get mask texture
	half uvMask = 1.0-tex2D(_IrisTex,IN.uv_MainTex).b;


	//CALCULATE IRIS TEXTURE
	half iSize = _irisSize * 0.6;
	float2 irUVc = IN.uv_MainTex;
	irUVc = float2((IN.uv_MainTex.x*iSize)-((iSize-1.0)/2.0),((IN.uv_MainTex.y)*iSize)-((iSize-1.0)/2.0));
	_pupilSize = lerp(lerp(0.5,0.2,iSize/5),lerp(1.2,0.75,iSize/5),_pupilSize);
	irUVc = (irUVc*((-1.0+(uvMask*_pupilSize)))-(0.5*(uvMask*_pupilSize)));


	//CALCULATE IRIS/PUPIL MASK TEXTURES
	float2 irUV;
	irUV.x = lerp((IN.uv_MainTex.x*0.75)-((0.75-1.0)/2.0),(IN.uv_MainTex.x*_pupilSize)-((_pupilSize-1.0)/2.0),IN.uv_MainTex.x);
	irUV.y = lerp((IN.uv_MainTex.y*0.75)-((0.75-1.0)/2.0),(IN.uv_MainTex.y*_pupilSize)-((_pupilSize-1.0)/2.0),IN.uv_MainTex.y);

	//get iris and pupil texture
	half4 irisColTex = tex2D(_IrisColorTex,irUVc);

	//combine sclera and iris colors
	irisColTex.rgb = lerp(irisColTex.rgb,irisColTex.rgb*_irisColor.rgb,_irisColor.a);
	o.Albedo = lerp(scleratex.rgb*(1.0-irismasktex),irisColTex.rgb,irismasktex);

	//backscatter effects
	o.Emission = o.Albedo*(5.0*_illumColor.a)*_illumColor.rgb*irismasktex;
	
	//darken iris edges
	o.Albedo *= lerp(1.0,1.0,(1.0-irismasktex)*irismasktex*2);
	

	//------------------------------
	//##  CALCULATE ALPHA / CLIP  ##
	//------------------------------
	o.Alpha = 1.0;
	
	
	//-------------------------
	//##  UNITY 5 Features  ##
	//-------------------------
	o.Specular = lerp(0.05,0.2,irismasktex) * _specsize;
	o.Smoothness = lerp(0.5, 0.8,irismasktex) * _smoothness;
	o.Albedo = o.Albedo * _brightShift;

	
}
ENDCG



}

Fallback "Diffuse"

}
