#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Harpnet.Carmine {
    public class CreateWeaponList {
        [MenuItem("Assets/Create/CarmineNeo/Create/Weapon List")]
        public static WeaponList Create() {
            WeaponList asset = ScriptableObject.CreateInstance<WeaponList>();

            AssetDatabase.CreateAsset(asset, "Assets/CarmineNeo/Data/WeaponList.asset");
            AssetDatabase.SaveAssets();
            return asset;
        }
    }
}
#endif