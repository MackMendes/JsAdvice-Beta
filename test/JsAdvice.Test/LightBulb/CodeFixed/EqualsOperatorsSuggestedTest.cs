using System.Threading;
using JsAdvice.Analyses.LightBulb.CodeFixed;
using JsAdvice.Analyses.LightBulb.CodeFixed.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using NSubstitute;
using JsAdvice.Analyses.Test.Helper;

namespace JsAdvice.Analyses.Test.LightBulb.CodeFixed
{
    [TestClass]
    public class EqualsOperatorsSuggestedTest
    {
        [TestMethod]
        public void EqualsOperators_NotHasCodeFixed()
        {
            // Preparar cénários
            ITextBuffer buffer = Substitute.For<ITextBuffer>();
            ITextView textView = Substitute.For<ITextView>();
            SnapshotSpan span = this.GetInstanceToJob("var teste;");

            // Ação (Executar)
            SuggestedActionBasicBase equalsOperators = new EqualsOperatorsSuggested(buffer, textView, span);

            // Afirmar (Verificar resultado)
            var hasCodeFixed = equalsOperators.VerifiyHasCodeFixed();
            Assert.IsFalse(hasCodeFixed);
        }

        [TestMethod]
        public void EqualsOperators_CodeFixed()
        {
            // Properties
            var contextType = "JavaScript";
            var codeFixed = "if (var1 == var 2)";
            var codeExpected = "if (var1 === var 2)";

            // Moqs
            ITextBuffer buffer = new MoqTextBuffer(codeFixed, contextType);
            SnapshotSpan span = this.GetInstanceToJob(codeFixed);
            ITextView textView = Substitute.For<ITextView>();
            textView.TextBuffer.Returns(buffer);
            
            // Preparar cénários
            SuggestedActionBasicBase equalsOperators = new EqualsOperatorsSuggested(buffer, textView, span);
            equalsOperators.Invoke(CancellationToken.None);

            var resultSuggested = buffer.CurrentSnapshot.GetText();
            Assert.AreEqual(codeExpected, resultSuggested);
        }


        private SnapshotSpan GetInstanceToJob(string spanText)
        {
            ITextSnapshot snapshot = Substitute.For<ITextSnapshot>();
            SnapshotSpan span = new SnapshotSpan(snapshot, new Span());
            span.GetText().Returns(spanText);
            return span;
        }
    }
}
