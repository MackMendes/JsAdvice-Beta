using System.Threading;
using JsAdvice.Analyses.LightBuld.CodeFixed.Base;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace JsAdvice.Analyses.LightBuld.CodeFixed
{
    public sealed class InitializeObjectSuggested : SuggestedActionBasicBase
    {
        #region Fixed

        private const string messagerDisplay = "Advised change the boot array value, changing 'new Object()' with '{}'. It is performative.";

        #endregion

        public InitializeObjectSuggested(ITextBuffer buffer, ITextView view, SnapshotSpan range)
            : base(buffer, view, range, messagerDisplay)
        { }

        #region Properties SuggestedActionBasicBase 

        public override string ValueFix => " new Object()";

        public override string ValueFixedUp => " {}";

        #endregion

        #region SuggestedActionBase

        public override void Invoke(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            SnapshotSpan getSpan = this.GetSpanWithCodeFixed();
            // Faço o replace
            this.TextBuffer.Replace(getSpan, ValueFixedUp);
        }

        #endregion
    }
}
