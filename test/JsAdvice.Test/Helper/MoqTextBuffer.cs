using System;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using NSubstitute;

namespace JsAdvice.Analyses.Test.Helper
{
    internal sealed class MoqTextBuffer : ITextBuffer
    {
        private MoqTextSnapshot _currentSnapshot;
        private PropertyCollection _properties;
        private IContentType _contenteType;

        public MoqTextBuffer(string text)
        {
            this._currentSnapshot = new MoqTextSnapshot(text);
        }

        public MoqTextBuffer(string text, string contentType):
            this(text)
        {
            this._contenteType = Substitute.For<IContentType>();
            this._contenteType.TypeName.Returns(contentType);
            this._contenteType.DisplayName.Returns(contentType);
        }

        public MoqTextSnapshot CurrentSnapshot
        {
            get
            {
                return this._currentSnapshot;
            }

            set
            {
                ITextSnapshot oldSnapshot = this._currentSnapshot;
                this._currentSnapshot = value;
                this.OnChanged(new TextContentChangedEventArgs(oldSnapshot, this._currentSnapshot, new EditOptions(), new object()));
            }
        }

        public EventHandler<TextContentChangedEventArgs> Changed { get; set; }

        #region ITextBuffer

        PropertyCollection IPropertyOwner.Properties
        {
            get { return this._properties ?? (this._properties = new PropertyCollection()); }
        }

        ITextEdit ITextBuffer.CreateEdit(EditOptions options, int? reiteratedVersionNumber, object editTag)
        {
            throw new NotImplementedException();
        }

        ITextEdit ITextBuffer.CreateEdit()
        {
            throw new NotImplementedException();
        }

        IReadOnlyRegionEdit ITextBuffer.CreateReadOnlyRegionEdit()
        {
            throw new NotImplementedException();
        }

        void ITextBuffer.TakeThreadOwnership()
        {
            throw new NotImplementedException();
        }

        bool ITextBuffer.CheckEditAccess()
        {
            throw new NotImplementedException();
        }

        void ITextBuffer.ChangeContentType(IContentType newContentType, object editTag)
        {
            throw new NotImplementedException();
        }

        ITextSnapshot ITextBuffer.Insert(int position, string text)
        {
            throw new NotImplementedException();
        }

        ITextSnapshot ITextBuffer.Delete(Span deleteSpan)
        {
            throw new NotImplementedException();
        }

        ITextSnapshot ITextBuffer.Replace(Span replaceSpan, string replaceWith)
        {
            var getTextJob = this._currentSnapshot.GetText(replaceSpan.Start, replaceSpan.Length);
            var replaceJob = this._currentSnapshot.GetText().Replace(getTextJob, replaceWith);
            this._currentSnapshot = new MoqTextSnapshot(replaceJob);
            return this._currentSnapshot;
        }

        bool ITextBuffer.IsReadOnly(int position)
        {
            throw new NotImplementedException();
        }

        bool ITextBuffer.IsReadOnly(int position, bool isEdit)
        {
            throw new NotImplementedException();
        }

        bool ITextBuffer.IsReadOnly(Span span)
        {
            throw new NotImplementedException();
        }

        bool ITextBuffer.IsReadOnly(Span span, bool isEdit)
        {
            throw new NotImplementedException();
        }

        NormalizedSpanCollection ITextBuffer.GetReadOnlyExtents(Span span)
        {
            throw new NotImplementedException();
        }

        IContentType ITextBuffer.ContentType
        {
            get { return this._contenteType; }
        }

        ITextSnapshot ITextBuffer.CurrentSnapshot
        {
            get { return this.CurrentSnapshot; }
        }

        bool ITextBuffer.EditInProgress
        {
            get { throw new NotImplementedException(); }
        }

        event EventHandler<SnapshotSpanEventArgs> ITextBuffer.ReadOnlyRegionsChanged
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event EventHandler<TextContentChangedEventArgs> ITextBuffer.Changed
        {
            add { this.Changed += value; }
            remove { this.Changed -= value; }
        }

        event EventHandler<TextContentChangedEventArgs> ITextBuffer.ChangedLowPriority
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event EventHandler<TextContentChangedEventArgs> ITextBuffer.ChangedHighPriority
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event EventHandler<TextContentChangingEventArgs> ITextBuffer.Changing
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event EventHandler ITextBuffer.PostChanged
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event EventHandler<ContentTypeChangedEventArgs> ITextBuffer.ContentTypeChanged
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        #endregion

        private void OnChanged(TextContentChangedEventArgs args)
        {
            if (this.Changed != null)
            {
                this.Changed(this, args);
            }
        }
    }
}
