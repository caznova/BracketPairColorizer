using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.CodeAnalysis.CSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace BracketPairColorizer
{
    class BracketTagger : ITagger<IClassificationTag>
    {
        private ITextBuffer theBuffer;
        private RoslynDocument cache;
        private IClassificationTypeRegistryService _classificationTypeRegistry;
#pragma warning disable CS0067
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore CS0067

        static Random Rand = new Random();

        internal BracketTagger(ITextBuffer buffer, IClassificationTypeRegistryService registry)
        {
            theBuffer = buffer;
            _classificationTypeRegistry = registry;
        }

        public IEnumerable<ITagSpan<IClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (spans.Count == 0)
            {
                return Enumerable.Empty<ITagSpan<IClassificationTag>>();
            }
            if (this.cache == null || this.cache.Snapshot != spans[0].Snapshot)
            {
                var task = RoslynDocument.Resolve(theBuffer, spans[0].Snapshot);
                task.Wait();
                if (task.IsFaulted)
                {
                    return Enumerable.Empty<ITagSpan<IClassificationTag>>();
                }
                cache = task.Result;
            }
            return FindBracketSpans(this.cache, spans);
        }


        public IEnumerable<ITagSpan<IClassificationTag>> FindBracketSpans(RoslynDocument doc, NormalizedSnapshotSpanCollection spans)
        {
            var snapshot = spans[0].Snapshot;

            List<ITagSpan<IClassificationTag>> _tagspans = new List<ITagSpan<IClassificationTag>>();
            var nodeOrToken = (SyntaxNodeOrToken)doc.SyntaxRoot;
            Action<List<ITagSpan<IClassificationTag>>, SyntaxNodeOrToken ,int  > _walkintochild = null;
            int _dept = 0; 
            _walkintochild = (List<ITagSpan<IClassificationTag>> __tagspans, SyntaxNodeOrToken nOrT, int __dept) =>
            {
                foreach (var child in nOrT.ChildNodesAndTokens())
                {
                    if( child.ChildNodesAndTokens().Count > 0)
                        _walkintochild(__tagspans, child,++__dept);
                }

                List<SyntaxNodeOrToken> __brace = nOrT.ChildNodesAndTokens().Where(child =>
                    child.IsKind(SyntaxKind.OpenBraceToken) || child.IsKind(SyntaxKind.CloseBraceToken)
                    || child.IsKind(SyntaxKind.OpenParenToken) || child.IsKind(SyntaxKind.CloseParenToken)
                    || child.IsKind(SyntaxKind.OpenBracketToken) || child.IsKind(SyntaxKind.CloseBracketToken)
                    ).ToList();

                if (__brace.Count == 0) return;

                if (__brace.Where(s => s.ToString() == "(").ToList().Count != __brace.Where(s => s.ToString() == ")").ToList().Count)
                {
                    return;
                }

                if (__brace.Where(s => s.ToString() == "{").ToList().Count != __brace.Where(s => s.ToString() == "}").ToList().Count)
                {
                    return;
                }

                if (__brace.Where(s => s.ToString() == "[").ToList().Count != __brace.Where(s => s.ToString() == "]").ToList().Count)
                {
                    return;
                }

                while (__brace.Count > 0)
                {
                    SyntaxNodeOrToken l1 = __brace.First();
                    SyntaxNodeOrToken l2 = null;
                    string _start_check = l1.ToString();
                    string _end_check = ")";
                    if (_start_check == "{")
                    {
                        _end_check = "}";
                    }
                    if (_start_check == "[")
                    {
                        _end_check = "]";
                    }
                    if (_start_check != "{" && _start_check != "(" && _start_check != "[")
                    {
                        break;
                    }

                    __brace.Remove(l1);
                    int par = 1;
                    for (int i = 0; i < __brace.Count; ++i)
                    {
                        SyntaxNodeOrToken _ncheck = __brace[i];
                        if (_ncheck.ToString() == _end_check) par--;
                        else if (_ncheck.ToString() == _start_check) par++;
                        if (par == 0)
                        {
                            l2 = _ncheck;
                            __brace.Remove(l2);
                            break;
                        }
                    }

                    string _tp = ((__dept % 8) + 1).ToString();
                    IClassificationType classificationType = _classificationTypeRegistry.GetClassificationType(_tp);

                    __tagspans.Add(l1.Span.ToTagSpan(snapshot, classificationType));
                    if (l2 != null)
                        __tagspans.Add(l2.Span.ToTagSpan(snapshot, classificationType));
                    else
                        return;
                }
            };

            _walkintochild(_tagspans, nodeOrToken, _dept);

            foreach(ITagSpan<IClassificationTag> _t in _tagspans)
            {
                yield return _t;
            }
        }

        public class RoslynDocument
        {
            public Workspace Workspace { get; private set; }
            public Document Document { get; private set; }
            public SemanticModel SemanticModel { get; private set; }
            public SyntaxNode SyntaxRoot { get; private set; }
            public ITextSnapshot Snapshot { get; private set; }

            private RoslynDocument()
            {
            }

            public static async Task<RoslynDocument> Resolve(
                ITextBuffer buffer,
                ITextSnapshot snapshot)
            {
                var workspace = buffer.GetWorkspace();
                var document = snapshot.GetOpenDocumentInCurrentContextWithChanges();
                var semanticModel = await document.GetSemanticModelAsync().ConfigureAwait(false);
                var syntaxRoot = await document.GetSyntaxRootAsync().ConfigureAwait(false);
                return new RoslynDocument
                {
                    Workspace = workspace,
                    Document = document,
                    SemanticModel = semanticModel,
                    SyntaxRoot = syntaxRoot,
                    Snapshot = snapshot
                };
            }
        }
    }
}