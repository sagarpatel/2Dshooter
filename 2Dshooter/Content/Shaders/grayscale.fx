sampler s;



float foo;
float time;



float4 PixelShaderFunction(float2 uv : TEXCOORD0, float4 c: COLOR0) : COLOR0
{
    // TODO: add your pixel shader code here.
 

	
	//Grayscale
    float4 color = tex2D(s, uv);
    return dot(color, float3(0.3, 0.59, 0.11));
    

}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.


        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
