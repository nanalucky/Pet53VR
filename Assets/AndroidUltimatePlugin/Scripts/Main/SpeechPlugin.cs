using UnityEngine;
using System.Collections;
using System;

public class SpeechPlugin : MonoBehaviour {

	private static SpeechPlugin instance;
	private static GameObject container;
	private const string TAG="[SpeechPlugin]: ";
	private static AUPHolder aupHolder;

	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	
	
	public bool isDebug =true;
	
	public static SpeechPlugin GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name="SpeechPlugin";
			instance = container.AddComponent( typeof(SpeechPlugin) ) as SpeechPlugin;
			DontDestroyOnLoad(instance.gameObject);
			aupHolder = AUPHolder.GetInstance();
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}
		
		return instance;
	}
	
	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.speech.SpeechPlugin");
		}
		#endif
	}
	
	/// <summary>
	/// Sets the debug.
	/// 0 - false, 1 - true
	/// </summary>
	/// <param name="debug">Debug.</param>
	public void SetDebug(int debug){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("SetDebug",debug);
			AUP.Utils.Message(TAG,"SetDebug");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	public bool CheckSpeechRecognizerSupport(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<bool>("checkSpeechRecognizer");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return false;
	}
	
	public void setSpeechEventListener(
		Action <string>onReadyForSpeech
		,Action <string>onBeginningOfSpeech
		,Action <string>onEndOfSpeech
		,Action <int>onError
		,Action <string>onResults
		){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			SpeechCallback speechCallback = new SpeechCallback();
			speechCallback.onReadyForSpeech = onReadyForSpeech;
			speechCallback.onBeginningOfSpeech = onBeginningOfSpeech;
			speechCallback.onEndOfSpeech = onEndOfSpeech;
			speechCallback.onError = onError;
			speechCallback.onResults = onResults;
			
			
			jo.CallStatic("setSpeechEventListener",speechCallback);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	/// <summary>
	/// Activate the listener for your speech or voice
	/// it will now detect what words you said
	/// although it's not always correct but it is alway nearest
	/// to the words that you said
	/// </summary>
	/// <param name="numberOfResults">Number of results.</param>
	public void StartListening(int numberOfResults){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("startListening",numberOfResults);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Starts the listening with mutebeep option
	/// </summary>
	/// <param name="numberOfResults">Number of results.</param>
	/// <param name="isMuteBeep">If set to <c>true</c> is mute beep.</param>
	public void StartListeningNoBeep(int numberOfResults,bool isMuteBeep){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("startListeningNoBeep",numberOfResults,isMuteBeep);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Cancels the speech recognition
	/// </summary>
	public void Cancel(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("cancel");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	/// <summary>
	/// Removes Speech Recognizer listener.
	/// </summary>
	public void StopListening(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("stopListening");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// if cancel and stop listener don't work use this
	/// Stops the cancel Speech Recognizer
	/// </summary>
	public void StopCancel(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("stopCancel");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	/// <summary>
	/// Destroys the speech Recognizer controller
	/// Note: Call this when you are done using it.
	/// </summary>
	public void DestroySpeechController(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("destroySpeechController");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	public void IncreaseVolume(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("increaseVolume");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	public void DecreaseVolume(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("decreaseVolume");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	public void IncreaseMusicVolumeByValue(int val){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("increaseMusicVolumeByValue",val);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	public void DecreaseMusicVolumeByValue(int val){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("decreaseMusicVolumeByValue",val);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
}