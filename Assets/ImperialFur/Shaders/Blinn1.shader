Shader "Imperial Fur/Main/Blinn/ 1 Shell" {
	Properties {
        _FurColor ("Fur Color", Color) = (1,1,1,1)
        _MainTex ("Fur Texture (RGB)", 2D) = "white" { }
        _ControlTex ("Control Texture (RGB)", 2D) = "white" { }               
        _NoiseTex ("Noise Texture (RGB)", 2D) = "white" { }
        _FurGlossiness("Smoothness", Range(0.0, 1.0)) = 0.5
        _FurSpecular ("Specular", Range(0.0, 1.0)) = 0.2
		_FurSpecularMap("Specular", 2D) = "white" {}

		_FurBumpScale("Normal Scale", Float) = 1.0
		[Normal]_FurBumpMap("Normal Map", 2D) = "bump" {}	
                                                
        _Color ("Color", Color) = (1,1,1,1)
		_SkinTex ("Skin Albedo (RGB)", 2D) = "white" {}

		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		[Gamma] _Specular("Specular",  Range(0.0, 1.0)) = 0.2
		_SpecGlossMap("Specular", 2D) = "white" {}
		
		_BumpScale("Normal Scale", Float) = 1.0
		[Normal]_BumpMap("Normal Map", 2D) = "bump" {}	
		
        _StrandThickness ("Strand Density", Range(0,10)) = 1
        _MaxHairLength ("Max Strand Length", Range(0,0.5)) = 0.08
        _EdgeFade ("Edge Fade", Range(0,1)) = 0.4
        _ShadowStrength ("Shadow Strength", Range(0,1)) = 0.75
        _ShadowWeakness ("Shadow Weakness", Range(0,1)) = 0.75

      	_RimColor ("Rim Color", Color) = (0,0,1,0.0)
      	_RimPower ("Rim Power", Range(0.5,8.0)) = 2.0
              
        [HideInInspector] _RimLightMode ("__rimlightmode", Float) = 0.0
        [HideInInspector] _ShadowMode ("__shadowmode", Float) = 0.0
        [HideInInspector] _SkinMode ("__skinmode", Float) = 0.0
        
        [HideInInspector] _UseFurSecondMap ("__usefursecondmap", Float) = 0.0
        [HideInInspector] _UseHeightMap ("__useheightmap", Float) = 0.0
        [HideInInspector] _UseBiasMap ("__usebiasmap", Float) = 0.0
        [HideInInspector] _UseSkinSecondMap ("__useskinsecondmap", Float) = 0.0
        [HideInInspector] _UseStrengthMap ("__usestrengthmap", Float) = 0.0
 	}
	SubShader {
		ZWrite On
		Tags { "QUEUE"="Transparent" "RenderType"="Opaque" "IgnoreProjector"="True"}	
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200

		CGPROGRAM
		#pragma surface surfSkin BlinnPhong fullforwardshadows vertex:vertSkin
		#pragma target 3.0		
		#define CURRENTLAYER 0.0
		#define NOISEFACTOR 0.0
 		#include "ImperialFurBlinn.cginc"
		ENDCG
	
		CGPROGRAM
		#pragma surface surf BlinnPhong fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 1.0
		#define NOISEFACTOR 0.05
 		#include "ImperialFurBlinn.cginc"
		ENDCG		
	} 
	
	FallBack "VertexLit"
    CustomEditor "ImperialFurShaderGUI"
}
