using Microsoft.VisualStudio.Text;

namespace JsAdvice.Analyses.Test.Helper
{
    internal class MoqTrackingSpan : ITrackingSpan
    {
        #region Attr

        private Span span;
        private SpanTrackingMode trackingMode;

        #endregion

        #region Constructor

        public MoqTrackingSpan(Span span, SpanTrackingMode trackingMode)
        {
            this.span = span;
            this.trackingMode = trackingMode;
        }

        #endregion

        #region Properties

        SpanTrackingMode ITrackingSpan.TrackingMode { get { return this.trackingMode; } }

        TrackingFidelityMode ITrackingSpan.TrackingFidelity { get { throw new System.NotImplementedException(); } }

        #endregion

        #region Methods

        SnapshotSpan ITrackingSpan.GetSpan(ITextSnapshot snapshot)
        {
            return new SnapshotSpan(snapshot, this.span);
        }

        Span ITrackingSpan.GetSpan(ITextVersion version)
        {
            return this.span;
        }

        string ITrackingSpan.GetText(ITextSnapshot snapshot)
        {
            return snapshot.GetText();
        }

        SnapshotPoint ITrackingSpan.GetStartPoint(ITextSnapshot snapshot) { throw new System.NotImplementedException(); }

        SnapshotPoint ITrackingSpan.GetEndPoint(ITextSnapshot snapshot) { throw new System.NotImplementedException(); }

        ITextBuffer ITrackingSpan.TextBuffer { get { throw new System.NotImplementedException(); } }

        #endregion
    }
}