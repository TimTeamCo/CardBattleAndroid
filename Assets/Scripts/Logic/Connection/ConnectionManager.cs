using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Logic.Connection
{
    public class ConnectionManager : MonoBehaviour, IConnection
    {
        public Action NoInternet;
        public Action HaveInternet;
        private readonly string _url = "https://www.google.com";
        private IEnumerator _internetConnectionCoroutine;
        
        public void Init()
        {
            _internetConnectionCoroutine = TestInternetConnection();
            StartCoroutine(_internetConnectionCoroutine);
        }

        private IEnumerator TestInternetConnection()
        {
            UnityWebRequest request = UnityWebRequest.Get(_url);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                HaveInternet?.Invoke();
            }
            else
            {
                NoInternet?.Invoke();
            }
            
            yield return new WaitForSeconds(10f);
            StartCoroutine(TestInternetConnection());
        }

        public void HardCheckInternetConnection()
        {
            StopCoroutine(_internetConnectionCoroutine);
            StartCoroutine(_internetConnectionCoroutine);
        }
    }
}