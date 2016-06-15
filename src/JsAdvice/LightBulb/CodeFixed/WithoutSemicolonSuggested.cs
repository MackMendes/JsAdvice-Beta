using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace JsAdvice.LightBulb.CodeFixed
{
    internal sealed class WithoutSemicolonSuggested : Base.SuggestedActionBase
    {
        #region Fixed
        private readonly char[] notNecessarySemicolon =
            new char[] { '{', '(', '[', ';', '\\', '~', '^', ' ' };

        private readonly string[] reservedWordBeginNotNecessarySemicolon = new string[] { "for", "while", "do", "with", "switch", "try", "if",
            "else", "class"};

        private readonly List<char> caracteresConcat = new List<char> { ',', '*', '&', '|', '=', '%', ':', '?', '/', '^', '>', '<' };

        private readonly static char signalPositive = '+';
        private readonly static char signalNegative = '-';

        private readonly string[] carateresAggregation = new string[] { string.Concat(signalPositive, signalPositive),
            string.Concat(signalNegative, signalNegative) };

        private readonly string[] reservedWordBeginNextLine = new string[] { "var", "const", "let" };

        private readonly string[] findEndNextLine = new string[] { "}" };




        private const string messagerDisplay = "Aconselhável colocar ponto e virgula no final do comando. Evita erros ao fazer Bundling e Minification.";

        #endregion


        #region Properties To Context


        #endregion


        public WithoutSemicolonSuggested(ITextBuffer buffer, ITextView view, SnapshotSpan range)
            : base(buffer, view, range, messagerDisplay)
        {
            reservedWordBeginNextLine.Intersect(reservedWordBeginNotNecessarySemicolon);
            notNecessarySemicolon.Intersect(caracteresConcat);
            caracteresConcat.Add(signalPositive);
            caracteresConcat.Add(signalNegative);
        }

        public override void Invoke(CancellationToken cancellationToken)
        {
            this.TextBuffer.Insert(this.Range.End.Position, ";");
        }

        internal override bool VerifiyHasCodeFixed()
        {
            var textLine = this.Range.GetText().Trim();
            var getLastCharacter = textLine.Length > 0 ? textLine.Substring(textLine.Length - 1).ToCharArray() : new char[] { ' ' };

            if (notNecessarySemicolon.Contains(getLastCharacter[0]))
                return false;

            return VerifyIncludeSemicolon(textLine);
        }

        private bool VerifyIncludeSemicolon(string textLine)
        {
            // Caso a linha comece com alguma palavra reservada da lista acima, não é necessário incluir o ponto e virgual
            string[] splitTextLine;
            splitTextLine = textLine.Split(' ');
            var getBeginWord = splitTextLine[0];
            var getEndWord = splitTextLine.Count() >= 2 ? splitTextLine[splitTextLine.Count() - 1] : string.Empty;
            if (reservedWordBeginNotNecessarySemicolon.Contains(getBeginWord))
                return false;

            // Se for a última linha do arquivo
            var treeSyntacsThisContext = this.TextBuffer.CurrentSnapshot.GetLineFromPosition(this.Range.Start.Position);
            if (treeSyntacsThisContext.LineNumber >= this.TextBuffer.CurrentSnapshot.LineCount)
                return true;

            var partterRegex = @"(([a-zA-Z0-9]((\++)|(\--))))$";
            var lastPositionWord = (getEndWord.Count() - 1);

            if (!(Regex.IsMatch(getBeginWord, partterRegex) || carateresAggregation.Contains(getEndWord)) &&
                !string.IsNullOrWhiteSpace(getEndWord) &&
                (getEndWord.LastIndexOf(signalPositive) == lastPositionWord || getEndWord.LastIndexOf(signalNegative) == lastPositionWord))
                return false;

            return VerifyDetails(treeSyntacsThisContext, getBeginWord, getEndWord);
        }


        private bool VerifyDetails(ITextSnapshotLine treeSyntacsThisContext, string getBeginWord, string getEndWord)
        {
            bool hasFixed = false;
            var AllLines = this.TextBuffer.CurrentSnapshot.Lines.ToArray();
            string wordBeginNextLine = "", nextLineInText, previuosLineInText;
            string[] splitCommandNextLine, splitCommandProviusLine;

            nextLineInText = this.GetNexLine(treeSyntacsThisContext, AllLines);

            splitCommandNextLine = nextLineInText.Trim().Split(' ');
            wordBeginNextLine = splitCommandNextLine[0];

            var caracteresConcatInString = caracteresConcat.ConvertAll<string>(x => x.ToString());
            if (reservedWordBeginNextLine.Contains(getBeginWord) && !string.IsNullOrWhiteSpace(getEndWord) &&
                !caracteresConcatInString.ToList<string>().Contains(getEndWord) && 
                ((wordBeginNextLine.IndexOf(signalPositive) != 0 && wordBeginNextLine.IndexOf(signalNegative) != 0) ||
                wordBeginNextLine.IndexOf("++") == 0 || wordBeginNextLine.IndexOf("--") == 0 || carateresAggregation.Contains(wordBeginNextLine)))
                return true;

            previuosLineInText = this.GetPreviousLine(treeSyntacsThisContext, AllLines);
            splitCommandProviusLine = previuosLineInText.Trim().Split(' ');
            var wordBeginProvLine = splitCommandProviusLine[0];
            var wordEndProvLine = splitCommandProviusLine[splitCommandProviusLine.Count() - 1];
            var notNecessarySemicolonInList = notNecessarySemicolon.ToList().ConvertAll<string>(x => x.ToString());
            if (reservedWordBeginNextLine.Contains(wordBeginNextLine) || (findEndNextLine.Contains(wordBeginNextLine) && 
                reservedWordBeginNextLine.Contains(wordBeginProvLine) && !notNecessarySemicolonInList.Contains(wordEndProvLine)))
                return true;

#warning "Ainda falta alguns cenários para concluir"

            return hasFixed;
        }


        private string GetPreviousLine(ITextSnapshotLine treeSyntacsThisContext, ITextSnapshotLine[] AllLines)
        {
            int moreLine = -1;
            string previousLineInText;
            ITextSnapshotLine previousLine;

            while (treeSyntacsThisContext.LineNumber + moreLine >= 0)
            {
                previousLine = AllLines[treeSyntacsThisContext.LineNumber + moreLine];
                previousLineInText = previousLine.GetText();
                if (!string.IsNullOrWhiteSpace(previousLineInText))
                    return previousLineInText;

                moreLine--;
            }

            return string.Empty;

        }


        private string GetNexLine(ITextSnapshotLine treeSyntacsThisContext, ITextSnapshotLine[] AllLines)
        {
            int moreLine = 1;
            string nextLineInText;
            ITextSnapshotLine nextLine;

            while (AllLines.Count() > treeSyntacsThisContext.LineNumber + moreLine)
            {
                nextLine = AllLines[treeSyntacsThisContext.LineNumber + moreLine];
                nextLineInText = nextLine.GetText();
                if (!string.IsNullOrWhiteSpace(nextLineInText))
                    return nextLineInText;

                moreLine++;
            }

            return string.Empty;

        }
    }
}
