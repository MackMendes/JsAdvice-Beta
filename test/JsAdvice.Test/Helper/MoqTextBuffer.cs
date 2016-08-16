using System;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using NSubstitute;

namespace JsAdvice.Analyses.Test.Helper
{
    internal sealed class MoqTextBuffer : ITextBuffer
    {
        #region Attr

        private MoqTextSnapshot _currentSnapshot;
        private PropertyCollection _properties;
        private IContentType _contenteType;

        #endregion

        #region Constructor

        public MoqTextBuffer(string text)
        {
            this._currentSnapshot = new MoqTextSnapshot(text);
        }

        public MoqTextBuffer(string text, string contentType) :
            this(text)
        {
            this.BuildContentType(contentType);
        }

        #endregion

        #region Methods

        public MoqTextSnapshot CurrentSnapshot
        {
            get { return this._currentSnapshot; }
            set
            {
                ITextSnapshot oldSnapshot = this._currentSnapshot;
                this._currentSnapshot = value;
                this.OnChanged(new TextContentChangedEventArgs(oldSnapshot, this._currentSnapshot, new EditOptions(), new object()));
            }
        }

        ITextSnapshot ITextBuffer.Replace(Span replaceSpan, string replaceWith)
        {
            var getTextJob = this._currentSnapshot.GetText(replaceSpan.Start, replaceSpan.Length);
            var replaceJob = this._currentSnapshot.GetText().Replace(getTextJob, replaceWith);
            this._currentSnapshot = new MoqTextSnapshot(replaceJob);
            return this._currentSnapshot;
        }

        ITextEdit ITextBuffer.CreateEdit(EditOptions options, int? reiteratedVersionNumber, object editTag) { throw new NotImplementedException(); }

        ITextEdit ITextBuffer.CreateEdit() { throw new NotImplementedException(); }

        IReadOnlyRegionEdit ITextBuffer.CreateReadOnlyRegionEdit() { throw new NotImplementedException(); }

        void ITextBuffer.TakeThreadOwnership() { throw new NotImplementedException(); }

        bool ITextBuffer.CheckEditAccess() { throw new NotImplementedException(); }

        void ITextBuffer.ChangeContentType(IContentType newContentType, object editTag) { throw new NotImplementedException(); }

        ITextSnapshot ITextBuffer.Insert(int position, string text) { throw new NotImplementedException(); }

        ITextSnapshot ITextBuffer.Delete(Span deleteSpan) { throw new NotImplementedException(); }

        bool ITextBuffer.IsReadOnly(int position) { throw new NotImplementedException(); }

        bool ITextBuffer.IsReadOnly(int position, bool isEdit) { throw new NotImplementedException(); }

        bool ITextBuffer.IsReadOnly(Span span) { throw new NotImplementedException(); }

        bool ITextBuffer.IsReadOnly(Span span, bool isEdit) { throw new NotImplementedException(); }

        NormalizedSpanCollection ITextBuffer.GetReadOnlyExtents(Span span) { throw new NotImplementedException(); }

        #endregion

        #region Event from ITextBuffer

        public event EventHandler<TextContentChangedEventArgs> ChangedLowPriority
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        public event EventHandler<TextContentChangedEventArgs> ChangedHighPriority
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        public event EventHandler<TextContentChangingEventArgs> Changing
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        public event EventHandler PostChanged
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        public event EventHandler<ContentTypeChangedEventArgs> ContentTypeChanged
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        public event EventHandler<SnapshotSpanEventArgs> ReadOnlyRegionsChanged
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event EventHandler<TextContentChangedEventArgs> ITextBuffer.Changed
        {
            add { this.Changed += value; }
            remove { this.Changed -= value; }
        }

        private void OnChanged(TextContentChangedEventArgs args)
        {
            this.Changed?.Invoke(this, args);
        }

        #endregion Event from ITextBuffer

        #region Properties

        public EventHandler<TextContentChangedEventArgs> Changed { get; set; }

        PropertyCollection IPropertyOwner.Properties
        {
            get { return this._properties ?? (this._properties = new PropertyCollection()); }
        }

        IContentType ITextBuffer.ContentType { get { return this._contenteType; } }

        ITextSnapshot ITextBuffer.CurrentSnapshot { get { return this.CurrentSnapshot; } }

        bool ITextBuffer.EditInProgress { get { throw new NotImplementedException(); } }

        #endregion Properties

        #region Method of Helper

        /// <summary>
        /// Build Object Moq of IContentType
        /// </summary>
        /// <param name="contentType">Content Type from object</param>
        private void BuildContentType(string contentType)
        {
            this._contenteType = Substitute.For<IContentType>();
            this._contenteType.TypeName.Returns(contentType);
            this._contenteType.DisplayName.Returns(contentType);
        }

        #endregion
    }
}
