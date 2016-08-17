using Microsoft.VisualStudio.Text;
using NSubstitute;

namespace JsAdvice.Analyses.Test.Helper
{
    public static class CommonHelper
    {
        internal static SnapshotSpan GetInstanceSnapshotSpan(string spanText)
        {
            ITextSnapshot snapshot = Substitute.For<ITextSnapshot>();
            SnapshotSpan span = new SnapshotSpan(snapshot, new Span());
            span.GetText().Returns(spanText);
            return span;
        }
    }
}
