using UnityEngine;
using UnityEngine.Serialization;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightStyle : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private Color _backgroundColor = Color.black;
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _wrongTextColor = Color.grey;
        [SerializeField] private Color _errorColor = Color.red;
        
        [Header("Metadata")]
        [SerializeField] private Color _metadataSeparatorColor = Color.white;
        [SerializeField] private Color _metadataColor = Color.white;
        
        [Header("Title")]
        [SerializeField] private Color _titleStartColor = Color.white;
        [SerializeField] private Color _titleColor = Color.white;

        [Header("GoTo")]
        [SerializeField] private Color _goToStartColor = Color.white;
        [SerializeField] private Color _goToColor = Color.white;

        [Header("Text Line")]
        [SerializeField] private Color _lineStartColor = Color.white;
        [SerializeField] private Color _authorColor = Color.white;
        [SerializeField] private Color _authorSeparatorColor = Color.white;

        [Header("Text Line/Selection")]
        [SerializeField] private Color _selectLineStartColor = Color.white;
        [SerializeField] private Color _selectLineColor = Color.white;

        [Header("Invoke Method")]
        [SerializeField] private Color _invokeMethodStartColor = Color.white;
        [SerializeField] private Color _invokeMethodColor = Color.white;
        [SerializeField] private Color _booleanParamColor = Color.white;
        [SerializeField] private Color _numericParamColor = Color.white;
        [SerializeField] private Color _stringParamColor = Color.white;
        [SerializeField] private Color _defaultParamColor = Color.white;

        [Header("Comment")]
        [SerializeField] private Color _commentStartColor = Color.gray;
        [SerializeField] private Color _commentColor = Color.gray;

        public Color BackgroundColor => _backgroundColor;
        public Color NormalColor => _normalColor;
        public Color WrongTextColor => _wrongTextColor;
        public Color ErrorColor => _errorColor;
        public Color MetadataSeparatorColor => _metadataSeparatorColor;
        public Color MetadataColor => _metadataColor;

        public Color TitleStartColor => _titleStartColor;
        public Color TitleColor => _titleColor;

        public Color GoToStartColor => _goToStartColor;
        public Color GoToColor => _goToColor;

        public Color LineStartColor => _lineStartColor;
        public Color AuthorColor => _authorColor;
        public Color AuthorSeparatorColor => _authorSeparatorColor;

        public Color SelectLineStartColor => _selectLineStartColor;
        public Color SelectLineColor => _selectLineColor;

        public Color InvokeMethodStartColor => _invokeMethodStartColor;
        public Color InvokeMethodColor => _invokeMethodColor;
        public Color BooleanParamColor => _booleanParamColor;
        public Color NumericParamColor => _numericParamColor;
        public Color StringParamColor => _stringParamColor;
        public Color DefaultParamColor => _defaultParamColor;

        public Color CommentStartColor => _commentStartColor;
        public Color CommentColor => _commentColor;
    }
}
