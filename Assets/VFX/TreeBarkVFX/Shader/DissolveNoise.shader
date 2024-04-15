Shader "LightSplash/Particles/DissolveNoise"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_TextureNoise("Texture Noise", 2D) = "white" {}
		_Dissolvenoise("Dissolve noise", 2D) = "white" {}
		_NoisespeedXYEmissonZPowerW("Noise speed XY / Emisson Z / Power W", Vector) = (0.5,0,2,1)
		_DissolvespeedXY("Dissolve speed XY", Vector) = (0,0,0,0)
		_Maincolor("Main color", Color) = (0.7609469,0.8547776,0.9433962,1)
		_Noisecolor("Noise color", Color) = (0.2470588,0.3012382,0.3607843,1)
		_Dissolvecolor("Dissolve color", Color) = (1,1,1,1)
		[Toggle]_Usetexturecolor("Use texture color", Float) = 0
		[Toggle]_Usetexturedissolve("Use texture dissolve", Float) = 0
		_Opacity("Opacity", Range( 0 , 1)) = 1
		[Toggle] _Usedepth ("Use depth?", Float ) = 0
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
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


				uniform sampler2D _MainTex;
				uniform float4 _MainTex_ST;
				uniform float _InvFade;
				uniform float _Usedepth;
				uniform float4 _NoisespeedXYEmissonZPowerW;
				uniform float _Usetexturecolor;
				uniform float4 _Maincolor;
				uniform float4 _Noisecolor;
				uniform sampler2D _TextureNoise;
				uniform sampler2D _Dissolvenoise;
				uniform float4 _Dissolvenoise_ST;
				uniform float4 _TextureNoise_ST;
				uniform float _Usetexturedissolve;
				uniform float4 _DissolvespeedXY;
				uniform float4 _Dissolvecolor;
				uniform float _Opacity;

				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					o.ase_texcoord3.xyz = v.ase_texcoord1.xyz;
					
					o.ase_texcoord3.w = 0;

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
						float fade = saturate (_InvFade * (sceneZ-partZ));
						lp *= lerp(1, fade, _Usedepth);
						i.color.a *= lp;
					#endif

					float Emission59 = _NoisespeedXYEmissonZPowerW.z;
					float2 appendResult38 = (float2(_NoisespeedXYEmissonZPowerW.x , _NoisespeedXYEmissonZPowerW.y));
					float3 uv1_Dissolvenoise = i.ase_texcoord3.xyz;
					uv1_Dissolvenoise.xy = i.ase_texcoord3.xyz.xy * _Dissolvenoise_ST.xy + _Dissolvenoise_ST.zw;
					float W120 = uv1_Dissolvenoise.z;
					float4 uv0_TextureNoise = i.texcoord;
					uv0_TextureNoise.xy = i.texcoord.xy * _TextureNoise_ST.xy + _TextureNoise_ST.zw;
					float2 panner39 = ( 1.0 * _Time.y * appendResult38 + ( W120 + float2( 0.2,0.4 ) + (uv0_TextureNoise).xy ));
					float Noisepower63 = _NoisespeedXYEmissonZPowerW.w;
					float4 temp_cast_0 = (Noisepower63).xxxx;
					float4 clampResult11 = clamp( ( pow( tex2D( _TextureNoise, panner39 ) , temp_cast_0 ) * Noisepower63 ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
					float4 lerpResult8 = lerp( _Maincolor , _Noisecolor , clampResult11);
					float2 appendResult109 = (float2(_DissolvespeedXY.x , _DissolvespeedXY.y));
					float2 panner111 = ( 1.0 * _Time.y * appendResult109 + ( (uv1_Dissolvenoise).xy + W120 ));
					float4 tex2DNode91 = tex2D( _Dissolvenoise, panner111 );
					float2 uv_MainTex = i.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					float4 tex2DNode4 = tex2D( _MainTex, uv_MainTex );
					float mainTexr123 = tex2DNode4.r;
					float temp_output_88_0 = step( lerp(tex2DNode91.r,( tex2DNode91.r * mainTexr123 ),_Usetexturedissolve) , uv0_TextureNoise.z );
					float4 temp_output_93_0 = ( lerpResult8 * ( 1.0 - temp_output_88_0 ) );
					float clampResult87 = clamp( ( (-4.0 + (( (-0.65 + (( 1.0 - uv0_TextureNoise.z ) - 0.0) * (0.65 - -0.65) / (1.0 - 0.0)) + lerp(tex2DNode91.r,( tex2DNode91.r * mainTexr123 ),_Usetexturedissolve) ) - 0.0) * (7.0 - -4.0) / (1.0 - 0.0)) * 3.0 ) , 0.0 , 1.0 );
					float4 lerpResult92 = lerp( lerp(temp_output_93_0,( temp_output_93_0 * tex2DNode4 ),_Usetexturecolor) , lerp(_Dissolvecolor,( _Dissolvecolor * tex2DNode4 ),_Usetexturecolor) , ( clampResult87 * temp_output_88_0 ));
					float clampResult99 = clamp( (-15.0 + (( lerp(tex2DNode91.r,( tex2DNode91.r * mainTexr123 ),_Usetexturedissolve) + (-0.65 + (uv0_TextureNoise.w - 0.0) * (0.65 - -0.65) / (1.0 - 0.0)) ) - 0.0) * (15.0 - -15.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
					float4 appendResult2 = (float4((( Emission59 * lerpResult92 * i.color )).rgb , ( i.color.a * tex2DNode4.a * clampResult99 * _Opacity )));
					
					fixed4 col = appendResult2;
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
}
