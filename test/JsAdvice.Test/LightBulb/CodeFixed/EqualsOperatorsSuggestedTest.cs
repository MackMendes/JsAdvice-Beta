﻿using System.Threading;
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
            SnapshotSpan span = CommonHelper.GetInstanceSnapshotSpan("var teste;");

            // Ação (Executar)
            SuggestedActionBasicBase equalsOperators = new EqualsOperatorsSuggested(buffer, textView, span);

            // Afirmar (Verificar resultado)
            var hasCodeFixed = equalsOperators.VerifiyHasCodeFixed();
            Assert.IsFalse(hasCodeFixed);
        }

        [TestMethod]
        public void EqualsOperators_HasCodeFixed()
        {
            // Preparar cénários e Ação (Executar)
            SuggestedActionBasicBase equalsOperators = this.GetInstanceSuggestedActionWithSucess();

            // Afirmar (Verificar resultado)
            Assert.IsTrue(equalsOperators.VerifiyHasCodeFixed());
        }

        [TestMethod]
        public void EqualsOperators_CodeFixed()
        {
            // Properties
            var codeExpected = "if (var1 === var 2)";

            // Preparar cénários
            SuggestedActionBasicBase equalsOperators = this.GetInstanceSuggestedActionWithSucess();
            equalsOperators.Invoke(CancellationToken.None);

            var resultSuggested = equalsOperators.TextBuffer.CurrentSnapshot.GetText();
            Assert.AreEqual(codeExpected, resultSuggested);
        }

        private SuggestedActionBasicBase GetInstanceSuggestedActionWithSucess()
        {
            // Properties
            var codeFixed = "if (var1 == var 2)";

            // Moqs
            ITextBuffer buffer = new MoqTextBuffer(codeFixed);
            SnapshotSpan span = CommonHelper.GetInstanceSnapshotSpan(codeFixed);
            ITextView textView = Substitute.For<ITextView>();
            textView.TextBuffer.Returns(buffer);

            // Instance of the Suggest to Job!
            return new EqualsOperatorsSuggested(buffer, textView, span);
        }
    }
}
