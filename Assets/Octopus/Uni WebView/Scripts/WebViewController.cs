using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Core;
using Octopus.Client;
using Octopus.VerifyInternet;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DG.Tweening;
using UnityEngine.Android;
using UnityEngine.UI;

public class WebViewController : MonoBehaviour
{
    [SerializeField, Header("Reference RectTransform")] private RectTransform _referenceRectTransform;
    
    [SerializeField, Header("Reload start page")] private bool canReload;
    
    private UniWebView _webView;

    private string _url;
    
    private bool _isVisible;
    
    private float _checkTimer = 0f;
    
    private float _checkInterval = 1f;
    
    private Coroutine rotationCoroutine;
    
    #if UNITY_ANDROID && !UNITY_EDITOR
    private KeyboardVisibilityDetector _keyboardDetector;
    #endif
    
    private bool _keyboardUsing;

    private string UrlB
    {
        get
        {
            //return "https://balloonswinner.life/privacypolicy/?3ve1kbqs8h=e9fecaf8-1376-4017-9115-e95a08119218&huax91ae4p=0&6x13a9aqh2=cmpgn=trident-dev-test_TEST-Deeplink_test1_%D1%82%D0%B5%D1%81%D1%822_TEST3_%D0%A2%D0%95%D0%A1%D0%A24_s%20p%20a%20c%20e";//✔
            return "https://betking.com.ua/";//✔
            //return "https://winboss.ua";//✔
            //return "http://www.http2demo.io/";//✔
            //return "https://slotscity.ua/";//✔
            //return "https://www.whatismybrowser.com/detect/are-third-party-cookies-enabled/";//✔
            //return "https://betoholictrack.com/wKWSmlPF?sub_id=23uejou1gt25f"; //✔
            
            if(!GameSettings.HasKey(Constants.IsFirstRunWebView))
            {
                GameSettings.SetFirstWebView();
                
                return GameSettings.GetValue(Constants.ReceiveUrl, "");
            }
            else
            {
                var url = GameSettings.GetValue(Constants.StartUrl, "");
                
                if(!GameSettings.HasKey(Constants.StartUrl))
                {
                    return GameSettings.GetValue(Constants.ReceiveUrl, "");
                }
                
                if (!GameSettings.HasKey(Constants.LastUrl))
                {
                    GameSettings.SetValue(Constants.LastUrl, url);
                }
                
                return url;
            }
        }
        set 
        {
            if(!GameSettings.HasKey(Constants.StartUrl))
            {
                GameSettings.SetValue(Constants.StartUrl, value);
            }
            
            GameSettings.SetValue(Constants.LastUrl, value);
        }
    }
    
    private void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
    {
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        _keyboardDetector = new KeyboardVisibilityDetector(activity);
        _keyboardDetector.OnKeyboardChanged += visible =>
        {
            Debug.Log($"[Keyboard] Changed: {(visible ? "Shown" : "Hidden")}");
            SetFrameForKeyboard(visible);
        };
    }
#endif
        
        InitializeWebView();
    }

    private void OnInitialize(bool? isConnection)
    {
        PrintMessage("### OnInitialize");
        
        CheckConnection(isConnection);
    }
    
    private void CheckConnection(bool? isConnection)
    {
        PrintMessage($"### CheckConnection: isConnection={isConnection}");
        
        if (isConnection != true) return;
        
        if(ConnectivityManager.Instance)
        {
            ConnectivityManager.Instance.OnChangedInternetConnection.AddListener(OnInitialize);
        }
        
        InitializeWebView();
    }

    private void InitializeWebView()
    {
        PrintMessage("### Initialize Webview");

        CreateWebView();

        LoadWebView();
    }

    private void CreateWebView()
    {
        PrintMessage("### Create WebView");
        
        if (_webView != null)
        {
            return;
        }

        UniWebView.SetAllowAutoPlay(true);//+
        UniWebView.SetAllowInlinePlay(true);//+
        UniWebView.SetEnableKeyboardAvoidance(true);//+
        UniWebView.SetJavaScriptEnabled(true);//+
        
        //UniWebView.SetAllowJavaScriptOpenWindow(true);//-
        //UniWebView.SetAllowUniversalAccessFromFileURLs(true);//-

        var webViewGameObject = new GameObject("UniWebView");
        
        _webView = webViewGameObject.AddComponent<UniWebView>();

        SetupWebview(_webView);

        SetUserAgent();

        RegisterShouldHandleRequest();

        SupportMultipleWindows();

        ShouldClose();

        SetFrame();

        Subscribe();
    }

    private void SetupWebview(UniWebView view)
    {
        _webView.EmbeddedToolbar.Hide();//+
        _webView.SetSupportMultipleWindows(true, true);//+
        _webView.SetAllowFileAccess(true);//+
        _webView.SetCalloutEnabled(true);//+
        _webView.SetBackButtonEnabled(true);//+
        _webView.SetAllowBackForwardNavigationGestures(true);//+
        _webView.SetAcceptThirdPartyCookies(true);//+
        
        //_webView.SetAllowFileAccessFromFileURLs(true);//-
        _webView.SetZoomEnabled(true);//-
        
        _webView.OnWebContentProcessTerminated += (view) =>
        {
            PrintMessage("🚨 WebView крашнувся — рестартуємо апку");

            //RestartAppAndroid();

            _webView.Reload();
        };

        _webView.OnOrientationChanged += (view, orientation) =>
        {
            PrintMessage($"### 🛫 OnOrientationChanged");
            
            if (rotationCoroutine != null)
            {
                StopCoroutine(rotationCoroutine);
            }

            rotationCoroutine = StartCoroutine(DelayedAdjust(view));
        };
        
        _webView.RegisterOnRequestMediaCapturePermission((permission) =>
        {
            PrintMessage($"### 📸 RegisterOnRequestMediaCapturePermission: request={string.Join(", ", permission.Resources)}");

            PrintMessage($"### 📸 RegisterOnRequestMediaCapturePermission: request={permission.Host}");
            
            var expected = "VIDEO";

            if (permission.Resources.Contains(expected))
            {
                if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
                { 
                    Permission.RequestUserPermission(Permission.Camera);

                    return UniWebViewMediaCapturePermissionDecision.Prompt;
                }
                 
                return UniWebViewMediaCapturePermissionDecision.Grant;
            }
            
            return UniWebViewMediaCapturePermissionDecision.Grant;
        });
        
        _webView.AddUrlScheme("paytmmp");//+
        _webView.AddUrlScheme("phonepe");//+
        _webView.AddUrlScheme("bankid");//+
        _webView.AddUrlScheme("playtoupi");//+
        _webView.AddUrlScheme("mobiw");//+
        _webView.AddUrlScheme("upi");//+
        
        _webView.OnMessageReceived += (v, message) => 
        {
            PrintMessage($"@@@ ⁉️ OnMessageReceived: message={message.RawMessage}");
            var url = message.RawMessage;
            UnityEngine.Application.OpenURL(url);
        };
    }
    
    private void RestartAppAndroid()
    {
    #if UNITY_ANDROID && !UNITY_EDITOR
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var packageManager = activity.Call<AndroidJavaObject>("getPackageManager");
            var packageName = activity.Call<string>("getPackageName");
            
            var intent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", packageName);
            
            intent.Call<AndroidJavaObject>("addFlags", 0x20000000);
            
            activity.Call("startActivity", intent);
            
            activity.Call("finish");
        }
    #endif
    }

    private void ShouldClose()
    {
        _webView.OnShouldClose += (view) =>
        {
            PrintMessage($"@@@ ⏪ OnShouldClose: url = {view.Url}");
            
            return false;
        };
    }
    
    public void SetFrame()
    {
        PrintMessage($"@@@ SetFrame");
        
        _webView.Frame = FlipRectY(Screen.safeArea);
    }

    private void LateUpdate()
    {
        _checkTimer += Time.deltaTime;

        if (_checkTimer >= _checkInterval)
        {
            _checkTimer = 0f;
            
            if(_keyboardUsing)
                return;

            if (_webView.Frame.width != Screen.safeArea.width || _webView.Frame.height != Screen.safeArea.height)
            {
                PrintMessage($"@@@ 🫥 Різні розміри: WebView={_webView.Frame.width}x{_webView.Frame.height}, Screen={Screen.width}x{Screen.height}");
                
                SetFrame();
            }
        }
    }

    private void SetFrameForKeyboard(bool keyboardVisible)
    {
        PrintMessage($"@@@ SetFrameWithKeyboard: keyboardVisible={keyboardVisible}");
        
        if (keyboardVisible)
        {
            PrintMessage("[WebView] Клавіатура відкрита в портреті — показуємо WebView на весь екран");

            _keyboardUsing = true;
            
            _webView.ReferenceRectTransform = null;
            
            _webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        }
        else
        {
            if (_keyboardUsing)
            {
                PrintMessage("[WebView] Клавіатура прихована в портреті — показуємо WebView з урахуванням SafeArea");

                _keyboardUsing = false;
            
                SetFrame();
            }
        }
    }

    private void RegisterShouldHandleRequest()
    {
        _webView.RegisterShouldHandleRequest(request => {

            PrintMessage($"@@@ 👁️RegisterShouldHandleRequest: request.Url={request.Url}");
            
            CultureInfo ci = new CultureInfo("en-US");
            
            string[] allowedPrefixes = { "http", "about:blank", "intent" };

            if (!allowedPrefixes.Any(prefix => request.Url.StartsWith(prefix, true, ci)))
            {
                PrintMessage($"⁉️ ️RegisterShouldHandleRequest");
            
                UnityEngine.Application.OpenURL(request.Url);
            
                return false;
            }

            if (!IsBlockedUrl(request.Url)) return true;
            
            PrintMessage($"### 🔒Blocked download files: {request.Url}");
                
            return false;
        });
    }

    private void SupportMultipleWindows()
    {
        _webView.OnMultipleWindowOpened += (view, windowId) => {
            PrintMessage($"📫 @@@ OnMultipleWindowOpened");
            PrintMessage($"        view.Url {view.Url}");
            PrintMessage($"        A new window with identifier '{windowId}' is opened");

            if (rotationCoroutine != null)
            {
                StopCoroutine(rotationCoroutine);
            }

            rotationCoroutine = StartCoroutine(DelayedAdjust(view));
            
            view.ScrollTo(0, 0, false);
        };
        
        _webView.OnMultipleWindowClosed += (view, windowId) => {
            PrintMessage($"📪 @@@ OnMultipleWindowClosed");
            PrintMessage($"        view.Url {view.Url}");
            PrintMessage($"        A new window with identifier '{windowId}' is closed");
        };
    }
    
    private IEnumerator DelayedAdjust(UniWebView webView)
    {
        yield return new WaitForEndOfFrame(); 
        
        PrintMessage($"🪃 @@@ DelayedAdjust");
        
        SetFrame();
    }

    private static Rect FlipRectY(Rect rect)
    {
        return new Rect(rect.x, Screen.height - rect.yMax, rect.width, rect.height);
    }
    
    private void Subscribe()
    {
        PrintMessage($"📥Subscribe");
        
        _webView.OnPageStarted += OnPageStarted;
            
        _webView.OnPageFinished += OnPageFinished;

        _webView.OnLoadingErrorReceived += OnLoadingErrorReceived;
    }

    private void SetUserAgent()
    { 
        var agent = _webView.GetUserAgent();
        
        agent = ClearAgent(agent);
        
        agent = Regex.Replace(agent, @"Version/\d+\.\d+", "");
        
        _webView.SetUserAgent(agent);
    }
    
    private string ClearAgent(string agent)
    {
        return agent.Replace("; wv", "");
    }
    
    private void UnSubscribe()
    {
        PrintMessage($"📤UnSubscribe");
        
        _webView.OnPageStarted -= OnPageStarted;
        
        _webView.OnPageFinished -= OnPageFinished;
        
        _webView.OnLoadingErrorReceived -= OnLoadingErrorReceived;
            
        _webView.UnregisterShouldHandleRequest();
    }
    
    private void LoadWebView()
    {
        PrintMessage($"LoadUrl: _webView = {_webView}");

        _url = UrlB;
       
        AddPermissionTrustDomain("forms.kycaid.com");
        
        _webView.Load(_url);
    }
    
    private void AddPermissionTrustDomain(string domain)
    {
        _webView.AddPermissionTrustDomain(domain);
    }

    private void OnLoadingErrorReceived(UniWebView view, int errorCode, string errorMessage, UniWebViewNativeResultPayload payload)
    {
        PrintMessage($"### 💀OnLoadingErrorReceived: errorCode={errorCode}, _webView.Url={_webView.Url}, errorMessage={errorMessage}");

        ShowWebView();
    }
    
    private void OnPageStarted(UniWebView view, string url)
    {
        PrintMessage($"### 🎬OnPageStarted UniWebView: url={url}");

        CultureInfo ci = new CultureInfo("en-US");
        
        if (!url.StartsWith("http", true, ci) && !url.StartsWith("about:blank", true, ci))
        {
            PrintMessage($"⁉️ OnPageStarted");
            
            UnityEngine.Application.OpenURL(url);
            
            if (_webView.CanGoBack) 
                _webView.GoBack();
        }
    }
    
    private void OnPageFinished(UniWebView view, int statusCode, string url)
    {
        PrintMessage($"### 🏁OnPageFinished: url={url}");
        
        view.EvaluateJavaScript("document.body.click();", _ => { PrintMessage("Click!");});
        
        if(url != "about:blank")
        {
            _url = url;

            UrlB = url;
        }
        else
        {
            PrintMessage($"⁉️ url == about:blank");
        }
        
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }

        rotationCoroutine = StartCoroutine(DelayedAdjust(view));
            
        ShowWebView();
        
        DOVirtual.DelayedCall(1, ()=>
        {
            Permissions.PermissionManager.AskPermission("android.permission.POST_NOTIFICATIONS");
        });
    }

    private void HideWebView()
    {
        if(_webView == null) return;
        
        if (!_isVisible) return;

        _isVisible = false;
        
        _webView.Hide();
        
        if(ConnectivityManager.Instance)
        {
            ConnectivityManager.Instance.CheckErrorReceived();
        }
        
        if(ConnectivityManager.Instance)
        {
            ConnectivityManager.Instance.OnChangedInternetConnection.AddListener(CheckConnection);
        }
    }

    private void ShowWebView()
    {
        if(_webView == null) return;
        
        if (_isVisible) return;

        _isVisible = true;
        
        _webView.Show();
    }
    
    private bool IsBlockedUrl(string url)
    {
        string[] blockedExtensions = { ".zip", ".rar", ".apk", ".pdf", ".exe", ".aab", ".bin" };
        
        foreach (var ext in blockedExtensions)
        {
            if (url.EndsWith(ext))
                return true;
        }
        return false;
    }

    private void PrintMessage(string message)
    {
        ConsoleReporter.Info($"@@@ WebViewController ->: {message}", new Color(0.2f, 0.9f, 0.2f));
    }
}
