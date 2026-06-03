using Newtonsoft.Json;
using SanderSaveli.UDK;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LockalTextManager : TextsManager<Language, LanguageStringData>
    {
        private IStorageService _storageService;
        [SerializeField] private string _url;
        [SerializeField] private string _path;
        private IAppSettings _appSettings;

        [Inject]
        public void Construct(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        private void Awake()
        {
            _storageService = new JsonToResourcesStorageService();
            APIServer.EnableLogging = true;
            ChangeLanguage(_appSettings.Language.Value);
        }

        public override string GetCurrentLanguageValue(LanguageStringData texts)
        {
            switch (Language)
            {
                case Language.en:
                    return texts.en;
                case Language.ru:
                    return texts.ru;
                case Language.it:
                    return texts.it;
                case Language.de:
                    return texts.de;
                case Language.fr:
                    return texts.fr;
                case Language.ja:
                    return texts.ja;
                case Language.ko:
                    return texts.ko;
                case Language.pt:
                    return texts.pt;
                case Language.es:
                    return texts.es;
                default:
                    throw new Exception($"There is no case for language type: {Language}");
            }
        }

        protected override void GetTextFromFile(Action<string> callback)
        {
            _storageService.Load(_path, callback);
        }

        protected override void GetTextFromServer(Action<string> callback)
        {
            StartCoroutine(APIServer.GET(_url, callback, HandleRespanceError));
        }

        protected override Dictionary<string, LanguageStringData> ParseResponce(string responce)
        {
            List<LanguageStringData> textStructs = JsonConvert.DeserializeObject<List<LanguageStringData>>(responce);

            Dictionary<string, LanguageStringData> pairs = new Dictionary<string, LanguageStringData>();

            foreach (LanguageStringData item in textStructs)
            {
                if (pairs.TryGetValue(item.key, out LanguageStringData str))
                {
                    Debug.LogError($"Multiple keys found {item.key}");
                }
                pairs.Add(item.key, item);
            }
            return pairs;
        }

        protected override void SaveToFile(string data)
        {
            _storageService.Save(_path, data, HandleSaveStatus);
        }

        private void HandleSaveStatus(bool isSuccsess)
        {
            if (!isSuccsess)
            {
                Debug.LogError("Error save in file");
            }
        }

        private void HandleRespanceError(string error)
        {
            Debug.LogError($"Error get text from server {error}");
        }
    }
}
