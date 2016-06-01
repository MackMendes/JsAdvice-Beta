using JsAdvice.LightBulb.CodeFixed;
using JsAdvice.LightBulb.CodeFixed.Base;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.Collections.Generic;
using System.Linq;

namespace JsAdvice.LightBulb.Helper
{
    internal static class VerifyCodeFixes
    {
        internal static List<SuggestedActionBase> VerifyExistsCodeFixes(CodeFixedActionsSource contextActual, SnapshotSpan range)
        {
            var listReturn = new List<SuggestedActionBase>();
            var listAll = GetAllCodeFixes(contextActual, range);

            for (int i = 0; i < listAll.Count(); i++)
            {
                if (listAll[i].VerifiyHasCodeFixed())
                    listReturn.Add(listAll[i]);
            }
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
                new EqualsOperatorsSuggestd(buffer, view, range), 
                new NotEqualsOperatorsSuggestd(buffer, view, range)
            };
        }
    }
}
