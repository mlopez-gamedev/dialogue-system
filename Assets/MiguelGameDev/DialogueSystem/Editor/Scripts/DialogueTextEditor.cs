using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace MiguelGameDev.DialogueSystem.Editor
{
	[CustomEditor(typeof(DialogueText))]
	public class DialogueTextEditor : UnityEditor.Editor
	{
		private SerializedObject _editorSerializedObject;
		private SerializedProperty _textProperty;
		private TextHighlighter _highlighter;

		private Vector2 _scrollPosition;
		
		private string _dialogue
		{
			get { return _textProperty != null ? _textProperty.stringValue : ""; }
			set { this._textProperty.stringValue = value; }
		}
		
		string _cachedDialogue { get; set; }
		string _cachedHighlightedDialogue { get; set; }
		
		void OnEnable()
		{
			_editorSerializedObject = new SerializedObject(this);
			_textProperty = serializedObject.FindProperty("_text");
			_highlighter = new TextHighlighter();
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			
			if (_editorSerializedObject != null)
			{
				_editorSerializedObject.Update();
			}
			
			Draw();
			
			if (_editorSerializedObject != null)
			{
				_editorSerializedObject.ApplyModifiedProperties();
			}
			
			serializedObject.ApplyModifiedProperties();

			Repaint();
		}
		
		private void Draw()
		{
			EditorGUILayout.LabelField("Dialogue");
			
			var minHeight = GUILayout.MinHeight(200);
			
			var width = EditorGUIUtility.currentViewWidth;
			var maxHeight = GUILayout.MaxHeight(Screen.height * (width / Screen.width) - 190f);
			_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, minHeight, maxHeight);
			{
				var style = new GUIStyle(GUI.skin.textArea);
				style.padding = new RectOffset(6, 6, 6, 6);
				//style.font = "CONSOLE";
				//style.fontSize = _highlighter.Style.si;
				style.wordWrap = true;

				_dialogue = DrawDialogue(_dialogue, style, GUILayout.ExpandHeight(true));
			}
			EditorGUILayout.EndScrollView();

			EditorGUILayout.Space();
		}
		
		private string DrawDialogue(string dialogue, GUIStyle style, params GUILayoutOption[] options) {
			var preBackgroundColor = GUI.backgroundColor;
			var preColor = GUI.color;
			//Color preSelection = GUI.skin.settings.selectionColor;
			//Color preCursor = GUI.skin.settings.cursorColor;
			//float preFlashSpeed = GUI.skin.settings.cursorFlashSpeed;

			GUI.backgroundColor = _highlighter.Style.BackgroundColor;
			//GUI.color = _highlighter.Style.NormalColor;
			//GUI.skin.settings.selectionColor = _highlighter.Style.BackgroundColor.SelectionColor;
			//GUI.skin.settings.cursorColor = _highlighter.Style.BackgroundColor.CursorColor;
			GUI.skin.settings.cursorColor = Color.white;
			
			var backStyle = new GUIStyle(style);
			backStyle.normal.textColor = Color.clear;
			backStyle.hover.textColor = Color.clear;
			backStyle.active.textColor = Color.clear;
			backStyle.focused.textColor = Color.clear;
			
			GUI.SetNextControlName("Dialogue");

			// IMPORTANT: 
			// Sadly, we cannot use TextEditor with (EditorGUILayout|EditorGUI).TextArea()... X(
			// And GUILayout.TextArea() cannot handle TAB key... ;_;
			// GUI.TextArea needs a lot of tasks to implement absic functions... T_T
			dialogue = EditorGUILayout.TextArea(dialogue, backStyle, GUILayout.ExpandHeight(true));

			// So, this does not work...
			// var editor = GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl) as TextEditor;
			// CheckEvents(editor);

			if (string.IsNullOrEmpty(_cachedHighlightedDialogue) || (_cachedDialogue != dialogue)) {
				_cachedDialogue = dialogue;
				_cachedHighlightedDialogue = _highlighter.Highlight(dialogue);
			}

			GUI.backgroundColor = Color.clear;

			var foreStyle = new GUIStyle(style);
			foreStyle.richText = true;
			foreStyle.normal.textColor = _highlighter.Style.NormalColor;
			foreStyle.hover.textColor = _highlighter.Style.NormalColor;
			foreStyle.active.textColor = _highlighter.Style.NormalColor;
			foreStyle.focused.textColor = _highlighter.Style.NormalColor;

			EditorGUI.TextArea(GUILayoutUtility.GetLastRect(), _cachedHighlightedDialogue, foreStyle);

			GUI.backgroundColor = preBackgroundColor;
			GUI.color = preColor;
			//GUI.skin.settings.selectionColor = preSelection;
			//GUI.skin.settings.cursorColor = preCursor;
			//GUI.skin.settings.cursorFlashSpeed = preFlashSpeed;
			
			return dialogue;
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
	
	
    //[CustomEditor(typeof(DialogueText))]
    //public class DialogueTextEditor : UnityEditor.Editor
    //{
    //    private SerializedProperty _textProperty;
    //    private TextHighlighter _highlighter;
    //    private GUIStyle _style;
    //    private GUIStyle _boxStyle;

    //    private Vector2 _scrollPosition = Vector2.zero;
    //    // private float _currentScrollViewHeight;

    //    private string _highlightedText;

    //    void OnEnable()
    //    {
    //        _textProperty = serializedObject.FindProperty("_text");

    //        _highlighter = new TextHighlighter();
    //    }

    //    public override void OnInspectorGUI()
    //    {
    //        var width = EditorGUIUtility.currentViewWidth;
    //        var height = Screen.height * (width / Screen.width) - 180f;
    //        var halfHeight = height / 2f;

    //        SetBoxStyle();
    //        SetTextStyle();

    //        //EditorGUILayout.BeginVertical();

    //        DrawProperties(halfHeight);
    //        DrawHighlightedText(width, halfHeight);

    //        //EditorGUILayout.EndVertical();
    //        Repaint();
    //    }

    //    private void SetTextStyle()
    //    {
    //        if (_style != null)
    //        {
    //            return;
    //        }

    //        _style = new GUIStyle(GUI.skin.label);
    //        _style.padding = new RectOffset(15, 15, 15, 15);
    //        _style.margin = new RectOffset(0, 0, -15, -15);
    //        _style.normal.textColor = _highlighter.Style.NormalColor;
    //        _style.normal.background = MakeTex(2, 2, _highlighter.Style.BackgroundColor);
    //        _style.border = new RectOffset(2, 2, 2, 2);
    //        _style.richText = true;
    //        _style.wordWrap = true;
    //    }

    //    private void SetBoxStyle()
    //    {
    //        if (_boxStyle != null)
    //        {
    //            return;
    //        }

    //        _boxStyle = new GUIStyle(GUI.skin.box);
    //        _boxStyle.normal.background = MakeTex(2, 2, _highlighter.Style.BackgroundColor);
    //    }

    //    private void DrawProperties(float height)
    //    {
    //        EditorGUI.BeginChangeCheck();
    //        serializedObject.UpdateIfRequiredOrScript();
    //        //_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, true, GUILayout.Height(height));
    //        //_textProperty.stringValue = EditorGUILayout.TextArea(_textProperty.stringValue, GUILayout.Height(height));
    //        //EditorGUILayout.EndScrollView();
    //        EditorGUILayout.PropertyField(_textProperty, GUILayout.Height(height));
    //        serializedObject.ApplyModifiedProperties();

    //        // ÑAPÓN: De algún modo con esto funciona el tabulador :D
    //        _textProperty.stringValue = GUI.TextArea(Rect.zero, _textProperty.stringValue);

    //        if (EditorGUI.EndChangeCheck() || string.IsNullOrEmpty(_highlightedText))
    //        {
    //            _highlightedText = _highlighter.Highlight(_textProperty.stringValue);
    //        }

    //        //Repaint();
    //    }

    //    private void DrawHighlightedText(float width, float height)
    //    {
    //        EditorGUILayout.LabelField("Highlighted text");
    //        //Rect labelRect = EditorGUILayout.GetControlRect();
    //        var labelContent = new GUIContent(_highlightedText);
    //        var labelHeight = _style.CalcHeight(labelContent, width);
    //        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, true, GUILayout.Height(height));
    //        EditorGUILayout.LabelField(labelContent, _style, GUILayout.Height(labelHeight));
    //        EditorGUILayout.EndScrollView();
    //    }

    //    Texture2D MakeTex(int width, int height, Color col)
    //    {
    //        Color[] pix = new Color[width * height];
    //        for (int i = 0; i < pix.Length; ++i)
    //        {
    //            pix[i] = col;
    //        }

    //        var result = new Texture2D(width, height);
    //        result.SetPixels(pix);
    //        result.Apply();

    //        return result;
    //    }
    //}
}
