using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Linq;
using System.Threading;

namespace JsAdvice.LightBulb.CodeFixed
{
    internal sealed class WithoutSemicolonSuggested : Base.SuggestedActionBase
    {
        #region Fixed
        private const string regexVerify = @"*\(|JPG|gif|GIF|doc|DOC|pdf|PDF)$";
        private readonly char[] notNecessarySemicolon =
            new char[] { '{', '(', '[', ';', ',', '+', '-', '*', '&', '|', '=', '%', ':', '?', '/', '\\', '~', '^', '>', '<', ' ' };

        private const string messagerDisplay = "Aconselhável colocar ponto e virgula no final do comando. Evita erros ao fazer Bundling e Minification.";

        #endregion

        public WithoutSemicolonSuggested(ITextBuffer buffer, ITextView view, SnapshotSpan range)
            : base(buffer, view, range, messagerDisplay)
        { }

        public override void Invoke(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        internal override bool VerifiyHasCodeFixed()
        {
            var textLine = this.Range.GetText().Trim();
            var getLastCharacter = textLine.Length > 0 ? textLine.Substring(textLine.Length - 1).ToCharArray() : new char[] { ' ' };

            if (notNecessarySemicolon.Contains(getLastCharacter[0]))
                return false;

            return VerifyIncludeSemicolon();
        }

        private bool VerifyIncludeSemicolon()
        {
            //var treeSyntacsThisContext = this.Range.Snapshot.

            return false;
        }
    }
}
