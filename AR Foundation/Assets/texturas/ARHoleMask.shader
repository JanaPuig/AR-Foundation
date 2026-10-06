// "Agujero" para AR: no pinta nada, pero tapa todo lo virtual que quede detrás.
// En ese trozo se sigue viendo la imagen de la cámara, es decir, la carta real.
// Pensado para URP (el que usa la plantilla AR Mobile de Unity 6).
Shader "Custom/ARHoleMask"
{
    SubShader
    {
        // Se dibuja antes que el resto de objetos opacos (el tablero).
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "Queue" = "Geometry-10" }

        Pass
        {
            ZWrite On       // reserva la profundidad...
            ZTest LEqual
            ColorMask 0     // ...pero no escribe ningún color
            Cull Off        // funciona mire hacia donde mire el quad

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }
    }
}
