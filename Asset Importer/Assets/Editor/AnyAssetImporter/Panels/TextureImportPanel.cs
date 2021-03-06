﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AnyAssetImporter
{
    public class TextureImportPanel : AImporterPanel<TextureAssetRule>
    {

        private TextureStyles m_style;

        private int platformIndex;

        private bool advanceFoldout = true;

        private TextureStyles styles
        {
            get
            {
                if(m_style == null)
                    m_style = new TextureStyles();
                return m_style;
            }
        }

        public override void OnInit()
        {
            base.OnInit();

            string[] folders = ImportPreferences.ImporterSettings.TextureFolders;
            List<FolderElement> folderEles = ImportPreferences.GetAllFolders(folders);
            this.initFolderTree(folderEles);
        }

        public override void OnRuleGUI(TextureAssetRule t)
        {
            base.OnRuleGUI(t);
            
            int index = Math.Max(Array.FindIndex(styles.TextureTypeOpts, opt => opt.text.Equals(t.TextureType.ToString())), 0);
            index = EditorGUILayout.Popup(styles.TextureType, index, styles.TextureTypeOpts);
            t.TextureType = styles.TextureTypeEnumOpts[index];

            GUILayout.Space(5);

            advanceFoldout = EditorGUILayout.Foldout(advanceFoldout, styles.Advanced);

            if (advanceFoldout)
            {
                EditorGUI.indentLevel ++;
                t.NpotScale = (TextureImporterNPOTScale)EditorGUILayout.EnumPopup(styles.NonPowerof2, t.NpotScale);
                t.IsReadable = EditorGUILayout.Toggle(styles.ReadWriteEnable, t.IsReadable);
                t.IsMipmap = EditorGUILayout.Toggle(styles.GenMipMaps, t.IsMipmap);
                EditorGUI.indentLevel--;
            }

            GUILayout.Space(5);

            t.WrapMode = (TextureWrapMode)EditorGUILayout.EnumPopup(styles.WrapMode, t.WrapMode);
            t.FilterMode = (FilterMode) EditorGUILayout.EnumPopup(styles.FilterMode, t.FilterMode);
            t.AnisoLevel = EditorGUILayout.IntSlider(styles.AnisoLevel ,t.AnisoLevel, 0, 16);

            GUILayout.Space(10);

            EditorGUI.indentLevel++;
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(this.platformIndex == 0, "Default", "ButtonLeft")) this.platformIndex = 0;
            if (GUILayout.Toggle(this.platformIndex == 1, "Android", "ButtonMid")) this.platformIndex = 1;
            if (GUILayout.Toggle(this.platformIndex == 2, "iOS", "ButtonRight")) this.platformIndex = 2;
            GUILayout.Space(10);
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("Box");
            int num = Array.FindIndex(styles.MaxSizeOpts, content => content.text.Equals(t.MaxTextureSize.ToString()));
            num = Math.Max(num, 0);
            num = EditorGUILayout.Popup(styles.MaxSize, num, styles.MaxSizeOpts);
            t.MaxTextureSize = Convert.ToInt32(styles.MaxSizeOpts[num].text);

            if (platformIndex == 0)
            {
                t.Compression = (TextureImporterCompression)EditorGUILayout.EnumPopup(styles.Compress, t.Compression);
            }
            else if (platformIndex == 1)
            {
                t.AndroidCompressFormat =(TextureImporterFormat)EditorGUILayout.EnumPopup(styles.CompressFormat, t.AndroidCompressFormat);
                t.CompressionQuality =(TextureCompressionQuality)EditorGUILayout.EnumPopup(styles.CompressionQuality, t.CompressionQuality);
            }
            else if (platformIndex == 2)
            {
                t.iOSCompressFormat = (TextureImporterFormat) EditorGUILayout.EnumPopup(styles.CompressFormat, t.iOSCompressFormat);
                t.CompressionQuality = (TextureCompressionQuality)EditorGUILayout.EnumPopup(styles.CompressionQuality, t.CompressionQuality);
            }
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }
        

    }


}
