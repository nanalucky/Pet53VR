using System;
using UnityEngine;
using UnityEditor;

public class ImperialFurShaderSimpleGUI : ShaderGUI
{
	private enum RimLightMode
	{
		None,
		Natural,
		AmbientLight,
		UserDefined
	}

	private enum ReflectionMode
	{
		None,
		HDR,
		LDR
	}
	
	private static class Styles
	{
		public static GUIStyle optionsButton = "PaneOptions";
		public static GUIContent uvSetLabel = new GUIContent ("UV Set");
		public static GUIContent[] uvSetOptions = new GUIContent[] {
			new GUIContent ("UV channel 0"),
			new GUIContent ("UV channel 1")
		};
		public static string emptyTootip = "";
		public static GUIContent furText = new GUIContent ("Diffuse", "Albedo (RGB) and Heightmap (A)");
		public static GUIContent controlText = new GUIContent ("Control", "Height Map (R) Strength Map (G)");
		public static GUIContent noiseText = new GUIContent ("Noise", "Noise");
		public static GUIContent strandText = new GUIContent ("Max Strand Length", "");
		public static GUIContent densityText = new GUIContent ("Strand Density", "");
		public static GUIContent shadowText = new GUIContent ("Shadow Strength", "");
		public static GUIContent fadeText = new GUIContent ("Edge Fade", "");
		public static GUIContent albedoText = new GUIContent ("Albedo", "Albedo (RGB) and Transparency (A)");
		public static GUIContent furSpecularMapText = new GUIContent ("Specular", "Specular (RGB) and Smoothness (A)");
		public static GUIContent furSmoothnessText = new GUIContent ("Smoothness", "");
		public static GUIContent specularMapText = new GUIContent ("Specular", "Specular (RGB) and Smoothness (A)");
		public static GUIContent metallicMapText = new GUIContent ("Metallic", "Metallic (R) and Smoothness (A)");
		public static GUIContent smoothnessText = new GUIContent ("Smoothness", "");
		public static GUIContent glossinessText = new GUIContent ("Glossiness", "");
		public static GUIContent normalMapText = new GUIContent ("Normal Map", "Normal Map");
		public static GUIContent cubeText = new GUIContent ("Reflection Map", "Reflection Map");
		public static GUIContent reflectivityText = new GUIContent ("Reflectivity", "");
		public static string whiteSpaceString = " ";
		public static string primaryMapsText = "Fur Properties";
		public static string secondaryMapsText = "Skin Properties";
		public static string rimLightText = "Lighting Properties";
		public static string reflectionText = "Reflection Properties";
		public static string reflectionsText = "Reflection Mode";
		public static string rimLightModeText = "Light Mode";
		public static string rimColorText = "Light Color";
		public static string rimWidthText = "Rim Width";
		public static readonly string[] reflectionNames = Enum.GetNames (typeof(ReflectionMode));
		public static readonly string[] rimLightNames = Enum.GetNames (typeof(RimLightMode));
	}

	MaterialProperty rimLightMode = null;
	MaterialProperty furColor = null;
	MaterialProperty rimColor = null;
	MaterialProperty rimWidth = null;
	MaterialProperty furMap = null;
	MaterialProperty furSpecularMap = null;
	MaterialProperty furSpecular = null;
	MaterialProperty furSmoothness = null;
	MaterialProperty noiseMap = null;
	MaterialProperty strandDensity = null;
	MaterialProperty strandLength = null;
	MaterialProperty shadowStrength = null;
	MaterialProperty edgeFade = null;
	MaterialProperty skinColor = null;
	MaterialProperty skinMap = null;
	MaterialProperty controlMap = null;
	MaterialProperty specularMap = null;
	MaterialProperty specular = null;
	MaterialProperty specularColor = null;
	MaterialProperty metallicMap = null;
	MaterialProperty metallic = null;
	MaterialProperty smoothness = null;
	MaterialProperty bumpScale = null;
	MaterialProperty bumpMap = null;
	MaterialProperty cube = null;
	MaterialProperty reflectivity = null;
	MaterialProperty reflectionMode = null;
	MaterialProperty useFurSecondMap = null;
	MaterialProperty useSkinSecondMap = null;
	MaterialProperty useStrengthMap = null;
	MaterialProperty useHeightMap = null;
	MaterialProperty useBiasMap = null;
	MaterialProperty useFurAlbedo = null;
	MaterialProperty useSkinAlbedo = null;
	MaterialEditor m_MaterialEditor;

	bool useHeight;
	bool useBias;
	bool useStrength;

	public void FindProperties (MaterialProperty[] props)
	{
		cube = FindProperty ("_Cube", props, false);
		reflectivity = FindProperty ("_Reflectivity", props);
		reflectionMode = FindProperty ("_ReflectionMode", props);
		rimLightMode = FindProperty ("_RimLightMode", props);
		furColor = FindProperty ("_FurColor", props);
		rimColor = FindProperty ("_RimColor", props);
		rimWidth = FindProperty ("_RimWidth", props);
		furSpecularMap = FindProperty ("_FurSpecularMap", props, false);
		furSpecular = FindProperty ("_FurSpecular", props, false);
		furSmoothness = FindProperty ("_FurGlossiness", props, false);
		skinColor = FindProperty ("_Color", props, false);
		furMap = FindProperty ("_MainTex", props);
		noiseMap = FindProperty ("_NoiseTex", props);
		skinMap = FindProperty ("_SkinTex", props, false);
		controlMap = FindProperty ("_ControlTex", props, false);
		specularMap = FindProperty ("_SpecGlossMap", props, false);
		specular = FindProperty ("_Specular", props, false);
		specularColor = FindProperty ("_SpecularColor", props, false);
		metallicMap = FindProperty ("_MetallicGlossMap", props, false);
		metallic = FindProperty ("_Metallic", props, false);
		smoothness = FindProperty ("_Glossiness", props, false);
		bumpScale = FindProperty ("_BumpScale", props, false);
		bumpMap = FindProperty ("_BumpMap", props, false);
		strandDensity = FindProperty ("_StrandThickness", props);
		strandLength = FindProperty ("_MaxHairLength", props);
		edgeFade = FindProperty ("_EdgeFade", props);
		shadowStrength = FindProperty ("_ShadowStrength", props, false);				
		useFurSecondMap = FindProperty ("_UseFurSecondMap", props, false);
		useSkinSecondMap = FindProperty ("_UseSkinSecondMap", props, false);
		useStrengthMap = FindProperty ("_UseStrengthMap", props, false);
		useHeightMap = FindProperty ("_UseHeightMap", props, false);
		useBiasMap = FindProperty ("_UseBiasMap", props, false);
		useFurAlbedo = FindProperty ("_UseFurAlbedo", props, false);
		useSkinAlbedo = FindProperty ("_UseSkinAlbedo", props, false);
	}
				
	public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] props)
	{
		FindProperties (props); // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly
		m_MaterialEditor = materialEditor;
		Material material = materialEditor.target as Material;
			
		ShaderPropertiesGUI (material);
			
		SetMaterialKeywords (material);
	}

	public void ShaderPropertiesGUI (Material material)
	{
		// Use default labelWidth
		EditorGUIUtility.labelWidth = 0f;
			
		// Primary properties
		GUILayout.Label (Styles.primaryMapsText, EditorStyles.boldLabel);
		DoFurArea (material);
			
		EditorGUILayout.Space ();

		// no normal map means its a no skin shader
		if (bumpMap != null && bumpScale != null) {
			GUILayout.Label (Styles.secondaryMapsText, EditorStyles.boldLabel);
			DoAlbedoArea (material);
			DoSpecularMetallicArea ();

			if (useSkinAlbedo == null)
				m_MaterialEditor.TexturePropertySingleLine (Styles.normalMapText, bumpMap, bumpMap.textureValue != null ? bumpScale : null);
			else
				m_MaterialEditor.TexturePropertySingleLine (Styles.normalMapText, bumpMap);
		}

		EditorGUILayout.Space ();

		GUILayout.Label (Styles.rimLightText, EditorStyles.boldLabel);
		DoRimLightArea (material);
			
		EditorGUILayout.Space ();

		GUILayout.Label (Styles.reflectionText, EditorStyles.boldLabel);
		DoReflectionArea (material);
	}

	void DoFurArea (Material material)
	{
		if (useFurAlbedo != null) {
			if (furMap.textureValue != null)
				m_MaterialEditor.TexturePropertySingleLine (Styles.furText, furMap);
			else
				m_MaterialEditor.TexturePropertySingleLine (Styles.furText, furMap, furColor);
		} else { 
			m_MaterialEditor.TexturePropertySingleLine (Styles.furText, furMap, furColor);
		}

		m_MaterialEditor.TexturePropertySingleLine (Styles.noiseText, noiseMap);
		m_MaterialEditor.TexturePropertySingleLine (Styles.controlText, controlMap);

		DoControlArea();

		if (furSpecularMap.textureValue == null)
			m_MaterialEditor.TexturePropertyTwoLines (Styles.furSpecularMapText, furSpecularMap, furSpecular, Styles.furSmoothnessText, furSmoothness);
		else
			m_MaterialEditor.TexturePropertySingleLine (Styles.furSpecularMapText, furSpecularMap);

		m_MaterialEditor.RangeProperty (strandDensity, Styles.densityText.text);
		m_MaterialEditor.RangeProperty (strandLength, Styles.strandText.text);
		m_MaterialEditor.RangeProperty (edgeFade, Styles.fadeText.text);
	}

	void DoControlArea() {
		if (controlMap.textureValue != null) {
			EditorGUIUtility.labelWidth = 140f;
			EditorGUI.indentLevel++;
			
			useHeight = (useHeightMap.floatValue == 1.0);
			useHeight = EditorGUILayout.Toggle("Use Height Map (R)", useHeight);
			if (useHeight)
				useHeightMap.floatValue = 1.0f;
			else
				useHeightMap.floatValue = 0f;
			
			useStrength = (useStrengthMap.floatValue == 1.0);
			useStrength = EditorGUILayout.Toggle("Use Stiffness Map (G)", useStrength);
			if (useStrength)
				useStrengthMap.floatValue = 1.0f;
			else
				useStrengthMap.floatValue = 0f;
			
			useBias = (useBiasMap.floatValue == 1.0);
			useBias = EditorGUILayout.Toggle("Use Bias Map (B)", useBias);
			if (useBias)
				useBiasMap.floatValue = 1.0f;
			else
				useBiasMap.floatValue = 0f;
			
			EditorGUIUtility.labelWidth = 0f;
			EditorGUI.indentLevel--;
		}
	}

	void DoRimLightArea (Material material)
	{
		if (shadowStrength != null)
			m_MaterialEditor.RangeProperty (shadowStrength, Styles.shadowText.text);

		EditorGUI.showMixedValue = rimLightMode.hasMixedValue;
		var mode = (RimLightMode)rimLightMode.floatValue;

		EditorGUI.BeginChangeCheck ();
		mode = (RimLightMode)EditorGUILayout.Popup (Styles.rimLightModeText, (int)mode, Styles.rimLightNames);
		if (mode != RimLightMode.None) {
			m_MaterialEditor.RangeProperty (rimWidth, Styles.rimWidthText);
			if (mode == RimLightMode.UserDefined)
				m_MaterialEditor.ColorProperty (rimColor, Styles.rimColorText);
		}

		if (EditorGUI.EndChangeCheck ()) {
			m_MaterialEditor.RegisterPropertyChangeUndo ("Rim Light Mode");
			rimLightMode.floatValue = (float)mode;
		}
			
		EditorGUI.showMixedValue = false;
	}

	void DoAlbedoArea (Material material)
	{
		if (skinMap != null && skinColor != null) {
			if (useSkinAlbedo != null) {
				if (skinMap.textureValue != null)
					m_MaterialEditor.TexturePropertySingleLine (Styles.albedoText, skinMap);
				else
					m_MaterialEditor.TexturePropertySingleLine (Styles.albedoText, skinMap, skinColor);
			} else {
				m_MaterialEditor.TexturePropertySingleLine (Styles.albedoText, skinMap, skinColor);
			}
		}
	}

	void DoReflectionArea (Material material)
	{
		EditorGUI.showMixedValue = reflectionMode.hasMixedValue;
		var mode = (ReflectionMode)reflectionMode.floatValue;
			
		EditorGUI.BeginChangeCheck ();
		mode = (ReflectionMode)EditorGUILayout.Popup (Styles.reflectionsText, (int)mode, Styles.reflectionNames);
		if (mode != ReflectionMode.None) {
			m_MaterialEditor.TexturePropertySingleLine (Styles.cubeText, cube);
			m_MaterialEditor.RangeProperty (reflectivity, Styles.reflectivityText.text);
		} 
		if (EditorGUI.EndChangeCheck ()) {
			m_MaterialEditor.RegisterPropertyChangeUndo ("Reflection Mode");
			reflectionMode.floatValue = (float)mode;
		}
			
		EditorGUI.showMixedValue = false;
	}

	void DoSpecularMetallicArea ()
	{
		if (specularMap != null && specularColor != null) {
			if (specularMap.textureValue == null)
				m_MaterialEditor.TexturePropertyTwoLines (Styles.specularMapText, specularMap, specularColor, Styles.smoothnessText, smoothness);
			else
				m_MaterialEditor.TexturePropertySingleLine (Styles.specularMapText, specularMap);
				
		} else if (metallicMap != null && metallic != null) {
			if (metallicMap.textureValue == null)
				m_MaterialEditor.TexturePropertyTwoLines (Styles.metallicMapText, metallicMap, metallic, Styles.smoothnessText, smoothness);
			else
				m_MaterialEditor.TexturePropertySingleLine (Styles.metallicMapText, metallicMap);
		} else if (specular != null) {
			if (specularMap.textureValue == null)
				m_MaterialEditor.TexturePropertyTwoLines (Styles.specularMapText, specularMap, specular, Styles.glossinessText, smoothness);
			else
				m_MaterialEditor.TexturePropertySingleLine (Styles.specularMapText, specularMap);
		}
	}
		
	void SetMaterialKeywords (Material material)
	{
		if (useFurAlbedo != null) {
			if (material.GetTexture("_MainTex"))
				useFurAlbedo.floatValue = 1.0f;
			else 
				useFurAlbedo.floatValue = 0.0f;
		}
		
		if (useSkinAlbedo != null) {
			if (material.GetTexture("_SkinTex"))
				useSkinAlbedo.floatValue = 1.0f;
			else 
				useSkinAlbedo.floatValue = 0.0f;
		}

		if (material.GetTexture ("_FurSpecularMap"))
			useFurSecondMap.floatValue = 1.0f;
		else
			useFurSecondMap.floatValue = 0;

		if ((specularMap != null && specularColor != null) || specular != null) {
			if (material.GetTexture ("_SpecGlossMap"))
				useSkinSecondMap.floatValue = 1.0f;
			else
				useSkinSecondMap.floatValue = 0;
		} else if (metallicMap != null && metallic != null) {
			if (material.GetTexture ("_MetallicGlossMap"))
				useSkinSecondMap.floatValue = 1.0f;
			else
				useSkinSecondMap.floatValue = 0;
		}
	}

	void MaterialChanged (Material material)
	{
		SetMaterialKeywords (material);
	}

	void SetKeyword (Material m, string keyword, bool state)
	{
		if (state)
			m.EnableKeyword (keyword);
		else
			m.DisableKeyword (keyword);
	}
}
	
