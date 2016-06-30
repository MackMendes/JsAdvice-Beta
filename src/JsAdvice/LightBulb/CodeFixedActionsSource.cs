using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using JsAdvice.LightBulb.CodeFixed;
using System.Linq;
using JsAdvice.LightBulb.CodeFixed.Base;
using JsAdvice.LightBulb.Helper;

namespace JsAdvice.LightBulb
{
    internal class CodeFixedActionsSource : ISuggestedActionsSource
    {
        private IList<SuggestedActionBase> _listSuggestedActionBase;

        private readonly CodeFixedSourceProvider codeFixedSourceProvider;
        internal ITextBuffer TextBuffer { get; }
        internal ITextView TextView { get; }

        public CodeFixedActionsSource(CodeFixedSourceProvider codeFixedSourceProvider, ITextView textView, ITextBuffer textBuffer)
        {
            this.codeFixedSourceProvider = codeFixedSourceProvider;
            this.TextView = textView;
            this.TextBuffer = textBuffer;
            //this.textBuffer.CreateEdit(new EditOptions(new Microsoft.VisualStudio.Text.Differencing.StringDifferenceOptions(Microsoft.VisualStudio.Text.Differencing.StringDifferenceTypes.Line), )
        }

        #region Base Implementetion

        public event EventHandler<EventArgs> SuggestedActionsChanged = (sender, e) => { };

        public void Dispose()
        {
            if (_listSuggestedActionBase != null)
            {
                _listSuggestedActionBase.Clear();
                _listSuggestedActionBase = null;
            }
        }

        public IEnumerable<SuggestedActionSet> GetSuggestedActions(ISuggestedActionCategorySet requestedActionCategories, SnapshotSpan range,
            CancellationToken cancellationToken)
        {
            TextExtent extent;
            if (!cancellationToken.IsCancellationRequested && TryGetWordUnderCaret(out extent) && extent.IsSignificant && _listSuggestedActionBase != null)
            {
                //// Posso fazer um centralizador de "possíveis" instâncias... mas, só efetivar as instâncias aqui...
                //// Utilizando o using para da Dispose sempre que 
                //ISuggestedAction comparison = new ComparisonOperatorsSuggestd(this.TextBuffer, this.TextView, range, extent);
                var suggestedActionSet = new SuggestedActionSet(_listSuggestedActionBase);

                Dispose();

                return new SuggestedActionSet[] { suggestedActionSet };
            }
            return Enumerable.Empty<SuggestedActionSet>();
        }

        public Task<bool> HasSuggestedActionsAsync(ISuggestedActionCategorySet requestedActionCategories, SnapshotSpan range, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                TextExtent extent;
                if (cancellationToken.IsCancellationRequested == false && TryGetWordUnderCaret(out extent))
                {
                    this.FillListSuggested(range);
                    return extent.IsSignificant && this._listSuggestedActionBase.Count > 0;
                }
                return false;
            });
        }

        public bool TryGetTelemetryId(out Guid telemetryId)
        {
            // This is a sample provider and doesn't participate in LightBulb telemetry
            telemetryId = Guid.Empty;
            return false;
        }

        #endregion

        #region Method

        private bool TryGetWordUnderCaret(out TextExtent wordExtent)
        {
            ITextCaret caret = TextView.Caret;
            SnapshotPoint point;

            if (caret.Position.BufferPosition > 0)
            {
                point = caret.Position.BufferPosition - 1;
                ITextStructureNavigator navigator = this.codeFixedSourceProvider.NavigatorService.GetTextStructureNavigator(TextBuffer);
                wordExtent = navigator.GetExtentOfWord(point);

                return true;
            }
            else
            {
                wordExtent = default(TextExtent);
                return false;
            }
        }

        private void FillListSuggested(SnapshotSpan range)
        {
            this._listSuggestedActionBase = VerifyCodeFixes.VerifyExistsCodeFixes(this, range);
        }

        #endregion
    }
}
