using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.UDK
{
    public class TestTextManager : TextsManager<LanguageType, TableTextStruct>
    {
        private IStorageService _storageService;
        [SerializeField] private string _url;
        [SerializeField] private string _path;

        private void Awake()
        {
            _storageService = new JsonToFileStorageService();
            APIServer.EnableLogging = true;
        }

        public override string GetCurrentLanguageValue(TableTextStruct texts)
        {
            switch (Language)
            {
                case LanguageType.EN:
                    return texts.EN;
                case LanguageType.RU:
                    return texts.RU;
                case LanguageType.DE:
                    return texts.DE;
                case LanguageType.FR:
                    return texts.FR;
                case LanguageType.IT:
                    return texts.IT;
                case LanguageType.ES:
                    return texts.ES;
                case LanguageType.JA:
                    return texts.JA;
                case LanguageType.KO:
                    return texts.KO;
                case LanguageType.PT:
                    return texts.PT;
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

        protected override Dictionary<string, TableTextStruct> ParseResponce(string responce)
        {
            List<TableTextStruct> textStructs = JsonConvert.DeserializeObject<List<TableTextStruct>>(responce);

            Dictionary<string, TableTextStruct> pairs = new Dictionary<string, TableTextStruct>();

            foreach (TableTextStruct item in textStructs)
            {
                if (pairs.TryGetValue(item.KEY, out TableTextStruct str))
                {
                    Debug.LogError($"Multiple keys found {item.KEY}");
                }
                pairs.Add(item.KEY, item);
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
