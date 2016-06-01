using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace JsAdvice.LightBulb.CodeFixed.Base
{
    internal abstract class SuggestedActionBasicBase : SuggestedActionBase
    {
        public SuggestedActionBasicBase(ITextBuffer buffer, ITextView view, SnapshotSpan range, string messagerDisplay)
            : base(buffer, view, range, messagerDisplay)
        { }

        #region Preperties

        public abstract string ValueFix { get; }

        public abstract string ValueFixedUp { get; }

        #endregion

        #region SuggestedActionBase

        public override bool HasPreview { get; } = true;

        public override Task<object> GetPreviewAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return base.GetPreviewAsync(cancellationToken);

            var textJob = base.Range.GetText();
            var inline = new Run(textJob.Replace(ValueFix, ValueFixedUp));
            var textBlock = new TextBlock(inline);
            return Task.FromResult<object>(textBlock);
        }

        #endregion

        #region Method

        protected virtual SnapshotSpan GetSpanWithCodeFixed()
        {
            var textShotActual = this.TextBuffer.CurrentSnapshot;

            // Com a linha atual, verifico em qual posição da linha o valor que vou corrigir está.
            int indexOfCompare = Range.GetText().IndexOf(ValueFix);
            // Pego a posição de onde está o valor a corrigir no arquivo (primeiro caracter + a posição do valor à corrigir)
            int newStartPosition = Range.Start.Position + indexOfCompare;

            // Vou criar um novo trexo de palavra, do contexto geral (todo o arquivo), da posição onde inicia o valor que vou corrigir até o final dela mesmo.
            ITrackingSpan trakingSpanNew = textShotActual.CreateTrackingSpan(newStartPosition, ValueFix.Length, SpanTrackingMode.EdgeInclusive);

            // Depois, pego a palavra (Span) para fazer o replace com a palavra corrigida
            return trakingSpanNew.GetSpan(textShotActual);
        }

        internal override bool VerifiyHasCodeFixed()
        {
            if (Range.GetText().IndexOf(ValueFix) > -1)
                return true;

            return false;
        }

        #endregion

    }
}
