#pragma strict

var enableAutoDilation : boolean = true;
var sceneLightObject : Transform;
var lightSensitivity : float = 1.0;
var dilationSpeed : float = 0.1;
var maxDilation : float = 1.0;

private var sceneLight : Light;
private var lightIntensity : float;
private var lightAngle : float;
private var dilateTime : float = 0.0;
private var pupilDilation : float = 0.5;
private var currTargetDilation : float = -1.0;
private var targetDilation : float = 0.0;
private var currLightSensitivity : float = -1;
private var eyeRenderer : Renderer;
//private var lookPos : Vector3;

function Start () {

	if (sceneLightObject != null){
		sceneLight = sceneLightObject.GetComponent(Light);
	}

	eyeRenderer = gameObject.GetComponent(Renderer);

}


function LateUpdate () {

	if (sceneLight != null){

		//set scene lighting
		lightIntensity = sceneLight.intensity;

		//handle auto dilation
		if (enableAutoDilation){

			//handle timer
			if (currTargetDilation != targetDilation || currLightSensitivity != lightSensitivity){
				dilateTime = 0.0;
				currTargetDilation = targetDilation;
				currLightSensitivity = lightSensitivity;
			}

			//calculate look angle
			lightAngle = Vector3.Angle(sceneLightObject.transform.forward,transform.forward) / 180.0;
			targetDilation = Mathf.Lerp(1.0,0.0,lightAngle * sceneLight.intensity * lightSensitivity);

			//handle dilation
			dilateTime += Time.deltaTime*dilationSpeed;
			pupilDilation = Mathf.Clamp(pupilDilation,0.0,maxDilation);
			pupilDilation = Mathf.Lerp(pupilDilation,targetDilation,dilateTime);

			eyeRenderer.sharedMaterial.SetFloat("_pupilSize",pupilDilation);



			//Shader based
			//lookPos.x = transform.eulerAngles.x/360.0;
			//lookPos.y = transform.eulerAngles.y/360.0;
			//lookPos.z = transform.eulerAngles.z/360.0;
			//eyeRenderer.sharedMaterial.SetColor("EyeAdv_LookDir",Vector4(lookPos.x,lookPos.y,lookPos.z,0.0));

		}



	}

}