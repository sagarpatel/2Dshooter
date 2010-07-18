sampler s;



float foo;
float time;


float4 PixelShaderFunction(float2 uv : TEXCOORD0, float4 c: COLOR0) : COLOR0
{
    // TODO: add your pixel shader code here.
 
        
    //blur
    float4 color;
    color = tex2D(s, uv); //normal
    color += tex2D(s, uv + (cos(time/500)/2 ));
	color += tex2D(s, uv - (cos(time/500)/2 ));
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
