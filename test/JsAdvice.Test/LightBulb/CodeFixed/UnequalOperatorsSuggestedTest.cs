using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.Text;
using NSubstitute;
using JsAdvice.Analyses.Test.Helper;
using Microsoft.VisualStudio.Text.Editor;
using JsAdvice.Analyses.LightBulb.CodeFixed.Base;
using JsAdvice.Analyses.LightBulb.CodeFixed;
using System.Threading;

namespace JsAdvice.Analyses.Test.LightBulb.CodeFixed
{
    [TestClass]
    public class UnequalOperatorsSuggestedTest
    {
        [TestMethod]
        public void UnequalOperators_NotHasCodeFixed()
        {
            // Preparar cénários
            ITextBuffer buffer = Substitute.For<ITextBuffer>();
            ITextView textView = Substitute.For<ITextView>();
            SnapshotSpan span = CommonHelper.GetInstanceSnapshotSpan("var teste;");

            // Ação (Executar)
            SuggestedActionBasicBase equalsOperators = new UnequalOperatorsSuggested(buffer, textView, span);

            // Afirmar (Verificar resultado)
            var hasCodeFixed = equalsOperators.VerifiyHasCodeFixed();
            Assert.IsFalse(hasCodeFixed);
        }

        [TestMethod]
        public void UnequalOperators_HasCodeFixed()
        {
            // Preparar cénários e Ação (Executar)
            SuggestedActionBasicBase equalsOperators = this.GetInstanceSuggestedActionWithSucess();

            // Afirmar (Verificar resultado)
            Assert.IsTrue(equalsOperators.VerifiyHasCodeFixed());
        }

        [TestMethod]
        public void UnequalOperators_CodeFixed()
        {
            // Properties
            var codeExpected = "if (var1 !== var 2)";

            // Preparar cénários
            SuggestedActionBasicBase equalsOperators = this.GetInstanceSuggestedActionWithSucess();
            equalsOperators.Invoke(CancellationToken.None);

            var resultSuggested = equalsOperators.TextBuffer.CurrentSnapshot.GetText();
            Assert.AreEqual(codeExpected, resultSuggested);
        }

        private SuggestedActionBasicBase GetInstanceSuggestedActionWithSucess()
        {
            // Properties
            var contextType = "JavaScript";
            var codeFixed = "if (var1 != var 2)";

            // Moqs
            ITextBuffer buffer = new MoqTextBuffer(codeFixed, contextType);
            SnapshotSpan span = CommonHelper.GetInstanceSnapshotSpan(codeFixed);
            ITextView textView = Substitute.For<ITextView>();
            textView.TextBuffer.Returns(buffer);

            // Instance of the Suggest to Job!
            return new UnequalOperatorsSuggested(buffer, textView, span);
        }
    }
}
