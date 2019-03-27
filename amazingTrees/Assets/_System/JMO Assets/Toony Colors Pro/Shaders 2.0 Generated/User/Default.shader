// Toony Colors Pro+Mobile 2
// (c) 2014-2018 Jean Moreno

Shader "Toony Colors Pro 2/User/Default"
{
	Properties
	{
	[TCP2HeaderHelp(BASE, Base Properties)]
		//TOONY COLORS
		_Color ("Color", Color) = (1,1,1,1)
		_HColor ("Highlight Color", Color) = (0.785,0.785,0.785,1.0)
		_SColor ("Shadow Color", Color) = (0.195,0.195,0.195,1.0)
		_HighlightMultiplier ("Highlight Multiplier", Range(0,4)) = 1
		_ShadowMultiplier ("Shadow Multiplier", Range(0,4)) = 1

		//DIFFUSE
		_MainTex ("Main Texture", 2D) = "white" {}
	[TCP2Separator]

		//TOONY COLORS RAMP
		[TCP2Header(RAMP SETTINGS)]

		[TCP2Gradient] _Ramp			("Toon Ramp (RGB)", 2D) = "gray" {}
	[TCP2Separator]

	[Header(Masks)]
		[NoScaleOffset]
		_Mask1 ("Mask 1 (Emission)", 2D) = "black" {}
	[TCP2Separator]

	[TCP2HeaderHelp(EMISSION, Emission)]
		[NoScaleOffset] _EmissionMap ("Emission (RGB)", 2D) = "black" {}
		_EmissionColor ("Emission Color", Color) = (1,1,1,1.0)
	[TCP2Separator]

	[TCP2HeaderHelp(TRANSPARENCY)]
		//Blending
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendTCP2 ("Blending Source", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlendTCP2 ("Blending Dest", Float) = 10
		//Alpha Testing
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	[TCP2Separator]


		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{

		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend [_SrcBlendTCP2] [_DstBlendTCP2]
		Cull Off

		CGPROGRAM

		#pragma surface surf ToonyColorsCustom keepalpha exclude_path:deferred exclude_path:prepass
		#pragma target 3.0


		//================================================================
		// VARIABLES

		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _Mask1;
		fixed4 _EmissionColor;
		sampler2D _EmissionMap;
		fixed _SketchSpeed;
		fixed _Cutoff;

		#define UV_MAINTEX uv_MainTex

		struct Input
		{
			half2 uv_MainTex;
		};

		//================================================================
		// CUSTOM LIGHTING

		//Lighting-related variables
		fixed4 _HColor;
		fixed4 _SColor;
		fixed _HighlightMultiplier;
		fixed _ShadowMultiplier;
		sampler2D _Ramp;

		// Instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			half atten;
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Gloss;
			fixed Alpha;
		};

		inline half4 LightingToonyColorsCustom (inout SurfaceOutputCustom s, half3 viewDir, UnityGI gi)
		{
		#define IN_NORMAL s.Normal
	
			half3 lightDir = gi.light.dir;
		#if defined(UNITY_PASS_FORWARDBASE)
			half3 lightColor = _LightColor0.rgb;
			half atten = s.atten;
		#else
			half3 lightColor = gi.light.color.rgb;
			half atten = 1;
		#endif

			IN_NORMAL = normalize(IN_NORMAL);
			fixed ndl = max(0, dot(IN_NORMAL, lightDir));
			#define NDL ndl


			#define		RAMP_TEXTURE	_Ramp

			fixed3 ramp = tex2D(RAMP_TEXTURE, fixed2(NDL,NDL)).rgb;
		#if !(POINT) && !(SPOT)
			ramp *= atten;
		#endif
		#if !defined(UNITY_PASS_FORWARDBASE)
			_SColor = fixed4(0,0,0,1);
		#endif
			_SColor = lerp(_HColor, _SColor, _SColor.a * _ShadowMultiplier);	//Shadows intensity through alpha
			ramp = lerp(_SColor.rgb, _HColor.rgb * _HighlightMultiplier, ramp);
			fixed4 c;
			c.rgb = s.Albedo * lightColor.rgb * ramp;
			c.a = s.Alpha;

		#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
			c.rgb += s.Albedo * gi.indirect.diffuse;
		#endif

		#if defined(UNITY_PASS_FORWARDADD)
			//multiply RGB with alpha for additive lights for proper transparency behavior
			c.rgb *= c.a;
		#endif
			return c;
		}

		void LightingToonyColorsCustom_GI(inout SurfaceOutputCustom s, UnityGIInput data, inout UnityGI gi)
		{
			half colorNoAtten = max(gi.light.color.r, max(gi.light.color.g, gi.light.color.b));
			gi = UnityGlobalIllumination(data, 1.0, IN_NORMAL);

			s.atten = max(gi.light.color.r, max(gi.light.color.g, gi.light.color.b)) / colorNoAtten;	//try to extract attenuation (shadowmap + shadowmask) for lighting function
			gi.light.color = _LightColor0.rgb;	//remove attenuation
		}

		//================================================================
		// SURFACE FUNCTION

		void surf(Input IN, inout SurfaceOutputCustom o)
		{
			fixed4 mainTex = tex2D(_MainTex, IN.UV_MAINTEX);

			//Masks
			fixed4 mask1 = tex2D(_Mask1, IN.UV_MAINTEX);
			o.Albedo = mainTex.rgb * _Color.rgb;
			o.Alpha = mainTex.a * _Color.a;
	
			//Cutout (Alpha Testing)
			clip (o.Alpha - _Cutoff);

			//Emission
			half3 emissiveColor = half3(1,1,1);
			emissiveColor *= mainTex.rgb * mask1.a;
			emissiveColor *= tex2D(_EmissionMap, IN.UV_MAINTEX);
			emissiveColor *= _EmissionColor.rgb * _EmissionColor.a;
			o.Emission += emissiveColor;
		}

		ENDCG
	}

	Fallback "Diffuse"
	CustomEditor "TCP2_MaterialInspector_SG"
}
