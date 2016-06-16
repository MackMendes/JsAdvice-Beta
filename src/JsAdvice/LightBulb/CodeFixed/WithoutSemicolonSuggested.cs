using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace JsAdvice.LightBulb.CodeFixed
{
    /// <summary>
    /// Refactory this Class 
    /// </summary>
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

        private readonly string[] reservedWordBeginCommand = new string[] { "var", "const", "let", "return" };

        private readonly string[] findEndNextLine = new string[] { "}" };

        #endregion


        #region Properties To Context
        
        private const string messagerDisplay = "Advisable to include a semicolon (;) at the end of the command. Prevents errors.";

        #endregion

        public override bool HasPreview { get; } = true;

        public override Task<object> GetPreviewAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return base.GetPreviewAsync(cancellationToken);

            var textJob = base.Range.GetText().Trim();
            var inline = new Run(string.Concat(" ", textJob, "; "));
            var textBlock = new TextBlock(inline);
            return Task.FromResult<object>(textBlock);
        }

        public WithoutSemicolonSuggested(ITextBuffer buffer, ITextView view, SnapshotSpan range)
            : base(buffer, view, range, messagerDisplay)
        {
            reservedWordBeginCommand.Intersect(reservedWordBeginNotNecessarySemicolon);
            notNecessarySemicolon.Intersect(caracteresConcat);
            caracteresConcat.Add(signalPositive);
            caracteresConcat.Add(signalNegative);
        }

        public override void Invoke(CancellationToken cancellationToken)
        {
            this.TextBuffer.Insert(this.Range.End.Position, ";");
        }

        #region Fixed

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

            return VerifyDetails(treeSyntacsThisContext, getBeginWord, getEndWord, textLine);
        }


        private bool VerifyDetails(ITextSnapshotLine treeSyntacsThisContext, string getBeginWord, string getEndWord, string textLine)
        {
            bool hasFixed = false;
            var AllLines = this.TextBuffer.CurrentSnapshot.Lines.ToArray();
            string wordBeginNextLine = "", nextLineInText, previuosLineInText;
            string[] splitCommandNextLine, splitCommandProviusLine;

            nextLineInText = this.GetNexLine(treeSyntacsThisContext, AllLines);

            splitCommandNextLine = nextLineInText.Trim().Split(' ');
            wordBeginNextLine = splitCommandNextLine[0];

            var caracteresConcatInString = caracteresConcat.ConvertAll<string>(x => x.ToString());
            if (reservedWordBeginCommand.Contains(getBeginWord) && !string.IsNullOrWhiteSpace(getEndWord) &&
                !caracteresConcatInString.Contains(getEndWord) &&
                ((wordBeginNextLine.IndexOf(signalPositive) != 0 && wordBeginNextLine.IndexOf(signalNegative) != 0) ||
                wordBeginNextLine.IndexOf("++") == 0 || wordBeginNextLine.IndexOf("--") == 0 || carateresAggregation.Contains(wordBeginNextLine)))
                return true;

            previuosLineInText = this.GetPreviousLine(treeSyntacsThisContext, AllLines);
            splitCommandProviusLine = previuosLineInText.Trim().Split(' ');
            var wordBeginProvLine = splitCommandProviusLine[0];
            var wordEndProvLine = splitCommandProviusLine[splitCommandProviusLine.Count() - 1];
            var notNecessarySemicolonInList = notNecessarySemicolon.ToList().ConvertAll<string>(x => x.ToString());

            //if (reservedWordBeginCommand.Contains(wordBeginProvLine) && !notNecessarySemicolonInList.Contains(wordEndProvLine))
            //    return true;

            if (reservedWordBeginCommand.Contains(wordBeginNextLine))
                return true;

            // caso de:  
            // { teste: 1  
            // }
            if (findEndNextLine.Contains(wordBeginNextLine))
                if (textLine.IndexOf(':') == -1 && wordEndProvLine.IndexOf(':') == -1)
                    return true;

#pragma warning disable CS1030 // #warning: '"Ainda falta alguns cenários para concluir"'
#warning "Ainda falta alguns cenários para concluir"

            return hasFixed;
#pragma warning restore CS1030 // #warning: '"Ainda falta alguns cenários para concluir"'
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

        #endregion
    }
}
