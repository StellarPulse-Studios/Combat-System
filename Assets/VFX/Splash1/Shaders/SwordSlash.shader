Shader "LightSplash/Particles/SwordSlash"
{
	Properties
	{
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_MainTexture("MainTexture", 2D) = "white" {}
		_EmissionTex("EmissionTex", 2D) = "white" {}
		_Opacity("Opacity", Float) = 20
		_Dissolve("Dissolve", 2D) = "white" {}
		_SpeedMainTexUVNoiseZW("Speed MainTex U/V + Noise Z/W", Vector) = (0,0,0,0)
		_Emission("Emission", Float) = 5
		_Remap("Remap", Vector) = (-2,1,0,0)
		_AddColor("Add Color", Color) = (0,0,0,0)
		_Desaturation("Desaturation", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		[MaterialToggle] _Usedepth ("Use depth?", Float ) = 0
	}

	Category 
	{
		SubShader
		{
			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off
			ZTest LEqual
			
			Pass {			
				CGPROGRAM			
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				#include "UnityShaderVariables.cginc"
				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					float4 ase_texcoord1 : TEXCOORD1;
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
					float4 ase_texcoord3 : TEXCOORD3;
				};			
				
				#if UNITY_VERSION >= 560
				UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
				#else
				uniform sampler2D_float _CameraDepthTexture;
				#endif


				uniform float4 _AddColor;
				uniform float _Emission;
				uniform sampler2D _EmissionTex;
				uniform float4 _EmissionTex_ST;
				uniform sampler2D _MainTexture;
				uniform float4 _MainTexture_ST;
				uniform float _Desaturation;
				uniform float2 _Remap;
				uniform float4 _SpeedMainTexUVNoiseZW;
				uniform float _Opacity;
				uniform sampler2D _Dissolve;
				uniform float4 _Dissolve_ST;
				uniform fixed _Usedepth;
				uniform float _InvFade;
				
				float3 HSVToRGB( float3 c )
				{
					float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
					float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
					return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
				}
				
				float3 RGBToHSV(float3 c)
				{
					float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
					float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
					float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
					float d = q.x - min( q.w, q.y );
					float e = 1.0e-10;
					return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
				}

				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					o.ase_texcoord3 = v.ase_texcoord1;

					v.vertex.xyz +=  float3( 0, 0, 0 ) ;
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					float lp = 1;
					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate ((sceneZ-partZ) / _InvFade);
						lp *= lerp(1, fade, _Usedepth);
						i.color.a *= lp;
					#endif

					float2 uv_EmissionTex = i.texcoord.xy * _EmissionTex_ST.xy + _EmissionTex_ST.zw;
					float3 hsvTorgb46 = RGBToHSV( tex2D( _EmissionTex, uv_EmissionTex ).rgb );
					float4 uv1_MainTexture = i.ase_texcoord3;
					uv1_MainTexture.xy = i.ase_texcoord3.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
					float3 hsvTorgb47 = HSVToRGB( float3(( hsvTorgb46.x + uv1_MainTexture.z ),hsvTorgb46.y,hsvTorgb46.z) );
					float3 desaturateInitialColor42 = hsvTorgb47;
					float desaturateDot42 = dot( desaturateInitialColor42, float3( 0.299, 0.587, 0.114 ));
					float3 desaturateVar42 = lerp( desaturateInitialColor42, desaturateDot42.xxx, _Desaturation );
					float2 _Vector2 = float2(0,1);
					float3 temp_cast_1 = (_Vector2.x).xxx;
					float3 temp_cast_2 = (_Vector2.y).xxx;
					float3 temp_cast_3 = (_Remap.x).xxx;
					float3 temp_cast_4 = (_Remap.y).xxx;
					float3 clampResult37 = clamp( (temp_cast_3 + (desaturateVar42 - temp_cast_1) * (temp_cast_4 - temp_cast_3) / (temp_cast_2 - temp_cast_1)) , float3( 0,0,0 ) , float3( 1,1,1 ) );
					float2 appendResult20 = (float2(_SpeedMainTexUVNoiseZW.x , _SpeedMainTexUVNoiseZW.y));
					float2 panner31 = ( 1.0 * _Time.y * appendResult20 + uv1_MainTexture.xy);
					float clampResult6 = clamp( ( tex2D( _MainTexture, panner31 ).a * _Opacity ) , 0.0 , 1.0 );
					float2 appendResult21 = (float2(_SpeedMainTexUVNoiseZW.z , _SpeedMainTexUVNoiseZW.w));
					float4 uv0_Dissolve = i.texcoord;
					uv0_Dissolve.xy = i.texcoord.xy * _Dissolve_ST.xy + _Dissolve_ST.zw;
					float2 panner9 = ( 1.0 * _Time.y * appendResult21 + uv0_Dissolve.xy);
					float2 break55 = panner9;
					float2 appendResult53 = (float2(break55.x , ( uv1_MainTexture.w + break55.y )));
					float Step27 = step( ( 1.0 - uv0_Dissolve.x ) , uv0_Dissolve.w );
					float W18 = uv0_Dissolve.z;
					float3 _Vector1 = float3(0.3,0,1);
					float ifLocalVar23 = 0;
					if( tex2D( _Dissolve, appendResult53 ).r >= ( Step27 * W18 ) )
					ifLocalVar23 = _Vector1.y;
					else
					ifLocalVar23 = _Vector1.z;
					float4 appendResult3 = (float4(( ( _AddColor * i.color ) + ( _Emission * float4( clampResult37 , 0.0 ) * i.color ) ).rgb , ( i.color.a * clampResult6 * ifLocalVar23 )));		

					fixed4 col = appendResult3;
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
	
	
	
}
