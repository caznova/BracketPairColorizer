using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace BracketPairColorizer
{
    public class BracketClassifier : IClassifier
    {
        IClassificationTypeRegistryService _classificationTypeRegistry;
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

        static Random rand = new Random();
        internal BracketClassifier(IClassificationTypeRegistryService registry)
        {
            this._classificationTypeRegistry = registry;
        }

        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            ITextSnapshot snapshot = span.Snapshot;

            List<ClassificationSpan> spans = new List<ClassificationSpan>();

            if (snapshot.Length == 0)
                return spans;

            int _linestart = 0;
            int _lineend = snapshot.LineCount - 1;

            List<Tuple<int,int,char>> _bracket = new List<Tuple<int, int, char>>();

            for (int i = _linestart; i <= _lineend; i++)
            {
                ITextSnapshotLine line = snapshot.GetLineFromLineNumber(i);
                string text = line.Snapshot.GetText(new SnapshotSpan(line.Start, line.Length));

                int _loc_open = text.IndexOf("(");
                int _loc_close = text.IndexOf(")");
                while (_loc_open > -1 || _loc_close > - 1)
                {
                    if (_loc_open > _loc_close && _loc_close != -1)
                    {
                        _bracket.Add(new Tuple<int, int, char>(i, _loc_close, ')'));
                        _loc_open = text.IndexOf("(", _loc_close + 1);
                        _loc_close = text.IndexOf(")", _loc_close + 1);
                    }
                    else if (_loc_open < _loc_close && _loc_open != -1)
                    {
                        _bracket.Add(new Tuple<int, int, char>(i, _loc_open, '('));
                        _loc_close = text.IndexOf(")", _loc_open + 1);
                        _loc_open = text.IndexOf("(", _loc_open + 1);           
                    }
                    else if (_loc_open == -1 && _loc_close != -1)
                    {
                        _bracket.Add(new Tuple<int, int, char>(i, _loc_close, ')'));
                        break;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            if (  _bracket.Where(s=>s.Item3 == '(').ToList().Count  != _bracket.Where(s => s.Item3 == ')').ToList().Count )
            {
                return spans;
            }

            while (_bracket.Count > 0)
            {
                Tuple<int, int,char> l1 = _bracket.First();
                Tuple<int, int, char> l2 = null;
                if (l1.Item3 != '(' )
                {
                    return spans;
                }

                _bracket.Remove(l1);
                int par = 1;
                for(int i = 0; i < _bracket.Count; ++i)
                {
                    Tuple<int, int, char> bcheck = _bracket[i];
                    if (bcheck.Item3 == ')') par--;
                    else if (bcheck.Item3 == '(') par++;

                    if(par == 0)
                    {
                        l2 = bcheck;
                        _bracket.Remove(l2);
                        break;
                    }
                }

                string _tp = rand.Next(1, 9).ToString();
                IClassificationType classificationType = _classificationTypeRegistry.GetClassificationType(_tp);

                ITextSnapshotLine lineSt = snapshot.GetLineFromLineNumber(l1.Item1);
                ITextSnapshotLine lineSp = snapshot.GetLineFromLineNumber(l2.Item1);

                SnapshotSpan bStartSpan = new SnapshotSpan(span.Snapshot, new Span(lineSt.Start + l1.Item2, 1));
                SnapshotSpan bEndSpan = new SnapshotSpan(span.Snapshot, new Span(lineSp.Start + l2.Item2, 1));
                if (classificationType != null)
                {
                    spans.Add(new ClassificationSpan(bStartSpan, classificationType));
                    spans.Add(new ClassificationSpan(bEndSpan, classificationType));
                }
            }
            
            return spans;
        }
    }
}
