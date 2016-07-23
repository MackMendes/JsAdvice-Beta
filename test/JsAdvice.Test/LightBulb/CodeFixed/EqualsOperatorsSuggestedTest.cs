using System;
using JsAdvice.Analyses.LightBulb.CodeFixed;
using JsAdvice.Analyses.LightBulb.CodeFixed.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using NSubstitute;

namespace JsAdvice.Analyses.Test.LightBulb.CodeFixed
{
    [TestClass]
    public class EqualsOperatorsSuggestedTest
    {





        [TestMethod]
        public void EqualsOperators_NotHasCodeFixed()
        {
            // Moqs
            ITextBuffer buffer = Substitute.For<ITextBuffer>();
            ITextView textView = Substitute.For<ITextView>();

            SnapshotSpan span = this.GetInstanceToJob("var teste;");

            // Preparar cénários
            SuggestedActionBasicBase equalsOperators = new EqualsOperatorsSuggested(buffer, textView, span);


            var hasCodeFixed = equalsOperators.VerifiyHasCodeFixed();

            Assert.IsFalse(hasCodeFixed);
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
