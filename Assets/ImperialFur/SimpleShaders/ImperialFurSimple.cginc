 		 #include "UnityCG.cginc" 
		 #include "AutoLight.cginc"

 	 	 sampler2D _MainTex;
 	 	 sampler2D _SkinTex;
 	 	 sampler2D _ControlTex;
 	 	 sampler2D _NoiseTex;
 	 	 sampler2D _FurSpecularMap;
         samplerCUBE _Cube;   

 	 	 uniform float3 Displacement;
		 fixed _MaxHairLength;
		 fixed3 _RimColor;
		 fixed4 _FurColor;
		 fixed3 _FurSpecular;
		 fixed _FurGlossiness;
 	 	 fixed _ShadowStrength;
 	 	 float _EdgeFade; 
 	 	 fixed _RimWidth;
 	 	 half _RimLightMode;
 	 	 half _StrandThickness;
 	 	 fixed _Reflectivity;
 	 	 half _ReflectionMode; 	 	 
		 fixed4 _LightColor0; 
		 fixed _UseFurSecondMap;		 
   	 	 fixed _UseStrengthMap;
 	 	 fixed _UseHeightMap;
		 float _UseBiasMap;

         struct v2f {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
            float2 shadowUv : TEXCOORD2;
            float3 vertexLighting : TEXCOORD3;
            float3 normalDir : TEXCOORD4;
            float4 posWorld : TEXCOORD5;
            fixed3 rimColor: TEXCOORD6;
            float3 viewDir : TEXCOORD7;
            LIGHTING_COORDS(8,9)
         };
 
		 float4 _MainTex_ST;
         float4 pos;
                  
        half3 DecodeRGBM(float4 rgbm)
		{
			fixed MaxRange=8;
    		return rgbm.rgb * (rgbm.a * MaxRange);
		}
         
        v2f vert(appdata_base v, out float alpha : COLOR) {
	       	v2f o;
			
      		float movementFactor = pow(CURRENTLAYER, 2);  
           	float4 c = tex2Dlod (_ControlTex, float4(v.texcoord.xy,0,0));
			
			if (_UseBiasMap) {
				fixed skinLength = _MaxHairLength - (_MaxHairLength * c.b);
				v.vertex.xyz -= v.normal * skinLength;
			}
          
			float hairLength = _MaxHairLength * CURRENTLAYER;
            float3 tPos = v.vertex.xyz + (v.normal * hairLength);
            
        	if (_UseStrengthMap != 0) {
				tPos += Displacement * movementFactor * hairLength * c.g;
        	} else {
				tPos += Displacement * movementFactor * hairLength;
        	}
			o.pos = mul(UNITY_MATRIX_MVP, float4(tPos,1.0));              
                  
            alpha = 1 - (CURRENTLAYER * CURRENTLAYER);	
			float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
			o.viewDir = viewDir;		
			alpha += dot(viewDir, v.normal) - _EdgeFade;
			alpha = clamp(alpha, 0, 1);
            
            float4 znormal = 1 - dot(float4(v.normal, 0.0), float4(0,0,1,0));
            
            o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);            
            o.shadowUv = TRANSFORM_TEX (v.texcoord, _MainTex) + znormal * 0.0011;
 			
 			if (_RimLightMode != 0) {
            	float dotProduct = 1 - dot(v.normal, viewDir);
            	o.rimColor = smoothstep(1 - _RimWidth, 1.0, dotProduct);
            	if (_RimLightMode == 3)
	           		o.rimColor *= _RimColor;	           	
            	else if (_RimLightMode == 2)
	           		o.rimColor *= UNITY_LIGHTMODEL_AMBIENT.rgb;
 			}
 			
			o.posWorld = mul(_Object2World, v.vertex);
            o.normalDir = normalize(mul(float4(v.normal, 0.0), _World2Object).xyz); 
            o.vertexLighting = float4(0.0, 0.0, 0.0, 1.0);
            #ifdef VERTEXLIGHT_ON
            for (int index = 0; index < 4; index++)
            {    
               float4 lightPosition = float4(unity_4LightPosX0[index], 
                  unity_4LightPosY0[index], 
                  unity_4LightPosZ0[index], 1.0);
 
               float3 vertexToLightSource = 
                  lightPosition.xyz - o.posWorld.xyz;        
               float3 lightDirection = normalize(vertexToLightSource);
               float squaredDistance = 
                  dot(vertexToLightSource, vertexToLightSource);
               float attenuation = 1.0 / (1.0 + 
                  unity_4LightAtten0[index] * squaredDistance);
               float3 diffuseReflection = attenuation 
                  * unity_LightColor[index].rgb * _FurColor.rgb 
                  * max(0.0, dot(o.normalDir, lightDirection));         
  
               o.vertexLighting = 
                  o.vertexLighting + diffuseReflection;
            }
            #endif  
              
            TRANSFER_VERTEX_TO_FRAGMENT(o);  
            return o;
         }
         
        
		fixed4 frag (v2f i, in float alpha : COLOR) : SV_Target
        {        
        
			fixed4 texcol = tex2D (_MainTex, i.uv) * _FurColor;
			fixed4 noisecol = tex2D (_NoiseTex, i.uv * _StrandThickness);
				
			texcol.a *= (noisecol.r / NOISEFACTOR);			
			// noise map
			fixed alphaFactor = 1;					
		    // heightmap
			if (_UseHeightMap != 0) {
				fixed4 controlcol = tex2D (_ControlTex, i.uv);
				alphaFactor *= controlcol.r;
				texcol.a = (CURRENTLAYER > controlcol.r) ? 0 : texcol.a;
				if (texcol.a > 0)
					texcol.a *= CURRENTLAYER;
			}
			if (texcol.a == 0)
				return texcol;
				
			// edge fade
			texcol.a *= alpha;
				
		    float shadow = lerp(1,CURRENTLAYER/alphaFactor,_ShadowStrength);
			texcol.rgb *= shadow;
												
			float3 normalDirection = i.normalDir; 
            float3 viewDirection = normalize(_WorldSpaceCameraPos - i.posWorld.xyz);
            float3 lightDirection;
            float attenuation;
            attenuation = LIGHT_ATTENUATION(i);

            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = normalize(_WorldSpaceLightPos0.xyz);
            } 
            else // point or spot light
            {
               float3 vertexToLightSource = 
                  _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
               float distance = length(vertexToLightSource);
               lightDirection = normalize(vertexToLightSource);
            }
 
            float3 ambientLighting = 
                UNITY_LIGHTMODEL_AMBIENT.rgb * texcol.rgb;
  
            float3 diffuseReflection = 
               attenuation * _LightColor0.rgb * texcol.rgb 
               * max(0.0, dot(normalDirection, lightDirection));
 
            fixed4 speccol;
			if (_UseFurSecondMap != 0) {
            	speccol = tex2D (_FurSpecularMap, i.uv);
			} else {
	            speccol.rgb = _FurSpecular;
            	speccol.a = _FurGlossiness;
			}
			
            float3 specularReflection;
            if (dot(normalDirection, lightDirection) < 0.0) 
               // light source on the wrong side?
            {
               specularReflection = float3(0.0, 0.0, 0.0);         
            }
            else // light source on the right side
            {
    	    	specularReflection = attenuation * _LightColor0.rgb 
                  * speccol.rgb * pow(max(0.0, dot(
                  reflect(-lightDirection, normalDirection), 
                  viewDirection)), speccol.a) * CURRENTLAYER;
            }
                        
            texcol = float4(i.vertexLighting, texcol.a) + float4(ambientLighting, 0) + float4(diffuseReflection, 0) + float4(specularReflection, 0);
            
			if (_RimLightMode != 0) {
				if (_RimLightMode == 1) 
					texcol += texcol * float4(i.rimColor,0);
				else
					texcol += float4(i.rimColor, 0);
			}
			
			if (_ReflectionMode != 0) {
	    		float3 reflectedDir = reflect(-i.viewDir, normalize(i.normalDir));
				fixed4 reflcol = texCUBE (_Cube, reflectedDir);
				if (_ReflectionMode == 1)
					reflcol=float4(DecodeRGBM(reflcol),1);
				reflcol *= speccol.a;
				texcol.rgb += reflcol.rgb * _Reflectivity * texcol.a;
			}
			
            return texcol;
         }