Shader "Imperial Fur/Main/Metallic/20 Shell" {
	Properties {
        _FurColor ("Fur Color", Color) = (1,1,1,1)
        _MainTex ("Fur Texture (RGB)", 2D) = "white" { }
        _ControlTex ("Control Texture (RGB)", 2D) = "white" { }               
        _NoiseTex ("Noise Texture (RGB)", 2D) = "white" { }
        _FurGlossiness("Smoothness", Range(0.0, 1.0)) = 0.5
        _FurMetallic ("Metallic", Range(0.0, 1.0)) = 0.0
		_FurMetallicMap("Metallic", 2D) = "white" {}

		_FurBumpScale("Normal Scale", Float) = 1.0
		[Normal]_FurBumpMap("Normal Map", 2D) = "bump" {}	
                                                
        _Color ("Color", Color) = (1,1,1,1)
		_SkinTex ("Skin Albedo (RGB)", 2D) = "white" {}

		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		[Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
		_MetallicGlossMap("Metallic", 2D) = "white" {}
		
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
		#pragma surface surfSkin Standard fullforwardshadows vertex:vertSkin
		#pragma target 3.0		
		#define CURRENTLAYER 0.0
		#define NOISEFACTOR 0.0
 		#include "ImperialFurMetallic.cginc"
		ENDCG

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.05
		#define NOISEFACTOR 0.05
 		#include "ImperialFurMetallic.cginc"
		ENDCG
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.1
		#define NOISEFACTOR 0.1
 		#include "ImperialFurMetallic.cginc"
		ENDCG

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.15
		#define NOISEFACTOR 0.15
 		#include "ImperialFurMetallic.cginc"
		ENDCG

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.20
		#define NOISEFACTOR 0.20
 		#include "ImperialFurMetallic.cginc"
		ENDCG

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.25
		#define NOISEFACTOR 0.25
 		#include "ImperialFurMetallic.cginc"
		ENDCG

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.3
		#define NOISEFACTOR 0.3
 		#include "ImperialFurMetallic.cginc"
		ENDCG
	
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.35
		#define NOISEFACTOR 0.35
 		#include "ImperialFurMetallic.cginc"
		ENDCG
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.4
		#define NOISEFACTOR 0.4
 		#include "ImperialFurMetallic.cginc"
		ENDCG
			
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.45
		#define NOISEFACTOR 0.45
 		#include "ImperialFurMetallic.cginc"
		ENDCG
				
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.5
		#define NOISEFACTOR 0.5
 		#include "ImperialFurMetallic.cginc"
		ENDCG 
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.55
		#define NOISEFACTOR 0.55
 		#include "ImperialFurMetallic.cginc"
		ENDCG
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.6
		#define NOISEFACTOR 0.6
 		#include "ImperialFurMetallic.cginc"
		ENDCG

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.65
		#define NOISEFACTOR 0.65
 		#include "ImperialFurMetallic.cginc"
		ENDCG

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.7
		#define NOISEFACTOR 0.7
 		#include "ImperialFurMetallic.cginc"
		ENDCG

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.75
		#define NOISEFACTOR 0.75
 		#include "ImperialFurMetallic.cginc"
		ENDCG

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.8
		#define NOISEFACTOR 0.8
 		#include "ImperialFurMetallic.cginc"
		ENDCG
	
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.85
		#define NOISEFACTOR 0.85
 		#include "ImperialFurMetallic.cginc"
		ENDCG
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.9
		#define NOISEFACTOR 0.9
 		#include "ImperialFurMetallic.cginc"
		ENDCG
			
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 0.95
		#define NOISEFACTOR 0.95
 		#include "ImperialFurMetallic.cginc"
		ENDCG
				
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend vertex:vert
		#pragma target 3.0
		#define CURRENTLAYER 1.0
		#define NOISEFACTOR 1.0
 		#include "ImperialFurMetallic.cginc"
		ENDCG 			
	} 
	
 
	FallBack "VertexLit"
    CustomEditor "ImperialFurShaderGUI"
}
