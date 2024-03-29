﻿Shader "_URP_/Lit-Custom"
{
    Properties
    {
		_TintColor("TintColor", color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
	{
		Pass
		{
			Name "URPDefault"

			Tags {
				"RenderPipeline" = "UniversalPipeline"
				"RenderType" = "Opaque"
				"Queue" = "Geometry+0"
			}
			LOD 100

			HLSLPROGRAM
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma vertex vert
			#pragma fragment frag
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

			// GPU Instanceing
			#pragma multi_compile_instancing
			#pragma multi_compile_fog

			// Recieve shadow
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOW_SOFT

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float fogCoord : TEXCOORD1;
				float3 normal : NORMAL;
				float4 shadowCoord : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
				half4 _TintColor;
				TEXTURE2D(_MainTex);
				SAMPLER(sampler_MainTex);
				half4 _MainTex_ST;
			CBUFFER_END

			v2f vert(appdata v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.vertex = TransformObjectToHClip(v.vertex.xyz);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = TransformObjectToWorldNormal(v.normal);
				o.fogCoord = ComputeFogFactor(o.vertex.z);

				//#ifdef _MAIN_LIGHT_SHADOWS
				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
				o.shadowCoord = GetShadowCoord(vertexInput);
				//#endif

				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

				Light mainLight = GetMainLight(i.shadowCoord);

				float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) * _TintColor;

				float NdotL = saturate(dot(_MainLightPosition.xyz, i.normal));
				half3 ambient = SampleSH(i.normal);

				col.rgb *= NdotL * _MainLightColor.rgb * mainLight.shadowAttenuation * mainLight.distanceAttenuation + ambient;

				// apply fog
				col.rgb = MixFog(col.rgb, i.fogCoord);

				return col;
			}
			ENDHLSL
		}

		Pass
		{
			Name "ShadowCaster"
			Tags {"LightMode" = "ShadowCaster"}
			Cull[_Cull]
			HLSLPROGRAM

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			#pragma vertex ShadowPassVertex
			#pragma fragment ShadowPassFragment

			#pragma shader_feature _ALPHATEST_ON

				// GPU Instancing
				#pragma multi_compile_instancing

				#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"

				CBUFFER_START(UnityPerMaterial)
				CBUFFER_END

				struct VertexInput
				{
					float4 vertex : POSITION;
					float4 normal : NORMAL;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct VertexOutput
				{
					float4 vertex : SV_POSITION;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
				};

				VertexOutput ShadowPassVertex(VertexInput v)
				{
					VertexOutput o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

					float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
					float3 normalWS = TransformObjectToWorldNormal(v.normal.xyz);
					float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _MainLightPosition.xyz));
					o.vertex = positionCS;

					return o;
				}

				half4 ShadowPassFragment(VertexOutput i) : SV_TARGET
				{
					UNITY_SETUP_INSTANCE_ID(i);
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
					return 0;
				}

				ENDHLSL
		}

		pass 
		{
			Name "DepthOnly"
			Tags {"LightMode" = "DepthOnly"}
			
			ZWrite On
			ColorMask 0
			Cull[_Cull]

			HLSLPROGRAM
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			#pragma vertex DepthOnlyVertex
			#pragma fragment DepthOnlyFragment
			#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"

			CBUFFER_START(UnityPerMaterial)
			CBUFFER_END

			struct VertexInput {
				float4 vertex : POSITION;
			};

			struct VertexOutput {
				float4 vertex : SV_POSITION;
			};

			VertexOutput DepthOnlyVertex(VertexInput v) {
				VertexOutput o;
				o.vertex = TransformObjectToHClip(v.vertex.xyz);
				return o;
			}

			half4 DepthOnlyFragment(VertexOutput IN) : SV_TARGET
			{
				return 0;
			}
			ENDHLSL
		}
    }
}
