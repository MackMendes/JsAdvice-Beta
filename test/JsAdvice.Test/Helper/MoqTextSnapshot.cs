using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace JsAdvice.Analyses.Test.Helper
{
    internal sealed class MoqTextSnapshot : ITextSnapshot
    {
        #region Attr

        private readonly string text;

        #endregion

        #region Constructor

        public MoqTextSnapshot(string text)
        {
            Debug.Assert(text != null, "text");
            this.text = text;
        }

        #endregion

        #region Properties

        public ITextBuffer TextBuffer { get { throw new NotImplementedException(); } }

        public IContentType ContentType { get { throw new NotImplementedException(); } }

        public ITextVersion Version { get { throw new NotImplementedException(); } }

        public int Length { get { return this.text.Length; } }

        public int LineCount { get { throw new NotImplementedException(); } }

        public char this[int position] { get { throw new NotImplementedException(); } }

        public IEnumerable<ITextSnapshotLine> Lines { get { throw new NotImplementedException(); } }

        #endregion

        #region Method

        public string GetText(Span span)
        {
            return this.text.Substring(span.Start, span.Length);
        }

        public string GetText(int startIndex, int length)
        {
            return this.text.Substring(startIndex, length);
        }

        public string GetText()
        {
            return this.text;
        }

        public char[] ToCharArray(int startIndex, int length)
        {
            return this.GetText(startIndex, length).ToCharArray();
        }

        public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            throw new NotImplementedException();
        }

        public ITrackingPoint CreateTrackingPoint(int position, PointTrackingMode trackingMode)
        {
            throw new NotImplementedException();
        }

        public ITrackingPoint CreateTrackingPoint(int position, PointTrackingMode trackingMode, TrackingFidelityMode trackingFidelity)
        {
            throw new NotImplementedException();
        }

        public ITrackingSpan CreateTrackingSpan(Span span, SpanTrackingMode trackingMode)
        {
            return new MoqTrackingSpan(span, trackingMode);
        }

        public ITrackingSpan CreateTrackingSpan(Span span, SpanTrackingMode trackingMode, TrackingFidelityMode trackingFidelity)
        {
            throw new NotImplementedException();
        }

        public ITrackingSpan CreateTrackingSpan(int start, int length, SpanTrackingMode trackingMode)
        {
            return this.CreateTrackingSpan(new Span(start, length), trackingMode);
        }

        public ITrackingSpan CreateTrackingSpan(int start, int length, SpanTrackingMode trackingMode, TrackingFidelityMode trackingFidelity)
        {
            throw new NotImplementedException();
        }

        public ITextSnapshotLine GetLineFromLineNumber(int lineNumber) { throw new NotImplementedException(); }

        public ITextSnapshotLine GetLineFromPosition(int position) { throw new NotImplementedException(); }

        public int GetLineNumberFromPosition(int position) { throw new NotImplementedException(); }

        public void Write(TextWriter writer, Span span) { throw new NotImplementedException(); }

        public void Write(TextWriter writer) { throw new NotImplementedException(); }

        #endregion
    }
}

