using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace JsAdvice.Analyses.LightBuld.CodeFixed.Base
{
    public abstract class SuggestedActionBase : ISuggestedAction
    {
        public SuggestedActionBase(ITextBuffer buffer, ITextView view, SnapshotSpan range, string displayText)
        {
            this.TextBuffer = buffer;
            this.TextView = view;
            this.Range = range;
            this.DisplayText = displayText;            
        }

        #region Properties ISuggestedAction

        public string DisplayText { get; }

        public virtual bool HasActionSets { get; } = false;

        public virtual bool HasPreview { get; } = false;

        public string IconAutomationText { get; }

        public ImageMoniker IconMoniker { get; protected set; }

        public string InputGestureText { get; protected set; }

        #endregion

        #region Properties

        public ITextBuffer TextBuffer { get; }
        public ITextView TextView { get; }
        public SnapshotSpan Range { get; }

        #endregion

        #region ISuggestedAction

        public Task<IEnumerable<SuggestedActionSet>> GetActionSetsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Enumerable.Empty<SuggestedActionSet>());
        }

        public virtual Task<object> GetPreviewAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }

        public abstract void Invoke(CancellationToken cancellationToken);

        public virtual bool TryGetTelemetryId(out Guid telemetryId)
        {
            telemetryId = Guid.Empty;
            return false;
        }

        #endregion

        #region Method

        internal abstract bool VerifiyHasCodeFixed();

        #endregion

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
