﻿using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.Threading;

namespace JsAdvice.LightBulb.CodeFixed
{
    internal sealed class InicialyzeObjectSuggested : Base.SuggestedActionBasicBase
    {
        #region Fixed

        private const string messagerDisplay = "Advised change the boot array value, changing 'new Object()' with '{}'. It is performative.";

        #endregion

        public InicialyzeObjectSuggested(ITextBuffer buffer, ITextView view, SnapshotSpan range)
            : base(buffer, view, range, messagerDisplay)
        { }

        #region Properties SuggestedActionBasicBase 

        public override string ValueFix { get { return " new Object()"; } }

        public override string ValueFixedUp { get { return " {}"; } }

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
