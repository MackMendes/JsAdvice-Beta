using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsAdvice.Analyses.LightBulb.CodeFixed.Base;
using Microsoft.VisualStudio.Text;
using JsAdvice.Analyses.Test.Helper;
using NSubstitute;
using Microsoft.VisualStudio.Text.Editor;
using JsAdvice.Analyses.LightBulb.CodeFixed;
using System.Threading;

namespace JsAdvice.Analyses.Test.LightBulb.CodeFixed
{
    [TestClass]
    public class InitializeObjectSuggestedTest
    {
        [TestMethod]
        public void InitializeObject_NotHasCodeFixed()
        {
            // Preparar cénários
            ITextBuffer buffer = Substitute.For<ITextBuffer>();
            ITextView textView = Substitute.For<ITextView>();
            SnapshotSpan span = CommonHelper.GetInstanceSnapshotSpan("var teste;");

            // Ação (Executar)
            SuggestedActionBasicBase initializeObject = new InitializeObjectSuggested(buffer, textView, span);

            // Afirmar (Verificar resultado)
            var hasCodeFixed = initializeObject.VerifiyHasCodeFixed();
            Assert.IsFalse(hasCodeFixed);
        }

        [TestMethod]
        public void InitializeObject_HasCodeFixed()
        {
            // Preparar cénários e Ação (Executar)
            SuggestedActionBasicBase equalsOperators = this.GetInstanceInitializeObjectWithSucess();

            // Afirmar (Verificar resultado)
            Assert.IsTrue(equalsOperators.VerifiyHasCodeFixed());
        }

        [TestMethod]
        public void InitializeObject_CodeFixed()
        {
            // Properties
            var codeExpected = " {}";

            // Preparar cénários
            SuggestedActionBasicBase equalsOperators = this.GetInstanceInitializeObjectWithSucess();
            equalsOperators.Invoke(CancellationToken.None);

            var resultSuggested = equalsOperators.TextBuffer.CurrentSnapshot.GetText();
            Assert.AreEqual(codeExpected, resultSuggested);
        }

        private SuggestedActionBasicBase GetInstanceInitializeObjectWithSucess()
        {
            // Properties
            var codeFixed = " new Object()";

            // Moqs
            ITextBuffer buffer = new MoqTextBuffer(codeFixed);
            SnapshotSpan span = CommonHelper.GetInstanceSnapshotSpan(codeFixed);
            ITextView textView = Substitute.For<ITextView>();
            textView.TextBuffer.Returns(buffer);

            // Instance of the Suggest to Job!
            return new InitializeObjectSuggested(buffer, textView, span);
        }
    }
}
