sampler s;
sampler s2;


float foo;
float time;

 struct ParticleData
        {
            float BirthTime;
            
            float NowAge;
            float2 OrginalPosition;
            float2 Accelaration;
            float2 Direction;
            float2 Position;
            float Scaling;
            float4 ModColor;
            bool IsHoming;
            bool IsAlive;

        };

float4 PixelShaderFunction(float2 uv : TEXCOORD0, float4 c: COLOR0) : COLOR0
{
    // TODO: add your pixel shader code here.
 
        
    //blur
    float4 color;
    color = tex2D(s, uv); //normal
    color += tex2D(s, uv + (0.05) );
    color += tex2D(s, uv - (0.05) );
	color = color/2;
	

    return color ;

}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.


        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
