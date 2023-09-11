using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Harpnet.Carmine;

namespace com.harpnet.carmine.editor {
    public class CarmineNeoEditor : EditorWindow {
        public WeaponList weaponList;
        private int viewIndex = 1;

        [MenuItem("Window/CarmineNeo Editor")]
        private static void Init() {
            EditorWindow.GetWindow(typeof(CarmineNeoEditor));
        }

        private void OnEnable() {
            if(EditorPrefs.HasKey("ObjectPath")) {
                string objectPath = EditorPrefs.GetString("ObjectPath");
                weaponList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(WeaponList)) as WeaponList;
            }
        }

        private void OnGUI() {
            GUILayout.BeginHorizontal();
            GUILayout.Label("CarmineNeo Editor", EditorStyles.boldLabel);

            if(weaponList != null) {
                if(GUILayout.Button("Show Weapon List")) {
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = weaponList;
                }
            }

            if(GUILayout.Button("Open Weapon List")) {
                OpenWeaponList();
            }

            if(GUILayout.Button("New Weapon List")) {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = weaponList;
            }

            GUILayout.EndHorizontal();

            if(weaponList == null) {
                GUILayout.BeginHorizontal();

                GUILayout.Space(10);
                if(GUILayout.Button("Create New Weapon List", GUILayout.ExpandWidth(false)))
                    CreateNewWeaponList();
                if(GUILayout.Button("Open Exisiting Weapon List", GUILayout.ExpandWidth(false)))
                    OpenWeaponList();

                GUILayout.EndHorizontal();
            }

            if(weaponList != null) {
                GUILayout.BeginHorizontal();

                GUILayout.Space(10);

                if(GUILayout.Button("Prev", GUILayout.ExpandWidth(false))) {
                    if(viewIndex > 1)
                        --viewIndex;
                }

                GUILayout.Space(5);

                if(GUILayout.Button("Next", GUILayout.ExpandWidth(false))) {
                    if(viewIndex < weaponList.weapons.Count)
                        ++viewIndex;
                }

                GUILayout.Space(60);

                if(GUILayout.Button("Add Weapon", GUILayout.ExpandWidth(false)))
                    AddWeapon();

                if(GUILayout.Button("Remove Weapon", GUILayout.ExpandWidth(false)))
                    DeleteWeapon(viewIndex - 1);

                GUILayout.EndHorizontal();
            }

            if(weaponList.weapons == null)
                Debug.Log("Weapon List is Empty!");
            if(weaponList.weapons.Count > 0) {
                GUILayout.BeginHorizontal();

                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Weapon", viewIndex, GUILayout.ExpandWidth(false)), 1, weaponList.weapons.Count);

                EditorGUILayout.LabelField("of  " + weaponList.weapons.Count.ToString() + "  weapons", "", GUILayout.ExpandWidth(false));

                GUILayout.EndHorizontal();

                /* Core Values */
                weaponList.weapons[viewIndex - 1].weaponName = EditorGUILayout.TextField("Weapon Name", weaponList.weapons[viewIndex - 1].weaponName as string);
                weaponList.weapons[viewIndex - 1].weaponSprite = EditorGUILayout.ObjectField("Weapon Sprite", weaponList.weapons[viewIndex - 1].weaponSprite, typeof(Sprite), false) as Sprite;
                weaponList.weapons[viewIndex - 1].weaponPrefab = EditorGUILayout.ObjectField("Weapon Prefab", weaponList.weapons[viewIndex - 1].weaponPrefab, typeof(GameObject), false) as GameObject;

                /* Weapon Stats */
                weaponList.weapons[viewIndex - 1].sound = EditorGUILayout.ObjectField("Firing Sound", weaponList.weapons[viewIndex - 1].sound, typeof(AudioClip), false) as AudioClip;
                weaponList.weapons[viewIndex - 1].attackDelay = EditorGUILayout.IntField("Attack Delay", weaponList.weapons[viewIndex - 1].attackDelay);
                weaponList.weapons[viewIndex - 1].damage = EditorGUILayout.IntField("Damage", weaponList.weapons[viewIndex - 1].damage);
                weaponList.weapons[viewIndex - 1].critcalDamage = EditorGUILayout.IntField("Critical Damage", weaponList.weapons[viewIndex - 1].critcalDamage);
                weaponList.weapons[viewIndex - 1].spread = EditorGUILayout.IntField("Spread", weaponList.weapons[viewIndex - 1].spread);
                weaponList.weapons[viewIndex - 1].projectileSpeed = EditorGUILayout.IntField("Projectile Speed", weaponList.weapons[viewIndex - 1].projectileSpeed);
                weaponList.weapons[viewIndex - 1].recoilAmount = EditorGUILayout.IntField("Recoil", weaponList.weapons[viewIndex - 1].recoilAmount);
                weaponList.weapons[viewIndex - 1].range = EditorGUILayout.IntField("Range", weaponList.weapons[viewIndex - 1].range);
                weaponList.weapons[viewIndex - 1].rays = EditorGUILayout.IntField("Rays", weaponList.weapons[viewIndex - 1].rays);
                weaponList.weapons[viewIndex - 1].hitPush = EditorGUILayout.IntField("Hit Push", weaponList.weapons[viewIndex - 1].hitPush);
                weaponList.weapons[viewIndex - 1].ammoAdd = EditorGUILayout.IntField("Ammo Add", weaponList.weapons[viewIndex - 1].ammoAdd);
                weaponList.weapons[viewIndex - 1].ammoMax = EditorGUILayout.IntField("Ammo Max", weaponList.weapons[viewIndex - 1].ammoMax);
                weaponList.weapons[viewIndex - 1].explosionRadius = EditorGUILayout.IntField("Explosion Radius", weaponList.weapons[viewIndex - 1].explosionRadius);
                weaponList.weapons[viewIndex - 1].timeToExplode = EditorGUILayout.IntField("Time Till Explode", weaponList.weapons[viewIndex - 1].timeToExplode);

                /* Secondary Fire */
                weaponList.weapons[viewIndex - 1].secondaryFire = EditorGUILayout.ToggleLeft("Secondary Fire?", weaponList.weapons[viewIndex - 1].secondaryFire);
                if(weaponList.weapons[viewIndex - 1].secondaryFire)
                    weaponList.weapons[viewIndex - 1].secondaryConsume = EditorGUILayout.IntField("Secondary Consume", weaponList.weapons[viewIndex - 1].secondaryConsume);
            }
        }

        private void CreateNewWeaponList() {
            viewIndex = 1;
            weaponList = CreateWeaponList.Create();
            if(weaponList) {
                weaponList.weapons = new List<WeaponData>();
                string relPath = AssetDatabase.GetAssetPath(weaponList);
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }

        private void OpenWeaponList() {
            string absPath = EditorUtility.OpenFilePanel("Select Weapon Item List", "", "");
            if(absPath.StartsWith(Application.dataPath)) {
                string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
                weaponList = AssetDatabase.LoadAssetAtPath(relPath, typeof(WeaponList)) as WeaponList;

                if(weaponList.weapons == null)
                    weaponList.weapons = new List<WeaponData>();
                if(weaponList)
                    EditorPrefs.SetString("ObjectPath", relPath);
            }
        }

        private void AddWeapon() {
            WeaponData newWeapon = new WeaponData();
            newWeapon.weaponName = "New Weapon";
            weaponList.weapons.Add(newWeapon);
            viewIndex = weaponList.weapons.Count;
        }

        private void DeleteWeapon(int index) {
            weaponList.weapons.RemoveAt(index);
        }
    }
}