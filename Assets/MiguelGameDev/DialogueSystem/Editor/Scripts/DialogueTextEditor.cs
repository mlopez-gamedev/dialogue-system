using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace MiguelGameDev.DialogueSystem.Editor
{
    [CustomEditor(typeof(DialogueText))]
    public class DialogueTextEditor : UnityEditor.Editor
    {
        private SerializedProperty _textProperty;
        private TextHighlighter _highlighter;
        private GUIStyle _style;
        private GUIStyle _boxStyle;

        private Vector2 _scrollPosition = Vector2.zero;
        // private float _currentScrollViewHeight;

        private string _highlightedText;

        void OnEnable()
        {
            _textProperty = serializedObject.FindProperty("_text");

            _highlighter = new TextHighlighter();
        }

        public override void OnInspectorGUI()
        {
            var width = EditorGUIUtility.currentViewWidth;
            var height = Screen.height * (width / Screen.width) - 180f;
            var halfHeight = height / 2f;

            SetBoxStyle();
            SetTextStyle();

            //EditorGUILayout.BeginVertical();

            DrawProperties(halfHeight);
            DrawHighlightedText(width, halfHeight);

            //EditorGUILayout.EndVertical();
            Repaint();
        }

        private void SetTextStyle()
        {
            if (_style != null)
            {
                return;
            }

            _style = new GUIStyle(GUI.skin.label);
            _style.padding = new RectOffset(15, 15, 15, 15);
            _style.margin = new RectOffset(0, 0, -15, -15);
            _style.normal.textColor = _highlighter.Style.NormalColor;
            _style.normal.background = MakeTex(2, 2, _highlighter.Style.BackgroundColor);
            _style.border = new RectOffset(2, 2, 2, 2);
            _style.richText = true;
            _style.wordWrap = true;
        }

        private void SetBoxStyle()
        {
            if (_boxStyle != null)
            {
                return;
            }

            _boxStyle = new GUIStyle(GUI.skin.box);
            _boxStyle.normal.background = MakeTex(2, 2, _highlighter.Style.BackgroundColor);
        }

        private void DrawProperties(float height)
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.UpdateIfRequiredOrScript();
            //_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, true, GUILayout.Height(height));
            //_textProperty.stringValue = EditorGUILayout.TextArea(_textProperty.stringValue, GUILayout.Height(height));
            //EditorGUILayout.EndScrollView();
            EditorGUILayout.PropertyField(_textProperty, GUILayout.Height(height));
            serializedObject.ApplyModifiedProperties();

            // ÑAPÓN: De algún modo con esto funciona el tabulador :D
            _textProperty.stringValue = GUI.TextArea(Rect.zero, _textProperty.stringValue);

            if (EditorGUI.EndChangeCheck() || string.IsNullOrEmpty(_highlightedText))
            {
                _highlightedText = _highlighter.Highlight(_textProperty.stringValue);
            }

            //Repaint();
        }

        private void DrawHighlightedText(float width, float height)
        {
            EditorGUILayout.LabelField("Highlighted text");
            //Rect labelRect = EditorGUILayout.GetControlRect();
            var labelContent = new GUIContent(_highlightedText);
            var labelHeight = _style.CalcHeight(labelContent, width);
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, true, GUILayout.Height(height));
            EditorGUILayout.LabelField(labelContent, _style, GUILayout.Height(labelHeight));
            EditorGUILayout.EndScrollView();
        }

        Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }

            var result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }
    }
}
