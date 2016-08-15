using System.Threading;
using JsAdvice.Analyses.LightBulb.CodeFixed.Base;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace JsAdvice.Analyses.LightBulb.CodeFixed
{
    public sealed class EqualsOperatorsSuggested : SuggestedActionBasicBase
    {
        #region Fixed

        private const string _messagerDisplay = "Advisable to use three equal (===). It is performative.";

        #endregion

        public EqualsOperatorsSuggested(ITextBuffer buffer, ITextView view, SnapshotSpan range)
        : base(buffer, view, range, _messagerDisplay)
        { }

        #region Preperties SuggestedActionBase

        public override string ValueFix => " == ";

        public override string ValueFixedUp => " === ";

        #endregion

        #region SuggestedActionBase

        public override void Invoke(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            SnapshotSpan getSpan = base.GetSpanWithCodeFixed();
            // Faço o replace
            this.TextBuffer.Replace(getSpan, ValueFixedUp);
        }

        #endregion

    }
}
