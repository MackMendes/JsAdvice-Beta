using Microsoft.VisualStudio.Text;

namespace JsAdvice.Analyses.Test.Helper
{
    internal class MoqTrackingSpan : ITrackingSpan
    {
        private Span span;
        private SpanTrackingMode trackingMode;

        public MoqTrackingSpan(Span span, SpanTrackingMode trackingMode)
        {
            this.span = span;
            this.trackingMode = trackingMode;
        }

        #region ITrackingSpan

        SnapshotSpan ITrackingSpan.GetSpan(ITextSnapshot snapshot)
        {
            return new SnapshotSpan(snapshot, this.span);
        }

        Span ITrackingSpan.GetSpan(ITextVersion version)
        {
            throw new System.NotImplementedException();
        }

        string ITrackingSpan.GetText(ITextSnapshot snapshot)
        {
            throw new System.NotImplementedException();
        }

        SnapshotPoint ITrackingSpan.GetStartPoint(ITextSnapshot snapshot)
        {
            throw new System.NotImplementedException();
        }

        SnapshotPoint ITrackingSpan.GetEndPoint(ITextSnapshot snapshot)
        {
            throw new System.NotImplementedException();
        }

        ITextBuffer ITrackingSpan.TextBuffer
        {
            get { throw new System.NotImplementedException(); }
        }

        SpanTrackingMode ITrackingSpan.TrackingMode
        {
            get { return this.trackingMode; }
        }

        TrackingFidelityMode ITrackingSpan.TrackingFidelity
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}