using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.Collections.Generic;
using System.Linq;
using JsAdvice.Analyses.LightBulb.CodeFixed;
using JsAdvice.Analyses.LightBulb.CodeFixed.Base;

namespace JsAdvice.LightBulb.Helper
{
    internal static class VerifyCodeFixes
    {
        internal static List<SuggestedActionBase> VerifyExistsCodeFixes(CodeFixedActionsSource contextActual, SnapshotSpan range)
        {
            var listAll = GetAllCodeFixes(contextActual, range);
            var listReturn = listAll.Where(t => t.VerifiyHasCodeFixed()).ToList();
            listAll.Clear();
            return listReturn;
        }

        private static List<SuggestedActionBase> GetAllCodeFixes(CodeFixedActionsSource contextActual, SnapshotSpan range)
        {
            var textBuffer = contextActual.TextBuffer;
            var textView = contextActual.TextView;

            return AllFixeds(textBuffer, textView, range);
        }

        private static List<SuggestedActionBase> AllFixeds(ITextBuffer buffer, ITextView view, SnapshotSpan range)
        {
            return new List<SuggestedActionBase> {
                new EqualsOperatorsSuggested(buffer, view, range), 
                new UnequalOperatorsSuggested(buffer, view, range),
                new InitializeArraySuggested(buffer, view, range),
                new InitializeObjectSuggested(buffer, view, range),
                new WithoutSemicolonSuggested(buffer, view, range),
            };
        }
    }
}
