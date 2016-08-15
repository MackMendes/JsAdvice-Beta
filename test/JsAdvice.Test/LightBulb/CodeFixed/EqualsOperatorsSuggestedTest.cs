using System.Collections.Generic;
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
            // Moqs
            ITextBuffer buffer = Substitute.For<ITextBuffer>();
            buffer.ContentType.TypeName.Returns("JavaScript");
            buffer.ContentType.DisplayName.Returns("JavaScript");
            //buffer.CurrentSnapshot.GetText().Returns("if (var1 == var 2)");

            SnapshotSpan span = this.GetInstanceToJob("if (var1 == var 2)");

            //ITrackingSpan trackingSpan = Substitute.For<ITrackingSpan>();

            //trackingSpan.When(x => x.GetSpan(Arg.Any<ITextSnapshot>())).Do(x =>
            //{
            //    ((ITextSnapshot)x[0]).TextBuffer.CurrentSnapshot.GetText();

            //});

            //trackingSpan.GetSpan(Arg.Any<ITextSnapshot>()).ReturnsForAnyArgs(x =>
            //    this.GetInstanceToJob(x.Arg<ITextSnapshot>().TextBuffer.CurrentSnapshot.GetText())
            //);


            ITextSnapshot snapshot = Substitute.For<ITextSnapshot>();
            snapshot.TextBuffer.Returns(buffer);
            snapshot.Lines.Returns(new List<ITextSnapshotLine>() { Substitute.For<ITextSnapshotLine>() });
            //snapshot.CreateTrackingSpan(0, 3, SpanTrackingMode.EdgeExclusive).
            //    ReturnsForAnyArgs(returnThis:
            //        x =>
            //        {
            //            return GetInstanceToJob(buffer.CurrentSnapshot.GetText()
            //                .Substring(Convert.ToInt32(x[0]), Convert.ToInt32(x[1])))
            //        });

            //snapshot.When(x => x.CreateTrackingSpan(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<SpanTrackingMode>()))
            //    .Do(x =>
            //    {
            //        GetInstanceToJob(span.GetText().Substring(((int) x[0]), (int) x[1]));
            //    });

            //snapshot.CreateTrackingSpan(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<SpanTrackingMode>()).Returns(trackingSpan);


            snapshot.CreateTrackingSpan(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<SpanTrackingMode>())
            .Returns(x =>
            {
                var trackingReturn = Substitute.For<ITrackingSpan>();
                trackingReturn.GetSpan(Arg.Any<ITextSnapshot>()).Returns(y =>
                {
                    return GetInstanceToJob(span.GetText().Substring(((int)x[0]), (int)x[1]));
                });

                return trackingReturn;
            });



            buffer.CurrentSnapshot.Returns(snapshot);

            ITextView textView = Substitute.For<ITextView>();
            textView.TextBuffer.Returns(buffer);
            //textView.



            buffer.Replace(Arg.Any<Span>(), Arg.Any<string>()).Returns(x =>
            {
                buffer.CurrentSnapshot.GetText().Returns(y =>
                {
                    var spanJob = ((SnapshotSpan) x[0]);
                    return span.GetText().Replace(spanJob.GetText(), (string) x[1]);

                });
                return null;
            });
            //SnapshotSpan span = this.GetInstanceToJob("if (var1 == var 2)");

            // Preparar cénários
            SuggestedActionBasicBase equalsOperators = new EqualsOperatorsSuggested(buffer, textView, span);


            equalsOperators.Invoke(CancellationToken.None);

            var newBuffer = buffer.CurrentSnapshot.GetText();

        }

        [TestMethod]
        public void EqualsOperators_CodeFixed_Moq2()
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
