using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.Threading;

namespace JsAdvice.LightBulb.CodeFixed
{
    internal sealed class EqualsOperatorsSuggested : Base.SuggestedActionBasicBase
    {
        #region Fixed

        private const string messagerDisplay = "Aconselhável utilizar três iguais (===). É mais performático.";

        #endregion

        public EqualsOperatorsSuggested(ITextBuffer buffer, ITextView view, SnapshotSpan range)
        : base(buffer, view, range, messagerDisplay)
        { }

        #region Preperties SuggestedActionBase

        public override string ValueFix { get { return " == "; } }

        public override string ValueFixedUp { get { return " === "; } }

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
