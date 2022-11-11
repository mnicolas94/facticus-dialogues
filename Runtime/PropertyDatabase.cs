using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor;
using UnityEngine;

namespace Dialogues
{
    public class PropertyDatabaseHandler<T> where T : ScriptableObject
    {
        private ScriptableObject _assetOwner;
        private List<T> _list;
        private string _assetPrefix;

        public ReadOnlyCollection<T> List => _list.AsReadOnly();

        public PropertyDatabaseHandler(ScriptableObject assetOwner, List<T> list, string assetPrefix)
        {
            _assetOwner = assetOwner;
            _list = list;
            _assetPrefix = assetPrefix;
        }

        public string GetDisplayName(T asset)
        {
            return asset.name.Replace(_assetPrefix, "");
        }
        
        public T CreateNew(string name)
        {
            var asset = ScriptableObject.CreateInstance<T>();
            var assetName = $"{_assetPrefix}{name}";
            asset.name = assetName;
            _list.Add(asset);

            AddAsSubAsset(asset);

            return asset;
        }
        
        private void AddAsSubAsset(ScriptableObject asset)
        {
            var databasePath = AssetDatabase.GetAssetPath(_assetOwner);
            AssetDatabase.AddObjectToAsset(asset, databasePath);
            EditorUtility.SetDirty(_assetOwner);
            AssetDatabase.SaveAssets();
        }
        
        public void Remove(T asset)
        {
            _list.Remove(asset);
            AssetDatabase.RemoveObjectFromAsset(asset);
        }
        
        public void Clear()
        {
            var temp = new List<T>(_list);
            foreach (var asset in temp)
            {
                Remove(asset);
            }
            
            EditorUtility.SetDirty(_assetOwner);
            AssetDatabase.SaveAssets();
        }
    }
}