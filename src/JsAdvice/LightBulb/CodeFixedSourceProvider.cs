using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace JsAdvice.LightBulb
{
    [Export(typeof(ISuggestedActionsSourceProvider))]
    [Name("CodeFixedSourceProvider")]
    [ContentType("javascript")]
    internal class CodeFixedSourceProvider : ISuggestedActionsSourceProvider
    {
        [Import(typeof(ITextStructureNavigatorSelectorService))]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        public ISuggestedActionsSource CreateSuggestedActionsSource(ITextView textView, ITextBuffer textBuffer)
        {
            if (textBuffer == null && textView == null || textBuffer.ContentType.TypeName != "JavaScript")
                return null;

            return new CodeFixedActionsSource(this, textView, textBuffer);
        }
    }
}
